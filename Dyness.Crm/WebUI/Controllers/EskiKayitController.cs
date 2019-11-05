using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Helpers;
using Core.Entities.Dto;
using System.Collections.Generic;
using System.Data;
using System;

namespace WebUI.Controllers
{
    public class EskiKayitController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public EskiKayitController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new EskiKayitDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<EskiKayit>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IEskiKayitService>();
                var model = service.Get(x => x.EskiKayitId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new EskiKayit();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(EskiKayitDuzenleViewModel viewModel)
        {
            ModelState.Clear();
            var service = serviceFactory.CreateService<IEskiKayitService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.EskiKayitId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.EskiKayitId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.EskiKayitId })
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

            var service = serviceFactory.CreateService<IEskiKayitService>();

            var operationResult = service.DeleteById(id);
            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult EskiKayitVeriGuncelle()
        {
            var viewModel = new EskiKayitVeriGuncelleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<EskiKayit>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult EskiKayitVeriGuncelle(EskiKayitVeriGuncelleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IEskiKayitService>();
            var subeService = serviceFactory.CreateService<ISubeService>();

            var sube = subeService.Get(x => x.SubeId == viewModel.SubeId);

            if (sube == null)
            {
                TempData["OperationResult"] = new EntityOperationResult<EskiKayit>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = Resources.LangResources.SubeBulunamadi, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };
            }

            var returnVal = ExcelHelper.ExcelToDataSet(viewModel.PostedFileVeri, Server.MapPath("~/VeriGuncelle/EskiKayit/"), $"sube{sube.SubeId}");

            if (!string.IsNullOrEmpty(returnVal.Item2))
            {
                TempData["OperationResult"] = new EntityOperationResult<OgrenciSozlesme>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = returnVal.Item2, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };
            }

            var dataTable = returnVal.Item1.Tables[0];

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                TempData["OperationResult"] = new EntityOperationResult<OgrenciSozlesme>
                {
                    MessageInfos = new List<MessageInfo> { new MessageInfo { Message = Resources.LangResources.VeriBulunamadi, MessageInfoType = MessageInfoType.Error } },
                    Status = false
                };
            }

            var errorDataTable = dataTable.Clone();
            errorDataTable.Clear();

            errorDataTable.Columns.Add(new DataColumn(Resources.LangResources.Hata, typeof(string)));

            var messages = new List<MessageInfo>();

            for (int i = 3; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var hataMesaji = string.Empty;

                try
                {
                    #region Excel

                    var sozlesmeDurumu = row[0].ToString().Trim();
                    var ogrenciTckn = row[1].ToString().Trim();
                    var ogrenciId = row[2].ToString().Trim();
                    var ogrenciNo = row[3].ToString().Trim();
                    var ogrenciAdi = row[4].ToString().Trim();
                    var ogrenciSoyadi = row[5].ToString().Trim();
                    var ogrenciTelefon = row[6].ToString().Trim();
                    var ogrenciAdres = row[7].ToString().Trim();
                    var ogrenciEposta = row[8].ToString().Trim();
                    var servisBilgisi = row[9].ToString().Trim();
                    var nakit = row[10].ToString().Trim();
                    var cek = row[11].ToString().Trim();
                    var krediKartiPosCihazi = row[12].ToString().Trim();
                    var havale = row[13].ToString().Trim();
                    var krediKartiSanalPos = row[14].ToString().Trim();
                    var mailOrder = row[15].ToString().Trim();
                    var kayitUcreti = row[16].ToString().Trim();
                    var kalanOdeme = row[17].ToString().Trim();
                    var odemeTuru = row[18].ToString().Trim();
                    var kayitTarihi = row[19].ToString().Trim();
                    var biziNeredenDuydunuz = row[20].ToString().Trim();
                    var sinifSeviyesi = row[21].ToString().Trim();
                    var sinif = row[22].ToString().Trim();
                    var brans = row[23].ToString().Trim();
                    var sezon = row[24].ToString().Trim();
                    var subeBilgi = row[25].ToString().Trim();
                    var adres = row[26].ToString().Trim();
                    var ilce = row[27].ToString().Trim();
                    var il = row[28].ToString().Trim();
                    var ulke = row[29].ToString().Trim();
                    var veliAnne = row[30].ToString().Trim();
                    var anneTckn = row[31].ToString().Trim();
                    var anneTel = row[32].ToString().Trim();
                    var veliBaba = row[33].ToString().Trim();
                    var babaTckn = row[34].ToString().Trim();
                    var babaTel = row[35].ToString().Trim();
                    var veliDiger = row[36].ToString().Trim();
                    var digerTckn = row[37].ToString().Trim();
                    var digerTel = row[38].ToString().Trim();
                    var faturaAdSoyad = row[39].ToString().Trim();
                    var vergiDairesi = row[40].ToString().Trim();
                    var vergiTckNo = row[41].ToString().Trim();
                    var faturaAdres = row[42].ToString().Trim();
                    var faturaSemt = row[43].ToString().Trim();
                    var faturaIlce = row[44].ToString().Trim();
                    var faturaSehir = row[45].ToString().Trim();
                    var faturaPostaKodu = row[46].ToString().Trim();
                    var gorusen = row[47].ToString().Trim();
                    var kaydiYapan = row[48].ToString().Trim();
                    var referans = row[49].ToString().Trim();

                    #endregion

                    var model = new EskiKayit
                    {
                        SubeId = sube.SubeId,
                        SozlesmeDurumu = sozlesmeDurumu,
                        OgrenciTckn = ogrenciTckn,
                        OgrenciId = ogrenciId,
                        OgrenciNo = ogrenciNo,
                        OgrenciAdi = ogrenciAdi,
                        OgrenciSoyadi = ogrenciSoyadi,
                        OgrenciTelefon = ogrenciTelefon,
                        OgrenciAdres = ogrenciAdres,
                        OgrenciEposta = ogrenciEposta,
                        ServisBilgisi = servisBilgisi,
                        Nakit = nakit,
                        Cek = cek,
                        KrediKartiPosCihazi = krediKartiPosCihazi,
                        Havale = havale,
                        KrediKartiSanalPos = krediKartiSanalPos,
                        MailOrder = mailOrder,
                        KayitUcreti = kayitUcreti,
                        KalanOdeme = kalanOdeme,
                        OdemeTuru = odemeTuru,
                        KayitTarihi = kayitTarihi,
                        BiziNeredenDuydunuz = biziNeredenDuydunuz,
                        SinifSeviyesi = sinifSeviyesi,
                        Sinif = sinif,
                        Brans = brans,
                        Sezon = sezon,
                        SubeBilgi = subeBilgi,
                        Adres = adres,
                        Ilce = ilce,
                        Il = il,
                        Ulke = ulke,
                        VeliAnne = veliAnne,
                        AnneTckn = anneTckn,
                        AnneTel = anneTel,
                        VeliBaba = veliBaba,
                        BabaTckn = babaTckn,
                        BabaTel = babaTel,
                        VeliDiger = veliDiger,
                        DigerTckn = digerTckn,
                        DigerTel = digerTel,
                        FaturaAdSoyad = faturaAdSoyad,
                        VergiDairesi = vergiDairesi,
                        VergiTckNo = vergiTckNo,
                        FaturaAdres = faturaAdres,
                        FaturaSemt = faturaSemt,
                        FaturaIlce = faturaIlce,
                        FaturaSehir = faturaSehir,
                        FaturaPostaKodu = faturaPostaKodu,
                        Gorusen = gorusen,
                        KaydiYapan = kaydiYapan,
                        Referans = referans
                    };

                    var operationResult = service.Add(model);

                    if (!operationResult.Status)
                    {
                        for (int j = 0; j < operationResult.MessageInfos.Count; j++)
                        {
                            var messageInfo = operationResult.MessageInfos[j];

                            messages.Add(new MessageInfo { Message = $"{messageInfo.Message} - T.C. No : {model.OgrenciTckn}" , Field = messageInfo.Field , MessageInfoType = messageInfo.MessageInfoType } );
                        }
                    }
                }
                catch (Exception ex)
                {
                    messages.Add(new MessageInfo { Message = ex.Message, MessageInfoType = MessageInfoType.Error });
                }
            }

            viewModel = new EskiKayitVeriGuncelleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<EskiKayit>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if(messages.Count > 0)
            {
                if(viewModel.OperationResult?.MessageInfos == null)
                {
                    viewModel.OperationResult = new EntityOperationResult<EskiKayit>
                    {
                        MessageInfos = new List<MessageInfo>()
                    };
                }

                viewModel.OperationResult.MessageInfos.AddRange(messages);
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = new EskiKayit();

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IEskiKayitService>();
                viewModel = service.Get(x => x.EskiKayitId == id,
                    y => y.Sube);

                if (viewModel == null)
                    return Redirect("/Error/NotFound");
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
            var viewModel = new EskiKayitListeleViewModel
            {
                Model = new EskiKayit()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(EskiKayitListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IEskiKayitService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }
    }
}