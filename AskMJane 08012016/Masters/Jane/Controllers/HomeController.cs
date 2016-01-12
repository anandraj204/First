using System.Web.Mvc;
using Jane.Web.Infrastructure;

namespace Jane.Web.Controllers
{
    //[RequireHttps]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //if (Request.IsAuthenticated)
            //{
            //    return Redirect("/menu");
            //}
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("privacy")]
        public ActionResult Privacy()
        {
            ViewBag.Message = "Privacy Page";
            return View();
        }

        [Route("terms")]
        public ActionResult Terms()
        {
            ViewBag.Message = "Terms of use Page";
            return View();
        }
    }
}