using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class HesapController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public HesapController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        public Hesap GetModel(int id)
        {
            var service = serviceFactory.CreateService<IHesapService>();
            var model = service.Get(x => x.HesapId == id, y => y.HesapTur);

            return model;
        }

        private void GetLists(HesapDuzenleViewModel viewModel)
        {
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            var hesapturGrupId = Request.QueryString["Gelir"] != null
                ? 1
                : Request.QueryString["Gider"] != null
                    ? 2
                    : 0;

            viewModel.HesapTurSelectList = selectListHelper.HesapTurSelectList(hesapturGrupId);
            viewModel.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        [Route("Hesap/Duzenle/{genelHesapTurGrupId}/{id?}")]
        public ActionResult Duzenle(int genelHesapTurGrupId, int? id)
        {
            var viewModel = new HesapDuzenleViewModel
            {
                HesapTurGrupId = genelHesapTurGrupId
            };

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Hesap>)TempData["OperationResult"];
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
                viewModel.Model = new Hesap();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        [Route("Hesap/Duzenle/{genelHesapTurGrupId}/{id?}")]
        public ActionResult Duzenle(HesapDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IHesapService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.Model.HesapId == 0)
                        {
                            var selectedGenelHesapId = viewModel.HesapTurGrupId;
                            var transferTipId = selectedGenelHesapId == 1 ||
                                selectedGenelHesapId == 4 ||
                                selectedGenelHesapId == 5
                                    ? 1
                                    : selectedGenelHesapId == 2
                                        ? 2
                                        : selectedGenelHesapId == 3
                                            ? 4
                                            : 0;

                            if (transferTipId > 0)
                            {
                                viewModel.Model.TransferTipId = transferTipId;
                            }

                            var hesapTurId = selectedGenelHesapId == 1 ||
                                selectedGenelHesapId == 2 ||
                                selectedGenelHesapId == 3
                                    ? 4
                                    : viewModel.Model.HesapTurId;

                            viewModel.Model.HesapTurId = hesapTurId;
                        }

                        viewModel.OperationResult = viewModel.Model.HesapId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return RedirectToAction("Duzenle");
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

            var service = serviceFactory.CreateService<IHesapService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = new OgrenciDetayViewModel();

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IOgrenciService>();
                var model = service.Get(x => x.OgrenciId == id,
                    y => y.Hesap,
                    y => y.Ulke,
                    y => y.Sehir,
                    y => y.Ilce,
                    y => y.NeredenDuydunuz,
                    y => y.AnneOgrenciYakiniIletisim,
                    y => y.BabaOgrenciYakiniIletisim,
                    y => y.YakiniOgrenciYakiniIletisim,
                    y => y.OgrenciSozlesmeler.Select(z => z.Brans),
                    y => y.OgrenciSozlesmeler.Select(z => z.DanismanPersonel),
                    y => y.OgrenciSozlesmeler.Select(z => z.EhliyetTur),
                    y => y.OgrenciSozlesmeler.Select(z => z.Etkinlik),
                    y => y.OgrenciSozlesmeler.Select(z => z.FaturaBilgi),
                    y => y.OgrenciSozlesmeler.Select(z => z.GorusenPersonel),
                    y => y.OgrenciSozlesmeler.Select(z => z.KurumaGetirenPersonel),
                    y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeDersSecimler.Select(a => a.Ders)),
                    y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeKiyafetDurumlar.Select(a => a.Kiyafet.KiyafetBeden)),
                    y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeOdemeBilgi.OgrenciSozlesmeOdemeBilgiSenetImzalayan),
                    y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeTur),
                    y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeYayinlar.Select(a => a.Yayin)),
                    y => y.OgrenciSozlesmeler.Select(z => z.OkulTur),
                    y => y.OgrenciSozlesmeler.Select(z => z.OzelDersDurum),
                    y => y.OgrenciSozlesmeler.Select(z => z.SinifSeviye),
                    y => y.OgrenciSozlesmeler.Select(z => z.Sube),
                    y => y.OgrenciSozlesmeler.Select(z => z.Sinif),
                    y => y.OgrenciSozlesmeler.Select(z => z.Sezon),
                    y => y.OgrenciSozlesmeler.Select(z => z.Servis));

                if (model == null)
                    return Redirect("/Error/NotFound");

                var hesapHareketService = serviceFactory.CreateService<IHesapHareketService>();
                model.Hesap.HesapHareketler = hesapHareketService.List(
                    x =>
                        x.AlacakliHesapId == model.Hesap.HesapId ||
                        x.BorcluHesapId == model.Hesap.HesapId,
                    y => y.AlacakliHesap.Sube).
                    OrderBy(x => x.HesapHareketId).
                    ToList();

                model.OgrenciSozlesmeler = model.OgrenciSozlesmeler.OrderByDescending(x => x.OgrenciSozlesmeId).ToList();

                viewModel.Model = model;
            }

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult HesapSubeIdParaBirimIdListele(int? subeId, int? paraBirimId)
        {
            if (subeId == null) subeId = 0;
            if (paraBirimId == null) paraBirimId = 0;

            var selectList = selectListHelper.SubeAltHesapSelectList((int)subeId, (int)paraBirimId);

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

        [CheckAuthorizedActionFilter]
        public ContentResult HesapParaBirimIdListele(int paraBirimId, int hesapTurGrupId)
        {
            var selectList = selectListHelper.HesapParaBirimSelectList(paraBirimId, hesapTurGrupId);

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

        [CheckAuthorizedActionFilter]
        public ContentResult HesapParaBirimIdAltHesapListele(int paraBirimId, int ustHesapId, int hesapTurGrupId)
        {
            var selectList = selectListHelper.HesapParaBirimAltHesapSelectList(paraBirimId, ustHesapId, hesapTurGrupId);

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

        [CheckAuthorizedActionFilter]
        public ContentResult HesapParaBirimIdHesapTurIdAltHesapListele(int paraBirimId, int ustHesapId, int hesapTurId)
        {
            var selectList = selectListHelper.HesapParaBirimHesapTurIdAltHesapSelectList(paraBirimId, ustHesapId, hesapTurId);

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

        [HttpGet]
        [CheckAuthorizedActionFilter]
        [Route("Hesap/Listele/{hesapTurGrupId?}")]
        public ViewResult HesapListele(int? hesapTurGrupId)
        {
            var viewModel = new HesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search()
            };

            viewModel.HesapTurGrupSelectList = selectListHelper.HesapTurGrupSelectList();

            if (hesapTurGrupId != null)
            {
                viewModel.HesapTurGrupId = (int)hesapTurGrupId;
            }

            viewModel.HesapTurSelectList = selectListHelper.HesapTurSelectList(viewModel.HesapTurGrupId);
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.HesapId = Identity.SubeId;

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        [Route("Hesap/Listele/{hesapTurGrupId?}")]
        public ContentResult HesapDtoListele(HesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("HesapTurId", viewModel.HesapTurId),
                new Parameter("HesapTurGrupId", viewModel.HesapTurGrupId),
                new Parameter("UstHesapId", viewModel.HesapId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.HesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult SubeHesapListele()
        {
            var viewModel = new SubeHesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search(),
                Yil = DateTime.Now.Year
            };

            viewModel.AySelectList = selectListHelper.AySelectList();
            viewModel.YilSelectList = selectListHelper.YilSelectList();
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.HesapId = Identity.SubeId;

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SubeHesapListele(SubeHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("Ay", viewModel.Ay),
                new Parameter("Yil", viewModel.Yil == 0 ? DateTime.Now.Year : viewModel.Yil),
                new Parameter("HesapId", viewModel.HesapId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.SubeHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult KasaBankaHesapListele()
        {
            var viewModel = new KasaBankaHesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search(),
                Yil = DateTime.Now.Year
            };

            viewModel.AySelectList = selectListHelper.AySelectList();
            viewModel.YilSelectList = selectListHelper.YilSelectList();
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.HesapId = Identity.SubeId;

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult KasaBankaHesapListele(KasaBankaHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("Ay", viewModel.Ay),
                new Parameter("Yil", viewModel.Yil == 0 ? DateTime.Now.Year : viewModel.Yil),
                new Parameter("UstHesapId", viewModel.HesapId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.KasaBankaHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult OgrenciHesapListele()
        {
            var viewModel = new OgrenciHesapListeleViewModel
            {
                Model = new OgrenciHesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<OgrenciHesapDto>(),
                Search = new Search()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult OgrenciHesapListele(OgrenciHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("VadesiGecen", viewModel.VadesiGecen),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.OgrenciHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult PersonelHesapListele()
        {
            var viewModel = new PersonelHesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search(),
                Yil = DateTime.Now.Year
            };

            viewModel.AySelectList = selectListHelper.AySelectList();
            viewModel.YilSelectList = selectListHelper.YilSelectList();
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult PersonelHesapListele(PersonelHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("Ay", viewModel.Ay),
                new Parameter("Yil", (viewModel.Yil == 0 ? DateTime.Now.Year : viewModel.Yil)),
                new Parameter("SubeId", viewModel.SubeId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.PersonelHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult GiderHesapListele()
        {
            var viewModel = new GiderHesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search(),
                Yil = DateTime.Now.Year
            };

            viewModel.AySelectList = selectListHelper.AySelectList();
            viewModel.YilSelectList = selectListHelper.YilSelectList();
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.HesapId = Identity.SubeId;

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult GiderHesapListele(GiderHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("Ay", viewModel.Ay),
                new Parameter("Yil", viewModel.Yil == 0 ? DateTime.Now.Year : viewModel.Yil),
                new Parameter("UstHesapId", viewModel.HesapId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.GiderHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult GelirHesapListele()
        {
            var viewModel = new GiderHesapListeleViewModel
            {
                Model = new HesapDto(),
                EntityPagedDataSource = new EntityPagedDataSource<HesapDto>(),
                Search = new Search(),
                Yil = DateTime.Now.Year
            };

            viewModel.AySelectList = selectListHelper.AySelectList();
            viewModel.YilSelectList = selectListHelper.YilSelectList();
            viewModel.HesapSelectList = selectListHelper.SubeSelectList();

            if (Identity.KurumId != -1)
                viewModel.HesapId = Identity.SubeId;

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult GelirHesapListele(GiderHesapListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("Ay", viewModel.Ay),
                new Parameter("Yil", viewModel.Yil == 0 ? DateTime.Now.Year : viewModel.Yil),
                new Parameter("UstHesapId", viewModel.HesapId),
                new Parameter("EtkinMi", viewModel.EtkinMi)
            };

            var service = serviceFactory.CreateService<IHesapService>();

            var pagedDataSource = service.GelirHesapDtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult HesapHareketListele(int id)
        {
            var service = serviceFactory.CreateService<IHesapService>();
            var hesap = service.Get(x => x.HesapId == id);

            if (hesap != null)
            {
                var hesapHareketService = serviceFactory.CreateService<IHesapHareketService>();
                hesap.HesapHareketler = hesapHareketService.List(
                    x =>
                        x.AlacakliHesapId == id ||
                        x.BorcluHesapId == id,
                    y => y.AlacakliHesap.Sube,
                    y => y.BorcluHesap.Sube,
                    y => y.Personel).
                    OrderBy(x => x.HesapHareketId).
                    ToList();

                var hesapBilgiService = serviceFactory.CreateService<IHesapBilgiService>();

                hesap.HesapBilgiler = hesapBilgiService.List(x => x.HesapId == id).ToList();
            }

            hesap.Odenebilir = false;
            return PartialView("~/Views/Shared/_OgrenciHesapHareketDetayView.cshtml", hesap);
        }
    }
}