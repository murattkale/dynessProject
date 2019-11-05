using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class OgrenciBilgiController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public OgrenciBilgiController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Giris()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var viewModel = new OgrenciGirisViewModel();

                ViewBag.ReturnURL = Request.QueryString["ReturnUrl"];
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Giris(OgrenciGirisViewModel viewModel)
        {
            ModelState.Clear();

            var dogumTarihi = viewModel.OgrenciDogumTarihiFormatted;

            if (!string.IsNullOrEmpty(dogumTarihi))
            {
                try
                {
                    viewModel.OgrenciDogumTarihi = new DateTime(
                        Convert.ToInt32(dogumTarihi.Split('.')[2]),
                        Convert.ToInt32(dogumTarihi.Split('.')[1]),
                        Convert.ToInt32(dogumTarihi.Split('.')[0]));
                }
                catch
                {
                    viewModel.OperationResult.Status = false;
                    viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                    {
                        new MessageInfo { Message = Resources.LangResources.DogumTarihiYanlis, MessageInfoType = MessageInfoType.Error}
                    };

                    return View(viewModel);
                }
            }

            var service = serviceFactory.CreateService<IOgrenciService>();

            Ogrenci model = null;

            var veliYaDaOgrenci = 0;
            var sifreDegistir = false;

            if (!string.IsNullOrEmpty(viewModel.OgrenciTcKimlikNo))
            {
                veliYaDaOgrenci = 2;

                if (!string.IsNullOrEmpty(viewModel.OgrenciSifre))
                {
                    model = service.GetBilgi(x =>
                    x.TcKimlikNo == viewModel.OgrenciTcKimlikNo &&
                    x.OgrenciSifre == viewModel.OgrenciSifre,
                    y => y.Sube.Kurum);
                }
                else if (viewModel.OgrenciDogumTarihi != null)
                {
                    model = service.GetBilgi(x =>
                    x.TcKimlikNo == viewModel.OgrenciTcKimlikNo &&
                    x.DogumTarihi == viewModel.OgrenciDogumTarihi,
                    y => y.Sube.Kurum);

                    if (!string.IsNullOrEmpty(model.OgrenciSifre))
                    {
                        viewModel.OperationResult.Status = false;
                        viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                        {
                            new MessageInfo { Message = Resources.LangResources.OgrenciSifreGirmeli, MessageInfoType = MessageInfoType.Error}
                        };

                        return View(viewModel);
                    }

                    sifreDegistir = true;
                }
                else
                {
                    viewModel.OperationResult.Status = false;
                    viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                    {
                        new MessageInfo { Message = Resources.LangResources.EksikBilgi, MessageInfoType = MessageInfoType.Error}
                    };

                    return View(viewModel);
                }
            }
            else if (!string.IsNullOrEmpty(viewModel.VeliTcKimlikNo))
            {
                veliYaDaOgrenci = 1;

                if (!string.IsNullOrEmpty(viewModel.VeliSifre))
                {
                    model = service.GetBilgi(x =>
                    x.AnneOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                    x.VeliSifre == viewModel.VeliSifre,
                    y => y.Sube.Kurum);

                    if (model == null)
                        model = service.GetBilgi(x =>
                        x.BabaOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                        x.VeliSifre == viewModel.VeliSifre,
                        y => y.Sube.Kurum);
                    if (model == null)
                        model = service.GetBilgi(x =>
                        x.YakiniOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                        x.VeliSifre == viewModel.VeliSifre,
                        y => y.Sube.Kurum);
                }
                else if (viewModel.OgrenciDogumTarihi != null)
                {
                    model = service.GetBilgi(x =>
                    x.AnneOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                    x.DogumTarihi == viewModel.OgrenciDogumTarihi &&
                    x.VeliSonGirisTarihi == null,
                    y => y.Sube.Kurum);

                    if (model == null)
                        model = service.GetBilgi(x =>
                        x.BabaOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                        x.DogumTarihi == viewModel.OgrenciDogumTarihi &&
                        x.VeliSonGirisTarihi == null,
                        y => y.Sube.Kurum);
                    if (model == null)
                        model = service.GetBilgi(x =>
                        x.YakiniOgrenciYakiniIletisim.TcKimlikNo == viewModel.VeliTcKimlikNo &&
                        x.DogumTarihi == viewModel.OgrenciDogumTarihi &&
                        x.VeliSonGirisTarihi == null,
                        y => y.Sube.Kurum);

                    if (model != null && !string.IsNullOrEmpty(model.VeliSifre))
                    {
                        viewModel.OperationResult.Status = false;
                        viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                        {
                            new MessageInfo { Message = Resources.LangResources.OgrenciSifreGirmeli, MessageInfoType = MessageInfoType.Error}
                        };

                        return View(viewModel);
                    }

                    sifreDegistir = true;
                }
                else
                {
                    viewModel.OperationResult.Status = false;
                    viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                    {
                        new MessageInfo { Message = Resources.LangResources.EksikBilgi, MessageInfoType = MessageInfoType.Error}
                    };
                }
            }

            if (model != null && (veliYaDaOgrenci == 1 || veliYaDaOgrenci == 2))
            {
                var kullaniciAd = veliYaDaOgrenci == 1
                    ? $"{Resources.LangResources.Veli} - {model.AdSoyad}"
                    : model.AdSoyad;

                var sonGirisTarihi = veliYaDaOgrenci == 1
                    ? model.VeliSonGirisTarihi
                    : model.OgrenciSonGirisTarihi;

                string identityParameters =
                   $"{model.OgrenciId}|{model.Sube.KurumId}|{model.SubeId}|{0}|{kullaniciAd}|{model.Eposta ?? ""}|{kullaniciAd}|{model.GorselYol}|{""}|//{""}|{model.YasadigiUlkeAd}|{string.Format(AyarlarService.Get().GecerliTarihSaatFormati, sonGirisTarihi)}|{model.Sube.Kurum.LogoYol}|{RolModel.Ogrenci}|{veliYaDaOgrenci}";

                FormsAuthentication.SetAuthCookie(identityParameters, false);

                if (sifreDegistir)
                    return Redirect($"/OgrenciBilgi/SifreDegistir");

                if (veliYaDaOgrenci == 1)
                {
                    model.VeliSonGirisTarihi = DateTime.Now;
                }
                else if (veliYaDaOgrenci == 2)
                {
                    model.OgrenciSonGirisTarihi = DateTime.Now;
                }

                service.Update(model);

                return Redirect($"/OgrenciBilgi/");
            }

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult SifreDegistir()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("OgrenciBilgi", "Giris");
            else
                return View(new OgrenciSifreDegistirViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult SifreDegistir(OgrenciSifreDegistirViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciService>();
            var model = service.GetBilgi(x =>
            x.OgrenciId == Identity.PersonelId,
            y => y.AnneOgrenciYakiniIletisim,
            y => y.BabaOgrenciYakiniIletisim,
            y => y.YakiniOgrenciYakiniIletisim);

            if (model == null)
            {
                viewModel.OperationResult.Status = false;
                viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                {
                    new MessageInfo { Message = Resources.LangResources.EksikBilgi, MessageInfoType = MessageInfoType.Error}
                };

                return View(viewModel);
            }

            if (!string.Equals(viewModel.Sifre, viewModel.SifreTekrar))
            {
                viewModel.OperationResult.Status = false;
                viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                {
                    new MessageInfo { Message = Resources.LangResources.EksikBilgi, MessageInfoType = MessageInfoType.Error}
                };

                return View(viewModel);
            }

            if (string.Equals(viewModel.TcKimlikNo, model.TcKimlikNo))
            {
                model.OgrenciSifre = viewModel.Sifre;
            }
            else if ((model.AnneOgrenciYakiniIletisim != null && string.Equals(viewModel.TcKimlikNo, model.AnneOgrenciYakiniIletisim.TcKimlikNo)) ||
                (model.BabaOgrenciYakiniIletisim != null && string.Equals(viewModel.TcKimlikNo, model.BabaOgrenciYakiniIletisim.TcKimlikNo)) ||
                (model.YakiniOgrenciYakiniIletisim != null && string.Equals(viewModel.TcKimlikNo, model.YakiniOgrenciYakiniIletisim.TcKimlikNo)))
            {
                model.VeliSifre = viewModel.Sifre;
            }
            else
            {
                viewModel.OperationResult.Status = false;
                viewModel.OperationResult.MessageInfos = new List<MessageInfo>
                {
                    new MessageInfo { Message = Resources.LangResources.TcKimikNoDogrulanamadi, MessageInfoType = MessageInfoType.Error}
                };

                return View(viewModel);
            }

            viewModel.OperationResult = service.Update(model);

            if (!viewModel.OperationResult.Status)
            {
                return View(viewModel);
            }

            return Redirect($"/OgrenciBilgi/");
        }

        [HttpGet]
        public ActionResult YetkiliGiris(int id)
        {
            if (Identity.KurumId != -1)
                return Redirect("/");

            var service = serviceFactory.CreateService<IOgrenciService>();
            var model = service.Get(x => x.OgrenciId == id, y => y.Sube.Kurum);

            if (model != null)
            {
                FormsAuthentication.SignOut();

                var kullaniciAd = model.AdSoyad;

                string identityParameters =
                   $"{model.OgrenciId}|{model.Sube.KurumId}|{model.SubeId}|{0}|{kullaniciAd}|{model.Eposta ?? ""}|{kullaniciAd}|{model.GorselYol}|{""}|//{""}|{model.YasadigiUlkeAd}|{string.Format(AyarlarService.Get().GecerliTarihSaatFormati, DateTime.Now)}|{model.Sube.Kurum.LogoYol}|{RolModel.Ogrenci}|{2}";

                FormsAuthentication.SetAuthCookie(identityParameters, false);

                return Redirect($"/OgrenciBilgi/");
            }

            return Redirect("/ogrencibilgi/");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciDetay()
        {
            var viewModel = new OgrenciDetayViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Ogrenci>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            var id = Identity.PersonelId;

            var service = serviceFactory.CreateService<IOgrenciService>();
            var model = service.Get(x => x.OgrenciId == id,
                y => y.Hesap,
                y => y.Ulke,
                y => y.Sehir,
                y => y.Ilce,
                y => y.EkleyenPersonel,
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
                y => y.OgrenciSozlesmeler.Select(z => z.OgrenciSozlesmeOdemeBilgi.SonGuncelleyenPersonel),
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
                y => y.AlacakliHesap.Sube,
                y => y.BorcluHesap.Sube,
                y => y.Personel,
                y => y.OgrenciSozlesmeHesapHareket).
                OrderBy(x => x.VadeTarihi).
                ThenBy(x => x.BorcluHesapId != id).
                ThenBy(x => !x.IslemGerceklestiMi).
                ToList();

            var hesapBilgiService = serviceFactory.CreateService<IHesapBilgiService>();

            model.Hesap.HesapBilgiler = hesapBilgiService.List(x => x.HesapId == model.Hesap.HesapId).ToList();

            model.OgrenciSozlesmeler = model.OgrenciSozlesmeler.OrderByDescending(x => x.OgrenciSozlesmeId).ToList();

            if (model.Hesap.HesapBilgiler != null && model.Hesap.HesapBilgiler.Count > 0)
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var yetkisiVar = false;

                    foreach (var sozlesme in model.OgrenciSozlesmeler)
                    {
                        yetkisiVar = yetkiliSubeIdler.Contains(sozlesme.SubeId);

                        if (yetkisiVar) break;
                    }

                    if (!yetkisiVar)
                    {
                        model.Hesap.HesapHareketler = null;
                        model.Hesap.HesapBilgiler = null;
                        model.OgrenciSozlesmeler = null;
                    }
                }
            }

            if (model.OgrenciSozlesmeler != null)
            {
                var subeService = serviceFactory.CreateService<ISubeService>();
                var subeler = new List<Sube>();

                foreach (var ogrenciSozlesme in model.OgrenciSozlesmeler)
                {
                    var addSube = subeService.Get(x => x.SubeId == ogrenciSozlesme.SubeId);

                    if (addSube != null)
                        subeler.Add(addSube);
                }

                viewModel.Subeler = subeler;
            }

            var ogrenciSinavKontrolService = serviceFactory.CreateService<IOgrenciSinavKontrolService>();
            model.OgrenciSinavKontroller = ogrenciSinavKontrolService.List(x =>
                x.OgrenciId == model.OgrenciId,
                y => y.SinavKitapcik.Sinav.Sezon).
            ToList();

            viewModel.Model = model;
            viewModel.Model.Hesap.Odenebilir = false;

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult EgitimListele()
        {
            var viewModel = new VideoListeleViewModel
            {
                DersSelectList = selectListHelper.DersSelectList(),
                VideoKategoriSelectList = new List<SelectListItem>(),
                KonuSelectList = new List<SelectListItem>()
            };

            viewModel.VideoKategoriSelectList.Add(new SelectListItem
            {
                Selected = true,
                Text = Resources.LangResources.VideoKategoriler,
                Value = "0"
            });

            viewModel.KonuSelectList.Add(new SelectListItem
            {
                Selected = true,
                Text = Resources.LangResources.Konular,
                Value = "0"
            });

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult DersVideoKategoriListele(int DersId)
        {
            var selectList = selectListHelper.DersVideoKategoriSelectList(DersId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.VideoKategoriler,
                Value = "0"
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ContentResult DersKonuListele(int dersId)
        {
            var selectList = selectListHelper.DersKonuSelectList(dersId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Konular,
                Value = "0"
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult EgitimVideoListele(int DersId, string VideoKategoriIdler, string KonuIdler, string Search)
        {
            var videoService = serviceFactory.CreateService<IVideoService>();

            VideoKategoriIdler = string.IsNullOrEmpty(VideoKategoriIdler) ? "" : VideoKategoriIdler;
            KonuIdler = string.IsNullOrEmpty(KonuIdler) ? "" : KonuIdler;

            var paramters = new List<Parameter>
            {
                new Parameter("OgrenciId", Identity.PersonelId),
                new Parameter("DersId", DersId),
                new Parameter("VideoKategoriIdler", VideoKategoriIdler),
                new Parameter("KonuIdler", KonuIdler),
                new Parameter("Search", Search)
            };

            var videolar = videoService.VideoListele(paramters);

            var jSonlist = JsonHelper.ObjectToJsonString(videolar);

            return Content(jSonlist, "application/json");
        }
    }
}