using System;
using System.Web;
using System.Web.Mvc;

namespace Jane.API.Infrastructure.Filters
{
    public class UserTokenFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var current = filterContext.RequestContext.HttpContext.Request.Cookies.Get("usertoken");
            if (current != null && !string.IsNullOrWhiteSpace(current.Value)) return;
            var cookie = new HttpCookie("usertoken") { Value = Guid.NewGuid().ToString(), Expires = DateTime.Now.AddDays(5) };
            filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie);
        }
    }
}