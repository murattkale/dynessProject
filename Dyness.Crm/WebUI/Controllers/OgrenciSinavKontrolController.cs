using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class OgrenciSinavKontrolController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public OgrenciSinavKontrolController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        public OgrenciSinavKontrolViewModel GetModel(int? id, int ogrenciAdet, int subeOgrenciAdet, int sinifOgrenciAdet)
        {
            var viewModel = new OgrenciSinavKontrolViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<OgrenciSinavKontrol>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IOgrenciSinavKontrolService>();
                var model = service.Get(x => x.OgrenciSinavKontrolId == id,
                    y => y.Ogrenci,
                    y => y.Sube.Kurum,
                    y => y.OgrenciSinavKontrolPuanTurPuanlar.Select(z => z.PuanTur),
                    y => y.OgrenciSinavKontrolDersBilgiler.Select(z => z.Ders.DersGrup),
                    y => y.SinavKitapcik.Sinav.SinavTur.SinavTurDersKatSayilar.Select(z => z.PuanTur),
                    y => y.SinavKitapcik.Sinav.SinavTur.SinavTurDersKatSayilar.Select(z => z.DersGrup),
                    y => y.SinavKitapcik.Sinav.SinavTur.SinavTurDersler.Select(z => z.Ders.DersGrup),
                    y => y.SinavKitapcik.SinavKitapcikDersBilgiler.Select(z => z.Ders.DersGrup));

                if (model == null)
                    return null;

                if (model.OgrenciSinavKontrolDersBilgiler == null || !model.OgrenciSinavKontrolDersBilgiler.Any())
                    return null;

                if (model.OgrenciSinavKontrolPuanTurPuanlar == null || !model.OgrenciSinavKontrolPuanTurPuanlar.Any())
                    return null;

                if (model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler == null || !model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler.Any())
                    return null;

                if (model.OgrenciSinavKontrolDersBilgiler.Count != model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler.Count)
                    return null;

                model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler = model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler.OrderBy(x => x.Sira).ToList();

                model.Sorular = new List<OgrenciSinavKontrolSoru>();

                var soruSayi = 0;

                var konular = new List<Konu>();
                var konuService = serviceFactory.CreateService<IKonuService>();

                for (int i = 0; i < model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler.Count; i++)
                {
                    var sinavTurDers = model.SinavKitapcik.Sinav.SinavTur.SinavTurDersler[i];
                    var ogrenciSinavKontrolDersBilgi = model.OgrenciSinavKontrolDersBilgiler.FirstOrDefault(x => x.DersId == sinavTurDers.DersId);
                    var sinavKitapcikDersBilgi = model.SinavKitapcik.SinavKitapcikDersBilgiler.FirstOrDefault(x => x.DersId == ogrenciSinavKontrolDersBilgi.DersId);

                    for (int j = 0; j < sinavKitapcikDersBilgi.CevapAnahtartari.Length; j++)
                    {
                        var konuId = sinavKitapcikDersBilgi.DersKonuBilgi == null ||
                            string.IsNullOrEmpty(sinavKitapcikDersBilgi.DersKonuBilgi.Trim()) ||
                            string.IsNullOrEmpty(sinavKitapcikDersBilgi.DersKonuBilgi.Split(',')[j].ToString().Trim())
                            ? 0
                            : Convert.ToInt32(sinavKitapcikDersBilgi.DersKonuBilgi.Split(',')[j].ToString().Trim());

                        Konu konu = null;

                        if (konuId > 0)
                        {
                            konu = konular.FirstOrDefault(x => x.DersId == sinavTurDers.DersId && x.KonuId == konuId);

                            if (konu == null)
                            {
                                konu = konuService.Get(x => x.DersId == sinavTurDers.DersId && x.KonuId == konuId);

                                if (konu != null)
                                {
                                    konular.Add(konu);
                                }
                            }
                        }

                        var soru = new OgrenciSinavKontrolSoru
                        {
                            Soru = soruSayi + j + 1,
                            Ders = sinavTurDers.Ders,
                            DersGrup = sinavTurDers.Ders.DersGrup,
                            DogruCevap = sinavKitapcikDersBilgi.CevapAnahtartari[j].ToString(),
                            OgrenciCevap = ogrenciSinavKontrolDersBilgi.SoruCevaplar[j].ToString()
                        };

                        if (konu != null)
                        {
                            soru.Konu = konu;
                        }

                        model.Sorular.Add(soru);
                    }

                    soruSayi += (int)sinavTurDers.SoruSayi;
                }

                viewModel.Model = model;

                viewModel.OgrenciAdet = ogrenciAdet > 0
                    ? ogrenciAdet
                    : service.GetCount(x => x.SinavKitapcik.SinavId == model.SinavKitapcik.SinavId);

                viewModel.SubeOgrenciAdet = subeOgrenciAdet > 0
                    ? subeOgrenciAdet
                    : service.GetCount(x => x.SinavKitapcik.SinavId == model.SinavKitapcik.SinavId && x.SubeId == model.SubeId);

                viewModel.SinifOgrenciAdet = sinifOgrenciAdet > 0
                    ? sinifOgrenciAdet
                    : service.GetCount(x => x.SinavKitapcik.SinavId == model.SinavKitapcik.SinavId && x.Sinif == model.Sinif);

                var sinavSorular = model.Sorular;
                var dersler = sinavSorular.Select(x => x.Ders).Distinct().ToList();

                model.SinavKitapcik.SinavKonuBilgiler = new List<SinavKonuBilgi>();

                for (int i = 0; i < dersler.Count; i++)
                {
                    var ders = dersler[i];

                    var soruadet = sinavSorular.Count(x => x.Ders.DersId == ders.DersId);
                    var dogruAdet = sinavSorular.Count(x => x.Ders.DersId == ders.DersId && x.Durum == SoruDurum.Dogru);
                    var yanlisAdet = sinavSorular.Count(x => x.Ders.DersId == ders.DersId && x.Durum == SoruDurum.Yanlis);
                    var bosAdet = sinavSorular.Count(x => x.Ders.DersId == ders.DersId && x.Durum == SoruDurum.Bos);

                    model.SinavKitapcik.SinavKonuBilgiler.Add(new SinavKonuBilgi
                    {
                        DersAd = ders.DersAd,
                        SoruAdet = soruadet,
                        DogruAdet = dogruAdet,
                        YanlisAdet = yanlisAdet,
                        BosAdet = bosAdet
                    });

                    var dersKonular = sinavSorular.Where(x => x.Ders.DersId == ders.DersId).Select(x => x.Konu).Distinct().ToList();

                    if (dersKonular.Count(x => x != null) > 0)
                    {
                        for (int j = 0; j < dersKonular.Count; j++)
                        {
                            var dersKonu = dersKonular[j];

                            if (dersKonu == null)
                                continue;

                            soruadet = sinavSorular.Count(x => x.Konu.KonuId == dersKonu.KonuId);
                            dogruAdet = sinavSorular.Count(x => x.Konu.KonuId == dersKonu.KonuId && x.Durum == SoruDurum.Dogru);
                            yanlisAdet = sinavSorular.Count(x => x.Konu.KonuId == dersKonu.KonuId && x.Durum == SoruDurum.Yanlis);
                            bosAdet = sinavSorular.Count(x => x.Konu.KonuId == dersKonu.KonuId && x.Durum == SoruDurum.Bos);

                            model.SinavKitapcik.SinavKonuBilgiler.Add(new SinavKonuBilgi
                            {
                                DersAd = ders.DersAd,
                                Konu = dersKonu.Baslik,
                                SoruAdet = soruadet,
                                DogruAdet = dogruAdet,
                                YanlisAdet = yanlisAdet,
                                BosAdet = bosAdet
                            });
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            return viewModel;
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = GetModel(id, 0, 0, 0);

            if (viewModel == null)
                return Redirect("/Error/NotFound");

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult SinavSonuc(int? id)
        {
            var viewModel = GetModel(id, 0, 0, 0);

            if (viewModel == null)
                return Redirect("/Error/NotFound");

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult TopluSinavSonuc(int? id)
        {
            if (id == null)
                return Redirect("/Error/NotFound");

            var yetkiliSubeIdler = PersonelSubeYetkiService.Get(Identity.PersonelId).Select(x => x.SubeId).ToList();
            var subeYetkilimi = yetkiliSubeIdler.Contains((int)id) || yetkiliSubeIdler.Count() == 0;

            if (!subeYetkilimi)
                return Redirect("/Error/NotFound");

            var service = serviceFactory.CreateService<IOgrenciSinavKontrolService>();
            var ogrenciSinavKontroller = service.List(x => x.SubeId == id).ToList();

            var models = new List<OgrenciSinavKontrolViewModel>();

            OgrenciSinavKontrolViewModel ilkModel = null;

            for (int i = 0; i < ogrenciSinavKontroller.Count; i++)
            {
                if (i == 0)
                {
                    ilkModel = GetModel(ogrenciSinavKontroller[i].OgrenciSinavKontrolId, 0, 0, 0);
                    models.Add(ilkModel);
                }
                else
                {
                    models.Add(GetModel(ogrenciSinavKontroller[i].OgrenciSinavKontrolId, ilkModel.OgrenciAdet, ilkModel.SubeOgrenciAdet, ilkModel.SinifOgrenciAdet));
                }
            }

            var viewModel = new TopluOgrenciSinavKontrolViewModel
            {
                Model = models
            };

            return View(viewModel);
        }
    }
}