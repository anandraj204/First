using System.Threading.Tasks;
using System.Web.Mvc;
using Jane.Data.EntityFramework.Entities;
using Jane.Web.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Jane.Web.Areas.Patient.Controllers
{
    public class PatientController : BaseController
    {
#pragma warning disable 1998
        public async Task<ActionResult> Apply()
        {
            return View();
        }
        public async Task<ActionResult> ApplyV2()
        {
            if (!Request.IsAuthenticated)
            {
                return Redirect("/login?redirectTo=patient/applyv2");
            }
            var userId = User.Identity.GetUserId<int>();
            var user = UserManager.FindById(userId);
            if (user.PatientInfo.ApprovalStatus == ApproalStatus.ACCEPTED)
            {
                return Redirect("/cart");
            }
            return View();
        }
#pragma warning restore 1998
    }
}
