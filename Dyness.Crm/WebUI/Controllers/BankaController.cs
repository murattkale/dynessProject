using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BankaController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public BankaController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(BankaDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.BankaSelectList = selectListHelper.BankaSelectList(false);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new BankaDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Banka>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IBankaService>();
                var model = service.Get(x => x.BankaId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Banka();
            }

            GetLists(viewModel, viewModel.Model.BankaId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(BankaDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IBankaService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.BankaId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.BankaId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.BankaId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.BankaId);

            return View(viewModel);
        }
    }
}