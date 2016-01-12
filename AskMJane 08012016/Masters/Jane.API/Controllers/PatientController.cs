using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Core.Services;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Twilio;
using Address = Jane.Data.EntityFramework.Entities.Address;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Patient")]
    public class PatientController : BaseApiController
    {
        [HttpPost]
        [Route("SubmitApplication")]
        [Authorize]
        public async Task<IHttpActionResult> SubmitApplication([FromBody] UserModel user)
        {
            var uid = HttpContext.Current.User.Identity.GetUserId<int>();
            if (uid == user.Id || HttpContext.Current.User.IsInRole("GlobalAdmin"))
            {

                var entity = await HGContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                if (!string.IsNullOrWhiteSpace(user.FirstName))
                {
                    entity.FirstName = user.FirstName;
                }
                if (!string.IsNullOrWhiteSpace(user.LastName))
                {
                    entity.LastName = user.LastName;
                }
                if (!string.IsNullOrWhiteSpace(user.Zipcode))
                {
                    entity.Zipcode = user.Zipcode;
                }
                if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    entity.PhoneNumber = user.PhoneNumber;
                }
                entity.Birthday = user.Birthday;

                var result = await HGContext.SaveChangesAsync();

                if (ValidatePatientApplication(entity) && entity.PatientInfo.ApprovalStatus != ApproalStatus.APPLIED
                    && entity.PatientInfo.ApprovalStatus != ApproalStatus.ACCEPTED)
                {
                    entity.PatientInfo.ApprovalStatus = ApproalStatus.APPLIED;
                    var applicationresult = await HGContext.SaveChangesAsync();
                    //Send email to user and to AskMJane to process patient application
                    await SendEmailConfirmationToPatient(entity);
                    await SendEmailConfirmationToAskMJane(entity);
                    return Ok();
                }
                else
                {
                    return BadRequest("Patient Application was not completed.");
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("GetNonPatients")]
        [Authorize]
#pragma warning disable 1998
        public async Task<HttpResponseMessage> GetNonPatients()
        {
            var entities = HGContext.Users.Include("PatientInfo").Include("Orders").Where(x => x.PatientInfo.ApprovalStatus != ApproalStatus.OTHER);
            List<UserModel> mapped = new List<UserModel>();

            foreach (var user in entities.ToList())
            {
                var userModel = Mapper.Map<UserModel>(user);

                if (user.Orders.Any())
                {
                    var dispensaryModel = userModel.Orders.SelectMany(g => g.DispensaryProductVariantOrders
                            .Select(k => k.DispensaryProductVariant
                            .DispensaryProduct.Dispensary))
                            .GroupBy(g => g).Select(k => new
                            {
                                dispensary = k.Key,
                                count = k.Count()
                            }
                      ).OrderByDescending(cs => cs.count).FirstOrDefault();

                    userModel.Dispensary = dispensaryModel.dispensary;
                }

                userModel.Orders = null;
                mapped.Add(userModel);
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, mapped);
        }
#pragma warning restore 1998

        [HttpPost]
        [Route("SetApproval")]
        [Authorize]
        public async Task<IHttpActionResult> SetApproval([FromBody]UserModel user)
        {
            var uid = HttpContext.Current.User.Identity.GetUserId<int>();
            Dictionary<string, string> replaceList = new Dictionary<string, string>();
            if (HttpContext.Current.User.IsInRole("GlobalAdmin"))
            {
                var entity = await HGContext.Users.Include("PatientInfo").FirstOrDefaultAsync(x => x.Id == user.Id);
                entity.FirstName = user.FirstName;
                entity.LastName = user.LastName;
                entity.Birthday = user.Birthday;
                if (entity.Address == null)
                {
                    entity.Address = new Address();
                }
                entity.Address.Address1 = user.Address.Address1;
                entity.Address.Address2 = user.Address.Address2;
                entity.Address.City = user.Address.City;
                entity.Address.State = user.Address.State;
                entity.Address.Zip = user.Address.Zip;
                entity.Address.Country = "US";
                entity.Address.CreatedAt = DateTimeOffset.UtcNow;
                entity.Address.UpdatedAt = DateTimeOffset.UtcNow;
                entity.PatientInfo.DriversLicenseNumber = user.PatientInfo.DriversLicenseNumber;
                entity.PatientInfo.MedicalCardExpirationDate = user.PatientInfo.MedicalCardExpirationDate;
                entity.PatientInfo.MedicalCardNumber = user.PatientInfo.MedicalCardNumber;
                replaceList.Add("UserName", user.FirstName);
                replaceList.Add("LastName", user.LastName);

                if (user.PatientInfo.ApprovalStatus == ApproalStatusModel.ACCEPTED)
                {
                    entity.PatientInfo.ApprovalStatus = ApproalStatus.ACCEPTED;
                    entity.PatientInfo.IsApproved = true;
                    entity.PatientInfo.MedicalCardValidationDate = DateTime.UtcNow;
                    replaceList.Add("Status", "APPROVED");
                    await SendVerificationEmail(user.Email, GetEmailTemplate(replaceList, "RegistrationStatusMail"), "Jane Registration Status");
                }
                if (user.PatientInfo.ApprovalStatus == ApproalStatusModel.REJECTED)
                {
                    entity.PatientInfo.ApprovalStatus = ApproalStatus.REJECTED;
                    replaceList.Add("Status", "DISAPPROVED");
                    await SendVerificationEmail(user.Email, GetEmailTemplate(replaceList, "RegistrationStatusMail"), "Jane Registration Status");
                }
                var result = await HGContext.SaveChangesAsync();
                if (result > 0)
                {

                    return Ok();
                }

                else
                {
                    return BadRequest();
                }
            }
            return Unauthorized();
        }



        [HttpPost]
        [Route("ConfirmPhoneNumber")]
        [Authorize]
        public async Task<IHttpActionResult> ConfirmPhoneNumber([FromUri] int id, [FromUri]string phonenumber)
        {
            var uid = HttpContext.Current.User.Identity.GetUserId<int>();
            if (uid == id || HttpContext.Current.User.IsInRole("GlobalAdmin"))
            {
                var entity = await HGContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                entity.PhoneNumber = phonenumber;
                entity.PhoneNumberConfirmed = true;
                var result = await HGContext.SaveChangesAsync();
                if (result > 0)
                {

                    return Ok();
                }

                else
                {
                    return BadRequest();
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("GetMobileVerificationCode")]
        [Authorize]
        public HttpResponseMessage GetMobileVerificationCode([FromUri]string phonenumber)
        {
            string AccountSid = "AC2e5f7d94e91c4fbbd699af50f4e9735f";
            string AuthToken = "df12f5becb880e57e918397461e010d3";

            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            Random r = new Random();
            var code = r.Next(1000, 9999);
            var message = twilio.SendMessage("+13158779594", "+1" + phonenumber, "@AskMJane: Your verification code is " + code, "");
            _logger.DebugFormat("Mobile verification code {0} sent to {1}", code, phonenumber);
            return Request.CreateResponse(HttpStatusCode.Accepted, code);
        }

        [HttpPost]
        [System.Web.Http.Route("SetIdUrl")]
        public async Task<IHttpActionResult> SetIdUrl(SetPathBindingModel model)
        {
            var entity = await HGContext.PatientInfos.FirstOrDefaultAsync(x => x.Id == model.Id);
            entity.DriversLicenseImageUrl = model.Path;
            var response = await HGContext.SaveChangesAsync();
            if (response > 0)
            {
                return Ok();
            }
            else
            {
                _logger.ErrorFormat("Error while updating PatientInfo Id: {0} PhotoIdUrl: {1}", model.Id, model.Path);
                return BadRequest();
            }
        }
        [HttpPost]
        [System.Web.Http.Route("SetRxUrl")]
        public async Task<IHttpActionResult> SetRxUrl(SetPathBindingModel model)
        {
            try
            {
                var entity = await HGContext.PatientInfos.FirstOrDefaultAsync(x => x.Id == model.Id);
                entity.RecommendationImageUrl = model.Path;
                var response = await HGContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                _logger.ErrorFormat("Error while updating PatientInfo Id: {0} RxUrl: {1}", model.Id, model.Path);
                return BadRequest();
            }
        }

        private async Task<bool> SendEmailConfirmationToAskMJane(User user)
        {
            var emailService = new SendGridEmailService();
            var subject = "AskMJane Patient Application Confirmation";
            var body =
                String.Format(
                    "Hi {0},  We've received your patient application and will contact you shortly letting you know when our review process has been completed." +
                    "Thanks! The AskMJane Team", user.FirstName);
            var to = user.Email;
            await emailService.SendEmailAsync(to, subject, body);
            return true;
        }

        private async Task<bool> SendEmailConfirmationToPatient(User user)
        {
            var emailService = new SendGridEmailService();
            var subject = "AskMJane Patient Application Review";
            var body =
                String.Format(
                    "UserId : {0} {1} {2} {3}  Needs review", user.Id, user.FirstName, user.LastName, user.Email);
            var to = "mike@askmjane.com";
            await emailService.SendEmailAsync(to, subject, body);
            return true;
        }

        private bool ValidatePatientApplication(User user)
        {

            DateTime startDate = new DateTime(1900, 1, 1, 0, 0, 0);
            var today = DateTime.Today;
            DateTime endDate = new DateTime(today.Year - 18, today.Month, today.Day, 0, 0, 0);

            if (!string.IsNullOrWhiteSpace(user.Zipcode) && !string.IsNullOrWhiteSpace(user.PhoneNumber)
                && user.PhoneNumberConfirmed && !string.IsNullOrWhiteSpace(user.PatientInfo.DriversLicenseImageUrl) &&
                !string.IsNullOrWhiteSpace(user.PatientInfo.RecommendationImageUrl))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public class SetPathBindingModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
