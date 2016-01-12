using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jane.Core.Logging;

namespace Jane.Core.Proxies
{
    public class ApiProxy
    {
        private const string BaseAddress = "";
        public static string ApiAddress
        {
            get
            {
                var addr = ConfigurationManager.AppSettings["BaseApiAddress"];
                if (string.IsNullOrEmpty(addr))
                {
                    addr = BaseAddress;
                }

                //get rid of trailing "/" and add prefix "/" for all api calls
                return addr.EndsWith("/") ? addr.Remove(addr.Length - 1) : addr;
            }
        }
        private const string AcceptHeader = "application/json";
        private readonly ILogger _logger = null;
        private System.Web.HttpRequestBase Request;
        private readonly string _authToken = "";

        public ApiProxy()
        {
            _logger = new Logger();
        }

        public ApiProxy(System.Web.HttpRequestBase Request)
        {
            if (Request.Cookies["access_token"] != null)
            {
                _authToken = "Bearer " + Request.Cookies["access_token"].Value;
            }
            this.Request = Request;
        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiAddress);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _authToken);
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await client.GetAsync(path, cancellationTokenSource.Token);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiAddress);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _authToken);
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await client.PostAsync(path, null,cancellationTokenSource.Token);
            return response;
        }

        public HttpResponseMessage Post(string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiAddress);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _authToken);
            var cancellationTokenSource = new CancellationTokenSource();
            var response = client.PostAsync(path, null).Result;
            return response;
        }

      
    }
}