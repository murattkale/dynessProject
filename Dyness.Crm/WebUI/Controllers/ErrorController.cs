using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult YetkisizIslem()
        {
            return View();
        }

        public ActionResult Hata()
        {
            return View();
        }
    }
}