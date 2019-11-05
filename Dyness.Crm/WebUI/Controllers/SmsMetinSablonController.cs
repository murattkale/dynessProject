using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SmsMetinSablonController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SmsMetinSablonController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SmsMetinSablonDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.SmsMetinSablonSelectList = selectListHelper.SmsMetinSablonSelectList(false, new int[] { selectedItemId });
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SmsMetinSablonDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SmsMetinSablon>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISmsMetinSablonService>();
                var model = service.Get(x => x.SmsMetinSablonId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new SmsMetinSablon();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SmsMetinSablonDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISmsMetinSablonService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SmsMetinSablonId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SmsMetinSablonId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SmsMetinSablonId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.SmsMetinSablonId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISmsMetinSablonService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Detay(int? id)
        {
            var service = serviceFactory.CreateService<ISmsMetinSablonService>();
            var model = service.Get(x => x.SmsMetinSablonId == id);

            var jSonlist = JsonHelper.ObjectToJsonString(model);

            return Content(jSonlist, "application/json");
        }
    }
}