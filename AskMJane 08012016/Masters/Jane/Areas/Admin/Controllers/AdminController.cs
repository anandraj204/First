using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Jane.Core.Models;
using Jane.Core.Proxies;
using Jane.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Jane.Web.Areas.Admin.Controllers
{

    public class AdminController : BaseController
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["admin"] = true;
                return Redirect("/login");
            }
            Session["admin"] = null;
            ViewBag.CurrentPage = "admin";
            var userId = User.Identity.GetUserId<int>();
            // 
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "DispensaryAdmin"))
            {
                var user = UserManager.FindById(userId);
                return View(user);
            }
            return Redirect("/login");
        }
        [Authorize]
        // GET: Admin/Admin
        public async Task<ActionResult> Dispensaries()
        {
            ViewBag.CurrentPage = "Dispensaries";
            var userId = User.Identity.GetUserId<int>();
            // 
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "DispensaryAdmin"))
            {
                var api = new ApiProxy(Request);
                var response = await api.GetAsync("dispensaries");
                var dispensaries = new List<DispensaryModel>();
                if (response.IsSuccessStatusCode)
                {
                    dispensaries = JsonConvert.DeserializeObject<List<DispensaryModel>>
                        (response.Content.ReadAsStringAsync().Result);
                }

                return View(dispensaries);
            }

            return RedirectToAction("Index", "Home");

        }
        // GET: Admin/Admin
        [Authorize]
        public async Task<ActionResult> Inventory()
        {
            ViewBag.CurrentPage = "Inventory";
            var userId = User.Identity.GetUserId<int>();
            if (UserManager.IsInRole(userId, "GlobalAdmin"))
            {
                var api = new ApiProxy(Request);
                var response = await api.GetAsync("dispensaries");
                var dispensaries = new List<DispensaryModel>();
                if (response.IsSuccessStatusCode)
                {
                    dispensaries = JsonConvert.DeserializeObject<List<DispensaryModel>>
                        (response.Content.ReadAsStringAsync().Result);
                }

                return View(dispensaries);
            }

            return RedirectToAction("Index", "Home");
        }
#pragma warning disable 1998
        [Authorize]
        public async Task<ActionResult> MasterProduct()
        {
            ViewBag.CurrentPage = "MasterProduct";
            var userId = User.Identity.GetUserId<int>();
            if (UserManager.IsInRole(userId, "GlobalAdmin"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
#pragma warning restore 1998

        // GET: Admin/Admin
        [Authorize]
        public ActionResult Orders()
        {
            ViewBag.CurrentPage = "Orders";
            var userId = User.Identity.GetUserId<int>();
            // 
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "DispensaryAdmin"))
            {
                var user = UserManager.FindById(userId);
                return View(user);
            }

            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public ActionResult Deliveries()
        {
            ViewBag.CurrentPage = "Deliveries";
            var userId = User.Identity.GetUserId<int>();
            // 
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "DispensaryAdmin"))
            {
                var user = UserManager.FindById(userId);
                return View(user);
            }

            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        public ActionResult PatientApprovals()
        {
            ViewBag.CurrentPage = "PatientApprovals";
            var userId = User.Identity.GetUserId<int>();
            // 
            if (UserManager.IsInRole(userId, "GlobalAdmin"))
            {
                var user = UserManager.FindById(userId);
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult RolesManager()
        {
            ViewBag.CurrentPage = "RolesManager";
            var userId = User.Identity.GetUserId<int>();
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "Admin") || UserManager.IsInRole(userId, "Dispensary Manager"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult DispensaryDashboard()
        {
            ViewBag.CurrentPage = "DispensaryDashboard";
            var userId = User.Identity.GetUserId<int>();
            if (UserManager.IsInRole(userId, "GlobalAdmin") || UserManager.IsInRole(userId, "Admin") || UserManager.IsInRole(userId, "Dispensary Manager"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult PendingDispensaries()
        {
            ViewBag.CurrentPage = "PendingDispensaries";
            var userId = User.Identity.GetUserId<int>();

            if (UserManager.IsInRole(userId, "GlobalAdmin"))
            {
                var user = UserManager.FindById(userId);
                return View(user);
            }

            return RedirectToAction("Index", "Home");

        }
    }
}