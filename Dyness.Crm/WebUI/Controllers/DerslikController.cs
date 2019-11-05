using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class DerslikController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public DerslikController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(DerslikDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.DerslikSelectList = selectListHelper.DerslikSelectList(false, new int[] { selectedItemId });
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new DerslikDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Derslik>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IDerslikService>();
                var model = service.Get(x => x.DerslikId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Derslik();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(DerslikDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IDerslikService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.DerslikId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.DerslikId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.DerslikId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.DerslikId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IDerslikService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }
    }
}