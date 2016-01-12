using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using Jane.Core.Logging;
using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Managers;
using MailChimp;
using MailChimp.Types;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Text;
using System.Web;

namespace Jane.API.Infrastructure.Common
{

    public abstract class BaseApiController : ApiController
    {
        public ILogger _logger;
        public HGContext _HgContext;
        private ModelFactory _modelFactory;
        private HgUserManager _hgUserManager = null;
        private HgRoleManager _hgRoleManager = null;

        public BaseApiController()
        {
            _logger = new Logger();
        }

        protected HgRoleManager HgRoleManager
        {
            get
            {
                return _hgRoleManager ?? Request.GetOwinContext().GetUserManager<HgRoleManager>();
            }
            set
            {
                _hgRoleManager = value;
            }
        }
        protected HgUserManager UserManager
        {
            get
            {
                return _hgUserManager ?? Request.GetOwinContext().GetUserManager<HgUserManager>();
            }
            set
            {
                _hgUserManager = value;
            }
        }

        public HGContext HGContext { get { return _HgContext ?? Request.GetOwinContext().Get<HGContext>(); } }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.UserManager);
                }
                return _modelFactory;
            }
        }
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest("Unknown state - Bad Request");
                }

                return BadRequest(ModelState);
            }
            return null;
        }

        protected string[] GetModelErrors(IdentityResult result)
        {
            var errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                .Select(m => m.ErrorMessage).ToArray();
            return errors;

        }

        protected async Task<bool> SendVerificationEmail(List<string> toMails, string body, string subject)
        {
            string email = ConfigurationManager.AppSettings["emailService:FromAddress"];
            string password = ConfigurationManager.AppSettings["emailService:Password"];
            var loginInfo = new NetworkCredential(email, password);

            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email, "Askmjane");

            foreach (var toMail in toMails)
            {
                msg.To.Add(new MailAddress(toMail));
            }


            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            await smtpClient.SendMailAsync(msg);
            return true;
        }

        protected async Task<bool> SendVerificationEmail(string to, string body, string subject)
        {
            string email = ConfigurationManager.AppSettings["emailService:FromAddress"];
            string password = ConfigurationManager.AppSettings["emailService:Password"];
            var loginInfo = new NetworkCredential(email, password);

            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email, "Askmjane");

            msg.To.Add(new MailAddress(to));

            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            await smtpClient.SendMailAsync(msg);
            return true;
        }


        protected  void AddEmailToMailChimp(string email, string firstName, string lastName, string listId)
        {
            string apiKey = ConfigurationManager.AppSettings["MailChimpApiKey"];
            

            var options = new List.SubscribeOptions();
            options.DoubleOptIn = true;
            options.EmailType = List.EmailType.Html;
            options.SendWelcome = true;

            var mergeText = new List.Merges(email, List.EmailType.Text)
                    {
                        {"FNAME", firstName},
                        {"LNAME", lastName}
                    };
            var merges = new List<List.Merges> { mergeText };

            var mcApi = new MCApi(apiKey, false);
            var batchSubscribe = mcApi.ListBatchSubscribe(listId, merges, options);

            if (batchSubscribe.Errors.Count > 0)
            {
                _logger.Error(batchSubscribe.Errors[0].Message, null);
            }
        }

        public string GetEmailTemplate(Dictionary<string, string> replaceList, string templateFile)
        {
            StringBuilder emailTemplate = new StringBuilder();
            string filePath = HttpContext.Current.Server.MapPath("~/" + templateFile + ".html");

            if (System.IO.File.Exists(filePath))
            {
                emailTemplate = new StringBuilder(System.IO.File.ReadAllText(filePath));
                foreach (var item in replaceList)
                    emailTemplate.Replace("<%" + item.Key + "%>", HttpContext.Current.Server.HtmlEncode(item.Value));

                return emailTemplate.ToString();
            }
            return string.Empty;
        }
    }


}
