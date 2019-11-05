using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;

namespace WebUI.Controllers
{
    public class SinifSeviyeController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SinifSeviyeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SinifSeviyeDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.SinifSeviyeSelectList = selectListHelper.SinifSeviyeSelectList(false);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SinifSeviyeDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SinifSeviye>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinifSeviyeService>();
                var model = service.Get(x => x.SinifSeviyeId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new SinifSeviye();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SinifSeviyeDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinifSeviyeService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SinifSeviyeId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SinifSeviyeId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SinifSeviyeId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.SinifSeviyeId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinifSeviyeService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }
    }
}