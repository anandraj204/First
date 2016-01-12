using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Jane.Core.Logging;
using Jane.Core.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Jane.Core.Services
{
    public class OnfleetService
    {
        private RestClient _client;
        private readonly Logger _logger;

        public OnfleetService()
        {
            _logger = new Logger();
            _client = new RestClient("https://onfleet.com/api/v2");
            _client.Authenticator = new HttpBasicAuthenticator(ConfigurationManager.AppSettings["OnfleetApiKey"],"");
        }

        public OnfleetRecipient CreateRecipient(string name, string phone, string notes, bool skipSMSNotifications,
            bool skipPhoneNumberValidation)
        {
            var recipient = new OnfleetRecipient()
            {
                name = name,
                phone = "+1" + phone,
                notes = notes,
                skipPhoneNumberValidation = skipPhoneNumberValidation,
                skipSMSNotifications = skipSMSNotifications
            };
            var request = new RestRequest("recipients", Method.POST);
            request.JsonSerializer.ContentType = "application/json; charset=utf-8";
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(recipient); 

            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetRecipient>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Create Recipient  : {0}", response.ErrorMessage);
                return null;
            }
        }
        public OnfleetOrganization GetOnfleetOrganization()
        {
            var request = new RestRequest("organization", Method.GET);
            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetOrganization>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Organization : {0}", response.ErrorMessage);
                return null;
            }
        }

        public OnfleetRecipient FindRecipientByName(string name)
        {
            var request = new RestRequest("recipients/name/" + name , Method.GET);
            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetRecipient>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Find Recipient By Name : {0}", response.ErrorMessage);
                return null;
            }
        }
        public OnfleetRecipient FindRecipientByPhone(string phone)
        {
            var request = new RestRequest("recipients/phone/" + phone, Method.GET);
            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetRecipient>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Find Recipient By Phone : {0}", response.ErrorMessage);
                return null;
            }
        }

        public OnfleetTask CreateTask(OnfleetOrganization organization, OnfleetDestination destination, string recipient, string notes, bool pickupTask, string[] dependencies,
            bool autoAssign)
        {
            var task = new OnfleetCreateTask()
            {
                organization = organization.id,
                destination = destination,
                recipients = new List<string>(),
                notes = notes,
                pickupTask = pickupTask,
                dependencies = dependencies,
                merchant = organization.id,
                executor = organization.id
            };
            task.recipients.Add(recipient);
            var request = new RestRequest("tasks", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(task);

            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetTask>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Create task failed : {0}", response.ErrorMessage);
                return null;
            }
        }
        public OnfleetDestination CreateDestination(AddressModel address)
        {
            var addr = address.Address1.Split(' ');
            var num = addr[0];
            var street = "";
            for (int i = 0; i < addr.Length; i++)
            {
                if (i != 0)
                {
                    street = street + " " +  addr[i];
                }
            }
            var destination = new OnfleetDestination()
            {
                address = new OnfleetAddress()
                {
                    name="",
                    number = num,
                    street=street,
                    apartment = address.Address2,
                    unparsed=address.FormattedAddress,
                    city=address.City,
                    state =address.State,
                    postalCode = address.Zip,
                    country="USA"
                },
               // location = new double[]{(double)address.Longitude,(double)address.Latitude}
            };
            var request = new RestRequest("destinations", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(destination);

            var response = _client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OnfleetDestination>(response.Content);
            }
            else
            {
                _logger.ErrorFormat("Onfleet Create destination failed : {0}", response.ErrorMessage);
                return null;
            }
        }


      
    }

    public class OnfleetCreateTask
    {

        public string id { get; set; }
        public string organization { get; set; }
        public string shortId { get; set; }
        public string merchant { get; set; }
        public string executor { get; set; }
        public string creator { get; set; }
        public OnfleetDestination destination { get; set; }
        public List<string> recipients { get; set; }
        public OnfleetTaskState state { get; set; }
        public string[] dependencies { get; set; }
        public bool pickupTask { get; set; }
        public string notes { get; set; }
        public string trackingURL { get; set; }
    }
    public class OnfleetTask
    {

        public string id { get; set; }
        public string organization { get; set; }
        public string shortId { get; set; }
        public string merchant { get; set; }
        public string executor { get; set; }
        public string creator { get; set; }
        public OnfleetDestination destination { get; set; }
        public List<OnfleetRecipient> recipients { get; set; }
        public OnfleetTaskState state { get; set; }
        public string[] dependencies { get; set; }
        public bool pickupTask { get; set; }
        public string notes { get; set; }
        public string trackingURL { get; set; }
    }
    public enum OnfleetTaskState
    {
        UNASSIGNED = 0,
        ASSIGNED = 1,
        ACTIVE = 2,
        COMPLETED = 3
    }

    public class OnfleetOrganization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string country { get; set; }

    }

    public class OnfleetDestination
    {
        public string id { get; set; }
        public string[] tasks { get; set; }

        public OnfleetAddress address { get; set; }
        public double[] location { get; set; } 
        public string notes { get; set; }
    }

    public class OnfleetAddress
    {
        public string name { get; set; }
        public string number { get; set; }
        public string street { get; set; }
        public string apartment { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string unparsed { get; set; }
    }
    public class OnfleetRecipient
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string notes { get; set; }
        public bool skipSMSNotifications { get; set; }
        public bool skipPhoneNumberValidation { get; set; }

    }
}
