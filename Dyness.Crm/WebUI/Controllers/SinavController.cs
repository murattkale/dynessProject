using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SinavController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        struct SoruKonu
        {
            public int Soru { get; set; }

            public string Dogru { get; set; }

            public string Ders { get; set; }

            public string Konu { get; set; }

            public string Kod { get; set; }

            public int KonuId { get; set; }
        }

        public SinavController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SinavDuzenleViewModel viewModel)
        {
            viewModel.SinavTurSelectList = selectListHelper.SinavTurSelectList();
            viewModel.SezonSelectList = selectListHelper.SezonSelectList();
            viewModel.OptikFormSelectList = selectListHelper.OptikFormSelectList();
            viewModel.KurumSelectList = selectListHelper.KurumSelectList();

            if (viewModel.Model == null)
                return;

            #region Yetki Şube

            int[] selectedYetkiSubeItems = viewModel.Model?.SinavSubeler != null && viewModel.Model.SinavSubeler.Count() > 0
                ? viewModel.Model.SinavSubeler.Select(x => x.SubeId).ToArray()
                : null;

            viewModel.YetkiSubeSelectList = selectListHelper.SubeSelectList(true, selectedYetkiSubeItems);

            #endregion
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SinavDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sinav>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinavService>();
                var model = service.Get(x =>
                    x.SinavId == id,
                    y => y.SinavKitapciklar,
                    y => y.SinavSubeler);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;

                if (model.SinavKitapciklar == null || model.SinavKitapciklar.Count < 4)
                {
                    if (viewModel.Model.SinavKitapciklar == null)
                    {
                        viewModel.Model.SinavKitapciklar = new List<SinavKitapcik>
                        {
                            new SinavKitapcik { SinavId = (int)id },
                            new SinavKitapcik { SinavId = (int)id },
                            new SinavKitapcik { SinavId = (int)id },
                            new SinavKitapcik { SinavId = (int)id }
                        };
                    }
                    else
                    {
                        for (int i = model.SinavKitapciklar.Count; i < 4; i++)
                        {
                            viewModel.Model.SinavKitapciklar.Add(new SinavKitapcik { SinavId = (int)id });
                        }
                    }
                }
            }
            else
            {
                viewModel.Model = new Sinav();
                viewModel.Model.SinavKitapciklar = new List<SinavKitapcik>
                {
                    new SinavKitapcik { Sinav = viewModel.Model },
                    new SinavKitapcik { Sinav = viewModel.Model },
                    new SinavKitapcik { Sinav = viewModel.Model },
                    new SinavKitapcik { Sinav = viewModel.Model }
                };

                if (id != null && id > 0)
                {
                    viewModel.Model.SinavTurId = (int)id;
                }
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SinavDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinavService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.SelectedYetkiliSubeler != null && viewModel.SelectedYetkiliSubeler.Length > 0)
                        {
                            viewModel.Model.SinavSubeler = new List<SinavSube>();

                            foreach (var sube in viewModel.SelectedYetkiliSubeler)
                            {
                                viewModel.Model.SinavSubeler.Add(new SinavSube
                                {
                                    Sinav = viewModel.Model,
                                    SubeId = sube
                                });
                            }
                        }
                        else
                        {
                            var subeService = serviceFactory.CreateService<ISubeService>();
                            var subeler = subeService.List(x => x.KurumId == viewModel.Model.KurumId).ToList();

                            if (subeler?.Count > 0)

                                viewModel.Model.SinavSubeler = new List<SinavSube>();

                            foreach (var sube in subeler)
                            {
                                viewModel.Model.SinavSubeler.Add(new SinavSube
                                {
                                    Sinav = viewModel.Model,
                                    SubeId = sube.SubeId
                                });
                            }

                        }

                        var sinavTurService = serviceFactory.CreateService<ISinavTurService>();
                        var sinavTurModel = sinavTurService.Get(x =>
                            x.SinavTurId == viewModel.Model.SinavTurId,
                            y => y.SinavTurDersler);

                        if (sinavTurModel.SinavTurDersler != null && sinavTurModel.SinavTurDersler.Any())
                        {
                            if (viewModel.Model.SinavId == 0)
                            {
                                foreach (var sinavKitapcik in viewModel.Model.SinavKitapciklar)
                                {
                                    if (string.IsNullOrEmpty(sinavKitapcik.Baslik))
                                        continue;

                                    sinavKitapcik.SinavKitapcikDersBilgiler = new List<SinavKitapcikDersBilgi>();

                                    var sinavTurDersler = sinavTurModel.SinavTurDersler.OrderBy(x => x.Sira).ToList();

                                    for (int i = 0; i < sinavTurDersler.Count; i++)
                                    {
                                        var sinavTurDers = sinavTurDersler[i];

                                        var sinavKitapcikDersBilgi = new SinavKitapcikDersBilgi
                                        {
                                            DersId = sinavTurDers.DersId,
                                            SinavKitapcik = sinavKitapcik
                                        };

                                        sinavKitapcik.SinavKitapcikDersBilgiler.Add(sinavKitapcikDersBilgi);
                                    }
                                }
                            }

                            viewModel.OperationResult = viewModel.Model.SinavId == 0
                                ? service.Add(viewModel.Model)
                                : service.Update(viewModel.Model);

                            viewModel.Model = viewModel.OperationResult.Model;
                        }

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SinavId > 0
                            ? RedirectToAction("Detay", new { id = viewModel.Model.SinavId })
                            : RedirectToAction("Duzenle");
                    }
            }

            if (viewModel.OperationResult.Status && viewModel.Model.SinavId > 0)
                return RedirectToAction("Detay", new { id = viewModel.Model.SinavId });

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult SinavKitapcikDuzenle(int? id)
        {
            var viewModel = new SinavKitapcikDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SinavKitapcik>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinavKitapcikService>();
                var model = service.Get(x =>
                    x.SinavKitapcikId == id,
                    y => y.Sinav.SinavTur.SinavTurDersler.Select(z => z.Ders),
                    y => y.SinavKitapcikDersBilgiler.Select(z => z.Ders));

                if (model == null)
                    return Redirect("/Error/NotFound");

                if (model.SinavKitapcikDersBilgiler == null)
                    model.SinavKitapcikDersBilgiler = new List<SinavKitapcikDersBilgi>();

                var sinavDersler = model.Sinav?.SinavTur?.SinavTurDersler.OrderBy(x => x.Sira).ToList();

                if (sinavDersler == null || !sinavDersler.Any())
                    return Redirect("/Error/NotFound");

                model.SinavSorular = new List<SinavSoru>();

                for (int i = 0; i < sinavDersler.Count; i++)
                {
                    var sinavDers = sinavDersler[i];

                    var sinavKitapcikDersBilgi = model.SinavKitapcikDersBilgiler.FirstOrDefault(x => x.DersId == sinavDers.DersId);

                    if (sinavKitapcikDersBilgi == null || sinavKitapcikDersBilgi.CevapAnahtartari == null || string.IsNullOrEmpty(sinavKitapcikDersBilgi.CevapAnahtartari))
                    {
                        for (int j = 0; j < sinavDers.SoruSayi; j++)
                        {
                            var soru = new SinavSoru
                            {
                                DersId = sinavDers.DersId,
                                Ders = sinavDers.Ders,
                                SinavKitapcikId = (int)id,
                                Soru = j + 1
                            };

                            model.SinavSorular.Add(soru);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < sinavKitapcikDersBilgi.CevapAnahtartari.Length; j++)
                        {
                            var dogruCevap = sinavKitapcikDersBilgi.CevapAnahtartari[j];

                            int? konuId = null;

                            if (!string.IsNullOrEmpty(sinavKitapcikDersBilgi.DersKonuBilgi) && sinavKitapcikDersBilgi.DersKonuBilgi.Split(',').Length - 1 == sinavKitapcikDersBilgi.CevapAnahtartari.Length)
                            {
                                konuId = Convert.ToInt32(sinavKitapcikDersBilgi.DersKonuBilgi.Split(',')[j]);
                            }

                            var soru = new SinavSoru
                            {
                                DersId = sinavDers.DersId,
                                Ders = sinavDers.Ders,
                                Dogru = dogruCevap.ToString(),
                                KonuId = konuId,
                                SinavKitapcikId = (int)id,
                                Soru = j + 1
                            };

                            model.SinavSorular.Add(soru);
                        }
                    }
                }

                viewModel.Model = model;
            }
            else
            {
                return Redirect("/Error/NotFound");
            }

            var konuService = serviceFactory.CreateService<IKonuService>();
            viewModel.Konular = konuService.List().ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult SinavKitapcikDuzenle(SinavKitapcikDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var bosVarMi = false;

            foreach (var sinavSoru in viewModel.Model.SinavSorular)
            {
                if (string.IsNullOrEmpty(sinavSoru.Dogru))
                {
                    bosVarMi = true;
                    break;
                }
            }

            if (bosVarMi)
            {
                var konuService = serviceFactory.CreateService<IKonuService>();
                viewModel.Konular = konuService.List().ToList();

                return View(viewModel);
            }

            var service = serviceFactory.CreateService<ISinavKitapcikService>();
            var model = service.Get(x => x.SinavKitapcikId == viewModel.Model.SinavKitapcikId, y => y.SinavKitapcikDersBilgiler);

            if (model.SinavKitapcikDersBilgiler == null)
                model.SinavKitapcikDersBilgiler = new List<SinavKitapcikDersBilgi>();

            var sinavSoruDersler = viewModel.Model.SinavSorular.Select(x => x.DersId).Distinct().ToList();

            for (int i = 0; i < sinavSoruDersler.Count; i++)
            {
                var sinavSoruDersId = sinavSoruDersler[i];
                var sinavKitapcikDersBilgi = model.SinavKitapcikDersBilgiler?.FirstOrDefault(x => x.DersId == sinavSoruDersId);

                var dersSinavSorular = viewModel.Model.SinavSorular.Where(x => x.DersId == sinavSoruDersId).ToList();

                var cevaplar = string.Join("", dersSinavSorular.Select(x => x.Dogru).ToArray());
                var konular = "";

                for (int j = 0; j < cevaplar.Length; j++)
                {
                    konular += $"{(dersSinavSorular[j].KonuId == null || dersSinavSorular[j].KonuId == 0 ? "" : dersSinavSorular[j].KonuId.ToString())},";
                }

                if (sinavKitapcikDersBilgi == null)
                {
                    sinavKitapcikDersBilgi = new SinavKitapcikDersBilgi
                    {
                        DersId = sinavSoruDersId,
                        SinavKitapcikId = viewModel.Model.SinavKitapcikId,
                        CevapAnahtartari = cevaplar,
                        DersKonuBilgi = konular
                    };

                    model.SinavKitapcikDersBilgiler.Add(sinavKitapcikDersBilgi);
                }
                else
                {
                    var index = model.SinavKitapcikDersBilgiler.IndexOf(sinavKitapcikDersBilgi);
                    model.SinavKitapcikDersBilgiler[index].CevapAnahtartari = cevaplar;
                    model.SinavKitapcikDersBilgiler[index].DersKonuBilgi = konular;
                }
            }

            viewModel.Model.SinavKitapcikDersBilgiler = model.SinavKitapcikDersBilgiler.OrderBy(x => x.Sira).ToList();

            var cevapAnahtari = "";

            foreach (var sinavKitapcikDersBilgi in viewModel.Model.SinavKitapcikDersBilgiler)
            {
                cevapAnahtari += sinavKitapcikDersBilgi.CevapAnahtartari;
            }

            viewModel.Model.CevapAnahtari = cevapAnahtari;

            var operationResult = service.Update(viewModel.Model);

            if (operationResult.Status)
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = operationResult.Status,
                    MessageInfos = operationResult.MessageInfos
                };

                return RedirectToAction("Detay", new { id = viewModel.Model.SinavId });
            }
            else
            {
                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = operationResult.Status,
                    MessageInfos = operationResult.MessageInfos
                };

                TempData["OperationResult"] = returnOperationResult;

                return RedirectToAction("SinavKitapcikDuzenle", "Sinav", new { id = viewModel.Model.SinavKitapcikId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult SinavKitapcikYukle(HttpPostedFileBase sinavKitapcikSorular, int dosyaSinavId, int dosyaSinavKitapcikId, string dosyaSinavKitapcik)
        {
            if (!string.Equals(Path.GetExtension(sinavKitapcikSorular.FileName), ".txt"))
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.SoruAnahtarDosyaYuklemeUzantiHata, MessageInfoType = MessageInfoType.Error } }
                };

                return RedirectToAction("Detay", new { id = dosyaSinavId });
            }

            var path = string.Empty;

            if (sinavKitapcikSorular != null)
            {
                var directory = Server.MapPath($"~/Sinav/{dosyaSinavId}/{dosyaSinavKitapcik}/");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                path = Path.Combine(Server.MapPath($"~/Sinav/{dosyaSinavId}/{dosyaSinavKitapcik}/"), "cevapAnahtar" + Path.GetExtension(sinavKitapcikSorular.FileName));
                sinavKitapcikSorular.SaveAs(path);
            }

            try
            {
                string[] lines;
                var list = new List<string>();
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                lines = list.ToArray();

                var yanitlar = lines[0];

                var service = serviceFactory.CreateService<ISinavService>();
                var model = service.Get(x => x.SinavId == dosyaSinavId,
                    y => y.SinavKitapciklar.Select(z => z.SinavKitapcikDersBilgiler),
                    y => y.SinavTur.SinavTurDersler);

                var sinavKitapcik = model.SinavKitapciklar.FirstOrDefault(x => x.Baslik == dosyaSinavKitapcik);
                var sinavTurDersler = model.SinavTur.SinavTurDersler.OrderBy(x => x.Sira).ToList();

                sinavKitapcik.CevapAnahtari = yanitlar;

                for (int i = 0; i < sinavKitapcik.SinavKitapcikDersBilgiler.Count; i++)
                {
                    sinavKitapcik.SinavKitapcikDersBilgiler[i].Sira = (int)model.SinavTur.SinavTurDersler.FirstOrDefault(x => x.DersId == sinavKitapcik.SinavKitapcikDersBilgiler[i].DersId).Sira;
                }

                sinavKitapcik.SinavKitapcikDersBilgiler = sinavKitapcik.SinavKitapcikDersBilgiler.OrderBy(x => x.Sira).ToList();

                for (int i = 0; i < sinavKitapcik.SinavKitapcikDersBilgiler.Count; i++)
                {
                    var sinavTurDers = sinavTurDersler.FirstOrDefault(x => x.DersId == sinavKitapcik.SinavKitapcikDersBilgiler[i].DersId);

                    var dersCevaplar = yanitlar.Substring(0, (int)sinavTurDers.SoruSayi);

                    sinavKitapcik.SinavKitapcikDersBilgiler[i].CevapAnahtartari = dersCevaplar;

                    yanitlar = yanitlar.Substring((int)sinavTurDers.SoruSayi, yanitlar.Length - dersCevaplar.Length);

                    if (dersCevaplar.Length != sinavTurDers.SoruSayi)
                    {
                        TempData["OperationResult"] = new EntityOperationResult<Sinav>
                        {
                            Status = false,
                            MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = $"{Resources.LangResources.SinavTurDersSoruSayiCevapAnahtariUyusmuyor} ({sinavTurDers.Ders.DersAd})" , MessageInfoType = MessageInfoType.Error } }
                        };

                        return RedirectToAction("Detay", new { id = dosyaSinavId });
                    }
                }

                var sinavKitapcikService = serviceFactory.CreateService<ISinavKitapcikService>();

                var operationResult = sinavKitapcikService.Update(sinavKitapcik);

                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = operationResult.Status,
                    MessageInfos = operationResult.MessageInfos
                };

                TempData["OperationResult"] = returnOperationResult;
            }
            catch (Exception ex)
            {
                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = ex.Message, MessageInfoType = MessageInfoType.Error } }
                };

                TempData["OperationResult"] = returnOperationResult;
            }

            return RedirectToAction("Detay", new { id = dosyaSinavId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult SinavSoruKonuYukle(HttpPostedFileBase soruKonular, int soruSinavId, int soruKonuKitapcikId, string soruKonuKitapcik)
        {
            if (!string.Equals(Path.GetExtension(soruKonular.FileName), ".xlsx"))
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.SinavKitapcikDosyaEkle, MessageInfoType = MessageInfoType.Error } }
                };

                return RedirectToAction("Detay", new { id = soruSinavId });
            }

            var path = string.Empty;

            if (soruKonular != null)
            {
                var directory = Server.MapPath($"~/Dosya/Sinav/{soruSinavId}/{soruKonuKitapcik}/");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                path = Path.Combine(Server.MapPath($"~/Dosya/Sinav/{soruSinavId}/{soruKonuKitapcik}/"), $"konuDagilim{Path.GetExtension(soruKonular.FileName)}");
                soruKonular.SaveAs(path);
            }

            try
            {
                var sorular = new List<SoruKonu>();

                var connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", path);

                var connection = new OleDbConnection(connectionString);
                var command = new OleDbCommand { Connection = connection };
                var adapter = new OleDbDataAdapter();
                var dt = new DataTable();

                connection.Open();

                var dtExcelSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null)
                {
                    var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                    connection.Close();

                    connection.Open();
                    command.CommandText = "SELECT * From [" + sheetName + "]";

                    adapter.SelectCommand = command;
                    adapter.Fill(dt);
                    connection.Close();

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];

                        int value;
                        if (!int.TryParse(row[0].ToString(), out value))
                            continue;

                        var soru = Convert.ToInt32(row[0]);
                        var dogru = row[1].ToString();
                        var ders = row[2].ToString();
                        var konu = row[3].ToString();
                        var konuKod = row[4].ToString();

                        sorular.Add(new SoruKonu { Soru = soru, Dogru = dogru, Ders = ders, Konu = konu, Kod = konuKod });
                    }
                }

                var service = serviceFactory.CreateService<ISinavService>();
                var model = service.Get(x => x.SinavId == soruSinavId,
                   y => y.SinavKitapciklar.Select(z => z.SinavKitapcikDersBilgiler.Select(k => k.Ders)),
                   y => y.SinavTur.SinavTurDersler);

                var sinavKitapcik = model.SinavKitapciklar.FirstOrDefault(x => x.Baslik == soruKonuKitapcik);

                var dersService = serviceFactory.CreateService<IDersService>();
                var konuService = serviceFactory.CreateService<IKonuService>();

                var cevapAnahtariGenel = "";

                for (int i = 0; i < sinavKitapcik.SinavKitapcikDersBilgiler.Count; i++)
                {
                    var cevapAnahtari = "";
                    var dersKonular = "";

                    var dersSorular = sorular.Where(x => x.Ders == sinavKitapcik.SinavKitapcikDersBilgiler[i].Ders.DersAd).ToList();

                    if (dersSorular == null || dersSorular.Count == 0)
                    {
                        TempData["OperationResult"] = new EntityOperationResult<Sinav>
                        {
                            Status = false,
                            MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = $"{Resources.LangResources.DersBulunamadi} - {sinavKitapcik.SinavKitapcikDersBilgiler[i].Ders.DersAd}", MessageInfoType = MessageInfoType.Error } }
                        };

                        return RedirectToAction("Detay", new { id = soruSinavId });
                    }

                    for (int j = 0; j < dersSorular.Count; j++)
                    {
                        var dersAd = dersSorular[j].Ders;
                        var konuBaslik = dersSorular[j].Konu;
                        var konuKod = sorular[j].Kod;

                        var ders = dersService.Get(x => x.DersAd == dersAd);

                        if (ders == null)
                        {
                            TempData["OperationResult"] = new EntityOperationResult<Sinav>
                            {
                                Status = false,
                                MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = $"{Resources.LangResources.DersBulunamadi} - {dersAd}", MessageInfoType = MessageInfoType.Error } }
                            };

                            return RedirectToAction("Detay", new { id = soruSinavId });
                        }

                        var konu = konuService.Get(x => x.DersId == ders.DersId && x.Kod == konuKod && x.KurumId == model.KurumId);

                        if (konu == null)
                        {
                            konu = new Konu
                            {
                                DersId = ders.DersId,
                                Baslik = konuBaslik,
                                Kod = konuKod,
                                EtkinMi = true,
                                KurumId = model.KurumId
                            };

                            var konuOperaitonResult = konuService.Add(konu);

                            konu = konuOperaitonResult.Model;
                        }

                        if (konu.KonuId == 0 || konu.DersId != sinavKitapcik.SinavKitapcikDersBilgiler[i].DersId)
                        {
                            TempData["OperationResult"] = new EntityOperationResult<Sinav>
                            {
                                Status = false,
                                MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.CevapAnahtariDersSirasinaGore, MessageInfoType = MessageInfoType.Error } }
                            };

                            return RedirectToAction("Detay", new { id = soruSinavId });
                        }

                        cevapAnahtari += dersSorular[j].Dogru;
                        dersKonular += $"{konu.KonuId},";
                    }

                    sinavKitapcik.SinavKitapcikDersBilgiler[i].CevapAnahtartari = cevapAnahtari.Trim();
                    sinavKitapcik.SinavKitapcikDersBilgiler[i].DersKonuBilgi = dersKonular.Trim();

                    cevapAnahtariGenel += cevapAnahtari.Trim();
                }

                sinavKitapcik.CevapAnahtari = cevapAnahtariGenel;

                var sinavKitapcikService = serviceFactory.CreateService<ISinavKitapcikService>();

                var operationResult = sinavKitapcikService.Update(sinavKitapcik);

                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = operationResult.Status,
                    MessageInfos = operationResult.MessageInfos
                };

                TempData["OperationResult"] = returnOperationResult;
            }
            catch (Exception ex)
            {
                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = ex.Message, MessageInfoType = MessageInfoType.Error } }
                };

                TempData["OperationResult"] = returnOperationResult;
            }

            return RedirectToAction("Detay", new { id = soruSinavId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult SubeOptikFormKaydet(HttpPostedFileBase subeOptikForm, int optikFormSinavId, int optikFormSubeId)
        {
            if (!string.Equals(Path.GetExtension(subeOptikForm.FileName.ToLower()), ".txt"))
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.SoruAnahtarDosyaYuklemeUzantiHata, MessageInfoType = MessageInfoType.Error } }
                };

                return RedirectToAction("Detay", new { id = optikFormSinavId });
            }

            var service = serviceFactory.CreateService<ISinavService>();
            var model = service.Get(x => x.SinavId == optikFormSinavId,
                   y => y.SinavSubeler,
                   y => y.SinavTur,
                   y => y.SinavTur.SinavTurDersKatSayilar.Select(z => z.DersGrup),
                   y => y.SinavTur.SinavTurDersler.Select(z => z.Ders),
                   y => y.OptikForm.OptikFormDersGrupBilgiler.Select(z => z.DersGrup),
                   y => y.SinavKitapciklar);

            var yetkiliSubeMi = true;

            var cevapAnahtariYuklenmisMi = model?.SinavKitapciklar?.Count(x => !string.IsNullOrWhiteSpace(x.CevapAnahtari)) == model?.SinavKitapciklar?.Count();

            if (!cevapAnahtariYuklenmisMi)
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.CevapAnahtariYuklenmeli, MessageInfoType = MessageInfoType.Error } }
                };

                return RedirectToAction("Detay", new { id = optikFormSinavId });
            }

            if (model.SinavSubeler != null && model.SinavSubeler.Any())
            {
                yetkiliSubeMi = model.SinavSubeler.Count(x => x.SubeId == optikFormSubeId) > 0;
            }

            if (!yetkiliSubeMi)
            {
                TempData["OperationResult"] = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.YetkisizIslem, MessageInfoType = MessageInfoType.Error } }
                };

                return RedirectToAction("Detay", new { id = optikFormSinavId });
            }

            var path = string.Empty;

            if (subeOptikForm != null)
            {
                var directory = Server.MapPath($"~{AyarlarService.Get().SubeSinavDosyaYol}{optikFormSinavId}/");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var dosyaAd = $"sube{optikFormSubeId}";

                path = Path.Combine(directory, $"{dosyaAd}{Path.GetExtension(subeOptikForm.FileName)}");

                subeOptikForm.SaveAs(path);

                var sinavSubeService = serviceFactory.CreateService<ISinavSubeService>();
                var sinavSubeModel = sinavSubeService.Get(x => x.SubeId == optikFormSubeId && x.SinavId == optikFormSinavId);

                sinavSubeModel.DosyaEkleyenPersonelId = Identity.PersonelId;
                sinavSubeModel.DosyaAd = $"{dosyaAd}{Path.GetExtension(subeOptikForm.FileName)}";
                sinavSubeModel.DosyaYuklenmeTarih = DateTime.Now;

                sinavSubeModel.Sinav = null;
                sinavSubeService.Update(sinavSubeModel);
            }

            try
            {
                var returnOperationResult = new EntityOperationResult<Sinav>() { MessageInfos = new List<MessageInfo>() };

                string[] lines;
                var list = new List<string>();
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                lines = list.ToArray();

                for (int i = 0; i < lines.Length; i++)
                {
                    if (string.IsNullOrEmpty(lines[i].Trim()))
                        continue;

                    var line = lines[i];

                    var tcKimlik = line.Substring((int)model.OptikForm.TcNoBasla, (int)model.OptikForm.TcNoAdet).Trim();
                    var ogrenciNo = model.OptikForm.OgrenciNoBasla == null || model.OptikForm.OgrenciNoAdet == null
                        ? string.Empty
                        : line.Substring((int)model.OptikForm.OgrenciNoBasla, (int)model.OptikForm.OgrenciNoAdet).Trim();

                    var ad = "";
                    var soyad = "";
                    var adSoyad = "";

                    if (model.OptikForm.AdSoyadBasla != null && model.OptikForm.AdSoyadAdet != null)
                    {
                        adSoyad = line.Substring((int)model.OptikForm.AdSoyadBasla, (int)model.OptikForm.AdSoyadAdet).Trim();

                        soyad = adSoyad.Split(' ')[adSoyad.Split(' ').Length - 1];
                        ad = adSoyad.Replace(soyad, "").Trim();

                        if (string.IsNullOrEmpty(ad))
                        {
                            ad = soyad;
                            soyad = "";
                        }

                    }
                    else if (model.OptikForm.AdBasla != null && model.OptikForm.AdAdet != null && model.OptikForm.SoyadBasla != null && model.OptikForm.SoyadAdet != null)
                    {
                        ad = line.Substring((int)model.OptikForm.AdBasla, (int)model.OptikForm.AdAdet).Trim();
                        soyad = line.Substring((int)model.OptikForm.SoyadBasla, (int)model.OptikForm.SoyadAdet).Trim();

                        adSoyad = $"{ad} {soyad}".Trim();
                    }

                    var sinif = model.OptikForm.SinifBasla == null || model.OptikForm.SinifAdet == null
                        ? string.Empty
                        : line.Substring((int)model.OptikForm.SinifBasla, (int)model.OptikForm.SinifAdet).Trim();
                    var kitapcik = line.Substring((int)model.OptikForm.KitapcikTurBasla, (int)model.OptikForm.KitapcikTurAdet).Trim();
                    var cinsiyet = model.OptikForm.CinsiyetBasla == null || model.OptikForm.CinsiyetAdet == null
                        ? string.Empty
                        : line.Substring((int)model.OptikForm.CinsiyetBasla, (int)model.OptikForm.CinsiyetAdet).Trim();

                    // OgrenciSinavKontrol'de Öğrenci Cevaplar alanını doldurmak için kullanacağız.
                    var soruCevaplar = "";

                    if (string.IsNullOrEmpty(kitapcik))
                    {
                        returnOperationResult.MessageInfos.Add(new MessageInfo { Message = $"{Resources.LangResources.OptikFormSinavKitapcikBos} ({adSoyad})", MessageInfoType = MessageInfoType.Error });
                        continue;
                    }

                    // Optik Formdan gelen, OptikFormDersGrupBilgi DERS GRUP indeks ve toplamlar ile LINE'den gelen satırı parçalıyoruz.
                    for (int j = 0; j < model.OptikForm.OptikFormDersGrupBilgiler.Count; j++)
                    {
                        var optikFormDersGrupBilgi = model.OptikForm.OptikFormDersGrupBilgiler[j];

                        // OptikFormDersGrupBilgi için, DersGrupBasla ve DersGrupAdet ile stringi buluyoruz.
                        var dersGrupOgrenciCevaplar = line.Substring((int)optikFormDersGrupBilgi.DersGrupBasla, (int)optikFormDersGrupBilgi.DersGrupAdet);

                        // Ders Gruba bağlı SinavTurDerslerde döneceğiz. SinavTurDers'ta OgrenciCevaplar alanını dolduracağız. Öğrencinin cevapları her ders için alıyoruz.
                        var dersGrupDersler = model.SinavTur.SinavTurDersler.Where(x => x.Ders.DersGrupId == optikFormDersGrupBilgi.DersGrupId).OrderBy(x => x.Sira).ToList();

                        // String olarak parcalayacağımız cevaplar değişkeni.
                        var parcalaDersGrupOgrenciCevaplar = dersGrupOgrenciCevaplar;

                        if (parcalaDersGrupOgrenciCevaplar.Length != dersGrupDersler.Sum(x => x.SoruSayi))
                        {
                            TempData["OperationResult"] = new EntityOperationResult<Sinav>
                            {
                                Status = false,
                                MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = $"{Resources.LangResources.OptikFormHata} : {optikFormDersGrupBilgi.DersGrup.DersGrupAd} ({Resources.LangResources.UzunlukHatasi})", MessageInfoType = MessageInfoType.Error } }
                            };

                            return RedirectToAction("Detay", new { id = optikFormSinavId });
                        }

                        // Ders Gruba bağlı derslerde dönüyoruz. Her biri için öğrenci cevapları almak istiyoruz.
                        for (int k = 0; k < dersGrupDersler.Count; k++)
                        {
                            var dersGrupDers = dersGrupDersler[k];

                            // dersGrupOgrenciCevaplar değişkenine, ders grup için cevapları almıştık. Ders gruba bağlı dersleri de sıraladığımız için, 0 indekste ilk ders olacak. Onu soru adeti kadar bölümü alacağız.
                            var dersGrupDersCevaplar = parcalaDersGrupOgrenciCevaplar.Substring(0, (int)dersGrupDers.SoruSayi);

                            // Listede OgrenciCevaplar alanını güncelleyeceğiz.
                            model.SinavTur.SinavTurDersler[model.SinavTur.SinavTurDersler.IndexOf(dersGrupDers)].OgrenciCevaplar = dersGrupDersCevaplar;

                            // Sonraki ders için, parcalaDersGrupOgrenciCevaplar değişkeninden önceki dersin cevaplarını çıkartıyoruz.
                            parcalaDersGrupOgrenciCevaplar = parcalaDersGrupOgrenciCevaplar.Substring((int)dersGrupDers.SoruSayi, parcalaDersGrupOgrenciCevaplar.Length - (int)dersGrupDers.SoruSayi);
                        }

                        // Öğrencinin tüm cevaplarını alacağız.
                        soruCevaplar += dersGrupOgrenciCevaplar;
                    }

                    // ogrenciSinavKontrol.Dogrulamalar alanı için kullanılacak. Tüm derslerin doğrulamaları
                    var dogrulamalar = "";

                    var sinavKitapcik = model.SinavKitapciklar.FirstOrDefault(x => x.Baslik == kitapcik);

                    // Her ders için, OgrenciSinavKontrol ekleyeceğiz.
                    model.SinavTur.SinavTurDersler = model.SinavTur.SinavTurDersler.OrderBy(x => x.Sira).ToList();

                    var puan = model.SinavTur.TabanPuan;

                    var dogruAdet = 0;
                    var bosAdet = 0;
                    var yanlisAdet = 0;
                    // Toplam net bulunacak
                    double netHesaplanan = 0;

                    var dersKatSayilar = new List<SinavTurDersKatSayi>();

                    var ogrenciSinavKontrol = new OgrenciSinavKontrol
                    {
                        TcKimlikNo = tcKimlik,
                        OgrenciNo = ogrenciNo,
                        Ad = ad,
                        Soyad = soyad,
                        AdSoyad = adSoyad,
                        Sinif = sinif,
                        KitapcikBaslik = kitapcik,
                        Cinsiyet = cinsiyet,
                        OgrenciSinavKontrolPuanTurPuanlar = new List<OgrenciSinavKontrolPuanTurPuan>(),
                        OgrenciSinavKontrolDersBilgiler = new List<OgrenciSinavKontrolDersBilgi>()
                    };

                    // Cevap anahtarının hepsi
                    var cevapAnahtari = sinavKitapcik.CevapAnahtari;

                    for (int j = 0; j < model.SinavTur.SinavTurDersler.Count; j++)
                    {
                        var sinavTurDers = model.SinavTur.SinavTurDersler[j];

                        // ders için cevap anahtarı alıyoruz.
                        var dogruYanitlar = cevapAnahtari.Substring(0, (int)sinavTurDers.SoruSayi);

                        var ogrenciSinavKontrolDersBilgi = new OgrenciSinavKontrolDersBilgi
                        {
                            DersId = sinavTurDers.DersId,
                            OgrenciSinavKontrol = ogrenciSinavKontrol,
                            SoruCevaplar = sinavTurDers.OgrenciCevaplar,
                            BosCevapAdet = 0,
                            DogruCevapAdet = 0,
                            YanlisCevapAdet = 0,
                            Net = 0
                        };

                        // Boş, doğru vb bulacağız
                        for (int k = 0; k < dogruYanitlar.Length; k++)
                        {
                            // Öğrencinin cevabı boş ise
                            if (string.IsNullOrEmpty(sinavTurDers.OgrenciCevaplar[k].ToString().Trim()))
                            {
                                dogrulamalar += "B";
                                bosAdet++;
                                ogrenciSinavKontrolDersBilgi.BosCevapAdet++;
                            }
                            // Öğrencinin cevabı ve doğru yanıt eşit ise
                            else if (string.Equals(dogruYanitlar[k].ToString(), sinavTurDers.OgrenciCevaplar[k].ToString()))
                            {
                                dogrulamalar += "D";
                                dogruAdet++;
                                ogrenciSinavKontrolDersBilgi.DogruCevapAdet++;
                            }
                            // Öğrencinin cevabı ve doğru yanıt eşit değil ise
                            else
                            {
                                dogrulamalar += "Y";
                                yanlisAdet++;
                                ogrenciSinavKontrolDersBilgi.YanlisCevapAdet++;
                            }
                        }

                        // Ders için net hesaplanıyor. Doğru Cevap adetten, yanlış cevap / SinavTur.KacYanlisBirDogruyuGoturur çıkartıyoruz.
                        var dersNet = ogrenciSinavKontrolDersBilgi.DogruCevapAdet -
                            (ogrenciSinavKontrolDersBilgi.YanlisCevapAdet > 0
                                ? ((double)ogrenciSinavKontrolDersBilgi.YanlisCevapAdet / model.SinavTur.KacYanlisBirDogruyuGoturur)
                                : 0) ?? 0;

                        ogrenciSinavKontrolDersBilgi.Net = Math.Round(dersNet, 3);
                        netHesaplanan += dersNet;

                        ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler.Add(ogrenciSinavKontrolDersBilgi);

                        // OgrenciSinavKontrolPuanTurPuan için, her ders gruptan ders için puan hesaplayacağız.
                        var sinavTurDersKatSayilar = model.SinavTur.SinavTurDersKatSayilar.Where(x => x.DersGrupId == sinavTurDers.Ders.DersGrupId);

                        foreach (var sinavTurDersKatSayi in sinavTurDersKatSayilar)
                        {
                            // Puantür için OgrenciSinavKontrolPuanTurPuan var mı kontrol ediyoruz. Varsa puanı toplayacağız, yoksa listeye ekleyeceğiz.
                            var ogrenciSinavKontrolPuanTurPuan = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar.FirstOrDefault(x => x.PuanTurId == sinavTurDersKatSayi.PuanTurId);

                            if (ogrenciSinavKontrolPuanTurPuan == null)
                            {
                                ogrenciSinavKontrolPuanTurPuan = new OgrenciSinavKontrolPuanTurPuan
                                {
                                    OgrenciSinavKontrol = ogrenciSinavKontrol,
                                    PuanTurId = (int)sinavTurDersKatSayi.PuanTurId
                                };

                                // Toplam puanı bulmak için her dersin puanını topluyoruz.
                                ogrenciSinavKontrolPuanTurPuan.Puan += ogrenciSinavKontrolDersBilgi.Net * (double)sinavTurDersKatSayi.KatSayi;
                                ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar.Add(ogrenciSinavKontrolPuanTurPuan);
                            }
                            else
                            {
                                var index = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar.IndexOf(ogrenciSinavKontrolPuanTurPuan);
                                // Toplam puanı bulmak için her dersin puanını topluyoruz.
                                ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar[index].Puan += ogrenciSinavKontrolDersBilgi.Net * (double)sinavTurDersKatSayi.KatSayi;
                            }
                        }

                        // Cevap anahtarından sonraki ders için, önceki kısmını kaldırıyoruz.
                        cevapAnahtari = cevapAnahtari.Substring((int)sinavTurDers.SoruSayi, cevapAnahtari.Length - (int)sinavTurDers.SoruSayi);
                    }

                    foreach (var ogrenciSinavKontrolPuanTurPuan in ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar)
                    {
                        ogrenciSinavKontrolPuanTurPuan.ToplamPuan = ogrenciSinavKontrolPuanTurPuan.Puan + (double)model.SinavTur.TabanPuan;
                    }

                    ogrenciSinavKontrol.SubeId = optikFormSubeId;
                    ogrenciSinavKontrol.TabanPuan = (double)model.SinavTur.TabanPuan;
                    ogrenciSinavKontrol.DogruCevapAdet = dogruAdet;
                    ogrenciSinavKontrol.YanlisCevapAdet = yanlisAdet;
                    ogrenciSinavKontrol.BosCevapAdet = bosAdet;
                    ogrenciSinavKontrol.Net = Math.Round(netHesaplanan, 3);
                    ogrenciSinavKontrol.SinavKitapcikId = sinavKitapcik.SinavKitapcikId;
                    ogrenciSinavKontrol.SoruCevaplar = soruCevaplar;
                    ogrenciSinavKontrol.Dogrulamalar = dogrulamalar;

                    var ogrenciService = serviceFactory.CreateService<IOgrenciService>();
                    var ogrenciModel = ogrenciService.Get(x => x.TcKimlikNo == tcKimlik);

                    if (ogrenciModel == null)
                    {
                        ogrenciSinavKontrol.OnKayit = new OnKayit
                        {
                            Ad = ogrenciSinavKontrol.Ad,
                            Soyad = ogrenciSinavKontrol.Soyad,
                            AdSoyad = ogrenciSinavKontrol.AdSoyad,
                            TcKimlikNo = ogrenciSinavKontrol.TcKimlikNo,
                            KadinMi = ogrenciSinavKontrol.Cinsiyet.ToLower() == "k"
                        };
                    }
                    else
                    {
                        ogrenciSinavKontrol.OgrenciId = ogrenciModel.OgrenciId;
                    }

                    var ogrenciSinavKontrolService = serviceFactory.CreateService<IOgrenciSinavKontrolService>();

                    var checkOgrenciSinav = ogrenciSinavKontrolService.Get(x =>
                        x.SinavKitapcikId == ogrenciSinavKontrol.SinavKitapcikId &&
                        x.AdSoyad == ogrenciSinavKontrol.AdSoyad &&
                        x.TcKimlikNo == ogrenciSinavKontrol.TcKimlikNo,
                        y => y.OgrenciSinavKontrolDersBilgiler,
                        y => y.OgrenciSinavKontrolPuanTurPuanlar);

                    if (checkOgrenciSinav == null)
                    {
                        var operationResult = ogrenciSinavKontrolService.Add(ogrenciSinavKontrol);

                        if (!operationResult.Status)
                        {
                            returnOperationResult.MessageInfos.AddRange(operationResult.MessageInfos);
                        }
                    }
                    else
                    {
                        checkOgrenciSinav.SoruCevaplar = ogrenciSinavKontrol.SoruCevaplar;
                        checkOgrenciSinav.DogruCevapAdet = ogrenciSinavKontrol.DogruCevapAdet;
                        checkOgrenciSinav.YanlisCevapAdet = ogrenciSinavKontrol.YanlisCevapAdet;
                        checkOgrenciSinav.BosCevapAdet = ogrenciSinavKontrol.BosCevapAdet;
                        checkOgrenciSinav.Net = ogrenciSinavKontrol.Net;
                        checkOgrenciSinav.TabanPuan = ogrenciSinavKontrol.TabanPuan;
                        checkOgrenciSinav.Dogrulamalar = ogrenciSinavKontrol.Dogrulamalar;
                        checkOgrenciSinav.OgrenciNo = ogrenciSinavKontrol.OgrenciNo;
                        checkOgrenciSinav.Sinif = ogrenciSinavKontrol.Sinif;
                        checkOgrenciSinav.Cinsiyet = ogrenciSinavKontrol.Cinsiyet;

                        foreach (var ogrenciSinavKontrolDersBilgi in checkOgrenciSinav.OgrenciSinavKontrolDersBilgiler)
                        {
                            var guncelleyecekOgrenciSinavKontrolDersBilgi = ogrenciSinavKontrol.OgrenciSinavKontrolDersBilgiler.FirstOrDefault(x => x.DersId == ogrenciSinavKontrolDersBilgi.DersId);

                            ogrenciSinavKontrolDersBilgi.SoruCevaplar = guncelleyecekOgrenciSinavKontrolDersBilgi.SoruCevaplar;
                            ogrenciSinavKontrolDersBilgi.DogruCevapAdet = guncelleyecekOgrenciSinavKontrolDersBilgi.DogruCevapAdet;
                            ogrenciSinavKontrolDersBilgi.YanlisCevapAdet = guncelleyecekOgrenciSinavKontrolDersBilgi.YanlisCevapAdet;
                            ogrenciSinavKontrolDersBilgi.BosCevapAdet = guncelleyecekOgrenciSinavKontrolDersBilgi.BosCevapAdet;
                            ogrenciSinavKontrolDersBilgi.Net = guncelleyecekOgrenciSinavKontrolDersBilgi.Net;
                        }

                        foreach (var ogrenciSinavKontrolPuanTurPuan in checkOgrenciSinav.OgrenciSinavKontrolPuanTurPuanlar)
                        {
                            var guncelleyecekOgrenciSinavKontrolPuanTurPuan = ogrenciSinavKontrol.OgrenciSinavKontrolPuanTurPuanlar.FirstOrDefault(x => x.PuanTurId == ogrenciSinavKontrolPuanTurPuan.PuanTurId);

                            ogrenciSinavKontrolPuanTurPuan.Puan = guncelleyecekOgrenciSinavKontrolPuanTurPuan.Puan;
                            ogrenciSinavKontrolPuanTurPuan.ToplamPuan = guncelleyecekOgrenciSinavKontrolPuanTurPuan.ToplamPuan;
                            ogrenciSinavKontrolPuanTurPuan.SinifSira = 0;
                            ogrenciSinavKontrolPuanTurPuan.SubeSira = 0;
                            ogrenciSinavKontrolPuanTurPuan.GenelSira = 0;
                        }

                        var operationResult = ogrenciSinavKontrolService.Update(checkOgrenciSinav);

                        if (!operationResult.Status)
                        {
                            returnOperationResult.MessageInfos.AddRange(operationResult.MessageInfos);
                        }
                    }
                }

                returnOperationResult.Status = returnOperationResult.MessageInfos.Count == 0;

                TempData["OperationResult"] = returnOperationResult;
            }
            catch (Exception ex)
            {

                var returnOperationResult = new EntityOperationResult<Sinav>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = ex.Message, MessageInfoType = MessageInfoType.Error } }
                };

                TempData["OperationResult"] = returnOperationResult;
            }

            return RedirectToAction("Detay", new { id = optikFormSinavId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult GoruntulenmeDurumuGuncelle(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinavService>();
            var model = service.Get(x => x.SinavId == id);

            model.SonuclarGoruntulenebilir = !model.SonuclarGoruntulenebilir;

            var operationResult = service.Update(model);

            // Sıralama Güncelleme
            if (model.SonuclarGoruntulenebilir && operationResult.Status)
            {
                var sinavSonucService = serviceFactory.CreateService<IOgrenciSinavKontrolPuanTurPuanService>();
                var sinavSonuclar = sinavSonucService.ListDto(id);

                var subeler = sinavSonuclar.Select(x => x.SubeId).Distinct().ToList();
                var siniflar = sinavSonuclar.Select(x => x.Sinif).Distinct().ToList();

                if (sinavSonuclar != null && sinavSonuclar.Any())
                {
                    var puanTurler = sinavSonuclar.Select(x => x.PuanTurId).Distinct().ToList();

                    foreach (var puanTur in puanTurler)
                    {
                        var genelSinavSonuclar = sinavSonuclar.Where(x => x.PuanTurId == puanTur).ToList();
                        var genelPuanlar = genelSinavSonuclar.Select(x => x.ToplamPuan).Distinct().OrderByDescending(x => x).ToList();

                        foreach (var genelSinavSonuc in genelSinavSonuclar)
                        {
                            var sira = genelPuanlar.IndexOf(genelSinavSonuc.ToplamPuan) + 1;
                            sinavSonuclar[sinavSonuclar.IndexOf(genelSinavSonuc)].GenelSira = sira;
                        }

                        foreach (var subeId in subeler)
                        {
                            var subeSinavSonuclar = sinavSonuclar.Where(x => x.PuanTurId == puanTur && x.SubeId == subeId).ToList();
                            var subePuanlar = subeSinavSonuclar.Select(x => x.ToplamPuan).Distinct().OrderByDescending(x => x).ToList();

                            foreach (var subeSinavSonuc in subeSinavSonuclar)
                            {
                                var sira = subePuanlar.IndexOf(subeSinavSonuc.ToplamPuan) + 1;
                                sinavSonuclar[sinavSonuclar.IndexOf(subeSinavSonuc)].SubeSira = sira;
                            }
                        }

                        foreach (var sinif in siniflar)
                        {
                            if (string.IsNullOrEmpty(sinif))
                                continue;

                            var sinifSinavSonuclar = sinavSonuclar.Where(x => x.PuanTurId == puanTur && x.Sinif == sinif).ToList();
                            var sinifPuanlar = sinifSinavSonuclar.Select(x => x.ToplamPuan).Distinct().OrderByDescending(x => x).ToList();

                            foreach (var sinifSinavSonuc in sinifSinavSonuclar)
                            {
                                var sira = sinifPuanlar.IndexOf(sinifSinavSonuc.ToplamPuan) + 1;
                                sinavSonuclar[sinavSonuclar.IndexOf(sinifSinavSonuc)].SinifSira = sira;
                            }
                        }
                    }

                    foreach (var sinavSonuc in sinavSonuclar)
                    {
                        sinavSonucService.UpdateDto(sinavSonuc);
                    }
                }
            }

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Detay", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinavService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = new SinavDetayViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sinav>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinavService>();
                var model = service.Get(x => x.SinavId == id,
                    y => y.Kurum,
                    y => y.SinavTur.SinavTurDersKatSayilar.Select(z => z.DersGrup),
                    y => y.SinavTur.SinavTurDersKatSayilar.Select(z => z.PuanTur),
                    y => y.SinavTur.SinavTurDersler.Select(z => z.Ders),
                    y => y.OptikForm,
                    y => y.SinavSubeler.Select(z => z.Sube),
                    y => y.SinavKitapciklar.Select(z => z.SinavKitapcikDersBilgiler.Select(t => t.Ders)),
                    y => y.SinavTur.SinavTurDersKatSayilar.Select(z => z.DersGrup.Dersler));

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
                viewModel.ListeleModel = new OgrenciSinavKontrolListeleViewModel
                {
                    SinavId = (int)id,
                    SubeSelectList = selectListHelper.SubeSelectList(),
                    SubeId = Identity.KurumId != -1 ? Identity.SubeId : 0,
                    SinavBaslik = model.Baslik
                };

                viewModel.Konular = new List<Tuple<Konu, int>>();

                if (model.SinavKitapciklar != null && model.SinavKitapciklar.Any())
                {
                    foreach (var sinavKitapcik in model.SinavKitapciklar)
                    {
                        if (sinavKitapcik.SinavKitapcikDersBilgiler != null && sinavKitapcik.SinavKitapcikDersBilgiler.Any())
                        {
                            foreach (var sinavKitapcikDersBilgi in sinavKitapcik.SinavKitapcikDersBilgiler)
                            {
                                if (!string.IsNullOrEmpty(sinavKitapcikDersBilgi.DersKonuBilgi))
                                {
                                    var konuService = serviceFactory.CreateService<IKonuService>();

                                    var konular = sinavKitapcikDersBilgi.DersKonuBilgi.Split(',');

                                    foreach (var konu in konular)
                                    {
                                        if (string.IsNullOrEmpty(konu))
                                            continue;

                                        var konuId = Convert.ToInt32(konu);

                                        var selectedKonu = viewModel.Konular.FirstOrDefault(x => x.Item1.KonuId == konuId);

                                        if (selectedKonu != null)
                                        {
                                            var index = viewModel.Konular.IndexOf(selectedKonu);
                                            viewModel.Konular[index] = new Tuple<Konu, int>(selectedKonu.Item1, selectedKonu.Item2 + 1);
                                        }
                                        else
                                        {
                                            var konuModel = konuService.Get(x =>
                                                x.KonuId == konuId,
                                                y => y.Ders.DersGrup);

                                            selectedKonu = new Tuple<Konu, int>(konuModel, 1);

                                            viewModel.Konular.Add(selectedKonu);
                                        }
                                    }
                                }
                            }

                            viewModel.Konular = viewModel.Konular.OrderBy(x => x.Item1.Ders.DersGrup.DersGrupAd).ThenBy(x => x.Item1.Ders.DersAd).ThenBy(x => x.Item1.Baslik).ToList();

                            break;
                        }
                    }
                }
            }
            else
            {
                return Redirect("/Error/NotFound");
            }

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SinavListeleViewModel
            {
                Model = new Sinav()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(SinavListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<ISinavService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SinavlarListele(string subeIdler, string sezonIdler)
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = true,
                    Text = Resources.LangResources.Secebilirsiniz,
                    Value = "",
                }
            };

            if (!string.IsNullOrEmpty(subeIdler) && !string.IsNullOrEmpty(sezonIdler))
            {
                var service = serviceFactory.CreateService<ISinavService>();

                var parameters = new List<Parameter>
                {
                    new Parameter("SubeIdler", subeIdler),
                    new Parameter("SezonIdler", sezonIdler)
                };

                var sinavlar = service.SinavListele(parameters).ToList();



                for (int i = 0; i < sinavlar.Count; i++)
                {
                    selectList.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = sinavlar[i].Baslik,
                        Value = sinavlar[i].SinavId.ToString(),
                    });
                }
            }

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult OgrenciSinavListele(OgrenciSinavKontrolListeleViewModel viewModel)
        {
            var subeId = Request.QueryString["ListeleModel_SubeId"] == null && Identity.KurumId != -1
                ? Identity.SubeId.ToString()
                : (Request.QueryString["ListeleModel_SubeId"] ?? "0");

            var paremeters = new List<Parameter>
            {
                new Parameter("SinavId",RouteData.Values["id"]),
                new Parameter("SubeId",subeId),
                new Parameter("PersonelSubeId",Identity.SubeId)
            };

            var service = serviceFactory.CreateService<IOgrenciSinavKontrolService>();

            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }
    }
}