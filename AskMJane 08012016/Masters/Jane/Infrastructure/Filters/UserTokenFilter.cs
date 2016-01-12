using System;
using System.Web;
using System.Web.Mvc;
using Jane.Core.Logging;
using Jane.Core.Proxies;

namespace Jane.Web.Infrastructure.Filters
{
    public class UserTokenFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var current = filterContext.RequestContext.HttpContext.Request.Cookies.Get("usertoken");
            if (current != null && !string.IsNullOrWhiteSpace(current.Value))
            {
                return;
            }
            else
            {
                var guid = Guid.NewGuid();
                ApiProxy p = new ApiProxy();
                var response = p.Post("api/account/settrackingtoken?Guid=" + guid.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var cookie = new HttpCookie("usertoken") {Value = guid.ToString()};
                    filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    var logger = new Logger();
                    logger.Error("Error While Saving New Anon User",null);
                }
            }
        }
    }
}