using System.Web.Mvc;

namespace Jane.Web.Areas.Checkout
{
    public class CheckoutAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Checkout";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("checkout", "checkout", new { controller = "Checkout", action = "Checkout" });
            context.MapRoute("cart", "cart", new { controller = "Checkout", action = "Cart" });
            context.MapRoute(
                "Checkout_default",
                "Checkout/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}