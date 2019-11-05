using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;

namespace WebUI.Controllers
{
    public class PuanTurController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public PuanTurController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(PuanTurDuzenleViewModel viewModel)
        {
            viewModel.PuanTurSelectList = selectListHelper.PuanTurSelectList(false);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new PuanTurDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<PuanTur>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IPuanTurService>();
                var model = service.Get(x => x.PuanTurId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new PuanTur();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(PuanTurDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPuanTurService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.PuanTurId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.PuanTurId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.PuanTurId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPuanTurService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }
    }
}