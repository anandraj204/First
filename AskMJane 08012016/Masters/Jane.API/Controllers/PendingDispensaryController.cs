using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/PendingDispensary")]
    public class PendingDispensaryController : BaseApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("AddPendingDispensary")]
        public async Task<HttpResponseMessage> AddPendingDispensary([FromBody] PendingDispensaryModel pendingDispensary)
        {
            bool addressFlag = false;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
                var addressKey = ModelState.Where(x => x.Value.Errors.Count > 0 && x.Key.Contains("AddressId")).Select(x => new { x.Key, x.Value.Errors }).FirstOrDefault();
                if (pendingDispensary.Id != 0)
                {
                    bool flag = false;
                    foreach (var error in errors)
                    {
                        if (!error.ToLower().Contains("password"))
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        UpadteDispensary(pendingDispensary);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                if (errors.Length <= 1 && addressKey != null)
                {
                    addressFlag = true;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }


            if (await HGContext.PendingDispensaries.AnyAsync(p => p.Email.ToLower() == pendingDispensary.Email.ToLower()) || await HGContext.Users.AnyAsync(p => p.Email.ToLower() == pendingDispensary.Email.ToLower())
                || await HGContext.Dispensaries.AnyAsync(p => p.EmailAddress.ToLower() == pendingDispensary.Email.ToLower()))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new[] { pendingDispensary.Email + " is already exist." });
            }

            if (addressFlag)
            {
                /*Add Data in Address Table*/

                var Address = new Address()
                {
                    Name = pendingDispensary.Address.Name,
                    Address1 = pendingDispensary.Address.Address1,
                    Address2 = pendingDispensary.Address.Address2,
                    City = pendingDispensary.Address.City,
                    State = pendingDispensary.Address.State,
                    Zip = pendingDispensary.Address.Zip,
                    Country = pendingDispensary.Address.Country,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                HGContext.Addresses.Add(Address);
                await HGContext.SaveChangesAsync();
                pendingDispensary.AddressId = Address.Id;
            }


            //TODO: Replace these line using automapper
            var entity = AutoMapModelToEntity(pendingDispensary);


            HGContext.PendingDispensaries.Add(entity);
            await HGContext.SaveChangesAsync();

            //await SendVerifyEmaiForSuperAdminAccounts(entity);
            string webUrl = ConfigurationManager.AppSettings["webclientURL"];
            await
                SendVerificationEmail(entity.Email,
                    "Your application has been submitted, please login <a href='" + webUrl + "/dispensary/login'>here</a> and complete your data to active your account", "Dispensary registeration");

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        private void UpadteDispensary(PendingDispensaryModel pendingDispensary)
        {
            var entity = HGContext.PendingDispensaries.SingleOrDefault(d => d.Id == pendingDispensary.Id);


            entity.Id = pendingDispensary.Id;
            entity.AddressId = pendingDispensary.AddressId;
            entity.Email = pendingDispensary.Email;
            entity.Name = pendingDispensary.Name;
            entity.PhoneNumber = pendingDispensary.PhoneNumber;
            entity.PendingDispensaryStatus = PendingDispensaryStatus.WaitingForApprove;
            entity.Type = pendingDispensary.Type;
            entity.Website = pendingDispensary.Website;
            HGContext.Entry(entity).State = EntityState.Modified;

            var address = HGContext.Addresses.SingleOrDefault(a => a.Id == pendingDispensary.Id);
            if (address != null)
            {
                address.Address1 = pendingDispensary.Address.Address1;
                address.Address2 = pendingDispensary.Address.Address2;
                address.City = pendingDispensary.Address.City;
                address.State = pendingDispensary.Address.State;
                address.Zip = pendingDispensary.Address.Zip;
                address.Country = pendingDispensary.Address.Country;
                HGContext.Entry(address).State = EntityState.Modified;
            }

            HGContext.SaveChanges();
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UpdatePendingDispensaryDocs")]
        public async Task<HttpResponseMessage> UpdatePendingDispensaryDocuments([FromBody] PendingDispensaryModel pendingDispensary)
        {
            var dispensary = await HGContext.PendingDispensaries.FindAsync(pendingDispensary.Id);
            dispensary.DriversLicenseImageUrl = pendingDispensary.DriversLicenseImageUrl;
            dispensary.RecommendationImageUrl = pendingDispensary.RecommendationImageUrl;
            dispensary.PendingDispensaryStatus = PendingDispensaryStatus.WaitingForApprove;
            HGContext.Entry(dispensary).State = EntityState.Modified;
            await HGContext.SaveChangesAsync();
            await SendVerifyEmaiForSuperAdminAccounts(dispensary);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("UpdatePendingDispensaryLicense")]
        public async Task<HttpResponseMessage> UpdatePendingDispensaryLicense([FromBody] PendingDispensaryModel pendingDispensary)
        {
            var dispensary = await HGContext.PendingDispensaries.FindAsync(pendingDispensary.Id);
            dispensary.IdNumber = pendingDispensary.IdNumber;
            dispensary.ExperationDate = pendingDispensary.ExperationDate;
            HGContext.Entry(dispensary).State = EntityState.Modified;
            await HGContext.SaveChangesAsync();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private PendingDispensary AutoMapModelToEntity(PendingDispensaryModel pendingDispensary)
        {
            var entity = new PendingDispensary()
            {
                Id = pendingDispensary.Id,
                AddressId = pendingDispensary.AddressId,
                Email = pendingDispensary.Email,
                Name = pendingDispensary.Name,
                PhoneNumber = pendingDispensary.PhoneNumber,
                PendingDispensaryStatus = PendingDispensaryStatus.Initilized,
                Type = pendingDispensary.Type,
                Website = pendingDispensary.Website,
                Password = UserManager.PasswordHasher.HashPassword(pendingDispensary.Password)
            };

            if (pendingDispensary.Address != null)
            {
                entity.Address = new Address()
                {
                    Address1 = pendingDispensary.Address.Address1,
                    Address2 = pendingDispensary.Address.Address2,
                    City = pendingDispensary.Address.City,
                    State = pendingDispensary.Address.State,
                    Zip = pendingDispensary.Address.Zip,
                    Country = pendingDispensary.Address.Country
                };
            }
            return entity;
        }

        private async Task<bool> SendVerifyEmaiForSuperAdminAccounts(PendingDispensary entity)
        {
            string body = "<b>" + entity.Name + "</b> is a new dispensary that has been registerd an waiting for the verification please check the pending list";
            var allUsers = HGContext.Users.AsNoTracking();
            List<string> globalAdminEmails = new List<string>();
            foreach (var user in allUsers)
            {
                if (await UserManager.IsInRoleAsync(user.Id, Role.GlobalAdmin))
                {
                    globalAdminEmails.Add(user.Email);
                }
            }

            await SendVerificationEmail(globalAdminEmails, body, "New dispensary");
            return true;
        }


        [System.Web.Http.Authorize(Roles = Role.GlobalAdmin)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetPendingDespensaries")]
        public async Task<HttpResponseMessage> GetPendingDespensaries()
        {
            var pendingDispensaries = await HGContext.PendingDispensaries.Include(d => d.Address).Where(d => d.PendingDispensaryStatus == PendingDispensaryStatus.WaitingForApprove).ToListAsync();
            return Request.CreateResponse(HttpStatusCode.OK, pendingDispensaries);
        }

        [System.Web.Http.Authorize(Roles = Role.DispensaryManager)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetPendingDispensary")]
        public async Task<HttpResponseMessage> GetPendingDispensary()
        {
            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            var pendingDispensary = await HGContext.PendingDispensaries.Include(d => d.Address).SingleOrDefaultAsync(d => d.Email.ToLower() == user.Email.ToLower());
            return Request.CreateResponse(HttpStatusCode.OK, pendingDispensary);
        }

        [System.Web.Http.Authorize(Roles = Role.GlobalAdmin)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Reject")]
        public async Task<HttpResponseMessage> Reject(int id)
        {
            var dispensary = await HGContext.PendingDispensaries.SingleOrDefaultAsync(d => d.Id == id);
            dispensary.PendingDispensaryStatus = PendingDispensaryStatus.Rejected;
            HGContext.Entry(dispensary).State = EntityState.Modified;
            await HGContext.SaveChangesAsync();
            await SendVerificationEmail(dispensary.Email, "Your requset has been rejected", "Dispensary rejected");
            return Request.CreateResponse(HttpStatusCode.OK, dispensary);
        }

        [System.Web.Http.Authorize(Roles = Role.GlobalAdmin)]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Approve")]
        public async Task<HttpResponseMessage> Approve(int id)
        {
            var dispensary = await HGContext.PendingDispensaries.Include(d => d.Address).SingleOrDefaultAsync(d => d.Id == id);
            dispensary.PendingDispensaryStatus = PendingDispensaryStatus.Approved;
            HGContext.Entry(dispensary).State = EntityState.Modified;

            var password = new Random().Next(100000, 999999);
            var guid = Guid.NewGuid().ToString();
            var idresult = await UserManager.CreateAsync(new User
            {
                Email = dispensary.Email,
                UserName = dispensary.Email,
                Guid = guid,
                Zipcode = dispensary.Address == null ? null : dispensary.Address.Zip,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                LastLogin = DateTimeOffset.UtcNow
            }, password.ToString());
            if (idresult.Succeeded)
            {
                var user = await UserManager.FindByEmailAsync(dispensary.Email);
                await UserManager.AddToRoleAsync(user.Id, Role.DispensaryManager);
                await HGContext.SaveChangesAsync();
                await SendVerificationEmail(dispensary.Email, "Your requset has been approved, your password for login is " + password, "Dispensary approved");
                return Request.CreateResponse(HttpStatusCode.OK, dispensary);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Login")]
        public async Task<HttpResponseMessage> Login(UserModel login)
        {
            var email = string.IsNullOrEmpty(login.Email) ? login.Username : login.Email;
            var dispensary =
                await
                    HGContext.PendingDispensaries.Include(d => d.Address).SingleOrDefaultAsync(
                        d =>
                            d.Email.ToLower() == email.ToLower() &&
                            d.PendingDispensaryStatus == PendingDispensaryStatus.WaitingForApprove);
            if (dispensary != null)
            {
                var result = UserManager.PasswordHasher.VerifyHashedPassword(dispensary.Password, login.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, dispensary);
                }
            }


            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "token";
            _logger.DebugFormat("{0} {1} {2} {3}", tokenServiceUrl, request.Url, request.Url.GetLeftPart(UriPartial.Authority), request.ApplicationPath);
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", login.Username ?? login.Email),
                new KeyValuePair<string, string>("password", login.Password)
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

                        if (!await UserManager.IsInRoleAsync(user.Id, Role.DispensaryManager))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new List<string> { "You seems to be a regular user, please login from regular login screen" });
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
                                                                                           x
                                                                                               .DispensaryProductVariantOrders
                                                                                               .Count > 0);
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

                        if (await UserManager.IsInRoleAsync(user.Id, Role.DispensaryManager))
                        {
                            if (
                                !await
                                    HGContext.Dispensaries.AnyAsync(
                                        d => d.EmailAddress.ToLower() == user.Email.ToLower()))
                            {
                                responseObj.Add("notCompleted", true);
                            }
                            else
                            {
                                responseObj.Add("notCompleted", false);
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Authorize(Roles = Role.DispensaryManager)]
        public HttpResponseMessage InsertDispensaryFromPending([FromBody]DispensaryModel dispensary)
        {
            try
            {
                var entity = Mapper.Map<Dispensary>(dispensary);
                entity.Slug = entity.LeaflySlug;
                var user = UserManager.FindByEmail(User.Identity.Name);
                entity.UserId = user.Id;
                var id = AddDispensary(dispensary, entity);
                return Request.CreateResponse(HGContext.Dispensaries.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception e)
            {
                _logger.Error("Dispensaries.Post", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        private int AddDispensary(DispensaryModel dispensary, Dispensary entity)
        {
            foreach (string zip in dispensary.ApprovalZipCodes.Split(','))
            {
                string trimmedZip = zip.Trim();
                if (!String.IsNullOrEmpty(trimmedZip))
                    entity.ApprovalZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
            }
            foreach (string zip in dispensary.DeliveryZipCodes.Split(','))
            {
                string trimmedZip = zip.Trim();
                if (!String.IsNullOrEmpty(trimmedZip))
                    entity.DeliveryZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
            }
            HGContext.Dispensaries.Add(entity);
            var id = HGContext.SaveChanges();
            return id;
        }

    }
}