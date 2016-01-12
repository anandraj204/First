using System.Web.Mvc;
using Jane.Web.Infrastructure.Attributes;
using Jane.Web.Infrastructure.Filters;

namespace Jane.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new HandleErrorAttribute());
            filters.Add(new UserTokenFilter());
            filters.Add(new HttpHandleErrorAttribute());
            filters.Add(new SourceAttribute());
           // filters.Add(new AuthFilter());
        }
    }
}
