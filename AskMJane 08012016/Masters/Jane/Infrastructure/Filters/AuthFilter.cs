using System;
using System.Web.Mvc;
using log4net;

namespace Jane.Web.Infrastructure.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        private static ILog _log = LogManager.GetLogger(typeof(AuthFilter));
   
        public  void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Request.Cookies["access_token"] != null &&
                    !String.IsNullOrEmpty(filterContext.HttpContext.Request.Cookies["access_token"].Value))
                {
                    filterContext.HttpContext.Request.Headers["Authorization"]= "Bearer " +
                                                                                 filterContext.HttpContext.Request
                                                                                     .Cookies["access_token"].Value;
                }

               // base.OnActionExecuting(filterContext);
            }
            catch
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
           // base.OnAuthorization(filterContext);
        }
    }
}