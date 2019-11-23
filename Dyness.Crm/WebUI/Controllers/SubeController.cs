using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;
using System;

namespace WebUI.Controllers
{
    public class SubeController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SubeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SubeDuzenleViewModel viewModel)
        {
            viewModel.KurumSelectList = selectListHelper.KurumSelectList();
            viewModel.UlkeSelectList = selectListHelper.UlkeSelectList();
            //viewModel.SehirSelectList = selectListHelper.SehirSelectList();
            //viewModel.UlkeSelectList = selectListHelper.IlceSelectList();
            viewModel.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SubeDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sube>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISubeService>();
                var model = service.Get(x => x.SubeId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Sube();
            }

            GetLists(viewModel);

            if (string.IsNullOrEmpty(viewModel.Model.Kod))
            {
                var service = serviceFactory.CreateService<ISubeService>();
                viewModel.Model.Kod = service.GetSonSubeKod();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SubeDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISubeService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SubeId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SubeId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SubeId })
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

            var service = serviceFactory.CreateService<ISubeService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SubeListeleViewModel
            {
                Model = new Sube()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(SubeListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<ISubeService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult KurumlarSubeListele(string KurumIdler)
        {
            var kurumIdler = new int[KurumIdler.Split(',').Length - 1];

            for (int i = 0; i < KurumIdler.Split(',').Length - 1; i++)
            {
                kurumIdler[i] = Convert.ToInt32(KurumIdler.Split(',')[i]);
            }

            var selectList = selectListHelper.KurumlarSubeSelectList(kurumIdler);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}