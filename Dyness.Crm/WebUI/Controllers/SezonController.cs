using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    public class SezonController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SezonController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SezonDuzenleViewModel viewModel)
        {
            viewModel.KurumSelectList = selectListHelper.KurumSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SezonDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sezon>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISezonService>();
                var model = service.Get(x => x.SezonId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Sezon();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SezonDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            GetLists(viewModel);

            var service = serviceFactory.CreateService<ISezonService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SezonId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SezonId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SezonId })
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

            var service = serviceFactory.CreateService<ISezonService>();

            var operationResult = service.DeleteById(id);
            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SezonListeleViewModel
            {
                Model = new Sezon()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(SezonListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<ISezonService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SezonlarListele(string subeIdler)
        {
            var selectList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(subeIdler))
            {
                var service = serviceFactory.CreateService<ISezonService>();

                var parameters = new List<Parameter>
                {
                    new Parameter("SubeIdler", subeIdler)
                };

                var sezonlar = service.SezonListele(parameters).ToList();

                for (int i = 0; i < sezonlar.Count; i++)
                {
                    selectList.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = sezonlar[i].SezonAd,
                        Value = sezonlar[i].SezonId.ToString(),
                    });
                }
            }

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult KurumlarSezonListele(string KurumIdler)
        {
            var kurumIdler = new int[KurumIdler.Split(',').Length - 1];

            for (int i = 0; i < KurumIdler.Split(',').Length - 1; i++)
            {
                kurumIdler[i] = Convert.ToInt32(KurumIdler.Split(',')[i]);
            }

            var selectList = selectListHelper.KurumlarSezonSelectList(kurumIdler);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}