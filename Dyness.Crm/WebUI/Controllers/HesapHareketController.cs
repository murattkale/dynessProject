using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class HesapHareketController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public HesapHareketController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        public HesapHareket GetModel(int id)
        {
            var service = serviceFactory.CreateService<IHesapHareketService>();
            var model = service.Get(x =>
                x.HesapHareketId == id,
                y => y.BorcluHesap,
                y => y.AlacakliHesap);

            return model;
        }

        private void GetLists(HesapHareketDuzenleViewModel viewModel)
        {
            viewModel.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        [Route("HesapHareket/Duzenle/{genelHesapTurGrupId}/{id?}")]
        public ActionResult Duzenle(int genelHesapTurGrupId, int? id)
        {
            var viewModel = new HesapHareketDuzenleViewModel
            {
                HesapTurGrupId = genelHesapTurGrupId
            };

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<HesapHareket>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var model = GetModel((int)id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new HesapHareket
                {
                    BorcluHesap = new Hesap(),
                    AlacakliHesap = new Hesap()
                };
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        [Route("HesapHareket/Duzenle/{genelHesapTurGrupId}/{id?}")]
        public ActionResult Duzenle(HesapHareketDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IHesapHareketService>();

            viewModel.Model.BorcluHesapId = viewModel.BorcluAltHesapId > 0
                ? viewModel.BorcluAltHesapId
                : viewModel.BorcluHesapId;

            viewModel.Model.AlacakliHesapId = viewModel.AlacakliAltHesapId > 0
                ? viewModel.AlacakliAltHesapId
                : viewModel.AlacakliHesapId;

            viewModel.Model.PersonelId = Identity.PersonelId;

            // Gelir eklerken, para çıkışı değil, girişi yapıyoruz.
            if (viewModel.HesapTurGrupId == 1)
            {
                var borcluHesapId = viewModel.Model.BorcluHesapId;
                var alacakliHesapId = viewModel.Model.AlacakliHesapId;

                viewModel.Model.BorcluHesapId = alacakliHesapId;
                viewModel.Model.AlacakliHesapId = borcluHesapId;
            }

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.Model.HesapHareketId == 0)
                            viewModel.Model.IslemGerceklestiMi = true;


                        viewModel.OperationResult = viewModel.Model.HesapHareketId == 0
                        ? service.Add(viewModel.Model)
                        : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return RedirectToAction("Listele");
                    }
            }

            if (viewModel.OperationResult.Status)
            {
                TempData["OperationResult"] = viewModel.OperationResult;
                return Redirect($"/HesapHareket/Duzenle/{viewModel.HesapTurGrupId}");
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

            var service = serviceFactory.CreateService<IHesapHareketService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult HesapHareketGetir(int id)
        {
            var service = serviceFactory.CreateService<IHesapHareketService>();

            var hesapHareket = service.Get(x =>
                x.HesapHareketId == id,
                y => y.AlacakliHesap,
                y => y.BorcluHesap);

            var subeService = serviceFactory.CreateService<ISubeService>();
            var sube = subeService.Get(x => x.SubeId == hesapHareket.BorcluHesapId);

            if (sube == null)
            {
                sube = subeService.Get(x => x.SubeId == hesapHareket.BorcluHesap.UstHesapId);

                if (sube != null)
                {
                    hesapHareket.SubeId = sube.SubeId;
                }
            }
            else
            {
                hesapHareket.SubeId = sube.SubeId;
            }

            return Content(JsonConvert.SerializeObject(hesapHareket), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        [Route("HesapHareket/Listele/{genelHesapTurGrupId}")]
        public ViewResult Listele(int genelHesapTurGrupId)
        {
            var viewModel = new HesapHareketListeleViewModel
            {
                Model = new HesapHareketDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapHareketDto>(),
                Search = new Search(),
                HesapTurGrupId = genelHesapTurGrupId
            };

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.SubeId = Identity.SubeId;

            viewModel.HareketTurSelectList = selectListHelper.HareketTurSelectList();
            viewModel.SorumluPersonelSelectList = selectListHelper.PersonelSelectList();
            viewModel.ZamanSelectList = selectListHelper.AyHaftaGunSelectList();

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        [Route("HesapHareket/Listele/{genelHesapTurGrupId}")]
        public ContentResult Listele(HesapHareketListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("SubeId", viewModel.SubeId),
                new Parameter("HareketTurId", viewModel.HesapTurGrupId),
                new Parameter("SorumluPersonelId", viewModel.SorumluPersonelId),
                new Parameter("IlkTarih", viewModel.IlkTarih),
                new Parameter("SonTarih", viewModel.SonTarih)
            };

            var service = serviceFactory.CreateService<IHesapHareketService>();

            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }
    }
}