using Core.Entities.Dto;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HesapTurController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public HesapTurController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(HesapTurDuzenleViewModel viewModel)
        {
            viewModel.HesapTurGrupSelectList = selectListHelper.HesapTurGrupSelectList(false);

            var hesapTurGrupId = viewModel.Model?.HesapTurGrupId != null && viewModel.Model.HesapTurGrupId != 0
                ? (int)viewModel.Model.HesapTurGrupId
                : viewModel.Gelir
                    ? 1
                    : viewModel.Gider
                        ? 2
                        : 0;

            viewModel.Model.HesapTurGrupId = hesapTurGrupId;

            viewModel.HesapTurSelectList = selectListHelper.HesapTurSelectList(hesapTurGrupId, etkinMi: false);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        [Route("HesapTur/Duzenle/{hesapTurgrupId}/{id?}")]
        public ActionResult Duzenle(int hesapTurgrupId, int? id)
        {
            var viewModel = new HesapTurDuzenleViewModel
            {
                HesapTurGrupId = hesapTurgrupId
            };

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<HesapTur>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IHesapTurService>();
                var model = service.Get(x => x.HesapTurId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new HesapTur();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        [Route("HesapTur/Duzenle/{hesapTurgrupId}/{id?}")]
        public ActionResult Duzenle(HesapTurDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IHesapTurService>();

            if (viewModel.Model.HesapTurId == 0)
            {
                var hesapTurGrupId = viewModel.Gelir
                    ? 1
                    : viewModel.Gider
                        ? 2
                        : viewModel.Model.HesapTurGrupId;

                viewModel.Model.HesapTurGrupId = hesapTurGrupId;
            }

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.HesapTurId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        if (viewModel.Gelir)
                            return Redirect("/HesapTur/Duzenle/1");
                        else if (viewModel.Gelir)
                            return Redirect("/HesapTur/Duzenle/2");
                        else
                            return Redirect("/HesapTur/Duzenle/0");
                    }
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult HesapTurListele(int hesapTurGrupId)
        {
            var service = serviceFactory.CreateService<IHesapTurService>();
            var hesapTurler = service.List(x => x.HesapTurGrupId == hesapTurGrupId);

            hesapTurler = hesapTurler.
                Where(x => !new List<int> { 1, 2, 3 }.Contains(x.HesapTurId)).
                OrderBy(x => x.HesapTurAd).
                ToList();

            var selectList = selectListHelper.ToSelectList(hesapTurler, x => x.HesapTurId, x => x.HesapTurAd);

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