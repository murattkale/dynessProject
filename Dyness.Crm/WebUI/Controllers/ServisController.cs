using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;

namespace WebUI.Controllers
{
    public class ServisController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public ServisController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(ServisDuzenleViewModel viewModel)
        {
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new ServisDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Servis>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IServisService>();
                var model = service.Get(x => x.ServisId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Servis();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(ServisDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IServisService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.ServisId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.ServisId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.ServisId })
                            : RedirectToAction("Duzenle");
                    }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IServisService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new ServisListeleViewModel
            {
                Model = new Servis()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(ServisListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IServisService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SubeServisListele(int? subeId)
        {
            if (subeId == null) subeId = 0;

            var selectList = selectListHelper.ServisSelectList((int)subeId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Seciniz,
                Value = ""
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}