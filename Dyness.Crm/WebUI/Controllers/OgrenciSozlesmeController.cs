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
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class OgrenciSozlesmeController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public OgrenciSozlesmeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        public Ogrenci GetModel(int id)
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
                y => y.YakiniOgrenciYakiniIletisim);

            return model;
        }

        private void GetLists(OgrenciSozlesmeDuzenleViewModel viewModel)
        {
            var subeService = serviceFactory.CreateService<ISubeService>();
            var subeler = subeService.List(x => x.EtkinMi).OrderBy(x => x.SubeAd);
            viewModel.Subeler = subeler.ToList();

            viewModel.OgrenciSozlesmeTurSelectList = selectListHelper.OgrenciSozlesmeTurSelectList();
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.SezonSelectList = selectListHelper.SezonSelectList();
            viewModel.UlkeSelectList = selectListHelper.UlkeSelectList();
            viewModel.NeredenDuydunuzSelectList = selectListHelper.NeredenDuydunuzSelectList();
            viewModel.OkulTurSelectList = selectListHelper.OkulTurSelectList();
            viewModel.EtkinlikSelectList = selectListHelper.EtkinlikSelectList();
            viewModel.EhliyetTurSelectList = selectListHelper.EhliyetTurSelectList();
            viewModel.SinifSeviyeSelectList = selectListHelper.SinifSeviyeSelectList();
            viewModel.BransSelectList = selectListHelper.BransSelectList();
            viewModel.ParaBirimSelectList = selectListHelper.ParaBirimSelectList();
            viewModel.OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList = selectListHelper.OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList();

            #region Yayinlar

            var yayinService = serviceFactory.CreateService<IYayinService>();
            var yayinlar = yayinService.List(x =>
                x.EtkinMi,
                y => y.Brans,
                y => y.Ders,
                y => y.SinifSeviye).
            Where(x => x.StoktaVarmi).
            OrderBy(x => x.YayinAd);

            if (viewModel.Model.OgrenciSozlesmeYayinlar == null)
                viewModel.Model.OgrenciSozlesmeYayinlar = new List<OgrenciSozlesmeYayin>();

            foreach (var yayin in yayinlar)
            {
                var existsCount = viewModel.Model.OgrenciSozlesmeYayinlar.Count(x => x.YayinId == yayin.YayinId);

                if (existsCount > 0)
                    continue;

                var ogrenciSozlesmeYayin = new OgrenciSozlesmeYayin
                {
                    Yayin = yayin,
                    YayinId = yayin.YayinId
                };

                viewModel.Model.OgrenciSozlesmeYayinlar.Add(ogrenciSozlesmeYayin);
            }

            if (viewModel.Model.OgrenciSozlesmeYayinlar != null && viewModel.Model.OgrenciSozlesmeYayinlar.Count > 0 && viewModel.Model.OgrenciSozlesmeYayinlar.Count(x => x.Yayin == null) > 0)
            {
                for (int i = 0; i < viewModel.Model.OgrenciSozlesmeYayinlar.Count; i++)
                {
                    var ogrenciSozlesmeYayin = viewModel.Model.OgrenciSozlesmeYayinlar[i];

                    if (ogrenciSozlesmeYayin.Yayin != null)
                        continue;

                    ogrenciSozlesmeYayin.Yayin = yayinlar.FirstOrDefault(x => x.YayinId == ogrenciSozlesmeYayin.YayinId);
                }
            }

            #endregion

            #region Kiyafetler

            var kiyafetService = serviceFactory.CreateService<IKiyafetService>();
            var kiyafetler = kiyafetService.List(x =>
                x.EtkinMi,
                y => y.KiyafetBeden,
                y => y.KiyafetTur).
            Where(x => x.StoktaVarmi).
            OrderBy(x => x.KiyafetAd);

            if (viewModel.Model.OgrenciSozlesmeKiyafetDurumlar == null)
                viewModel.Model.OgrenciSozlesmeKiyafetDurumlar = new List<OgrenciSozlesmeKiyafetDurum>();

            foreach (var kiyafet in kiyafetler)
            {
                var existsCount = viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Count(x => x.KiyafetId == kiyafet.KiyafetId);

                if (existsCount > 0)
                    continue;

                var ogrenciSozlesmeKiyafetDurum = new OgrenciSozlesmeKiyafetDurum
                {
                    Kiyafet = kiyafet,
                    KiyafetId = kiyafet.KiyafetId
                };

                viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Add(ogrenciSozlesmeKiyafetDurum);
            }

            if (viewModel.Model.OgrenciSozlesmeKiyafetDurumlar != null &&
                viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Count > 0 &&
                viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Count(x => x.Kiyafet == null) > 0)
            {
                for (int i = 0; i < viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Count; i++)
                {
                    var ogrenciSozlesmeOkulDetayKiyafetDurum = viewModel.Model.OgrenciSozlesmeKiyafetDurumlar[i];

                    if (ogrenciSozlesmeOkulDetayKiyafetDurum.Kiyafet != null)
                        continue;

                    ogrenciSozlesmeOkulDetayKiyafetDurum.Kiyafet = kiyafetler.FirstOrDefault(x => x.KiyafetId == ogrenciSozlesmeOkulDetayKiyafetDurum.KiyafetId);
                }
            }

            #endregion

            #region Dersler

            var selectedDersItems = viewModel.Model?.OgrenciSozlesmeDersSecimler != null &&
                    (viewModel.Model?.OgrenciSozlesmeDersSecimler).Any()
                        ? viewModel.Model.OgrenciSozlesmeDersSecimler.Select(x => x.DersId).ToArray()
                        : null;

            viewModel.DersSelectList = selectListHelper.DersSelectList(true, selectedDersItems);

            #endregion

            #region Aylik Taksit Bilgiler

            if (viewModel.Model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler == null || viewModel.Model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count == 0)
            {
                viewModel.Model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler = new List<AylikTaksitBilgi>();
            }

            if (viewModel.Model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Count == 0)
            {
                var maksimumTaksitAdeti = AyarlarService.Get().MaksimumTaksitAdeti;

                for (var i = 0; i < maksimumTaksitAdeti; i++)
                {
                    viewModel.Model.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Add(
                        new AylikTaksitBilgi
                        {
                            TaksitNo = i + 1
                        });
                }
            }

            #endregion
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new OgrenciSozlesmeDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<OgrenciSozlesme>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.Model = new OgrenciSozlesme
            {
                FaturaBilgi = new FaturaBilgi(),
                OgrenciSozlesmeOdemeBilgi = new OgrenciSozlesmeOdemeBilgi
                {
                    AylikTaksitBilgiler = new List<AylikTaksitBilgi>()
                }
            };

            var isNull = false;

            if (id != null)
            {
                viewModel.Model.Ogrenci = GetModel((int)id);
            }

            isNull = viewModel.Model.Ogrenci == null || id == null;

            if (isNull)
            {
                viewModel.Model.Ogrenci = new Ogrenci
                {
                    AnneOgrenciYakiniIletisim = new OgrenciYakiniIletisim(),
                    BabaOgrenciYakiniIletisim = new OgrenciYakiniIletisim(),
                    YakiniOgrenciYakiniIletisim = new OgrenciYakiniIletisim()
                };
            }
            else
            {
                viewModel.Model.OgrenciId = (int)id;
            }

            GetLists(viewModel);

            if (string.IsNullOrEmpty(viewModel.Model.Ogrenci.OgrenciNo))
            {
                var service = serviceFactory.CreateService<IOgrenciService>();
                viewModel.Model.Ogrenci.OgrenciNo = service.GetSonOgrenciNo();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(OgrenciSozlesmeDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        var ayarlar = AyarlarService.Get();

                        if (viewModel.PostedFileGorselDosyaAd != null && viewModel.Model.Ogrenci != null)
                        {
                            var oldGorselDosyaAd = viewModel.Model.Ogrenci.GorselDosyaAd;
                            var oldGorselYol = viewModel.Model.Ogrenci.GorselYol;

                            viewModel.Model.Ogrenci.GorselDosyaAd = FileHelper.OgrenciGorselImageName(
                                viewModel.PostedFileGorselDosyaAd,
                                viewModel.Model.Ogrenci.AdSoyad);

                            var logoSaveMessageInfo = FileHelper.ImageSave(
                                viewModel.PostedFileGorselDosyaAd,
                                ayarlar.OgrenciGorselYol,
                                viewModel.Model.Ogrenci.GorselYol,
                                viewModel.Model.Ogrenci.GetDisplayName(x => x.GorselDosyaAd),
                                ayarlar.OgrenciGorselSize);

                            if (viewModel.OperationResult?.MessageInfos == null)
                            {
                                viewModel.OperationResult = new EntityOperationResult<OgrenciSozlesme>();
                            }

                            viewModel.OperationResult.MessageInfos.Add(logoSaveMessageInfo);

                            if (logoSaveMessageInfo.MessageInfoType == MessageInfoType.Success)
                            {
                                if (oldGorselYol != null && oldGorselYol.IndexOf("default") == -1)
                                {
                                    FileHelper.IfFileExistsDeleteFile(oldGorselYol);
                                }
                            }
                            else if (!string.Equals(viewModel.Model.Ogrenci.GorselDosyaAd, oldGorselDosyaAd))
                            {
                                viewModel.Model.Ogrenci.GorselDosyaAd = oldGorselDosyaAd;
                            }
                        }

                        // Yayınlardan seçilmemiş olan varsa, çıkartıyoruz.
                        // Özel ders ise, yayın olmayacak.
                        if (viewModel.Model.OgrenciSozlesmeTurId != 3)
                        {
                            if (viewModel.Model.OgrenciSozlesmeYayinlar != null && viewModel.Model.OgrenciSozlesmeYayinlar.Count > 0)
                            {
                                var removedList = new List<OgrenciSozlesmeYayin>();

                                foreach (var ogrenciSozlesmeYayin in viewModel.Model.OgrenciSozlesmeYayinlar)
                                {
                                    if (!ogrenciSozlesmeYayin.SecildiMi)
                                        removedList.Add(ogrenciSozlesmeYayin);
                                }

                                if (removedList.Count > 0)
                                {
                                    viewModel.Model.OgrenciSozlesmeYayinlar = viewModel.Model.OgrenciSozlesmeYayinlar.Except(removedList).ToList();
                                }
                            }
                        }
                        else
                        {
                            viewModel.Model.OgrenciSozlesmeYayinlar = null;

                            // Sözleşme Özel Ders ise seçilen dersler
                            if (viewModel.SelectedDersler != null && viewModel.SelectedDersler.Length > 0 && viewModel.Model.OgrenciSozlesmeTurId == 3)
                            {
                                if (viewModel.Model.OgrenciSozlesmeDersSecimler == null)
                                {
                                    viewModel.Model.OgrenciSozlesmeDersSecimler = new List<OgrenciSozlesmeDersSecim>();
                                }

                                if (viewModel.Model.OgrenciSozlesmeId == 0)
                                {
                                    foreach (var ders in viewModel.SelectedDersler)
                                    {
                                        viewModel.Model.OgrenciSozlesmeDersSecimler.Add(new OgrenciSozlesmeDersSecim
                                        {
                                            OgrenciSozlesme = viewModel.Model,
                                            DersId = ders
                                        });
                                    }
                                }
                                else
                                {
                                    foreach (var ders in viewModel.SelectedDersler)
                                    {
                                        viewModel.Model.OgrenciSozlesmeDersSecimler.Add(new OgrenciSozlesmeDersSecim
                                        {
                                            OgrenciSozlesmeId = viewModel.Model.OgrenciSozlesmeId,
                                            DersId = ders
                                        });
                                    }
                                }
                            }
                        }

                        if (viewModel.Model.OgrenciSozlesmeTurId == 2 && viewModel.Model.OgrenciSozlesmeKiyafetDurumlar != null)
                        {
                            // Kıyafet seçilmediyse, listeden çıkartıyoruz.
                            var removeOgrenciSozlesmeKiyafetDurumlar = new List<OgrenciSozlesmeKiyafetDurum>();

                            foreach (var ogrenciSozlesmeKiyafetDurum in viewModel.Model.OgrenciSozlesmeKiyafetDurumlar)
                            {
                                if (!ogrenciSozlesmeKiyafetDurum.SecildiMi && ogrenciSozlesmeKiyafetDurum.OgrenciSozlesmeKiyafetDurumId == 0)
                                    removeOgrenciSozlesmeKiyafetDurumlar.Add(ogrenciSozlesmeKiyafetDurum);
                            }

                            viewModel.Model.OgrenciSozlesmeKiyafetDurumlar = viewModel.Model.OgrenciSozlesmeKiyafetDurumlar.Except(removeOgrenciSozlesmeKiyafetDurumlar).ToList();
                        }
                        else
                        {
                            viewModel.Model.OgrenciSozlesmeKiyafetDurumlar = null;
                        }

                        viewModel.OperationResult = service.Add(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.OgrenciSozlesmeId > 0
                            ? RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciSozlesmeId })
                            : RedirectToAction("Duzenle");
                    }
            }

            if (viewModel.Model.OgrenciSozlesmeId > 0)
            {
                return RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciId });
            }
            else
            {
                GetLists(viewModel);

                viewModel.Model.Ogrenci = GetModel(viewModel.Model.OgrenciId);

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeGuncelle(OgrenciSozlesmeGuncelleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            var oldModel = service.Get(x => x.OgrenciSozlesmeId == viewModel.Model.OgrenciSozlesmeId);

            viewModel.Model.EgitimTutar = oldModel.EgitimTutar;
            viewModel.Model.YayinTutar = oldModel.YayinTutar;
            viewModel.Model.ServisTutar = oldModel.ServisTutar;
            viewModel.Model.KiyafetTutar = oldModel.KiyafetTutar;
            viewModel.Model.YemekTutar = oldModel.YemekTutar;
            viewModel.Model.ToplamUcret = oldModel.ToplamUcret;
            viewModel.Model.ToplamOdenen = oldModel.ToplamOdenen;
            viewModel.Model.OlusturulmaTarihi = oldModel.OlusturulmaTarihi;
            viewModel.Model.DosyaAd = oldModel.DosyaAd;

            var operationResult = service.Update(viewModel.Model);

            TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
            {
                MessageInfos = operationResult.MessageInfos,
                Status = operationResult.Status
            };

            return RedirectToAction("Detay", "Ogrenci", new { id = viewModel.Model.OgrenciId });
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeGetir(int id)
        {
            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var model = service.Get(x => x.OgrenciSozlesmeId == id);

            var viewModel = new OgrenciSozlesmeGuncelleViewModel
            {
                OgrenciSozlesmeTurSelectList = selectListHelper.OgrenciSozlesmeTurSelectList(),
                SubeSelectList = selectListHelper.SubeSelectList(),
                SezonSelectList = selectListHelper.SezonSelectList(),
                OkulTurSelectList = selectListHelper.OkulTurSelectList(),
                EtkinlikSelectList = selectListHelper.EtkinlikSelectList(),
                EhliyetTurSelectList = selectListHelper.EhliyetTurSelectList(),
                SinifSeviyeSelectList = selectListHelper.SinifSeviyeSelectList(),
                BransSelectList = selectListHelper.BransSelectList(),
                PersonelSelectList = selectListHelper.SubePersonelSelectList(model.SubeId),
                Model = model,
                SinifId = model.SinifId
            };

            return PartialView("~/Views/Shared/_OgrenciSozlesmeGuncelleView.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(OgrenciDetayViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            var operationResult = service.DeleteById(viewModel.OgrenciSozlesmeSilModel.OgrenciSozlesmeId);

            TempData["OperationResult"] = new EntityOperationResult<Ogrenci>
            {
                MessageInfos = operationResult.MessageInfos,
                Status = operationResult.Status
            };

            return RedirectToAction("Detay", "Ogrenci", new { id = viewModel.OgrenciSozlesmeSilModel.OgrenciId });
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new OgrenciSozlesmeListeleViewModel
            {
                Model = new OgrenciSozlesmeDto(),
                EntityPagedDataSource = new EntityPagedDataSource<OgrenciSozlesmeDto>(),
                Search = new Search()
            };

            viewModel.OgrenciSozlesmeTurSelectList = selectListHelper.OgrenciSozlesmeTurSelectList();
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.SezonSelectList = selectListHelper.SezonSelectList();
            viewModel.SinifSelectList = selectListHelper.SinifSelectList();

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(OgrenciSozlesmeListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("OgrenciSozlesmeTurId",viewModel.OgrenciSozlesmeTurId),
                new Parameter("SubeId",viewModel.SubeId),
                new Parameter("SezonId",viewModel.SezonId),
                new Parameter("SinifId",viewModel.SinifId)
            };

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult SozlesmeYazdir(int id)
        {
            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var model = service.Get(x => x.OgrenciSozlesmeId == id,
                y => y.Sube.Kurum,
                y => y.Sube.Sehir,
                y => y.Ogrenci,
                y => y.OgrenciSozlesmeTur,
                y => y.OgrenciSozlesmeOdemeBilgi,
                y => y.Ogrenci.AnneOgrenciYakiniIletisim,
                y => y.Ogrenci.BabaOgrenciYakiniIletisim,
                y => y.GorusenPersonel,
                y => y.EkleyenPersonel,
                y => y.KurumaGetirenPersonel,
                y => y.Sezon,
                y => y.Brans,
                y => y.Sinif);

            if (model != null)
            {
                var ogrenciSozlesmeHesapHareketService = serviceFactory.CreateService<IOgrenciSozlesmeHesapHareketService>();

                model.OgrenciSozlesmeHesapHareketler = ogrenciSozlesmeHesapHareketService.List(x => x.OgrenciSozlesmeId == id && !x.HesapHareket.IslemGerceklestiMi, y => y.HesapHareket).ToList();
            }
            else
            {
                return Redirect("/Error/NotFound");
            }

            var viewModel = new OgrenciSozlesmeYazdirViewModel
            {
                Model = model
            };

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult ExcelIndir(int ogrenciSozlesmeTurId, int subeId, int sezonId, int sinifId, string filter)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("OgrenciSozlesmeTurId",ogrenciSozlesmeTurId),
                new Parameter("SubeId",subeId),
                new Parameter("SezonId",sezonId),
                new Parameter("SinifId",sinifId)
            };

            OgrenciSozlesmeListeleViewModel viewModel = new OgrenciSozlesmeListeleViewModel();
            viewModel.SearchText = filter;
            viewModel.Draw = 2;
            viewModel.Length = 99999;
            viewModel.Start = 0;

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[13] {
                      new DataColumn(FieldNameResources.AdSoyad),
                      new DataColumn(FieldNameResources.OgrenciNo),
                      new DataColumn(FieldNameResources.TcNo),
                      new DataColumn(FieldNameResources.CepTelefonu),
                      new DataColumn(FieldNameResources.Eposta),
                      new DataColumn(FieldNameResources.Cinsiyet),
                      new DataColumn(FieldNameResources.OgrenciSozlesmeTur),
                      new DataColumn(FieldNameResources.Sube),
                      new DataColumn(FieldNameResources.Sezon),
                      new DataColumn(FieldNameResources.Sinif),
                      new DataColumn(FieldNameResources.ToplamTutar),
                      new DataColumn(FieldNameResources.ToplamOdenen),
                      new DataColumn(FieldNameResources.ToplamKalan)
            });

            foreach (var item in pagedDataSource.data)
            {
                dt.Rows.Add(
                    item.AdSoyad,
                    item.OgrenciNo,
                    item.TcKimlikNo,
                    item.CepTelefon,
                    item.Eposta,
                    item.Cinsiyet,
                    item.OgrenciSozlesmeTurAd,
                    item.SubeAd,
                    item.SezonAd,
                    item.SinifAd,
                    item.ToplamUcretFormatted,
                    item.ToplamOdenenFormatted,
                    item.ToplamKalanFormatted);
            }

            dt.TableName = "OgrenciSozlesmeler";
            var file = ExcelHelper.ExcelIndir(new List<DataTable> { dt }, "OgrenciSozlesmeler");

            return File(file.FileContents, file.ContentType, file.FileDownLoadName);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeVeriGuncelle()
        {
            var viewModel = new OgrenciSozlesmeVeriGuncelleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<OgrenciSozlesme>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult OgrenciSozlesmeVeriGuncelle(OgrenciSozlesmeVeriGuncelleViewModel viewModel)
        {
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();
            var ogrenciService = serviceFactory.CreateService<IOgrenciService>();
            var subeService = serviceFactory.CreateService<ISubeService>();
            var ulkeService = serviceFactory.CreateService<IUlkeService>();
            var sehirService = serviceFactory.CreateService<ISehirService>();
            var ilceService = serviceFactory.CreateService<IIlceService>();
            var neredenDuydunuzService = serviceFactory.CreateService<INeredenDuydunuzService>();
            var bankaService = serviceFactory.CreateService<IBankaService>();
            var hesapService = serviceFactory.CreateService<IHesapService>();
            var transferTipService = serviceFactory.CreateService<ITransferTipService>();
            var ogrenciSozlesmeTurService = serviceFactory.CreateService<IOgrenciSozlesmeTurService>();
            var personelService = serviceFactory.CreateService<IPersonelService>();
            var sezonService = serviceFactory.CreateService<ISezonService>();
            var bransService = serviceFactory.CreateService<IBransService>();
            var sinifTurService = serviceFactory.CreateService<ISinifTurService>();
            var sinifSeviyeService = serviceFactory.CreateService<ISinifSeviyeService>();
            var derslikService = serviceFactory.CreateService<IDerslikService>();
            var sinifService = serviceFactory.CreateService<ISinifService>();

            var sube = subeService.Get(x => x.SubeId == viewModel.SubeId);

            if (sube == null)
            {
                TempData["OperationResult"] = new EntityOperationResult<OgrenciSozlesme>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = Resources.LangResources.SubeBulunamadi, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };

                return View(viewModel);
            }

            var returnVal = ExcelHelper.ExcelToDataSet(viewModel.PostedFileVeri, Server.MapPath("~/VeriGuncelle/"), $"sube{sube.SubeId}");

            if (!string.IsNullOrEmpty(returnVal.Item2))
            {
                TempData["OperationResult"] = new EntityOperationResult<OgrenciSozlesme>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = returnVal.Item2, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };

                return View(viewModel);
            }

            var dataTable = returnVal.Item1.Tables[0];

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                TempData["OperationResult"] = new EntityOperationResult<OgrenciSozlesme>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = Resources.LangResources.VeriBulunamadi, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };

                return View(viewModel);
            }

            var errorDataTable = dataTable.Clone();
            errorDataTable.Clear();

            errorDataTable.Columns.Add(new DataColumn(Resources.LangResources.Hata, typeof(string)));

            for (int i = 3; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var ekle = true;
                var hataMesaji = string.Empty;
                OgrenciSozlesme ogrenciSozlesme = null;

                try
                {
                    #region Excel

                    // Sözleşme Bilgi
                    var sozlesmeTur = row[0].ToString().Trim();
                    var referans = row[1].ToString().Trim();
                    var gorusenPersonel = row[2].ToString().Trim();
                    var kurumaGetirenPersonel = row[3].ToString().Trim();
                    var sezon = row[4].ToString().Trim();
                    var brans = row[5].ToString().Trim();
                    var sinifTur = row[6].ToString().Trim();
                    var sinifSeviye = row[7].ToString().Trim();
                    var derslik = row[8].ToString().Trim();
                    var egitimKucuPersonel = row[9].ToString().Trim();
                    var sinif = row[10].ToString().Trim();
                    var toplamDersSaati = row[11].ToString().Trim();
                    var sinifKapasite = row[12].ToString().Trim();
                    var kayitUcreti = row[13].ToString().Trim();
                    var egitimSuresi = row[14].ToString().Trim();
                    var okulTur = row[15].ToString().Trim();
                    var servis = row[16].ToString().Trim();
                    var yemekDahilMi = row[17].ToString().Trim();

                    // Öğrenci
                    var ogrenciNo = row[18].ToString().Trim();
                    var ogrenciAd = row[19].ToString().Trim();
                    var ogrenciSoyad = row[20].ToString().Trim();
                    var ogrenciDogumTarihi = row[21].ToString().Trim();
                    var ogrenciTC = row[22].ToString().Trim();
                    var ogrenciCinsiyet = row[23].ToString().Trim();
                    var ogrenciCepTelefonu = row[24].ToString().Trim();
                    var ogrenciTelefon = row[25].ToString().Trim();
                    var ogrenciEposta = row[26].ToString().Trim();
                    var ogrenciUlke = row[27].ToString().Trim();
                    var ogrenciSehir = row[28].ToString().Trim();
                    var ogrenciIlce = row[29].ToString().Trim();
                    var ogrenciAdres = row[30].ToString().Trim();
                    var ogrenciPostaKodu = row[31].ToString().Trim();
                    var ogrenciNot = row[32].ToString().Trim();
                    var ogrenciNeredenDuydunuz = row[33].ToString().Trim();

                    // Anne İletişim
                    var anneAd = row[34].ToString().Trim();
                    var anneSoyad = row[35].ToString().Trim();
                    var anneDogumTarihi = row[36].ToString().Trim();
                    var anneTC = row[37].ToString().Trim();
                    var anneCepTelefonu = row[38].ToString().Trim();
                    var anneCepTelefonu2 = row[39].ToString().Trim();
                    var anneEvTelefonu = row[40].ToString().Trim();
                    var anneIsTelefonu = row[41].ToString().Trim();
                    var anneEposta = row[42].ToString().Trim();
                    var anneEvAdresi = row[43].ToString().Trim();
                    var anneIsAdresi = row[44].ToString().Trim();

                    // Baba İletişim
                    var babaAd = row[45].ToString().Trim();
                    var babaSoyad = row[46].ToString().Trim();
                    var babaDogumTarihi = row[47].ToString().Trim();
                    var babaTC = row[48].ToString().Trim();
                    var babaCepTelefonu = row[49].ToString().Trim();
                    var babaCepTelefonu2 = row[50].ToString().Trim();
                    var babaEvTelefonu = row[51].ToString().Trim();
                    var babaIsTelefonu = row[52].ToString().Trim();
                    var babaEposta = row[53].ToString().Trim();
                    var babaEvAdresi = row[54].ToString().Trim();
                    var babaIsAdresi = row[55].ToString().Trim();

                    // Yakını iletişim
                    var yakiniAd = row[56].ToString().Trim();
                    var yakiniSoyad = row[57].ToString().Trim();
                    var yakiniDogumTarihi = row[58].ToString().Trim();
                    var yakiniTC = row[59].ToString().Trim();
                    var yakiniCepTelefonu = row[60].ToString().Trim();
                    var yakiniCepTelefonu2 = row[61].ToString().Trim();
                    var yakiniEvTelefonu = row[62].ToString().Trim();
                    var yakiniIsTelefonu = row[63].ToString().Trim();
                    var yakiniEposta = row[64].ToString().Trim();
                    var yakiniEvAdresi = row[65].ToString().Trim();
                    var yakiniIsAdresi = row[66].ToString().Trim();

                    // İletişim Bilgileri
                    var kendiyleIletisim = row[67].ToString().Trim();
                    var anneyleIletisim = row[68].ToString().Trim();
                    var babaylaIletisim = row[69].ToString().Trim();
                    var yakiniylaIletisim = row[70].ToString().Trim();

                    // Fatura
                    var faturaAdSoyad = row[71].ToString().Trim();
                    var faturaAdres = row[72].ToString().Trim();
                    var faturaVergiNo = row[73].ToString().Trim();
                    var faturaVergiDairesi = row[74].ToString().Trim();

                    // Öğrenci Sözleşme Ödeme Bilgi
                    var egitimTutar = row[75].ToString().Trim();
                    var yayinTutar = row[76].ToString().Trim();
                    var yemekTutar = row[77].ToString().Trim();
                    var servisTutar = row[78].ToString().Trim();
                    var kiyafetTutar = row[79].ToString().Trim();
                    var toplamTutar = row[80].ToString().Trim();
                    var pesinatTutari = row[81].ToString().Trim();
                    var odenenBanka = row[82].ToString().Trim();
                    var odenenHesapAd = row[83].ToString().Trim();
                    var transferTip = row[84].ToString().Trim();
                    var odemeNot = row[85].ToString().Trim();

                    var olusturulmaTarihi = row[86].ToString().Trim();

                    #endregion

                    #region Öğrenci

                    var ogrenciDogumTarihiGun = 0;
                    var ogrenciDogumTarihiAy = 0;
                    var ogrenciDogumTarihiYil = 0;

                    if (ogrenciDogumTarihi.Split(' ').Length > 1)
                    {
                        ogrenciDogumTarihi = ogrenciDogumTarihi.Split(' ')[0];
                    }

                    try
                    {
                        ogrenciDogumTarihiGun = Convert.ToInt32(ogrenciDogumTarihi.Split('.')[0]);
                        ogrenciDogumTarihiAy = Convert.ToInt32(ogrenciDogumTarihi.Split('.')[1]);
                        ogrenciDogumTarihiYil = Convert.ToInt32(ogrenciDogumTarihi.Split('.')[2]);
                    }
                    catch { }

                    if (ogrenciAd.Length > 50)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Ad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciSoyad.Length > 50)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Soyad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (string.IsNullOrEmpty(ogrenciAd) || string.IsNullOrEmpty(ogrenciSoyad))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.AdSoyad} {Resources.LangResources.BosOlamaz}, ";
                    }

                    if (ogrenciTC.Length > 11)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.TcNo} 11 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    var kadinMi = !string.IsNullOrEmpty(ogrenciCinsiyet) && string.Equals(ogrenciCinsiyet.ToLower(), "kadın");

                    if (ogrenciNo.Length > 20)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.OgrenciNo} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciCepTelefonu.Length > 20)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciTelefon.Length > 20)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Telefon} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciEposta.Length > 254)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Eposta} 254 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    var ogrenciEpostaDogruMu = false;

                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(ogrenciEposta);
                        ogrenciEpostaDogruMu = addr.Address == ogrenciEposta;
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(ogrenciEposta) && !ogrenciEpostaDogruMu)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Eposta} {Resources.LangResources.Yanlis}, ";
                    }

                    Ulke ogrenciUlkeEntity = null;

                    if (!string.IsNullOrEmpty(ogrenciUlke))
                    {
                        ogrenciUlkeEntity = ulkeService.Get(x => x.UlkeAd.Trim() == ogrenciUlke);

                        if (ogrenciUlkeEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.UlkeAd} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    Sehir ogrenciSehirEntity = null;

                    if (!string.IsNullOrEmpty(ogrenciSehir))
                    {
                        ogrenciSehirEntity = sehirService.Get(x => x.SehirAd.Trim() == ogrenciSehir);

                        if (ogrenciSehirEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.SehirAd} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    Ilce ogrenciIlceEntity = null;

                    if (!string.IsNullOrEmpty(ogrenciIlce) && !string.IsNullOrEmpty(ogrenciSehir))
                    {
                        ogrenciIlceEntity = ilceService.Get(x => x.IlceAd.Trim() == ogrenciIlce && x.Sehir.SehirAd == ogrenciSehir);

                        if (ogrenciIlceEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.IlceAd} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (ogrenciAdres.Length > 300)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Adres} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciPostaKodu.Length > 10)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.PostaKodu} 10 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (ogrenciNot.Length > 500)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Ogrenci} {FieldNameResources.Not} 500 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    NeredenDuydunuz ogrenciNeredenDuydunuzEntity = null;

                    if (!string.IsNullOrEmpty(ogrenciNeredenDuydunuz))
                    {
                        ogrenciNeredenDuydunuzEntity = neredenDuydunuzService.Get(x => x.NeredenDuydunuzBaslik.Trim() == ogrenciNeredenDuydunuz && x.KurumId == Identity.KurumId);

                        if (ogrenciNeredenDuydunuzEntity == null)
                        {
                            ogrenciNeredenDuydunuzEntity = new NeredenDuydunuz
                            {
                                NeredenDuydunuzBaslik = ogrenciNeredenDuydunuz,
                                KurumId = Identity.KurumId
                            };

                            var ogrenciNeredenDuydunuzReturnVal = neredenDuydunuzService.Add(ogrenciNeredenDuydunuzEntity);

                            if (ogrenciNeredenDuydunuzReturnVal.Status)
                            {
                                ogrenciNeredenDuydunuzEntity = ogrenciNeredenDuydunuzReturnVal.Model;
                            }
                        }
                    }

                    ogrenciSozlesme = new OgrenciSozlesme
                    {
                        Ogrenci = new Ogrenci
                        {
                            Ad = ogrenciAd,
                            Soyad = ogrenciSoyad,
                            DogumTarihi =
                                !string.IsNullOrEmpty(ogrenciDogumTarihi) &&
                                ogrenciDogumTarihiGun > 0 &&
                                ogrenciDogumTarihiAy > 0 &&
                                ogrenciDogumTarihiYil > 0
                                    ? new DateTime(ogrenciDogumTarihiYil, ogrenciDogumTarihiAy, ogrenciDogumTarihiGun)
                                    : (DateTime?)null,
                            OgrenciNo = ogrenciNo,
                            TcKimlikNo = string.IsNullOrEmpty(ogrenciTC) ? string.Empty : ogrenciTC,
                            KadinMi = kadinMi,
                            CepTelefon = ogrenciCepTelefonu,
                            Telefon = ogrenciTelefon,
                            Eposta = string.IsNullOrEmpty(ogrenciEposta) ? null : ogrenciEposta,
                            Adres = ogrenciAdres,
                            PostaKodu = ogrenciPostaKodu,
                            Not = ogrenciNot
                        }
                    };

                    if (ogrenciUlkeEntity != null)
                    {
                        ogrenciSozlesme.Ogrenci.UlkeId = ogrenciUlkeEntity.UlkeId;
                    }

                    if (ogrenciSehirEntity != null)
                    {
                        ogrenciSozlesme.Ogrenci.SehirId = ogrenciSehirEntity.SehirId;
                    }

                    if (ogrenciIlceEntity != null)
                    {
                        ogrenciSozlesme.Ogrenci.IlceId = ogrenciIlceEntity.IlceId;
                    }

                    if (ogrenciNeredenDuydunuzEntity != null)
                    {
                        ogrenciSozlesme.Ogrenci.NeredenDuydunuzId = ogrenciNeredenDuydunuzEntity.NeredenDuydunuzId;
                    }

                    #endregion

                    #region Anne

                    if (!string.IsNullOrEmpty(anneAd) && !string.IsNullOrEmpty(anneSoyad))
                    {
                        if (anneAd.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.Ad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneSoyad.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.Soyad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var anneDogumTarihiGun = 0;
                        var anneDogumTarihiAy = 0;
                        var anneDogumTarihiYil = 0;

                        if (anneDogumTarihi.Split(' ').Length > 1)
                        {
                            anneDogumTarihi = anneDogumTarihi.Split(' ')[0];
                        }

                        if (!string.IsNullOrEmpty(anneDogumTarihi))
                        {
                            try
                            {
                                anneDogumTarihiGun = Convert.ToInt32(anneDogumTarihi.Split('.')[0]);
                                anneDogumTarihiAy = Convert.ToInt32(anneDogumTarihi.Split('.')[1]);
                                anneDogumTarihiYil = Convert.ToInt32(anneDogumTarihi.Split('.')[2]);
                            }
                            catch { }
                        }

                        if (anneTC.Length > 11)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.TcNo} 11 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneCepTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneCepTelefonu2.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneEvTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.EvTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneIsTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.IsTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneEposta.Length > 254)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.Eposta} 254 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var anneEpostaDogruMu = false;

                        try
                        {
                            var addr = new System.Net.Mail.MailAddress(anneEposta);
                            anneEpostaDogruMu = addr.Address == anneEposta;
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(anneEposta) && !anneEpostaDogruMu)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.Eposta} {Resources.LangResources.Yanlis}, ";
                        }

                        if (anneEvAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.EvAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (anneIsAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Anne} {FieldNameResources.IsAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        ogrenciSozlesme.Ogrenci.AnneOgrenciYakiniIletisim = new OgrenciYakiniIletisim
                        {
                            Ad = anneAd,
                            Soyad = anneSoyad,
                            DogumTarihi =
                                !string.IsNullOrEmpty(anneDogumTarihi) &&
                                anneDogumTarihiGun > 0 &&
                                anneDogumTarihiAy > 0 &&
                                anneDogumTarihiYil > 0
                                    ? new DateTime(anneDogumTarihiYil, anneDogumTarihiAy, anneDogumTarihiGun)
                                    : (DateTime?)null,
                            TcKimlikNo = string.IsNullOrEmpty(anneTC) ? string.Empty : anneTC,
                            CepTelefon1 = anneCepTelefonu,
                            CepTelefon2 = anneCepTelefonu2,
                            EvTelefon = anneEvTelefonu,
                            IsTelefon = anneIsTelefonu,
                            Eposta = string.IsNullOrEmpty(anneEposta) ? null : anneEposta,
                            EvAdres = anneEvAdresi,
                            IsAdres = anneIsAdresi
                        };
                    }

                    #endregion

                    #region Baba

                    if (!string.IsNullOrEmpty(babaAd) && !string.IsNullOrEmpty(babaSoyad))
                    {
                        if (babaAd.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.Ad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaSoyad.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.Soyad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var BabaDogumTarihiGun = 0;
                        var BabaDogumTarihiAy = 0;
                        var BabaDogumTarihiYil = 0;

                        if (babaDogumTarihi.Split(' ').Length > 1)
                        {
                            babaDogumTarihi = babaDogumTarihi.Split(' ')[0];
                        }

                        if (!string.IsNullOrEmpty(babaDogumTarihi))
                        {
                            try
                            {
                                BabaDogumTarihiGun = Convert.ToInt32(babaDogumTarihi.Split('.')[0]);
                                BabaDogumTarihiAy = Convert.ToInt32(babaDogumTarihi.Split('.')[1]);
                                BabaDogumTarihiYil = Convert.ToInt32(babaDogumTarihi.Split('.')[2]);
                            }
                            catch { }
                        }

                        if (babaTC.Length > 11)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.TcNo} 11 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaCepTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaCepTelefonu2.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaEvTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.EvTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaIsTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.IsTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaEposta.Length > 254)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.Eposta} 254 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var BabaEpostaDogruMu = false;

                        try
                        {
                            var addr = new System.Net.Mail.MailAddress(babaEposta);
                            BabaEpostaDogruMu = addr.Address == babaEposta;
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(babaEposta) && !BabaEpostaDogruMu)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.Eposta} {Resources.LangResources.Yanlis}, ";
                        }

                        if (babaEvAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.EvAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (babaIsAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Baba} {FieldNameResources.IsAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        ogrenciSozlesme.Ogrenci.BabaOgrenciYakiniIletisim = new OgrenciYakiniIletisim
                        {
                            Ad = babaAd,
                            Soyad = babaSoyad,
                            DogumTarihi =
                                !string.IsNullOrEmpty(babaDogumTarihi) &&
                                BabaDogumTarihiGun > 0 &&
                                BabaDogumTarihiAy > 0 &&
                                BabaDogumTarihiYil > 0
                                    ? new DateTime(BabaDogumTarihiYil, BabaDogumTarihiAy, BabaDogumTarihiGun)
                                    : (DateTime?)null,
                            TcKimlikNo = string.IsNullOrEmpty(babaTC) ? string.Empty : babaTC,
                            CepTelefon1 = babaCepTelefonu,
                            CepTelefon2 = babaCepTelefonu2,
                            EvTelefon = babaEvTelefonu,
                            IsTelefon = babaIsTelefonu,
                            Eposta = string.IsNullOrEmpty(babaEposta) ? null : babaEposta,
                            EvAdres = babaEvAdresi,
                            IsAdres = babaIsAdresi
                        };
                    }

                    #endregion

                    #region Yakını

                    if (!string.IsNullOrEmpty(yakiniAd) && !string.IsNullOrEmpty(yakiniSoyad))
                    {
                        if (yakiniAd.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.Ad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniSoyad.Length > 50)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.Soyad} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var yakiniDogumTarihiGun = 0;
                        var yakiniDogumTarihiAy = 0;
                        var yakiniDogumTarihiYil = 0;

                        if (yakiniDogumTarihi.Split(' ').Length > 1)
                        {
                            yakiniDogumTarihi = yakiniDogumTarihi.Split(' ')[0];
                        }

                        if (!string.IsNullOrEmpty(yakiniDogumTarihi))
                        {
                            try
                            {
                                yakiniDogumTarihiGun = Convert.ToInt32(yakiniDogumTarihi.Split('.')[0]);
                                yakiniDogumTarihiAy = Convert.ToInt32(yakiniDogumTarihi.Split('.')[1]);
                                yakiniDogumTarihiYil = Convert.ToInt32(yakiniDogumTarihi.Split('.')[2]);
                            }
                            catch { }
                        }

                        if (yakiniTC.Length > 11)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.TcNo} 11 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniCepTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniCepTelefonu2.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.CepTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniEvTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.EvTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniIsTelefonu.Length > 20)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.IsTelefonu} 20 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniEposta.Length > 254)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.Eposta} 254 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        var yakiniEpostaDogruMu = false;

                        try
                        {
                            var addr = new System.Net.Mail.MailAddress(yakiniEposta);
                            yakiniEpostaDogruMu = addr.Address == yakiniEposta;
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(yakiniEposta) && !yakiniEpostaDogruMu)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.Eposta} {Resources.LangResources.Yanlis}, ";
                        }

                        if (yakiniEvAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.EvAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        if (yakiniIsAdresi.Length > 300)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.DigerYakini} {FieldNameResources.IsAdresi} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                        }

                        ogrenciSozlesme.Ogrenci.YakiniOgrenciYakiniIletisim = new OgrenciYakiniIletisim
                        {
                            Ad = yakiniAd,
                            Soyad = yakiniSoyad,
                            DogumTarihi =
                                !string.IsNullOrEmpty(yakiniDogumTarihi) &&
                                yakiniDogumTarihiGun > 0 &&
                                yakiniDogumTarihiAy > 0 &&
                                yakiniDogumTarihiYil > 0
                                    ? new DateTime(yakiniDogumTarihiYil, yakiniDogumTarihiAy, yakiniDogumTarihiGun)
                                    : (DateTime?)null,
                            TcKimlikNo = string.IsNullOrEmpty(yakiniTC) ? string.Empty : yakiniTC,
                            CepTelefon1 = yakiniCepTelefonu,
                            CepTelefon2 = yakiniCepTelefonu2,
                            EvTelefon = yakiniEvTelefonu,
                            IsTelefon = yakiniIsTelefonu,
                            Eposta = string.IsNullOrEmpty(yakiniEposta) ? null : yakiniEposta,
                            EvAdres = yakiniEvAdresi,
                            IsAdres = yakiniIsAdresi
                        };
                    }

                    #endregion

                    #region İletişim Seçenek

                    ogrenciSozlesme.Ogrenci.IletisimKendi = !string.IsNullOrEmpty(kendiyleIletisim) && string.Equals(kendiyleIletisim.ToLower(), "evet");
                    ogrenciSozlesme.Ogrenci.IletisimAnne = !string.IsNullOrEmpty(anneyleIletisim) && string.Equals(anneyleIletisim.ToLower(), "evet");
                    ogrenciSozlesme.Ogrenci.IletisimBaba = !string.IsNullOrEmpty(babaylaIletisim) && string.Equals(babaylaIletisim.ToLower(), "evet");
                    ogrenciSozlesme.Ogrenci.IletisimYakini = !string.IsNullOrEmpty(yakiniylaIletisim) && string.Equals(yakiniylaIletisim.ToLower(), "evet");

                    #endregion

                    #region Fatura

                    if (faturaAdSoyad.Length > 100)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.FaturaBilgi} {FieldNameResources.AdSoyad} 100 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (faturaVergiDairesi.Length > 100)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.FaturaBilgi} {FieldNameResources.VergiDairesi} 100 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (faturaVergiNo.Length > 50)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.FaturaBilgi} {FieldNameResources.VergiNo} 50 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    if (faturaAdres.Length > 300)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.FaturaBilgi} {FieldNameResources.Adres} 300 {Resources.LangResources.ExcelAktarimHataUzunluk}, ";
                    }

                    ogrenciSozlesme.FaturaBilgi = new FaturaBilgi
                    {
                        AdSoyad = faturaAdSoyad,
                        VergiDairesi = faturaVergiDairesi,
                        VergiNo = faturaVergiNo,
                        Adres = faturaAdres
                    };

                    var ogrenciFaturaBilgi = new OgrenciFaturaBilgi
                    {
                        Ogrenci = ogrenciSozlesme.Ogrenci,
                        FaturaBilgi = ogrenciSozlesme.FaturaBilgi
                    };

                    ogrenciSozlesme.Ogrenci.OgrenciFaturaBilgiler = new List<OgrenciFaturaBilgi>
                {
                    ogrenciFaturaBilgi
                };

                    #endregion

                    ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi = new OgrenciSozlesmeOdemeBilgi();

                    ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.Not = odemeNot;

                    #region Eğitim Tutar

                    decimal? egitimTutarDecimal = null;

                    if (!string.IsNullOrEmpty(egitimTutar))
                    {
                        try
                        {
                            egitimTutarDecimal = Convert.ToDecimal(egitimTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.EgitimTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }
                    else
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.EgitimTutar} {Resources.LangResources.BosOlamaz}, ";
                    }

                    ogrenciSozlesme.EgitimTutar = egitimTutarDecimal;

                    #endregion

                    #region Yayın Tutar

                    decimal? yayinTutarDecimal = null;

                    if (!string.IsNullOrEmpty(yayinTutar))
                    {
                        try
                        {
                            yayinTutarDecimal = Convert.ToDecimal(yayinTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.YayinTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    ogrenciSozlesme.YayinTutar = yayinTutarDecimal;

                    #endregion

                    #region Yemek Tutar

                    decimal? yemekTutarDecimal = null;

                    if (!string.IsNullOrEmpty(yemekTutar))
                    {
                        try
                        {
                            yemekTutarDecimal = Convert.ToDecimal(yemekTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.YemekTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    ogrenciSozlesme.YemekTutar = yemekTutarDecimal;

                    #endregion

                    #region Servis Tutar

                    decimal? servisTutarDecimal = null;

                    if (!string.IsNullOrEmpty(servisTutar))
                    {
                        try
                        {
                            servisTutarDecimal = Convert.ToDecimal(servisTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.ServisTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    ogrenciSozlesme.ServisTutar = servisTutarDecimal;

                    #endregion

                    #region Kıyafet Tutar

                    decimal? kiyafetTutarDecimal = null;

                    if (!string.IsNullOrEmpty(kiyafetTutar))
                    {
                        try
                        {
                            kiyafetTutarDecimal = Convert.ToDecimal(kiyafetTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.KiyafetTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    ogrenciSozlesme.KiyafetTutar = kiyafetTutarDecimal;

                    #endregion

                    #region Toplam Tutar

                    decimal? toplamTutarDecimal = null;

                    if (!string.IsNullOrEmpty(toplamTutar))
                    {
                        try
                        {
                            toplamTutarDecimal = Convert.ToDecimal(toplamTutar);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.ToplamTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }
                    else
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.ToplamTutar} {Resources.LangResources.BosOlamaz}, ";
                    }

                    ogrenciSozlesme.ToplamUcret = toplamTutarDecimal;

                    #endregion

                    #region Peşinat Tutar

                    decimal? pesinatTutarTutarDecimal = null;

                    if (!string.IsNullOrEmpty(pesinatTutari))
                    {
                        try
                        {
                            pesinatTutarTutarDecimal = Convert.ToDecimal(pesinatTutari);
                        }
                        catch
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.PesinatTutar} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatTutar = pesinatTutarTutarDecimal;

                    #endregion

                    #region Ödeme

                    Hesap seciliHesap = null;

                    if (string.IsNullOrEmpty(transferTip))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.TransferTip} {Resources.LangResources.BosOlamaz}, ";
                    }

                    var transferTipEntity = transferTipService.Get(x => x.TransferTipAd.Trim() == transferTip && x.EtkinMi);

                    if (transferTipEntity == null)
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.TransferTip} {Resources.LangResources.Yanlis}, ";
                    }

                    Banka banka = null;

                    if (!string.IsNullOrEmpty(odenenBanka))
                    {
                        banka = bankaService.Get(x => x.BankaAd.Trim() == odenenBanka && x.EtkinMi);

                        if (banka == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Banka} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    var hesaplar = hesapService.List(
                            x => x.UstHesapId == sube.SubeId &&
                            x.EtkinMi &&
                            x.ParaBirimId == sube.ParaBirimId,
                            y => y.BankaHesap.Banka).
                        ToList();

                    if (hesaplar != null && hesaplar.Count > 0)
                    {
                        if (banka != null)
                        {
                            seciliHesap = hesaplar.FirstOrDefault(x =>
                                x.BankaHesap.BankaId == banka.BankaId &&
                                x.TransferTipId == transferTipEntity.TransferTipId &&
                                x.EtkinMi);

                            if (seciliHesap == null)
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.Banka} {Resources.LangResources.Yanlis}, ";
                            }
                        }
                        else
                        {
                            seciliHesap = hesaplar.FirstOrDefault(x =>
                                x.TransferTipId == transferTipEntity.TransferTipId &&
                                x.EtkinMi);

                            if (seciliHesap == null)
                            {
                                seciliHesap = new Hesap
                                {
                                    BagliKurumId = Identity.KurumId,
                                    EtkinMi = true,
                                    HesapBaslik = transferTipEntity.TransferTipAd,
                                    UstHesapId = sube.SubeId,
                                    ParaBirimId = (int)sube.ParaBirimId,
                                    HesapTurId = 4
                                };
                            }
                        }
                    }
                    else
                    {
                        seciliHesap = new Hesap
                        {
                            BagliKurumId = Identity.KurumId,
                            EtkinMi = true,
                            HesapBaslik = transferTipEntity.TransferTipAd,
                            UstHesapId = sube.SubeId,
                            ParaBirimId = (int)sube.ParaBirimId,
                            HesapTurId = 4
                        };
                    }

                    if (seciliHesap.HesapId > 0)
                    {
                        ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatHesapId = seciliHesap.HesapId;
                    }
                    else
                    {
                        ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatHesap = seciliHesap;
                    }

                    // ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler = new List<AylikTaksitBilgi>();
                    //
                    // if (ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitAdet > 0)
                    // {
                    //     var tarihBaslangic = ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitBaslangicTarihi == null
                    //         ? DateTime.Now
                    //         : ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitBaslangicTarihi;
                    //
                    //     var odenenTaksitTutar = ogrenciSozlesme.ToplamOdenen ?? ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatTutar ?? 0;
                    //     var odenecekTutar = (decimal)ogrenciSozlesme.ToplamUcret - (ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.PesinatTutar ?? 0);
                    //
                    //     decimal taksitTutar = 0;
                    //
                    //     if (ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitTutar > 0)
                    //     {
                    //         taksitTutar = (decimal)ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitTutar;
                    //     }
                    //     else
                    //     {
                    //         taksitTutar = Convert.ToInt32(odenecekTutar / (int)ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitAdet);
                    //     }
                    //
                    //     for (int g = 0; g < ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitAdet; g++)
                    //     {
                    //         var aylikTaksitBilgi = new AylikTaksitBilgi
                    //         {
                    //             TaksitNo = g + 1,
                    //             TaksitTutari = g == ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.TaksitAdet - 1
                    //                 ? odenecekTutar
                    //                 : taksitTutar,
                    //             VadeTarihi = tarihBaslangic.Value.AddMonths(g)
                    //         };
                    //
                    //         odenecekTutar -= taksitTutar;
                    //
                    //         ogrenciSozlesme.OgrenciSozlesmeOdemeBilgi.AylikTaksitBilgiler.Add(aylikTaksitBilgi);
                    //     }
                    // }

                    #endregion

                    #region Sözleşme Bilgi

                    ogrenciSozlesme.SubeId = sube.SubeId;
                    ogrenciSozlesme.Referans = referans;

                    #region Sözleşme Tür

                    OgrenciSozlesmeTur sozlesmeTurEntity = null;

                    if (string.IsNullOrEmpty(sozlesmeTur))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.OgrenciSozlesmeTur} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        sozlesmeTurEntity = ogrenciSozlesmeTurService.Get(x => x.OgrenciSozlesmeTurAd.Trim() == sozlesmeTur);

                        if (sozlesmeTurEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.OgrenciSozlesmeTur} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (sozlesmeTurEntity != null)
                    {
                        ogrenciSozlesme.OgrenciSozlesmeTurId = sozlesmeTurEntity.OgrenciSozlesmeTurId;
                    }

                    #endregion

                    #region Görüşen Personel

                    Personel gorusenPersonelEntity = null;

                    if (string.IsNullOrEmpty(gorusenPersonel))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.GorusenPersonel} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        gorusenPersonelEntity = personelService.Get(x => x.Ad.Trim() + " " + x.Soyad.Trim() == gorusenPersonel);

                        if (gorusenPersonelEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.GorusenPersonel} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (gorusenPersonelEntity != null)
                    {
                        ogrenciSozlesme.GorusenPersonelId = gorusenPersonelEntity.PersonelId;
                    }

                    #endregion

                    #region Kuruma Getiren Personel

                    Personel kurumaGetirenPersonelEntity = null;

                    if (string.IsNullOrEmpty(kurumaGetirenPersonel))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.KurumaGetiren} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        kurumaGetirenPersonelEntity = personelService.Get(x => x.Ad.Trim() + " " + x.Soyad.Trim() == kurumaGetirenPersonel);

                        if (gorusenPersonelEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.KurumaGetiren} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (kurumaGetirenPersonelEntity != null)
                    {
                        ogrenciSozlesme.KurumaGetirenPersonelId = kurumaGetirenPersonelEntity.PersonelId;
                    }

                    #endregion

                    #region Sezon

                    Sezon sezonEntity = null;

                    if (string.IsNullOrEmpty(sezon))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Sezon} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        sezonEntity = sezonService.Get(x => x.SezonAd.Trim() == sezon);

                        if (sezonEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Sezon} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (sezonEntity != null)
                    {
                        ogrenciSozlesme.SezonId = sezonEntity.SezonId;
                    }

                    #endregion

                    #region Branş

                    Brans bransEntity = null;

                    if (string.IsNullOrEmpty(brans))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Brans} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        bransEntity = bransService.Get(x => x.BransAd.Trim() == brans);

                        if (sezonEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.Brans} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    if (bransEntity != null)
                    {
                        ogrenciSozlesme.BransId = bransEntity.BransId;
                    }

                    #endregion

                    #region Sınıf Tür

                    SinifTur sinifTurEntity = null;

                    if (!string.IsNullOrEmpty(sinifTur))
                    {
                        sinifTurEntity = sinifTurService.Get(x => x.SinifTurAd.Trim() == sinifTur);

                        if (sinifTurEntity == null)
                        {
                            sinifTurEntity = new SinifTur
                            {
                                KurumId = sube.KurumId,
                                SinifTurAd = sinifTur,
                                EtkinMi = true
                            };

                            var sinifTurOperationResult = sinifTurService.Add(sinifTurEntity);

                            if (sinifTurOperationResult.Status)
                            {
                                sinifTurEntity = sinifTurOperationResult.Model;
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.SinifTur} {Resources.LangResources.Yanlis}, ";
                            }
                        }
                    }

                    #endregion

                    #region Sınıf Seviye

                    SinifSeviye sinifSeviyeEntity = null;

                    if (!string.IsNullOrEmpty(sinifSeviye))
                    {
                        sinifSeviyeEntity = sinifSeviyeService.Get(x => x.SinifSeviyeAd.Trim() == sinifSeviye);

                        if (sinifSeviyeEntity == null)
                        {
                            sinifSeviyeEntity = new SinifSeviye
                            {
                                KurumId = sube.KurumId,
                                SinifSeviyeAd = sinifSeviye,
                                EtkinMi = true
                            };

                            var sinifSeviyeOperationResult = sinifSeviyeService.Add(sinifSeviyeEntity);

                            if (sinifSeviyeOperationResult.Status)
                            {
                                sinifSeviyeEntity = sinifSeviyeOperationResult.Model;
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.SinifSeviye} {Resources.LangResources.Yanlis}, ";
                            }
                        }
                    }

                    if (sinifSeviyeEntity != null)
                    {
                        ogrenciSozlesme.SinifSeviyeId = sinifSeviyeEntity.SinifSeviyeId;
                    }

                    #endregion

                    #region Derslik

                    Derslik derslikEntity = null;

                    if (!string.IsNullOrEmpty(derslik))
                    {
                        derslikEntity = derslikService.Get(x => x.DerslikAd.Trim() == derslik);

                        if (derslikEntity == null)
                        {
                            derslikEntity = new Derslik
                            {
                                KurumId = sube.KurumId,
                                DerslikAd = derslik,
                                EtkinMi = true
                            };

                            var derslikOperationResult = derslikService.Add(derslikEntity);

                            if (derslikOperationResult.Status)
                            {
                                derslikEntity = derslikOperationResult.Model;
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.Derslik} {Resources.LangResources.Yanlis}, ";
                            }
                        }
                    }

                    #endregion

                    #region Eğitim Koçu

                    Personel egitimKocuPersonelEntity = null;

                    if (!string.IsNullOrEmpty(egitimKucuPersonel))
                    {
                        egitimKocuPersonelEntity = personelService.Get(x => x.Ad.Trim() + " " + x.Soyad.Trim() == egitimKucuPersonel);

                        if (gorusenPersonelEntity == null)
                        {
                            ekle = false;
                            hataMesaji += $"{FieldNameResources.EgitimKocu} {Resources.LangResources.Yanlis}, ";
                        }
                    }

                    #endregion

                    #region Sinif

                    Sinif sinifEntity = null;

                    if (string.IsNullOrEmpty(sinif))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.Sinif} {Resources.LangResources.BosOlamaz}, ";
                    }
                    else
                    {
                        sinifEntity = sinifService.Get(x => x.SinifAd.Trim() == sinif && x.SezonId == sezonEntity.SezonId && x.SubeId == sube.SubeId);

                        if (sinifEntity == null)
                        {
                            #region Toplam Ders Saati

                            int? toplamDersSaatiInteger = null;

                            if (!string.IsNullOrEmpty(toplamDersSaati))
                            {
                                try
                                {
                                    toplamDersSaatiInteger = Convert.ToInt32(toplamDersSaati);
                                }
                                catch
                                {
                                    ekle = false;
                                    hataMesaji += $"{FieldNameResources.ToplamDersSaati} {Resources.LangResources.Yanlis}, ";
                                }
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.ToplamDersSaati} {Resources.LangResources.BosOlamaz}, ";
                            }

                            #endregion

                            #region Sınıf Kapasite

                            int? sinifKapasiteInteger = null;

                            if (!string.IsNullOrEmpty(sinifKapasite))
                            {
                                try
                                {
                                    sinifKapasiteInteger = Convert.ToInt32(sinifKapasite);
                                }
                                catch
                                {
                                    ekle = false;
                                    hataMesaji += $"{FieldNameResources.SinifKapasite} {Resources.LangResources.Yanlis}, ";
                                }
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.SinifKapasite} {Resources.LangResources.BosOlamaz}, ";
                            }

                            #endregion

                            #region Kayıt Ücreti

                            decimal? kayitUcretiDecimal = null;

                            if (!string.IsNullOrEmpty(kayitUcreti))
                            {
                                try
                                {
                                    kayitUcretiDecimal = Convert.ToDecimal(kayitUcreti);
                                }
                                catch
                                {
                                    ekle = false;
                                    hataMesaji += $"{FieldNameResources.KayitUcreti} {Resources.LangResources.Yanlis}, ";
                                }
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.KayitUcreti} {Resources.LangResources.BosOlamaz}, ";
                            }

                            #endregion

                            #region Eğitim Süresi

                            int? egitimSuresiInteger = null;

                            if (!string.IsNullOrEmpty(egitimSuresi))
                            {
                                try
                                {
                                    egitimSuresiInteger = Convert.ToInt32(egitimSuresi);
                                }
                                catch
                                {
                                    ekle = false;
                                    hataMesaji += $"{FieldNameResources.EgitimSüre} {Resources.LangResources.Yanlis}, ";
                                }
                            }
                            else
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.EgitimSüre} {Resources.LangResources.BosOlamaz}, ";
                            }

                            #endregion

                            sinifEntity = new Sinif
                            {
                                SubeId = sube.SubeId,
                                SezonId = sezonEntity.SezonId,
                                SinifAd = sinif,
                                ToplamDersSaat = toplamDersSaatiInteger,
                                SinifKapasite = sinifKapasiteInteger,
                                KayitUcreti = kayitUcretiDecimal,
                                EgitimSüre = egitimSuresiInteger,
                                EtkinMi = true
                            };

                            if (bransEntity != null)
                            {
                                sinifEntity.BransId = bransEntity.BransId;
                            }

                            if (sinifTurEntity != null)
                            {
                                sinifEntity.SinifTurId = sinifTurEntity.SinifTurId;
                            }

                            if (sinifSeviyeEntity != null)
                            {
                                sinifEntity.SinifSeviyeId = sinifSeviyeEntity.SinifSeviyeId;
                            }

                            if (derslikEntity != null)
                            {
                                sinifEntity.DerslikId = derslikEntity.DerslikId;
                            }

                            if (egitimKocuPersonelEntity != null)
                            {
                                sinifEntity.PersonelId = egitimKocuPersonelEntity.PersonelId;
                            }

                            var sinifAddOperationResult = sinifService.Add(sinifEntity);

                            if (!sinifAddOperationResult.Status)
                            {
                                ekle = false;
                                hataMesaji += $"{FieldNameResources.Sinif} {sinifAddOperationResult.MessageInfos[0].Message}, ";
                            }

                            ogrenciSozlesme.SinifId = sinifEntity.SinifId;
                        }
                    }

                    if (sezonEntity != null)
                    {
                        ogrenciSozlesme.SinifId = sinifEntity.SinifId;
                    }

                    #endregion

                    #endregion
                }
                catch (Exception ex)
                {
                    hataMesaji += $"{ex.Message}, ";
                }

                if (ekle && ogrenciSozlesme != null)
                {
                    if (!string.IsNullOrEmpty(ogrenciSozlesme.Ogrenci.TcKimlikNo))
                    {
                        var ogrenciCheckCount = ogrenciService.GetCount(x => x.TcKimlikNo.Trim() == ogrenciSozlesme.Ogrenci.TcKimlikNo);

                        if (ogrenciCheckCount > 0)
                        {
                            ekle = false;
                            hataMesaji += $"{Resources.LangResources.TCKimlikNoBenzerdegil}, ";
                        }
                    }

                    if (string.IsNullOrEmpty(ogrenciSozlesme.Ogrenci.OgrenciNo))
                    {
                        ekle = false;
                        hataMesaji += $"{FieldNameResources.OgrenciNo} {Resources.LangResources.BosOlamaz}, ";
                    }

                    if (!string.IsNullOrEmpty(ogrenciSozlesme.Ogrenci.OgrenciNo))
                    {
                        var ogrenciCheckCount = ogrenciService.GetCount(x => x.OgrenciNo.Trim() == ogrenciSozlesme.Ogrenci.OgrenciNo);

                        if (ogrenciCheckCount > 0)
                        {
                            ekle = false;
                            hataMesaji += $"{Resources.LangResources.OgrenciNoBenzersizOlmalidir}, ";
                        }
                    }

                    if (ekle)
                    {
                        var operationResult = service.Add(ogrenciSozlesme);

                        if (!operationResult.Status)
                        {
                            for (var z = 0; z < operationResult.MessageInfos.Count; z++)
                            {
                                ekle = false;
                                hataMesaji += $"{operationResult.MessageInfos[z].Message}, ";
                            }
                        }
                    }
                }

                if (!ekle)
                {
                    var array = new object[row.ItemArray.Length + 1];

                    for (int j = 0; j < row.ItemArray.Length; j++)
                    {
                        array[j] = row.ItemArray[j];
                    }

                    array[row.ItemArray.Length] = hataMesaji;

                    errorDataTable.Rows.Add(array);
                }
            }

            if (errorDataTable != null && errorDataTable.Rows.Count > 0)
            {
                errorDataTable.TableName = "OgrenciVerilerHataliSatirlar";
                var file = ExcelHelper.ExcelIndir(new List<DataTable> { errorDataTable }, "OgrenciVerilerHataliSatirlar");

                return File(file.FileContents, file.ContentType, file.FileDownLoadName);
            }
            else
            {
                return View(viewModel);
            }
        }
    }
}