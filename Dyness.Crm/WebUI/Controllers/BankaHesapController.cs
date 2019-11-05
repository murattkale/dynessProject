using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BankaHesapController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public BankaHesapController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(BankaHesapDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.BankaHesapSelectList = selectListHelper.BankaHesapSelectList(false);
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();
            viewModel.BankaSelectList = selectListHelper.BankaSelectList();
            viewModel.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new BankaHesapDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<BankaHesap>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IBankaHesapService>();
                var model = service.Get(x => x.BankaHesapId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new BankaHesap();
            }

            GetLists(viewModel, viewModel.Model.BankaHesapId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(BankaHesapDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IBankaHesapService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.BankaHesapId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.BankaHesapId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.BankaHesapId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.BankaHesapId);

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new BankaHesapListeleViewModel
            {
                Model = new BankaHesap()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(BankaHesapListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IBankaHesapService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }
    }
}