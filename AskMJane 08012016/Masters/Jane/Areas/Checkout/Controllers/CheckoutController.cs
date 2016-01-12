using System.Threading.Tasks;
using System.Web.Mvc;
using Jane.Data.EntityFramework.Entities;
using Jane.Web.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Jane.Web.Areas.Checkout.Controllers
{
    public class CheckoutController : BaseController
    {
#pragma warning disable 1998
        public async Task<ActionResult> Cart()
        {
            if (!Request.IsAuthenticated)
            {
                return Redirect("/login?redirectTo=cart");
            }
            var userId = User.Identity.GetUserId<int>();
            var user = UserManager.FindById(userId);

            //make all users able to see his own cart
            //if (user.PatientInfo.ApprovalStatus == ApproalStatus.NOTAPPLIED || user.PatientInfo.ApprovalStatus == ApproalStatus.REJECTED)
            //{
            //    return Redirect("/patient/applyv2");
            //}
            return View();
        }

        public async Task<ActionResult> Checkout()
        {
            if (!Request.IsAuthenticated)
            {
                return Redirect("/login?redirectTo=checkout");
            }
            var userId = User.Identity.GetUserId<int>();
            var user = UserManager.FindById(userId);

            if (user.PatientInfo.ApprovalStatus == ApproalStatus.NOTAPPLIED || user.PatientInfo.ApprovalStatus == ApproalStatus.REJECTED)
            {
                return Redirect("/patient/applyv2");
            }
            return View();
        }
#pragma warning restore 1998
    }
}
