using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BransController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public BransController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(BransDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.BransSelectList = selectListHelper.BransSelectList(false);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new BransDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Brans>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IBransService>();
                var model = service.Get(x => x.BransId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Brans();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(BransDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IBransService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.BransId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.BransId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.BransId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.BransId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IBransService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult KurumlarBransListele(string KurumIdler)
        {
            var kurumIdler = new int[KurumIdler.Split(',').Length - 1];

            for (int i = 0; i < KurumIdler.Split(',').Length - 1; i++)
            {
                kurumIdler[i] = Convert.ToInt32(KurumIdler.Split(',')[i]);
            }

            var selectList = selectListHelper.KurumlarBransSelectList(kurumIdler);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}