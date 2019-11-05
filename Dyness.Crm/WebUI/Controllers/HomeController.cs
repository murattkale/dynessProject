using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        [CheckAuthorizedActionFilter]
        public ActionResult Index()
        {
           

            return View();

        }


        private IServiceFactory serviceFactory;

        public HomeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }
    }
}