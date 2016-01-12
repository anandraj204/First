using System.Web.Mvc;

namespace Jane.Web.Areas.Dispensary
{
    public class DispensaryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dispensary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("dispensary_register", "dispensary/register", new { controller = "Dispensary", action = "RegisterDispensary" });
            context.MapRoute("dispensary_details", "dispensary/details/{id}", new { controller = "Dispensary", action = "Details", id = UrlParameter.Optional });
            context.MapRoute("dispensary_login", "dispensary/login", new { controller = "Dispensary", action = "Login" });
            context.MapRoute("dispensary_complete", "dispensary/complete", new { controller = "Dispensary", action = "CompleteData" });

            context.MapRoute(
/*                "dispensary_details",
                "{controller}/{action}/{id}",
                new { action = "Details", id = UrlParameter.Optional }
            );

            context.MapRoute(*/
                "Dispensary_default",
                "Dispensary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}