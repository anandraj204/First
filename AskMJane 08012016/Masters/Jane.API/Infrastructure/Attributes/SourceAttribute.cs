using System.Web.Mvc;
using Jane.API.Infrastructure.Context;

namespace Jane.API.Infrastructure.Attributes
{
    public class SourceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SourceContext.Instance.SetUtmValuesCookie();
            base.OnActionExecuting(filterContext);
        }
    }
}