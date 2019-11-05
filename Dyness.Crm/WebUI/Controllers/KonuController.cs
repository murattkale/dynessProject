using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class KonuController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public KonuController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(KonuDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.KonuSelectList = selectListHelper.KonuSelectList(false);
            viewModel.DersSelectList = selectListHelper.DersSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new KonuDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Konu>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IKonuService>();
                var model = service.Get(x => x.KonuId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Konu();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(KonuDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IKonuService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.KonuId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.KonuId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.KonuId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.KonuId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IKonuService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new KonuListeleViewModel
            {
                Model = new Konu()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(KonuListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IKonuService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult DersKonuListele(int dersId)
        {
            var selectList = selectListHelper.DersKonuSelectList(dersId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Seciniz,
                Value = "0"
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}