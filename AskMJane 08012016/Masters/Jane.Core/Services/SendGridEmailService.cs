using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using SendGrid;

namespace Jane.Core.Services
{
    public class SendGridEmailService
    {
        public async Task SendEmailAsync( string destination, string subject, string body )
        {
            var myMessage = new SendGridMessage();

            myMessage.AddTo(destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                ConfigurationManager.AppSettings["emailService:FromAddress"],
                ConfigurationManager.AppSettings["emailService:FromName"]);
            myMessage.Subject = subject;
            myMessage.Text = body;
            myMessage.Html = body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
                ConfigurationManager.AppSettings["emailService:Password"]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
    
}
