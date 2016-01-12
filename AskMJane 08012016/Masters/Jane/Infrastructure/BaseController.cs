using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Jane.Core.Logging;
using Jane.Data.EntityFramework.Managers;
using Microsoft.AspNet.Identity.Owin;

namespace Jane.Web.Infrastructure
{
    public abstract class BaseController : Controller
    {
        private HgUserManager _userManager;
        protected Logger Logger;

        public HgUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HgUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        public BaseController()
        {
            Logger = new Logger();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ViewBag.ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];
            base.Initialize(requestContext);

        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    string actionName = filterContext.ActionDescriptor.ActionName;
        //    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    if (actionName == "Index" && controllerName == "Admin" && !User.Identity.IsAuthenticated)
        //    {
        //        Session["admin"] = true;
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        public new RedirectToRouteResult RedirectToAction(string action, string controller)
        {
            return base.RedirectToAction(action, controller);
        }
    }
}