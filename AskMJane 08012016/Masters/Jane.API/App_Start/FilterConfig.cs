using System.Web.Mvc;
using Jane.API.Infrastructure.Attributes;

namespace Jane.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SourceAttribute());
        }
    }
}
