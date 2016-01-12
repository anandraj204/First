using System.Threading.Tasks;
using System.Web.Mvc;
using Jane.Web.Infrastructure;

namespace Jane.Web.Areas.Menu.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu/Menu
#pragma warning disable 1998
        public async Task<ActionResult> Index()
        {
            return View();
        }
#pragma warning restore 1998
    }
}