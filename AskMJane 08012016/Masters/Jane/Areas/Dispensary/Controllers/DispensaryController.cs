using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Entities;
using Jane.Web.Infrastructure;

namespace Jane.Web.Areas.Dispensary.Controllers
{
    public class DispensaryController : BaseController
    {

        public ActionResult RegisterDispensary()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        public ViewResult Details([FromUri] [Required] int id)
        {
            //DispensaryModel model;
            using (var context = new HGContext())
            {
                DispensaryModel disp = context.Dispensaries
                    .AsNoTracking().Where(d => !d.IsDeleted && d.Id == id).Select(d => new DispensaryModel()
                    {
                        Address = new AddressModel() { FormattedAddress = d.Address.FormattedAddress },
                        Description = d.Description,
                        EmailAddress = d.EmailAddress,
                        Name = d.Name,
                        PhoneNumber = d.PhoneNumber,
                        Type = d.Type
                    })
                    .FirstOrDefault();


                return View(disp);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [System.Web.Http.Authorize(Roles = Role.DispensaryManager)]
        public ActionResult CompleteData()
        {
            using (HGContext context = new HGContext())
            {
                if (context.Dispensaries.Any(d => d.EmailAddress.ToLower() == User.Identity.Name))
                {
                    return Redirect("/");
                }
            }
            return View();
        }
    }
}