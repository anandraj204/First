using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Jane.Data.EntityFramework.Contexts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jane.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ServicePointManager.DefaultConnectionLimit = 500;
            ServicePointManager.MaxServicePointIdleTime = 2500;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = false;
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            FormatterConfig.RegisterFormatters(GlobalConfiguration.Configuration.Formatters);
        
            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return (true);
            };

            Database.SetInitializer<HGContext>(null);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
