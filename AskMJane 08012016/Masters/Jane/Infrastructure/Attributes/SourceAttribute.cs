using System.Web.Mvc;
using Jane.Web.Infrastructure.Context;

namespace Jane.Web.Infrastructure.Attributes
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