using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Properties;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class OgrenciController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public OgrenciController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        public Ogrenci GetModel(int id)
        {
            var service = serviceFactory.CreateService<IOgrenciService>();
            var model = service.Get(x => x.OgrenciId == id,
                y => y.Ulke,
                y => y.Sehir,
                y => y.Ilce,
                y => y.EkleyenPersonel,
                y => y.NeredenDuydunuz,
                y => y.AnneOgrenciYakiniIletisim,
                y => y.BabaOgrenciYakiniIletisim,
                y => y.YakiniOgrenciYakiniIletisim,
                y => y.Hesap);

            if (model?.Hesap != null)
            {
                var hesapBilgiService = serviceFactory.CreateService<IHesapBilgiService>();

                model.Hesap.HesapBilgiler = hesapBilgiService.List(x => x.HesapId == model.Hesap.HesapId).ToList();
            }

            return model;
        }

        private void GetLists(OgrenciDuzenleViewModel viewModel)
        {
            viewModel.UlkeSelectList = selectListHelper.UlkeSelectList();
            viewModel.NeredenDuydunuzSelectList = selectListHelper.NeredenDuydunuzSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new OgrenciDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Ogrenci>)TempData["OperationResult"];
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
                viewModel.Model = new Ogrenci
                {
                    AnneOgrenciYakiniIletisim = new OgrenciYakiniIletisim(),
                    BabaOgrenciYakiniIletisim = new OgrenciYakiniIletisim(),
                    YakiniOgrenciYakiniIletisim = new OgrenciYakiniIletisim()
                };
            }

            GetLists(viewModel);

            if (string.IsNullOrEmpty(viewModel.Model.OgrenciNo))
            {
                var service = serviceFactory.CreateService<IOgrenciService>();
                viewModel.Model.OgrenciNo = service.GetSonOgrenciNo();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(OgrenciDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        var ayarlar = AyarlarService.Get();

                        if (viewModel.PostedFileGorselDosyaAd != null && viewModel.Model != null)
                        {
                            var oldGorselDosyaAd = viewModel.Model.GorselDosyaAd;
                            var oldGorselYol = viewModel.Model.GorselYol;

                            viewModel.Model.GorselDosyaAd = FileHelper.OgrenciGorselImageName(
                                viewModel.PostedFileGorselDosyaAd,
                                viewModel.Model.AdSoyad);

                            var logoSaveMessageInfo = FileHelper.ImageSave(
                                viewModel.PostedFileGorselDosyaAd,
                                ayarlar.OgrenciGorselYol,
                                viewModel.Model.GorselYol,
                                viewModel.Model.GetDisplayName(x => x.GorselDosyaAd),
                                ayarlar.OgrenciGorselSize);

                            if (viewModel.OperationResult?.MessageInfos == null)
                            {
                                viewModel.OperationResult = new EntityOperationResult<Ogrenci>();
                            }

                            viewModel.OperationResult.MessageInfos.Add(logoSaveMessageInfo);

                            if (logoSaveMessageInfo.MessageInfoType == MessageInfoType.Success)
                            {
                                if (oldGorselYol != null && oldGorselYol.IndexOf("default") == -1)
                                {
                                    FileHelper.IfFileExistsDeleteFile(oldGorselYol);
                                }
                            }
                            else if (!string.Equals(viewModel.Model.GorselDosyaAd, oldGorselDosyaAd))
                            {
                                viewModel.Model.GorselDosyaAd = oldGorselDosyaAd;
                            }
                        }

                        viewModel.OperationResult = viewModel.Model.OgrenciId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.OgrenciId > 0
                            ? RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciId })
                            : RedirectToAction("Duzenle");
                    }
            }

            if (viewModel.Model.OgrenciId > 0)
            {
                return RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciId });
            }
            else
            {
                GetLists(viewModel);

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return operationResult.Status
                       ? RedirectToAction("Duzenle")
                       : RedirectToAction("Duzenle", "Ogrenci", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult TaksitOde(OgrenciDetayViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IHesapHareketService>();
            var serviceOgrenciSozlesmeService = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            var model = service.Get(x => x.HesapHareketId == viewModel.ModelHesapHareket.HesapHareketId, y => y.OgrenciSozlesmeHesapHareket);

            if (model == null)
            {
                var toplamTutar = (decimal)viewModel.TaksitToplamTutar;

                var taksitler = service.List(
                    x =>
                        x.AlacakliHesapId == viewModel.Model.OgrenciId &&
                        !x.IslemGerceklestiMi &&
                        x.HareketTarihi == null,
                    y => y.OgrenciSozlesmeHesapHareket).
                    OrderBy(x => x.VadeTarihi).
                    ToList();

                var guncellenecekHesapHareketler = new List<HesapHareket>();
                var eklenecekHesapHareketler = new List<HesapHareket>();

                var ogrencisozlesme = new OgrenciSozlesme();

                if (taksitler.Any())
                {
                    var sozlesmeId = taksitler[0].OgrenciSozlesmeHesapHareket.OgrenciSozlesmeId;

                    ogrencisozlesme = serviceOgrenciSozlesmeService.Get(x => x.OgrenciSozlesmeId == sozlesmeId);

                    ogrencisozlesme.ToplamOdenen = ogrencisozlesme.ToplamOdenen == null ? 0 : ogrencisozlesme.ToplamOdenen;

                    ogrencisozlesme.ToplamOdenen += toplamTutar;
                    //Öğrenci sözleşme

                    for (var i = 0; i < taksitler.Count; i++)
                    {
                        if (toplamTutar == 0) break;

                        //Öğrenci sözleşme

                        if (toplamTutar >= taksitler[i].Tutar)
                        {
                            taksitler[i].HareketTarihi = viewModel.ModelHesapHareket.HareketTarihi;
                            taksitler[i].BorcluHesapId = viewModel.ModelHesapHareket.BorcluHesapId;
                            taksitler[i].PersonelId = Identity.PersonelId;
                            taksitler[i].IslemGerceklestiMi = true;
                            taksitler[i].Aciklama = $"{taksitler[i].Aciklama} ({Resources.LangResources.MakbuzNo} :{taksitler[i].HesapHareketId})";

                            guncellenecekHesapHareketler.Add(taksitler[i]);

                            toplamTutar -= (decimal)taksitler[i].Tutar;
                        }
                        else
                        {
                            var yeniTaksit = new HesapHareket
                            {
                                VadeTarihi = taksitler[i].VadeTarihi,
                                HareketTarihi = null,
                                Aciklama = $"{taksitler[i].Aciklama} - {viewModel.ModelHesapHareket.Aciklama}",
                                AlacakliHesapId = taksitler[i].AlacakliHesapId,
                                BorcluHesapId = taksitler[i].BorcluHesapId,
                                ParaBirimId = taksitler[i].ParaBirimId,
                                PersonelId = Identity.PersonelId,
                                IslemGerceklestiMi = false,
                                Tutar = taksitler[i].Tutar - toplamTutar
                            };

                            yeniTaksit.OgrenciSozlesmeHesapHareket = new OgrenciSozlesmeHesapHareket
                            {
                                OgrenciSozlesmeId = taksitler[i].OgrenciSozlesmeHesapHareket.OgrenciSozlesmeId,
                                HesapHareketId = yeniTaksit.HesapHareketId,
                                HesapHareket = yeniTaksit
                            };

                            eklenecekHesapHareketler.Add(yeniTaksit);

                            taksitler[i].HareketTarihi = viewModel.ModelHesapHareket.HareketTarihi;
                            taksitler[i].BorcluHesapId = viewModel.ModelHesapHareket.BorcluHesapId;
                            taksitler[i].PersonelId = Identity.PersonelId;
                            taksitler[i].IslemGerceklestiMi = true;
                            taksitler[i].Tutar = toplamTutar;

                            guncellenecekHesapHareketler.Add(taksitler[i]);

                            toplamTutar = 0;
                        }
                    }
                }

                var oeprationResult = service.AddUpdateLists(ogrencisozlesme, eklenecekHesapHareketler, guncellenecekHesapHareketler);

                var taksitHesapHareketIdler = "";

                if (eklenecekHesapHareketler != null && eklenecekHesapHareketler.Count > 0)
                {
                    for (int i = 0; i < eklenecekHesapHareketler.Count; i++)
                    {
                        if (eklenecekHesapHareketler[i].IslemGerceklestiMi)
                            taksitHesapHareketIdler += $"{eklenecekHesapHareketler[i].HesapHareketId},";
                    }
                }

                if (guncellenecekHesapHareketler != null && guncellenecekHesapHareketler.Count > 0)
                {
                    for (int i = 0; i < guncellenecekHesapHareketler.Count; i++)
                    {
                        if (guncellenecekHesapHareketler[i].IslemGerceklestiMi)
                            taksitHesapHareketIdler += $"{guncellenecekHesapHareketler[i].HesapHareketId},";
                    }
                }

                TempData["ODEME"] = taksitHesapHareketIdler;

                TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
                {
                    MessageInfos = oeprationResult.MessageInfos,
                    Status = oeprationResult.Status
                };
            }
            else
            {
                model.Aciklama = $"{viewModel.ModelHesapHareket.Aciklama} ({Resources.LangResources.MakbuzNo} :{viewModel.ModelHesapHareket.HesapHareketId})";
                model.HareketTarihi = viewModel.ModelHesapHareket.HareketTarihi;
                model.BorcluHesapId = viewModel.ModelHesapHareket.BorcluHesapId;
                model.PersonelId = Identity.PersonelId;
                model.IslemGerceklestiMi = true;

                var sozlesmeId = model.OgrenciSozlesmeHesapHareket.OgrenciSozlesmeId;

                var ogrencisozlesme = serviceOgrenciSozlesmeService.Get(x => x.OgrenciSozlesmeId == sozlesmeId);

                ogrencisozlesme.ToplamOdenen = ogrencisozlesme.ToplamOdenen == null ? 0 : ogrencisozlesme.ToplamOdenen;

                ogrencisozlesme.ToplamOdenen += model.Tutar;

                var oeprationResult = service.UpdateWithOgrenciSozlesme(model, ogrencisozlesme);

                TempData["ODEME"] = model.HesapHareketId.ToString();

                TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
                {
                    MessageInfos = oeprationResult.MessageInfos,
                    Status = oeprationResult.Status
                };
            }

            return RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeDosyaGuncelle(HttpPostedFileBase postedFileDosya, int dosyaOgrenciSozlesmeId, int dosyaOgrenciId)
        {
            ModelState.Clear();

            try
            {
                if (postedFileDosya.ContentLength > 0)
                {
                    var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
                    var entity = service.Get(x => x.OgrenciSozlesmeId == dosyaOgrenciSozlesmeId);
                    var ayarlar = AyarlarService.Get();

                    if (entity != null)
                    {
                        var fileExtension = Path.GetExtension(postedFileDosya.FileName);

                        var fileName = $"dosya{dosyaOgrenciSozlesmeId}-{DateTime.Now.ToShortDateString().Replace("/", ".").Replace(" ", "-")}{fileExtension}";

                        entity.DosyaAd = fileName;

                        var fileLocation = Server.MapPath($"~/{entity.DosyaYol}");

                        if (!Directory.Exists(Server.MapPath($"~/{ayarlar.OgrenciSozlesmeDosyaYol}")))
                            Directory.CreateDirectory(Server.MapPath($"~/{ayarlar.OgrenciSozlesmeDosyaYol}"));

                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        postedFileDosya.SaveAs(fileLocation);
                    }

                    var operationResult = service.Update(entity);

                    TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
                    {
                        MessageInfos = operationResult.MessageInfos,
                        Status = operationResult.Status
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = ex.Message, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };
            }

            return RedirectToAction("Detay", "Ogrenci", new { id = dosyaOgrenciId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeOdemeBilgiGuncelle(OgrenciSozlesmeOdemeBilgiGuncelleViewModel viewNodel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var model = service.Get(x => x.OgrenciSozlesmeId == viewNodel.OgrenciSozlesmeId,
                y => y.OgrenciSozlesmeOdemeBilgi,
                y => y.OgrenciSozlesmeHesapHareketler);

            model.OgrenciSozlesmeOdemeBilgi.PesinatHesapId = viewNodel.PesinatHesapId;
            model.OgrenciSozlesmeOdemeBilgi.OgrenciSozlesmeOdemeBilgiSenetImzalayanId = viewNodel.OgrenciSozlesmeOdemeBilgiSenetImzalayanId;
            if (viewNodel.OdenenlerSilinsinMi)
            {
                model.OgrenciSozlesmeOdemeBilgi.PesinatTutar = viewNodel.PesinatTutar;
                model.ToplamOdenen = viewNodel.PesinatTutar;
            }

            // Ödenen taksit varsa, + olacak.
            model.OgrenciSozlesmeOdemeBilgi.TaksitAdet = viewNodel.TaksitAdet;
            model.OgrenciSozlesmeOdemeBilgi.TaksitTutar = viewNodel.TaksitTutar;
            model.OgrenciSozlesmeOdemeBilgi.TaksitBaslangicTarihi = viewNodel.TaksitBaslangicTarihi;
            model.OgrenciSozlesmeOdemeBilgi.Not = viewNodel.Not;

            model.EgitimTutar = viewNodel.EgitimTutar;
            model.YayinTutar = viewNodel.YayinTutar;
            model.YemekTutar = viewNodel.YemekTutar;
            model.ServisTutar = viewNodel.ServisTutar;
            model.KiyafetTutar = viewNodel.KiyafetTutar;
            model.ToplamUcret = viewNodel.ToplamTutar;

            model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler = viewNodel.AylikTaksitBilgiler;

            var oeprationResult = service.UpdateOdemeBilgi(model, viewNodel.OdenenlerSilinsinMi);

            TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
            {
                MessageInfos = oeprationResult.MessageInfos,
                Status = oeprationResult.Status
            };

            return RedirectToAction("Detay", "Ogrenci", new { id = model.OgrenciId });
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeOdemeBilgiGetir(int id)
        {
            var service = serviceFactory.CreateService<IOgrenciSozlesmeOdemeBilgiService>();
            var ogrenciSozlesmeModel = service.Get(x => x.OgrenciSozlesme.OgrenciSozlesmeId == id,
                y => y.OgrenciSozlesme.Ogrenci,
                y => y.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler.Select(z => z.HesapHareket));

            var subeService = serviceFactory.CreateService<ISubeService>();
            var sube = subeService.Get(x => x.SubeId == ogrenciSozlesmeModel.OgrenciSozlesme.SubeId);

            var model = new OgrenciSozlesmeOdemeBilgiGuncelleViewModel
            {
                OgrenciId = ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciId,
                OgrenciSozlesmeId = ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeId,
                SubeId = ogrenciSozlesmeModel.OgrenciSozlesme.SubeId,
                EgitimTutar = ogrenciSozlesmeModel.OgrenciSozlesme.EgitimTutar,
                YayinTutar = ogrenciSozlesmeModel.OgrenciSozlesme.YayinTutar,
                ServisTutar = ogrenciSozlesmeModel.OgrenciSozlesme.ServisTutar == 0
                    ? null
                    : ogrenciSozlesmeModel.OgrenciSozlesme.ServisTutar,
                KiyafetTutar = ogrenciSozlesmeModel.OgrenciSozlesme.KiyafetTutar == 0
                    ? null
                    : ogrenciSozlesmeModel.OgrenciSozlesme.KiyafetTutar,
                YemekTutar = ogrenciSozlesmeModel.OgrenciSozlesme.YemekTutar == 0
                    ? null
                    : ogrenciSozlesmeModel.OgrenciSozlesme.YemekTutar,
                ToplamTutar = ogrenciSozlesmeModel.OgrenciSozlesme.ToplamUcret,
                PesinatTutar = ogrenciSozlesmeModel.PesinatTutar,
                OdenenTutar = ogrenciSozlesmeModel.OgrenciSozlesme.ToplamOdenen,
                KalanTutar = ogrenciSozlesmeModel.OgrenciSozlesme.ToplamKalan,
                Not = ogrenciSozlesmeModel.Not,

                TaksitAdet = ogrenciSozlesmeModel.TaksitAdet,
                TaksitTutar = ogrenciSozlesmeModel.TaksitTutar,
                TaksitBaslangicTarihi = ogrenciSozlesmeModel.TaksitBaslangicTarihi
            };

            model.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
            model.ParaBirimId = ogrenciSozlesmeModel.ParaBirimId;
            model.PesinatHesapSelectList = selectListHelper.SubeAltHesapSelectList(
                ogrenciSozlesmeModel.OgrenciSozlesme.SubeId,
                ogrenciSozlesmeModel.ParaBirimId);
            model.PesinatHesapId = ogrenciSozlesmeModel.PesinatHesapId;
            model.OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList = selectListHelper.OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList();
            model.OgrenciSozlesmeOdemeBilgiSenetImzalayanId = ogrenciSozlesmeModel.OgrenciSozlesmeOdemeBilgiSenetImzalayanId;

            if (ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler != null &&
                ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler.Count > 0)
            {
                var maksTaksit = sube.MaksimumTaksitAdeti ?? 0;

                if (maksTaksit == 0)
                {
                    var ayarlar = AyarlarService.Get();

                    maksTaksit = ayarlar.MaksimumTaksitAdeti;
                }

                ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler = ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler.Where(x => !x.HesapHareket.IslemGerceklestiMi).OrderBy(x => x.HesapHareket.VadeTarihi).ToList();

                model.AylikTaksitBilgiler = new List<AylikTaksitBilgi>();

                for (var i = 0; i < maksTaksit; i++)
                {
                    OgrenciSozlesmeHesapHareket orenciSozlesmeHesapHareket = null;

                    if (ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler.Count > i)
                    {
                        orenciSozlesmeHesapHareket = ogrenciSozlesmeModel.OgrenciSozlesme.OgrenciSozlesmeHesapHareketler[i];
                    }

                    if (orenciSozlesmeHesapHareket != null)
                    {
                        if (orenciSozlesmeHesapHareket.HesapHareket.VadeTarihi == null)
                            continue;

                        var aylikTaksitBilgi = new AylikTaksitBilgi
                        {
                            HesapHareketId = orenciSozlesmeHesapHareket.HesapHareketId,
                            TaksitNo = i + 1,
                            TaksitTutari = orenciSozlesmeHesapHareket.HesapHareket.Tutar,
                            VadeTarihi = orenciSozlesmeHesapHareket.HesapHareket.VadeTarihi,
                        };

                        model.AylikTaksitBilgiler.Add(aylikTaksitBilgi);
                    }
                    else
                    {
                        var aylikTaksitBilgi = new AylikTaksitBilgi
                        {
                            TaksitNo = i + 1
                        };

                        model.AylikTaksitBilgiler.Add(aylikTaksitBilgi);
                    }
                }

                if (model.AylikTaksitBilgiler.Count < maksTaksit)
                {
                    for (var i = model.AylikTaksitBilgiler.Count - 1; i < maksTaksit; i++)
                    {
                        var aylikTaksitBilgi = new AylikTaksitBilgi
                        {
                            TaksitNo = i + 1
                        };

                        model.AylikTaksitBilgiler.Add(aylikTaksitBilgi);
                    }
                }
            }

            return PartialView("~/Views/Shared/_OgrenciSozlesmeOdemeBilgiGuncelleView.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult FaturaBilgiGuncelle(FaturaBilgi model, int faturaBilgiOgrenciSozlesmeId, int faturaBilgiOgrenciId)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IFaturaBilgiService>();
            EntityOperationResult<FaturaBilgi> oeprationResult;

            if (model.FaturaBilgiId == 0)
            {
                oeprationResult = service.Add(model);

                if (oeprationResult.Status)
                {
                    var ogrenciSozlesmeService = serviceFactory.CreateService<IOgrenciSozlesmeService>();
                    var ogrenciSozlesme = ogrenciSozlesmeService.Get(x => x.OgrenciSozlesmeId == faturaBilgiOgrenciSozlesmeId);

                    if (ogrenciSozlesme != null)
                    {
                        ogrenciSozlesme.FaturaBilgiId = oeprationResult.Model.FaturaBilgiId;

                        var result = ogrenciSozlesmeService.Update(ogrenciSozlesme);

                        if (!result.Status)
                        {
                            TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
                            {
                                MessageInfos = result.MessageInfos,
                                Status = result.Status
                            };
                        }
                    }
                }
            }
            else
            {
                oeprationResult = service.Update(model);
            }

            TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
            {
                MessageInfos = oeprationResult.MessageInfos,
                Status = oeprationResult.Status
            };

            return RedirectToAction("Detay", "Ogrenci", new { id = faturaBilgiOgrenciId });
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult FaturaBilgiGetir(int id)
        {
            var service = serviceFactory.CreateService<IFaturaBilgiService>();
            var model = service.Get(x => x.FaturaBilgiId == id);

            return PartialView("~/Views/Shared/_FaturaBilgiView.cshtml", model);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = new OgrenciDetayViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Ogrenci>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
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
            }
            else
            {
                return Redirect("/Error/NotFound");
            }

            if (viewModel.ModelHesapHareket == null)
            {
                var listItems = new List<SelectListItem>();

                var hesapHareketService = serviceFactory.CreateService<IHesapHareketService>();

                var list = hesapHareketService.List(x => x.AlacakliHesapId == id && !x.IslemGerceklestiMi).OrderBy(x => x.VadeTarihi).ToList();

                if (list.Any())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = list[i].HesapHareketId.ToString(),
                            Text = $"{list[i].TutarFormatted} - {list[i].Aciklama}"
                        };

                        listItems.Add(selectListItem);
                    }

                    var selectListItemSon = new SelectListItem
                    {
                        Value = "-100",
                        Text = Resources.LangResources.TaksittenFarkliTutar
                    };

                    listItems.Add(selectListItemSon);
                }

                viewModel.HesapHareketSelectList = listItems;
            }

            viewModel.OgrenciSozlesmeSilModel = new OgrenciSozlesmeSilViewModel
            {
                OgrenciId = (int)id,
                OgrenciSozlesmeId = 0
            };

            viewModel.Model.Hesap.Odenebilir = true;

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new OgrenciListeleViewModel
            {
                Model = new OgrenciDto(),
                EntityPagedDataSource = new EntityPagedDataSource<OgrenciDto>(),
                Search = new Search()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(OgrenciListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IOgrenciService>();

            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult MakbuzYazdir(int id)
        {
            var service = serviceFactory.CreateService<IHesapHareketService>();
            var model = service.Get(x => x.HesapHareketId == id,
                y => y.AlacakliHesap.Sube.Kurum,
                y => y.BorcluHesap);

            if (model != null)
            {
                var hesapHareketService = serviceFactory.CreateService<IHesapHareketService>();
                var hesapHareket = hesapHareketService.Get(x => x.HesapHareketId == id);

                if (hesapHareket != null)
                {
                    model.AlacakliHesap.HesapHareketler = hesapHareketService.List(x =>
                        x.AlacakliHesapId == hesapHareket.AlacakliHesapId,
                        y => y.AlacakliHesap,
                        y => y.BorcluHesap.TransferTip,
                        y => y.BorcluHesap.UstHesap.TransferTip).
                   ToList();
                }
            }
            else
            {
                return Redirect("/Error/NotFound");
            }

            var subeService = serviceFactory.CreateService<ISubeService>();
            var sube = subeService.Get(x => x.SubeId == model.BorcluHesapId, y => y.Kurum);

            if (sube == null)
            {
                sube = subeService.Get(x => x.SubeId == model.BorcluHesap.UstHesapId, y => y.Kurum);
            }

            var persoenlService = serviceFactory.CreateService<IPersonelService>();
            var personel = persoenlService.Get(x => x.PersonelId == model.PersonelId);

            var viewModel = new MakbuzYazdirViewModel
            {
                HesapHareketId = id,
                Model = model.AlacakliHesap,
                Sube = sube,
                Personel = personel
            };

            var appPath = "\\Dosya\\Makbuz";

            var path = Server.MapPath(appPath + "\\MakbuzKontrol.txt");

            string[] lines = null;

            if (System.IO.File.Exists(path))
            {
                using (var streamReader = System.IO.File.OpenText(path))
                {
                    lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
            }

            var kopyaMi = false;

            if (lines != null && lines.Length > 0)
                kopyaMi = lines.Count(x => x.Trim() == id.ToString()) > 0;

            if (!Directory.Exists(Server.MapPath(appPath)))
                Directory.CreateDirectory(Server.MapPath(appPath));

            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(id + "\r\n");
                sw.Flush();
                sw.Close();
            }

            viewModel.KopyaMi = kopyaMi;

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult ExcelIndir()
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("SubeId",Identity.SubeId),
            };

            OgrenciListeleViewModel viewModel = new OgrenciListeleViewModel
            {
                SearchText = "",
                Draw = 2,
                Length = 99999,
                Start = 0
            };

            var service = serviceFactory.CreateService<IOgrenciService>();
            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[10] {
                      new DataColumn(FieldNameResources.AdSoyad),
                      new DataColumn(FieldNameResources.OgrenciNo),
                      new DataColumn(FieldNameResources.SubeAd),
                      new DataColumn(FieldNameResources.TcNo),
                      new DataColumn(FieldNameResources.CepTelefonu),
                      new DataColumn(FieldNameResources.Eposta),
                      new DataColumn(FieldNameResources.IlceAd),
                      new DataColumn(FieldNameResources.SehirAd),
                      new DataColumn(FieldNameResources.DogumTarihi),
                      new DataColumn(FieldNameResources.OlusturulmaTarihi)
            });

            foreach (var item in pagedDataSource.data)
            {
                dt.Rows.Add(
                    item.AdSoyad,
                    item.OgrenciNo,
                    item.SubeAd,
                    item.TcKimlikNo,
                    item.CepTelefon,
                    item.Eposta,
                    item.IlceAd,
                    item.SehirAd,
                    item.DogumTarihiFormatted,
                    item.OlusturulmaTarihiFormatted);
            }

            dt.TableName = "Ogrenciler";

            var file = ExcelHelper.ExcelIndir(new List<DataTable> { dt }, "Ogrenciler");

            return File(file.FileContents, file.ContentType, file.FileDownLoadName);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult TumExcelIndir()
        {
            var ogrenciService = serviceFactory.CreateService<IOgrenciService>();
            var hesapHareketService = serviceFactory.CreateService<IHesapHareketService>();
            var ogrenciSozlesmeService = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var ogrenciSinavKontrolService = serviceFactory.CreateService<IOgrenciSinavKontrolService>();

            var ogrenciler = ogrenciService.List(
                null,
                x => x.AnneOgrenciYakiniIletisim,
                x => x.BabaOgrenciYakiniIletisim,
                x => x.YakiniOgrenciYakiniIletisim,
                x => x.EkleyenPersonel,
                x => x.Hesap,
                x => x.Ilce.Sehir,
                x => x.NeredenDuydunuz,
                x => x.Sehir.Ulke,
                x => x.Sube);

            if (ogrenciler != null && ogrenciler.Any())
            {
                foreach (var ogrenci in ogrenciler)
                {
                    ogrenci.Hesap.HesapHareketler = hesapHareketService.List(
                        x => x.AlacakliHesapId == ogrenci.OgrenciId ||
                        x.BorcluHesapId == ogrenci.OgrenciId,
                        y => y.AlacakliHesap.UstHesap,
                        y => y.BorcluHesap.UstHesap,
                        y => y.ParaBirim,
                        y => y.Personel,
                        y => y.OgrenciSozlesmeHesapHareket).ToList();

                    ogrenci.OgrenciSozlesmeler = ogrenciSozlesmeService.List(
                        x => x.OgrenciId == ogrenci.OgrenciId,
                        y => y.EkleyenPersonel,
                        y => y.OgrenciSozlesmeTur,
                        y => y.Sube,
                        y => y.Sezon,
                        y => y.Brans,
                        y => y.SinifSeviye,
                        y => y.Sinif.SinifTur,
                        y => y.Sinif.SinifSeviye,
                        y => y.Sinif.SinifSeans,
                        y => y.Sinif.Derslik,
                        y => y.Sinif.Personel,
                        y => y.OkulTur,
                        y => y.Servis,
                        y => y.OzelDersDurum,
                        y => y.DanismanPersonel,
                        y => y.GorusenPersonel,
                        y => y.KurumaGetirenPersonel,
                        y => y.FaturaBilgi,
                        y => y.OgrenciSozlesmeOdemeBilgi.ParaBirim,
                        y => y.OgrenciSozlesmeOdemeBilgi.PesinatHesap.UstHesap,
                        y => y.OgrenciSozlesmeOdemeBilgi.PesinatHesap.TransferTip,
                        y => y.OgrenciSozlesmeOdemeBilgi.OgrenciSozlesmeOdemeBilgiSenetImzalayan).ToList();
                }
            }

            var ogrenciSinavKontroller = ogrenciSinavKontrolService.List(
                null,
                y => y.Sube,
                y => y.SinavKitapcik.Sinav,
                y => y.OgrenciSinavKontrolDersBilgiler.Select(z => z.Ders.DersGrup),
                y => y.OgrenciSinavKontrolPuanTurPuanlar.Select(z => z.PuanTur)).ToList();

            #region Öğrenciler DataTable

            DataTable dtOgrenciler = new DataTable
            {
                TableName = "Ogrenciler"
            };

            dtOgrenciler.Columns.AddRange(
                new DataColumn[] {
                      new DataColumn("Id"),
                      new DataColumn("Öğrenci Ad Soyad"),
                      new DataColumn("Öğrenci TC"),
                      new DataColumn("Öğrenci No"),
                      new DataColumn("Öğrenci CepTelefon"),
                      new DataColumn("Öğrenci Eposta"),
                      new DataColumn("Öğrenci Adres"),
                      new DataColumn("Öğrenci PostaKodu"),
                      new DataColumn("Öğrenci İlçe"),
                      new DataColumn("Öğrenci Şehir"),
                      new DataColumn("Öğrenci Ülke"),
                      new DataColumn("Öğrenci Not"),
                      new DataColumn("Öğrenci Cinsiyet"),
                      new DataColumn("Öğrenci Doğum Tarihi"),
                      new DataColumn("Öğrenci Şube"),
                      new DataColumn("Öğrenci Kaydı Oluşturulma Tarihi"),
                      new DataColumn("Öğrenci Kaydı Ekleyen Personel"),

                      new DataColumn("Anne Ad Soyad"),
                      new DataColumn("Anne TC"),
                      new DataColumn("Anne CepTelefon1"),
                      new DataColumn("Anne CepTelefon2"),
                      new DataColumn("Anne EvTelefon"),
                      new DataColumn("Anne IsTelefon"),
                      new DataColumn("Anne Eposta"),
                      new DataColumn("Anne EvAdres"),
                      new DataColumn("Anne IsAdres"),
                      new DataColumn("Anne Doğum Tarihi"),

                      new DataColumn("Baba Ad Soyad"),
                      new DataColumn("Baba TC"),
                      new DataColumn("Baba CepTelefon1"),
                      new DataColumn("Baba CepTelefon2"),
                      new DataColumn("Baba EvTelefon"),
                      new DataColumn("Baba IsTelefon"),
                      new DataColumn("Baba Eposta"),
                      new DataColumn("Baba EvAdres"),
                      new DataColumn("Baba IsAdres"),
                      new DataColumn("Baba Doğum Tarihi"),

                      new DataColumn("Yakını Ad Soyad"),
                      new DataColumn("Yakını TC"),
                      new DataColumn("Yakını CepTelefon1"),
                      new DataColumn("Yakını CepTelefon2"),
                      new DataColumn("Yakını EvTelefon"),
                      new DataColumn("Yakını IsTelefon"),
                      new DataColumn("Yakını Eposta"),
                      new DataColumn("Yakını EvAdres"),
                      new DataColumn("Yakını IsAdres"),
                      new DataColumn("Yakını Doğum Tarihi"),

                      new DataColumn("İletişim Kendi"),
                      new DataColumn("İletişim Anne"),
                      new DataColumn("İletişim Baba"),
                      new DataColumn("İletişim Yakını"),

                      new DataColumn("Nereden Duydunuz")
            });

            foreach (var ogrenci in ogrenciler)
            {
                if (ogrenci.AnneOgrenciYakiniIletisim == null)
                    ogrenci.AnneOgrenciYakiniIletisim = new OgrenciYakiniIletisim();

                if (ogrenci.BabaOgrenciYakiniIletisim == null)
                    ogrenci.BabaOgrenciYakiniIletisim = new OgrenciYakiniIletisim();

                if (ogrenci.YakiniOgrenciYakiniIletisim == null)
                    ogrenci.YakiniOgrenciYakiniIletisim = new OgrenciYakiniIletisim();

                dtOgrenciler.Rows.Add(
                    ogrenci.OgrenciId,
                    ogrenci.AdSoyad,
                    ogrenci.TcKimlikNo,
                    ogrenci.OgrenciNo,
                    ogrenci.CepTelefon,
                    ogrenci.Eposta,
                    ogrenci.Adres,
                    ogrenci.PostaKodu,
                    ogrenci.Ilce != null ? ogrenci.Ilce.IlceAd : string.Empty,
                    ogrenci.Sehir != null ? ogrenci.Sehir.SehirAd : string.Empty,
                    ogrenci.Ulke != null ? ogrenci.Ulke.UlkeAd : string.Empty,
                    ogrenci.Not,
                    ogrenci.Cinsiyet,
                    ogrenci.DogumTarihiFormatted,
                    ogrenci.Sube != null ? ogrenci.Sube.SubeAd : string.Empty,
                    ogrenci.OlusturulmaTarihiFormatted,
                    ogrenci.EkleyenPersonel != null ? ogrenci.EkleyenPersonel.AdSoyad : string.Empty,

                    ogrenci.AnneOgrenciYakiniIletisim.AdSoyad,
                    ogrenci.AnneOgrenciYakiniIletisim.TcKimlikNo,
                    ogrenci.AnneOgrenciYakiniIletisim.CepTelefon1,
                    ogrenci.AnneOgrenciYakiniIletisim.CepTelefon2,
                    ogrenci.AnneOgrenciYakiniIletisim.EvTelefon,
                    ogrenci.AnneOgrenciYakiniIletisim.IsTelefon,
                    ogrenci.AnneOgrenciYakiniIletisim.Eposta,
                    ogrenci.AnneOgrenciYakiniIletisim.EvAdres,
                    ogrenci.AnneOgrenciYakiniIletisim.IsAdres,
                    ogrenci.AnneOgrenciYakiniIletisim.DogumTarihiFormatted,

                    ogrenci.BabaOgrenciYakiniIletisim.AdSoyad,
                    ogrenci.BabaOgrenciYakiniIletisim.TcKimlikNo,
                    ogrenci.BabaOgrenciYakiniIletisim.CepTelefon1,
                    ogrenci.BabaOgrenciYakiniIletisim.CepTelefon2,
                    ogrenci.BabaOgrenciYakiniIletisim.EvTelefon,
                    ogrenci.BabaOgrenciYakiniIletisim.IsTelefon,
                    ogrenci.BabaOgrenciYakiniIletisim.Eposta,
                    ogrenci.BabaOgrenciYakiniIletisim.EvAdres,
                    ogrenci.BabaOgrenciYakiniIletisim.IsAdres,
                    ogrenci.BabaOgrenciYakiniIletisim.DogumTarihiFormatted,

                    ogrenci.YakiniOgrenciYakiniIletisim.AdSoyad,
                    ogrenci.YakiniOgrenciYakiniIletisim.TcKimlikNo,
                    ogrenci.YakiniOgrenciYakiniIletisim.CepTelefon1,
                    ogrenci.YakiniOgrenciYakiniIletisim.CepTelefon2,
                    ogrenci.YakiniOgrenciYakiniIletisim.EvTelefon,
                    ogrenci.YakiniOgrenciYakiniIletisim.IsTelefon,
                    ogrenci.YakiniOgrenciYakiniIletisim.Eposta,
                    ogrenci.YakiniOgrenciYakiniIletisim.EvAdres,
                    ogrenci.YakiniOgrenciYakiniIletisim.IsAdres,
                    ogrenci.YakiniOgrenciYakiniIletisim.DogumTarihiFormatted,

                    ogrenci.IletisimKendi ? "Evet" : "Hayır",
                    ogrenci.IletisimAnne ? "Evet" : "Hayır",
                    ogrenci.IletisimBaba ? "Evet" : "Hayır",
                    ogrenci.IletisimYakini ? "Evet" : "Hayır",

                    ogrenci.NeredenDuydunuz != null ? ogrenci.NeredenDuydunuz.NeredenDuydunuzBaslik : string.Empty
                  );
            }

            #endregion

            #region Hesap Hareketler DataTable

            DataTable dtHesapHareketler = new DataTable
            {
                TableName = "HesapHareketler"
            };

            dtHesapHareketler.Columns.AddRange(
                new DataColumn[] {
                      new DataColumn("Öğrenci Id"),
                      new DataColumn("Sözleşme Id"),
                      new DataColumn("Para Birim"),
                      new DataColumn("Şube"),
                      new DataColumn("Şube Kasa"),
                      new DataColumn("Personel"),
                      new DataColumn("Tutar"),
                      new DataColumn("Açıklama"),
                      new DataColumn("Vade Tarihi"),
                      new DataColumn("Hareket Tarihi"),
            });

            foreach (var ogrenci in ogrenciler)
            {
                if (ogrenci.Hesap == null || ogrenci.Hesap.HesapHareketler == null)
                    continue;

                foreach (var hesapHareket in ogrenci.Hesap.HesapHareketler)
                {
                    var ogrenciSozlesmeId = hesapHareket.OgrenciSozlesmeHesapHareket?.OgrenciSozlesme != null
                        ? hesapHareket.OgrenciSozlesmeHesapHareket.OgrenciSozlesme.OgrenciSozlesmeId.ToString()
                        : string.Empty;

                    var sube = hesapHareket.BorcluHesapId == ogrenci.OgrenciId
                        ? hesapHareket.AlacakliHesap.HesapBaslik
                        : hesapHareket.BorcluHesap.UstHesap != null
                            ? hesapHareket.BorcluHesap.UstHesap.HesapBaslik
                            : hesapHareket.BorcluHesap.HesapBaslik;

                    var subeKasa = hesapHareket.BorcluHesapId == ogrenci.OgrenciId
                        ? hesapHareket.AlacakliHesap.HesapBaslik
                        : hesapHareket.BorcluHesap.UstHesap != null
                            ? hesapHareket.BorcluHesap.HesapBaslik
                            : string.Empty;

                    dtHesapHareketler.Rows.Add(
                        ogrenci.OgrenciId,
                        ogrenciSozlesmeId,
                        hesapHareket.ParaBirim.ParaBirimAd,
                        sube,
                        subeKasa,
                        hesapHareket.PersonelAdSoyad,
                        hesapHareket.TutarFormatted,
                        hesapHareket.Aciklama,
                        hesapHareket.VadeTarihiFormatted,
                        hesapHareket.HareketTarihiFormatted);
                }
            }

            #endregion

            #region Öğrenci Sözleşme DataTable

            DataTable dtOgrenciSozlesmeler = new DataTable
            {
                TableName = "Sözlesmeler"
            };
            dtOgrenciSozlesmeler.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("Öğrenci Id"),
                    new DataColumn("Sözleşme Id"),

                    new DataColumn("Şube"),
                    new DataColumn("Ekleyen Personel"),
                    new DataColumn("Danışman Personel"),
                    new DataColumn("Görüşen Personel"),
                    new DataColumn("Kuruma Getiren Personel"),
                    new DataColumn("Referans"),
                    new DataColumn("Sözleşme Tür"),
                    new DataColumn("Sezon"),
                    new DataColumn("Branş"),
                    new DataColumn("Sınıf Tür"),
                    new DataColumn("Sınıf Seans"),
                    new DataColumn("Sınıf Seviye"),
                    new DataColumn("Sınıf Derslik"),
                    new DataColumn("Sınıf"),
                    new DataColumn("Okul Tür"),

                    new DataColumn("Servis"),
                    new DataColumn("Özel Ders Durum"),
                    new DataColumn("Not"),
                    new DataColumn("Yemek Dahil Mi"),
                    new DataColumn("Ders Adeti"),
                    new DataColumn("Ders Birim Fiyat"),

                    new DataColumn("FaturaBilgi Ad Soyad"),
                    new DataColumn("FaturaBilgi Vergi Dairesi"),
                    new DataColumn("FaturaBilgi Vergi No"),
                    new DataColumn("FaturaBilgi Adres"),

                    new DataColumn("Sözleşme Eğitim Tutar"),
                    new DataColumn("Sözleşme Yayın Tutar"),
                    new DataColumn("Sözleşme Servis Tutar"),
                    new DataColumn("Sözleşme Kıyafet Tutar   "),
                    new DataColumn("Sözleşme Yemek Tutar"),
                    new DataColumn("Sözleşme Toplam Tutar"),

                    new DataColumn("Sözleşme Oluşturulma Tarihi"),

                    new DataColumn("SözleşmeÖdemeB. ParaBirim"),
                    new DataColumn("SözleşmeÖdemeB. Peşinat Hesap"),
                    new DataColumn("SözleşmeÖdemeB. Peşinat Hesap Transfer Tip"),
                    new DataColumn("SözleşmeÖdemeB. Peşinat Tutar"),
                    new DataColumn("SözleşmeÖdemeB. Taksit Tutar"),
                    new DataColumn("SözleşmeÖdemeB. Taksit Adet"),
                    new DataColumn("SözleşmeÖdemeB. Not"),
                    new DataColumn("SözleşmeÖdemeB. Taksit Başlangıç Tarihi"),
                    new DataColumn("SözleşmeÖdemeB. Son Güncelleyen Personel"),
                    new DataColumn("SözleşmeÖdemeB. Son Güncelleme Tarihi"),
            });

            foreach (var ogrenci in ogrenciler)
            {
                if (ogrenci.OgrenciSozlesmeler == null || !ogrenci.OgrenciSozlesmeler.Any())
                    continue;

                foreach (var ogrenciSozlesme in ogrenci.OgrenciSozlesmeler)
                {
                    var sinifTurAd = string.Empty;
                    var sinifSeansad = string.Empty;
                    var derslikAd = string.Empty;

                    if (ogrenciSozlesme.Sinif != null)
                    {
                        sinifTurAd = ogrenciSozlesme.Sinif.SinifTurAd;
                        sinifSeansad = ogrenciSozlesme.Sinif.SinifSeansAd;
                        derslikAd = ogrenciSozlesme.Sinif.DerslikAd;
                    }

                    var faturaBilgiAdSoyad = string.Empty;
                    var faturaVergiDairesi = string.Empty;
                    var faturaBilgiVergiNo = string.Empty;
                    var faturaBilgiAdres = string.Empty;

                    if (ogrenciSozlesme.FaturaBilgi != null)
                    {
                        faturaBilgiAdSoyad = ogrenciSozlesme.FaturaBilgi.AdSoyad;
                        faturaVergiDairesi = ogrenciSozlesme.FaturaBilgi.VergiDairesi;
                        faturaBilgiVergiNo = ogrenciSozlesme.FaturaBilgi.VergiNo;
                        faturaBilgiAdres = ogrenciSozlesme.FaturaBilgi.Adres;
                    }

                    var sozlesmeOdemeBilgiParaBirim = string.Empty;
                    var sozlesmeOdemeBilgiPesinatHesap = string.Empty;
                    var sozlesmeOdemeBilgiPesinatHesapTransferTip = string.Empty;
                    var sozlesmeOdemeBilgiPesinatTutar = string.Empty;
                    var sozlesmeOdemeBilgiTaksitTutar = string.Empty;
                    var sozlesmeOdemeBilgiTaksitAdet = string.Empty;
                    var sozlesmeOdemeBilgiNot = string.Empty;
                    var sozlesmeOdemeBilgiTaksitBaslangicTarihi = string.Empty;
                    var sozlesmeOdemeBilgiSonGuncelleyenPersonel = string.Empty;
                    var sozlesmeOdemeBilgiSonGuncellemeTarihi = string.Empty;

                    if (ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi != null)
                    {
                        sozlesmeOdemeBilgiParaBirim = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.ParaBirim?.ParaBirimAd ?? string.Empty;
                        sozlesmeOdemeBilgiPesinatHesap = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatHesap?.HesapBaslik ?? string.Empty;
                        sozlesmeOdemeBilgiPesinatHesapTransferTip = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatHesap?.TransferTip?.TransferTipAd ?? string.Empty;
                        sozlesmeOdemeBilgiPesinatTutar = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatTutarFormatted;
                        sozlesmeOdemeBilgiTaksitTutar = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitTutarFormatted;
                        sozlesmeOdemeBilgiTaksitAdet = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitAdet?.ToString() ?? string.Empty;
                        sozlesmeOdemeBilgiNot = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.Not ?? string.Empty;
                        sozlesmeOdemeBilgiTaksitBaslangicTarihi = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitBaslangicTarihiFormatted;
                        sozlesmeOdemeBilgiSonGuncelleyenPersonel = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.SonGuncelleyenPersonel?.AdSoyad ?? string.Empty;
                        sozlesmeOdemeBilgiSonGuncellemeTarihi = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.SonGuncellenmeTarihiFormatted;
                    }

                    dtOgrenciSozlesmeler.Rows.Add(
                        ogrenciSozlesme.OgrenciId,
                        ogrenciSozlesme.OgrenciSozlesmeId,
                        ogrenciSozlesme.SubeAd,
                        ogrenciSozlesme.EkleyenPersonel.AdSoyad,
                        ogrenciSozlesme.DanismanPersonelAdSoyad,
                        ogrenciSozlesme.GorusenPersonelAdSoyad,
                        ogrenciSozlesme.KurumaGetirenPersonelAdSoyad,
                        ogrenciSozlesme.Referans,
                        ogrenciSozlesme.OgrenciSozlesmeTurAd,
                        ogrenciSozlesme.SezonAd,
                        ogrenciSozlesme.BransAd,
                        sinifTurAd,
                        sinifSeansad,
                        ogrenciSozlesme.SinifSeviyeAd,
                        derslikAd,
                        ogrenciSozlesme.SinifAd,
                        ogrenciSozlesme.OkulTurAd,
                        ogrenciSozlesme.ServisAd,
                        ogrenciSozlesme.OzelDersDurumAd,
                        ogrenciSozlesme.Not,
                        ogrenciSozlesme.YemekDahilMi ? "Evet" : "Hayır",
                        ogrenciSozlesme.DersAdeti?.ToString() ?? string.Empty,
                        ogrenciSozlesme.DersBirimFiyat?.ToString() ?? string.Empty,
                        faturaBilgiAdSoyad,
                        faturaVergiDairesi,
                        faturaBilgiVergiNo,
                        faturaBilgiAdres,
                        ogrenciSozlesme.EgitimTutarFormatted,
                        ogrenciSozlesme.YayinTutarFormatted,
                        ogrenciSozlesme.ServisTutarFormatted,
                        ogrenciSozlesme.KiyafetTutarFormatted,
                        ogrenciSozlesme.YemekTutarFormatted,
                        ogrenciSozlesme.ToplamUcretFormatted,
                        ogrenciSozlesme.OlusturulmaTarihiFormatted,
                        sozlesmeOdemeBilgiParaBirim,
                        sozlesmeOdemeBilgiPesinatHesap,
                        sozlesmeOdemeBilgiPesinatHesapTransferTip,
                        sozlesmeOdemeBilgiPesinatTutar,
                        sozlesmeOdemeBilgiTaksitTutar,
                        sozlesmeOdemeBilgiTaksitAdet,
                        sozlesmeOdemeBilgiNot,
                        sozlesmeOdemeBilgiTaksitBaslangicTarihi,
                        sozlesmeOdemeBilgiSonGuncelleyenPersonel,
                        sozlesmeOdemeBilgiSonGuncellemeTarihi);
                }
            }

            #endregion

            #region Öğrenci Sınav Kontroller DataTable

            DataTable dtOgrenciSinavKontroller = new DataTable
            {
                TableName = "Sınavlar"
            };

            dtOgrenciSinavKontroller.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("Öğrenci Id"),
                    new DataColumn("Şube"),

                    new DataColumn("Ad Soyad"),
                    new DataColumn("TC"),
                    new DataColumn("Öğrenci No"),
                    new DataColumn("Sınıf"),
                    new DataColumn("Cinsiyet"),
                    new DataColumn("Durum"),

                    new DataColumn("Sınav"),
                    new DataColumn("Sınav Tarihi"),
                    new DataColumn("Sınav Sezon"),
                    new DataColumn("Kitapçık"),
                    new DataColumn("Cevap Anahtarı"),

                    new DataColumn("Doğrulamalar"),
                    new DataColumn("Cevaplar"),
                    new DataColumn("Doğru"),
                    new DataColumn("Yanlış"),
                    new DataColumn("Boş"),
                    new DataColumn("Net"),
                    new DataColumn("Başarı")
            });

            if (ogrenciSinavKontroller != null && ogrenciSinavKontroller.Count > 0)
            {
                var maksDersBilgiCount = ogrenciSinavKontroller.Max(x => x.OgrenciSinavKontrolDersBilgiler.Count());
                var maksPuanTur = ogrenciSinavKontroller.Max(x => x.OgrenciSinavKontrolPuanTurPuanlar.Count());

                for (int i = 0; i < maksDersBilgiCount; i++)
                {
                    dtOgrenciSinavKontroller.Columns.AddRange(
                    new DataColumn[] {
                    new DataColumn($"DersBilgi - {i+1} Ders"),
                    new DataColumn($"DersBilgi - {i+1} Ders Grup"),
                    new DataColumn($"DersBilgi - {i+1} Soru Cevaplar"),
                    new DataColumn($"DersBilgi - {i+1} Doğru"),
                    new DataColumn($"DersBilgi - {i+1} Yanlış"),
                    new DataColumn($"DersBilgi - {i+1} Boş"),
                    new DataColumn($"DersBilgi - {i+1} Net")
                    });
                }

                for (int i = 0; i < maksPuanTur; i++)
                {
                    dtOgrenciSinavKontroller.Columns.AddRange(
                    new DataColumn[] {
                    new DataColumn($"PuanTür - {i+1} Ad"),
                    new DataColumn($"PuanTür - {i+1} Puan"),
                    new DataColumn($"PuanTür - {i+1} Toplam Puan"),
                    new DataColumn($"PuanTür - {i+1} Sınıf Sıra"),
                    new DataColumn($"PuanTür - {i+1} Şube Sıra"),
                    new DataColumn($"PuanTür - {i+1} Genel Sıra")
                    });
                }

                foreach (var ogrenciSinavKontrol in ogrenciSinavKontroller)
                {
                    var sinavKitapcik = ogrenciSinavKontrol.SinavKitapcik;
                    var sinav = sinavKitapcik.Sinav;

                    DataRow row = dtOgrenciSinavKontroller.NewRow();
                    dtOgrenciSinavKontroller.Rows.Add(row);

                    row[0] = ogrenciSinavKontrol.OgrenciId?.ToString() ?? string.Empty;
                    row[1] = ogrenciSinavKontrol.Sube.SubeAd;
                    row[2] = ogrenciSinavKontrol.AdSoyad;
                    row[3] = ogrenciSinavKontrol.TcKimlikNo;
                    row[4] = ogrenciSinavKontrol.OgrenciNo;
                    row[5] = ogrenciSinavKontrol.Sinif;
                    row[6] = ogrenciSinavKontrol.Cinsiyet;
                    row[7] = ogrenciSinavKontrol.Durum;
                    row[8] = sinav.Baslik;
                    row[9] = sinav.SinavTarihiFormatted;
                    row[10] = sinav.Sezon?.SezonAd ?? string.Empty;
                    row[11] = sinavKitapcik.Baslik;
                    row[12] = sinavKitapcik.CevapAnahtari;
                    row[13] = ogrenciSinavKontrol.Dogrulamalar;
                    row[14] = ogrenciSinavKontrol.SoruCevaplar;
                    row[15] = ogrenciSinavKontrol.DogruCevapAdet;
                    row[16] = ogrenciSinavKontrol.YanlisCevapAdet;
                    row[17] = ogrenciSinavKontrol.BosCevapAdet;
                    row[18] = ogrenciSinavKontrol.NetFormatted;
                    row[19] = ogrenciSinavKontrol.Basari;

                    var count = 0;

                    for (int i = 0; i < maksDersBilgiCount; i++)
                    {
                        if (i == maksDersBilgiCount - 1)
                            count = i * 7 + 27;

                        if (i > ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler.Count - 1)
                        {
                            row[i * 7 + 20] = string.Empty;
                            row[i * 7 + 21] = string.Empty;
                            row[i * 7 + 22] = string.Empty;
                            row[i * 7 + 23] = string.Empty;
                            row[i * 7 + 24] = string.Empty;
                            row[i * 7 + 25] = string.Empty;
                            row[i * 7 + 26] = string.Empty;

                            continue;
                        }

                        row[i * 7 + 20] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].Ders?.DersAd ?? string.Empty;
                        row[i * 7 + 21] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].Ders?.DersGrup?.DersGrupAd ?? string.Empty;
                        row[i * 7 + 22] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].SoruCevaplar ?? string.Empty;
                        row[i * 7 + 23] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].DogruCevapAdet;
                        row[i * 7 + 24] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].YanlisCevapAdet;
                        row[i * 7 + 25] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].BosCevapAdet;
                        row[i * 7 + 26] = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler[i].NetFormatted;
                    }

                    for (int i = 0; i < maksPuanTur; i++)
                    {
                        if (i > ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar.Count - 1)
                        {
                            row[i * 6 + count + 0] = string.Empty;
                            row[i * 6 + count + 1] = string.Empty;
                            row[i * 6 + count + 2] = string.Empty;
                            row[i * 6 + count + 3] = string.Empty;
                            row[i * 6 + count + 4] = string.Empty;
                            row[i * 6 + count + 5] = string.Empty;

                            continue;
                        }

                        row[i * 6 + count + 0] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].PuanTur?.PuanTurAd ?? string.Empty;
                        row[i * 6 + count + 1] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].Puan;
                        row[i * 6 + count + 2] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].ToplamPuan;
                        row[i * 6 + count + 3] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].SinifSira;
                        row[i * 6 + count + 4] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].SubeSira;
                        row[i * 6 + count + 5] = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[i].GenelSira;
                    }
                }
            }

            #endregion

            var dataTables = new List<DataTable> { dtOgrenciler, dtHesapHareketler, dtOgrenciSozlesmeler, dtOgrenciSinavKontroller };
            var file = ExcelHelper.ExcelIndir(dataTables, "Ogrenciler");

            return File(file.FileContents, file.ContentType, file.FileDownLoadName);
        }
    }
}