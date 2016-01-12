using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Entities;
using Jane.Data.EntityFramework.Managers;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using Twilio;
using Address = Jane.Data.EntityFramework.Entities.Address;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        private ApplicationSignInManager _signInManager;

        public AccountController(HgUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        public AccountController()
        {
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Update")]
        public async Task<HttpResponseMessage> UpdateUser(UserModel model)
        {
            List<string> errors = new List<string>();
            bool error = false;
            if (string.IsNullOrEmpty(model.Username))
            {
                errors.Add("Email is required");
                error = true;
            }
            if (string.IsNullOrEmpty(model.Zipcode))
            {
                errors.Add("Zipcode is required");
                error = true;
            }

            if (error)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
            }

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            user.Email = model.Email;
            if (user.Email.ToLower() != model.Email.ToLower())
            {
                user.EmailConfirmed = false;
            }
            if (user.PhoneNumber != model.PhoneNumber || string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumberConfirmed = false;
            }
            user.UserName = model.Username ?? model.Email;
            user.Zipcode = model.Zipcode;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Birthday = model.Birthday;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            IdentityResult result = await this.UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        // POST api/Account/Register
        // POST api/User/Register
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async Task<HttpResponseMessage> RegisterUser(UserModel model)
        {
            var user = await UserManager.FindByGuidAsync(model.Guid);
            if (user != null)
            {
                // Guid is already attached to a registered user -- so create a fresh user
                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, new List<string>(){"Email is already registered."});
                    var guid = System.Guid.NewGuid().ToString();
                    var idresult = await UserManager.CreateAsync(new User
                    {
                        Email = model.Email,
                        UserName = model.Username ?? model.Email,
                        Guid = guid,
                        Zipcode = model.Zipcode,
                        PatientInfo = new PatientInfo(),
                        Wallet = new Wallet(),
                        CreatedAt = DateTimeOffset.UtcNow,
                        UpdatedAt = DateTimeOffset.UtcNow,
                        LastLogin = DateTimeOffset.UtcNow
                    }, model.Password);
                    if (!idresult.Succeeded)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, idresult.Errors);
                    }
                }
                else // Update the existing anonymous user with registration info
                {
                    try
                    {
                        //var passwordResult = await UserManager.AddPasswordAsync(user.Id, model.Password);
                        //if (!passwordResult.Succeeded)
                        //{
                        //    return Request.CreateResponse(HttpStatusCode.BadRequest, passwordResult.Errors);
                        //}

                        user.Email = model.Email;
                        user.UserName = model.Username ?? model.Email;
                        user.Wallet = new Wallet();
                        user.UpdatedAt = DateTimeOffset.UtcNow;
                        user.LastLogin = DateTimeOffset.UtcNow;
                        IdentityResult result = await this.UserManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
                        }
                    }
                    catch { }
                }
            }
            else
            {
                _logger.ErrorFormat("Registration error should not hit user guid {0} and email {1}", model.Guid, model.Email);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new List<string>()
                {
                    "Registration is not valid. Clear your cookies and try again."
                });
            }
            user = await UserManager.FindByEmailAsync(model.Email);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            //await UserManager.SendEmailAsync(user.Id,
            //    "Confirm your account",
            //    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            await SendVerificationEmail(model.Email,
                 "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>", "E-mail confirmation");

            string patientListId = ConfigurationManager.AppSettings["MailChimpPatientListId"];
            AddEmailToMailChimp(model.Username ?? model.Email, model.FirstName, model.LastName, patientListId);

            // Auto login after register (successful user registration should return access_token)
            var loginResult = this.Login(new UserModel() { Password = model.Password, ConfirmPassword = model.Password, Username = model.Email, Email = model.Email, Guid = user.Guid });
            return await loginResult;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Logout")]
        public async Task<IHttpActionResult> Logout(string usertoken, string sessiontoken)
        {
            var user = await HGContext.Users.Include("UserSessions").FirstOrDefaultAsync(x => x.Guid == usertoken);
            var session = await HGContext.Sessions.FirstOrDefaultAsync(x => x.Token == sessiontoken);
            var usersession = await HGContext.UserSessions.FirstOrDefaultAsync(x => x.SessionId == session.Id);
            var sessionresult = HGContext.Sessions.Remove(session);
            var usersessionresult = HGContext.UserSessions.Remove(usersession);
            var result = await HGContext.SaveChangesAsync();
            return Ok();
        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Login")]
        public async Task<HttpResponseMessage> Login(UserModel login)
        {
            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            //var message = new MailMessage();
            //message.To.Add(new MailAddress("7essein.cis@gmail.com"));  // replace with valid value 
            //message.From = new MailAddress("admin@askmjane.com");  // replace with valid value
            //message.Subject = "Your email subject";
            //message.Body = string.Format(body, "AskMJane", "7essein.cis@gmail.com", "hi");
            //message.IsBodyHtml = true;

            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = "mva3212", // replace with valid value
            //        Password = "AskMJane123" // replace with valid value
            //    };
            //    smtp.Credentials = credential;
            //    smtp.Host = "smtp-mail.outlook.com";
            //    smtp.Port = 587;
            //    smtp.EnableSsl = true;
            //    await smtp.SendMailAsync(message);

            //}

            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "token";
            _logger.DebugFormat("{0} {1} {2} {3}", tokenServiceUrl, request.Url, request.Url.GetLeftPart(UriPartial.Authority), request.ApplicationPath);
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
            {
               // new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", login.Username ?? login.Email),
                //new KeyValuePair<string, string>("password", login.Password)
            };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                _logger.DebugFormat("{0} {1} {2} {3}", tokenServiceUrl, requestParams[0], requestParams[1], requestParams[2]);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                _logger.DebugFormat("{0} {1}", responseString, responseCode);
                var responseObj = JObject.Parse(responseString);

                if (responseCode == HttpStatusCode.OK)
                {
                    try
                    {
                        User user = null;
                        if (login.Email != null && !login.Email.IsNullOrWhiteSpace())
                        {
                            user =
                                await
                                    HGContext.Users.Include("UserSessions")
                                        .FirstOrDefaultAsync(x => x.Email == login.Email);
                        }
                        else
                        {
                            user =
                                await
                                    HGContext.Users.Include("UserSessions")
                                        .FirstOrDefaultAsync(x => x.UserName == login.Username);
                        }

                        if (await UserManager.IsInRoleAsync(user.Id, Role.DispensaryManager))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new List<string> { "You seems to be a dispensary manager, try to login from dispensary login screen." });
                        }


                        responseObj["usertoken"] = user.Guid;
                        var sessiontoken = Guid.NewGuid().ToString();
                        user.UserSessions.Add(new UserSession()
                        {
                            Session = new Session()
                            {
                                LastSeen = DateTimeOffset.UtcNow,
                                Token = sessiontoken
                            },
                            UserId = user.Id
                        });
                        var result = await HGContext.SaveChangesAsync();
                        if (result == 0)
                        {
                            _logger.ErrorFormat("Login:  Create Session failed --  UserId: {0} Guid: {1}", user.Id,
                                user.Guid);
                            return Request.CreateResponse(HttpStatusCode.BadRequest);
                        }
                        responseObj["sessiontoken"] = sessiontoken;
                        var anonuser = await HGContext.Users.FirstOrDefaultAsync(x => x.Guid == login.Guid);
                        if (user.Id != anonuser.Id && string.IsNullOrWhiteSpace(anonuser.PasswordHash))
                        {
                            var cart = await HGContext.Orders.FirstOrDefaultAsync(x =>
                                x.UserId == user.Id && x.IsCheckedOut == false &&
                                x.DispensaryProductVariantOrders.Count > 0);
                            //Set Anonymous user cart to user cart
                            var anoncart = await HGContext.Orders.FirstOrDefaultAsync(x => x.UserId == anonuser.Id
                                                                                           && x.IsCheckedOut == false &&
                                                                                           x.DispensaryProductVariantOrders.Count > 0);
                            if (anoncart != null)
                            {
                                anoncart.UserId = user.Id;
                                anoncart.UpdatedAt = DateTimeOffset.UtcNow;
                                var cartresult = await HGContext.SaveChangesAsync();
                                if (cartresult == 0)
                                {
                                    _logger.ErrorFormat(
                                        "Login:  Anonymous Cart swap failed -- CartId: {2} UserId: {0} Guid: {1}",
                                        anonuser.Id, anonuser.Guid, anoncart.Id);
                                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                                }
                            }


                            HGContext.Users.Remove(anonuser);
                            var anonuserresult = await HGContext.SaveChangesAsync();
                            if (anonuserresult == 0)
                            {
                                _logger.ErrorFormat("Login:  Anonymous User removal failed -- Id: {0} Guid: {1}",
                                    anonuser.Id, anonuser.Guid);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.ErrorFormat("Login error {0}", e);
                    }


                }
                else
                {
                    var error = new List<string>() { "The username or password is incorrect." };
                    return Request.CreateResponse(responseCode, error);
                }

                var response = Request.CreateResponse(responseCode, responseObj);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<HttpResponseMessage> ConfirmEmail(int userId, string code = "")
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                string fullyQualifiedUrl = ConfigurationManager.AppSettings["webclientURL"];
                response.Headers.Location = new Uri(fullyQualifiedUrl);
                return response;
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);

        }
        [System.Web.Http.Route("ChangePassword")]
        [System.Web.Http.Authorize]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors.ToArray());
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }



        [System.Web.Http.HttpGet]

        [System.Web.Http.Route("SendMailVerification")]
        [System.Web.Http.Authorize]
        public async Task<HttpResponseMessage> SendMailVerification()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            try
            {
                await SendVerificationEmail(user.Email,
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>", "E-mail confirmation");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private bool SendCodeToPhoneNumber(string code, string phoneNumber)
        {
            string accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            string authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            string fromPhoneNumber = ConfigurationManager.AppSettings["TwilioSenderPhone"];

            var twilio = new TwilioRestClient(accountSid, authToken);
            var sms = twilio.SendMessage(fromPhoneNumber, phoneNumber, "Verification code is " + code);

            if (sms.RestException != null)
            {
                _logger.Error(sms.ErrorMessage, null);
                return false;
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("SendCode")]
        [System.Web.Http.Authorize]
        public async Task<HttpResponseMessage> SendCode(string phoneNumber)
        {
            var user = UserManager.FindByName(User.Identity.Name);
            string code = new Random().Next(1111, 9999).ToString();
            var result = await UserManager.AddClaimAsync(user.Id, new Claim(ClaimTypes.MobilePhone, code));

            if (!result.Succeeded)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors.ToArray());
            }

            user.PhoneNumber = phoneNumber;
            if (!SendCodeToPhoneNumber(code, phoneNumber))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("verifyPhoneNumber")]
        [System.Web.Http.Authorize]
        public async Task<HttpResponseMessage> VerifyPhoneNumber(string code)
        {
            var user = UserManager.FindByName(User.Identity.Name);

            var result = await UserManager.GetClaimsAsync(user.Id);
            var existingUserClaim = result.Any(c => c.Value == code && c.Type == ClaimTypes.MobilePhone);

            if (existingUserClaim)
            {
                user.PhoneNumberConfirmed = true;
                await UserManager.UpdateAsync(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        public async Task<HttpResponseMessage> DeleteUser(int id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var userId = User.Identity.GetUserId<int>();
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "DispensaryAdmin"))
            {
                var appUser = await UserManager.FindByIdAsync(id);

                if (appUser != null)
                {
                   // IdentityResult result = await UserManager.DeleteAsync(appUser);
                    IdentityResult result = UserManager.SetLockoutEnabled(id, false);

                    if (!result.Succeeded)
                    {
                        var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);

                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "You must be an admin or global admin");
        }

        [System.Web.Http.Authorize(Roles = "GlobalAdmin")]
        [System.Web.Http.Route("user/{id}/roles")]
        [System.Web.Http.HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] int id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await UserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(HgRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Any())
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("GetByUserToken")]
        public async Task<IHttpActionResult> GetByTrackingToken([FromUri] string userToken)
        {
            var result = await UserManager.FindByGuidAsync(userToken);
            var model = Mapper.Map<UserModel>(result);
            return Ok(model);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetLoggedInUser")]
#pragma warning disable 1998
        public async Task<IHttpActionResult> GetUser()
        {
            using (HGContext context = new HGContext())
            {
                var user = context.Users.Include(u => u.PatientInfo).Include(u => u.Address)
                    .SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                var model = Mapper.Map<UserModel>(user.Result);
                return Ok(model);
            }
        }
#pragma warning restore 1998

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("GetByUserId")]
        public async Task<IHttpActionResult> GetByUserId([FromUri] int userId)
        {
            var result = await UserManager.FindByIdAsync(userId);
            var model = Mapper.Map<UserModel>(result);
            return Ok(model);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("SetTrackingToken")]
        public async Task<IHttpActionResult> SetTrackingToken([FromUri] string Guid)
        {
            // var u = HGContext.Users.Add(new User() {Guid = guid});
            // var s = await HGContext.SaveChangesAsync();
            var user = new User
            {
                Email = Guid + "@none.com",
                UserName = Guid,
                FirstName = "",
                LastName = "",
                Guid = Guid,
                PatientInfo = new PatientInfo(),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                LastLogin = DateTimeOffset.UtcNow
            };

            IdentityResult result = await this.UserManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> ForgotPassword(string email)
        {
            List<string> errors = new List<string>();


            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                errors.Add("We do not have this E-mail");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
            }

            var callbackUrl = new Uri(ConfigurationManager.AppSettings["webclientURL"] + "/resetpassword");

            await UserManager.RemovePasswordAsync(user.Id);
            Random random = new Random();
            var newPassword = random.Next(111111111, 999999999);
            await UserManager.AddPasswordAsync(user.Id, newPassword.ToString());
            try
            {
                await
                    SendVerificationEmail(email,
                        "Your new password is " + newPassword + " </br>" +
                        "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>",
                        "Reset password");
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch
            {
                errors.Add("An error has occured while sending E-mail, please try again.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordModel model)
        {
            List<string> errors = new List<string>();
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToList());
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                errors.Add("Don't reveal that the user does not exist");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
            }
            var result = await UserManager.ChangePasswordAsync(user.Id, model.SentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }

            errors.Add("An error has occured while changing password.");
            return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Authorize]
        [System.Web.Http.Route("UpdateUserDocuments")]
        public async Task<HttpResponseMessage> UpdateUserDocuments(PatientInfoModel model)
        {
            var user =
                        await
                            HGContext.Users.Include(u => u.PatientInfo)
                                .SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
            user.PatientInfo.DriversLicenseImageUrl = model.DriversLicenseImageUrl;
            user.PatientInfo.RecommendationImageUrl = model.RecommendationImageUrl;
            HGContext.Entry(user).State=EntityState.Modified;
            await HGContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Authorize]
        [System.Web.Http.Route("UpdateStateInfo")]
        public async Task<HttpResponseMessage> UpdateStateInfo(PatientInfoModel model)
        {
            List<string> errors = new List<string>();
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToList());
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
            try
            {
                using (HGContext context = new HGContext())
                {
                    var user =
                        await
                            context.Users.Include(u => u.PatientInfo)
                                .SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                    user.PatientInfo.MedicalCardNumber = model.MedicalCardNumber;
                    user.PatientInfo.MedicalCardExpirationDate = model.MedicalCardExpirationDate;
                    user.PatientInfo.ApprovalStatus = ApproalStatus.APPLIED;
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch
            {
                errors.Add("An error has occured while saving.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors.ToArray());
            }
        }




        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Authorize]
        [System.Web.Http.Route("UpdateAddressInfo")]
        public async Task<HttpResponseMessage> UpdateAddressInfo(AddressModel model)
        {
            try
            {
                using (HGContext context = new HGContext())
                {
                    var user =
                        await
                            context.Users.Include(u => u.Address)
                                .SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                    if (user.Address == null)
                    {
                        user.Address = new Address();
                    }
                    user.Address.Address1 = model.Address1;
                    user.Address.Address2 = model.Address2;
                    user.Address.City = model.City;
                    user.Address.State = model.State;
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.Authorize]
        //[System.Web.Http.Route("DeleteUser")]
        //public async Task<HttpResponseMessage> DeleteUser(User user)
        //{
        //    var result = await UserManager.DeleteAsync(user);
        //    if (result.Succeeded)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, user);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.BadRequest);
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}