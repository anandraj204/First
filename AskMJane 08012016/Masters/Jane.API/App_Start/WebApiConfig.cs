using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Jane.API.Infrastructure.Attributes;
using Jane.Core.Logging;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Jane.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Global Exception Logging
            config.Filters.Add(new LogExceptionFilterAttribute());
            config.Services.Add(typeof(IExceptionLogger), new Logger());

            config.Routes.MapHttpRoute(
               name: "CurrentUser",
               routeTemplate: "api/currentUser",
               defaults: new { controller = "Users", action = "CurrentUser", id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
