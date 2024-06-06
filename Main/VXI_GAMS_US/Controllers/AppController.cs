using System.Web.Mvc;

namespace VXI_GAMS_US.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}