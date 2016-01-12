using System.Web.Mvc;

namespace Jane.Web.Areas.Menu
{
    public class MenuAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Menu";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("menu","menu",new { controller = "Menu", action = "Index" });
            context.MapRoute(
                "Menu_default",
                "Menu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}