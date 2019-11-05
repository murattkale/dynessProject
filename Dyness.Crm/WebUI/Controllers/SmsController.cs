using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SmsController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SmsController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private List<SmsTelefonBilgiDto> GetSmsTelefonBilgiler(
            int GrupId,
            int? SinavId,
            string SubeIdler,
            string OgrenciGrupIdler,
            string SezonIdler,
            string SinifIdler,
            string PersonelGrupIdler)
        {
            var service = serviceFactory.CreateService<ISmsService>();

            List<SmsTelefonBilgiDto> smsTelefonBilgiler;

            if (GrupId == 1)
            {
                var paremeters = new List<Parameter>
                {
                    new Parameter("PersonelId", string.Join(",",Identity.PersonelId)),
                    new Parameter("SinavId", SinavId),
                    new Parameter("SubeId", SubeIdler),
                    new Parameter("SezonId",SezonIdler),
                    new Parameter("SinifId",SinifIdler)
                };

                smsTelefonBilgiler = service.SmsTelefonBilgiListele(0, paremeters).ToList();

                var ogrenciGrupIdList = OgrenciGrupIdler.Split(',');

                var secilenSmsTelefonBilgiler = new List<SmsTelefonBilgiDto>();

                for (int i = 0; i < ogrenciGrupIdList.Length; i++)
                {
                    var ogrenciGrupId = ogrenciGrupIdList[i];

                    if (string.IsNullOrEmpty(ogrenciGrupId))
                        continue;

                    if (ogrenciGrupId == "1")
                    {
                        for (int j = 0; j < smsTelefonBilgiler.Count; j++)
                        {
                            var ogrenciTelefon = smsTelefonBilgiler[j];

                            if (ogrenciTelefon.Bilgi == Bilgi.Kendisi &&
                                secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                            {
                                secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                            }
                        }
                    }
                    else if (ogrenciGrupId == "2")
                    {
                        var enYakiniTelefonlar = new List<SmsTelefonBilgiDto>();

                        var ogrenciIdler = smsTelefonBilgiler.Select(x => x.Id).Distinct().ToList();

                        for (int j = 0; j < ogrenciIdler.Count; j++)
                        {
                            var ogrenciTelefonlar = smsTelefonBilgiler.Where(x => x.Id == ogrenciIdler[j]).ToList();

                            for (int k = 0; k < ogrenciTelefonlar.Count; k++)
                            {
                                var ogrenciTelefon = ogrenciTelefonlar[k];

                                if (ogrenciTelefon.Bilgi == Bilgi.Anne &&
                                    secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                                {
                                    secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                                    break;
                                }
                                else if (ogrenciTelefon.Bilgi == Bilgi.Baba &&
                                        secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                                {
                                    secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                                    break;
                                }
                                else if (ogrenciTelefon.Bilgi == Bilgi.Yakini &&
                                        secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                                {
                                    secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                                    break;
                                }
                            }
                        }
                    }
                    else if (ogrenciGrupId == "3")
                    {
                        for (int j = 0; j < smsTelefonBilgiler.Count; j++)
                        {
                            var ogrenciTelefon = smsTelefonBilgiler[j];

                            if (ogrenciTelefon.Bilgi == Bilgi.Anne &&
                                secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                            {
                                secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                            }
                        }
                    }
                    else if (ogrenciGrupId == "4")
                    {
                        for (int j = 0; j < smsTelefonBilgiler.Count; j++)
                        {
                            var ogrenciTelefon = smsTelefonBilgiler[j];

                            if (ogrenciTelefon.Bilgi == Bilgi.Baba &&
                                secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                            {
                                secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                            }
                        }
                    }
                    else if (ogrenciGrupId == "5")
                    {
                        for (int j = 0; j < smsTelefonBilgiler.Count; j++)
                        {
                            var ogrenciTelefon = smsTelefonBilgiler[j];

                            if (ogrenciTelefon.Bilgi == Bilgi.Yakini &&
                                secilenSmsTelefonBilgiler.Count(x => x.Id == ogrenciTelefon.Id && x.Bilgi == ogrenciTelefon.Bilgi) == 0)
                            {
                                secilenSmsTelefonBilgiler.Add(ogrenciTelefon);
                            }
                        }
                    }
                }

                smsTelefonBilgiler = secilenSmsTelefonBilgiler;
            }
            else
            {
                var paremeters = new List<Parameter>
                {
                    new Parameter("PersonelId", Identity.PersonelId),
                    new Parameter("PersonelGrupId", PersonelGrupIdler),
                    new Parameter("SubeId", SubeIdler)
                };

                smsTelefonBilgiler = service.SmsTelefonBilgiListele(1, paremeters).ToList();
            }

            return smsTelefonBilgiler;
        }

        private void GetLists(SmsDuzenleViewModel viewModel)
        {
            viewModel.SmsHesapSelectList = selectListHelper.SmsHesapSelectList(1, true);
            viewModel.SmsSablonSelectList = selectListHelper.SmsMetinSablonSelectList();
            viewModel.GonderilenGrupSelectList = selectListHelper.SmsGonderenGrupSelectList();
            viewModel.OgrenciGonderilenGrupSelectList = selectListHelper.SmsOgrenciGonderenGrupSelectList();
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();

            viewModel.PersonelGrupSelectList = selectListHelper.PersonelGrupSelectList();

            if (viewModel.SelectedSezonlar != null && viewModel.SelectedSezonlar.Length > 0)
                viewModel.SezonSelectList = selectListHelper.SezonSelectList();
            else
                viewModel.SezonSelectList = new List<SelectListItem>();

            if (viewModel.SelectedSinavId > 0)
                viewModel.SinavSelectList = selectListHelper.SinavSelectList();
            else
                viewModel.SinavSelectList = new List<SelectListItem>();

            if (viewModel.SelectedSiniflar != null && viewModel.SelectedSiniflar.Length > 0)
                viewModel.SinifSelectList = selectListHelper.SinifSelectList();
            else
                viewModel.SinifSelectList = new List<SelectListItem>();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sms>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle()
        {
            var viewModel = new SmsDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sms>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SmsDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            viewModel.OperationResult = new EntityOperationResult<Sms>
            {
                MessageInfos = new List<MessageInfo>(),
                Status = true
            };

            var service = serviceFactory.CreateService<ISmsService>();
            var smsHesapService = serviceFactory.CreateService<ISmsHesapService>();
            var smsHesapModel = smsHesapService.Get(x => x.SmsHesapId == viewModel.SelectedSmsHesapId);

            if (smsHesapModel == null)
            {
                GetLists(viewModel);

                viewModel.OperationResult =  new EntityOperationResult<Sms>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() {
                            new MessageInfo {
                                Message = Resources.LangResources.SmsHesapBulunamadi,
                                MessageInfoType = MessageInfoType.Error } }
                };

                return View(viewModel);
            }

            var smsDurumService = serviceFactory.CreateService<ISmsDurumService>();
            var smsDurumlar = smsDurumService.List().ToList();

            if (!string.IsNullOrEmpty(viewModel.TelefonNumaralar))
            {
                var telefonNolar = viewModel.TelefonNumaralar.Split(',');

                for (int i = 0; i < telefonNolar.Length; i++)
                {
                    var telefonNo = telefonNolar[i];

                    if (string.IsNullOrEmpty(telefonNo))
                        continue;

                    var sms = new Sms
                    {
                        AdSoyad = "",
                        SmsDurumId = -1,
                        SmsHesapId = viewModel.SelectedSmsHesapId,
                        TelefonNo = telefonNo,
                        Mesaj = viewModel.Model.Mesaj,
                        GonderilecegiTarih = viewModel.Model.GonderilecegiTarih,
                        SmsMetinSablonId = viewModel.SelectedSmsSablonId
                    };

                    var smsGonderildi = SmsHelper.SmsGonder(smsHesapModel.Baslik, sms.TelefonNo, sms.Mesaj);

                    if (smsGonderildi.Item1)
                    {
                        var smsDurum = smsDurumlar.FirstOrDefault(x => x.Kod == smsGonderildi.Item2);

                        if (smsDurum != null)
                        {
                            sms.SmsDurumId = smsDurum.SmsDurumId;
                            sms.DlrId = smsGonderildi.Item3;
                        }
                        
                        var smsHesapHareket = new SmsHesapHareket
                        {
                            SmsHesapId = sms.SmsHesapId,
                            PersonelId = Identity.PersonelId,
                            Kredi = -sms.MesajOlcum,
                            Telefon = sms.TelefonNo,
                            HareketTarihi = DateTime.Now,
                            SmsHesapHareketTipId = 8
                        };

                        sms.SmsHesapHareket = smsHesapHareket;

                        var operationResult = service.Add(sms);

                        if (!operationResult.Status)
                            viewModel.OperationResult.Status = false;

                        viewModel.OperationResult.MessageInfos.AddRange(operationResult.MessageInfos);
                    }
                    else
                    {
                        viewModel.OperationResult.Status = false;

                        var smsDurum = smsDurumlar.FirstOrDefault(x => x.Kod == smsGonderildi.Item2);

                        if(smsDurum != null)
                        {
                            viewModel.OperationResult.MessageInfos.Add(new MessageInfo
                            {
                                MessageInfoType = MessageInfoType.Error,
                                Message = smsDurum.Aciklama
                            });
                        }
                    }
                }

                viewModel.OperationResult.MessageInfos = viewModel.OperationResult.MessageInfos.Distinct().ToList();

                TempData["OperationResult"] = viewModel.OperationResult;

                return Redirect("/Sms/Duzenle");
            }

            var smsTelefonBilgiler = viewModel.SmsTelefonBilgiler;

            if (smsTelefonBilgiler != null && smsTelefonBilgiler.Any(x => x.Checked))
            {
                var smsler = new List<Sms>();

                var smsMetinSablonService = serviceFactory.CreateService<ISmsMetinSablonService>();
                var smsMetinSablonModel = smsMetinSablonService.Get(x => x.SmsMetinSablonId == viewModel.SelectedSmsSablonId);

                var filtrelenenSmsTelefonBilgiler = GetSmsTelefonBilgiler(
                    (int)viewModel.SelectedGonderilenGrup,
                    viewModel.SelectedSinavId,
                    viewModel.SelectedSubeler != null
                        ? string.Join(",", viewModel.SelectedSubeler)
                        : "",
                     viewModel.SelectedOgrenciGonderilenGrup != null
                        ? string.Join(",", viewModel.SelectedOgrenciGonderilenGrup)
                        : "",
                     viewModel.SelectedSezonlar != null
                        ? string.Join(",", viewModel.SelectedSezonlar)
                        : "",
                    viewModel.SelectedSiniflar != null
                        ? string.Join(",", viewModel.SelectedSiniflar)
                        : "",
                    viewModel.SelectedPersonelGruplar != null
                        ? string.Join(",", viewModel.SelectedPersonelGruplar)
                        : "");

                foreach (var smsTelefonBilgi in filtrelenenSmsTelefonBilgiler)
                {
                    var kontrolSmsTelefonBilgi = smsTelefonBilgiler.FirstOrDefault(x => x.Id == smsTelefonBilgi.Id && x.Tip == smsTelefonBilgi.Tip && x.Bilgi == smsTelefonBilgi.Bilgi);
                    smsTelefonBilgi.Checked = kontrolSmsTelefonBilgi != null && kontrolSmsTelefonBilgi.Checked;
                }

                var sinavTarihi = "";
                var sinavAdi = "";

                if (viewModel.SelectedSinavId > 0)
                {
                    var sinavService = serviceFactory.CreateService<ISinavService>();
                    var sinavModel = sinavService.Get(x => x.SinavId == viewModel.SelectedSinavId);

                    sinavTarihi = sinavModel.SinavTarihiFormatted;
                    sinavAdi = sinavModel.Baslik;
                }

                var ogrenciSinavKontrolService = serviceFactory.CreateService<IOgrenciSinavKontrolService>();

                foreach (var smsTelefonBilgi in filtrelenenSmsTelefonBilgiler)
                {
                    if (!smsTelefonBilgi.Checked)
                        continue;

                    var mesaj = "";

                    if (viewModel.SelectedSmsSablonId != null)
                    {
                        if (smsMetinSablonModel == null)
                            break;

                        mesaj = smsMetinSablonModel.Sablon;

                        if (smsTelefonBilgi.Tip == Tip.Ogrenci)
                        {
                            var ogrenciService = serviceFactory.CreateService<IOgrenciService>();
                            var ogrenciModel = ogrenciService.Get(x =>
                                x.OgrenciId == smsTelefonBilgi.Id,
                                y => y.AnneOgrenciYakiniIletisim,
                                y => y.BabaOgrenciYakiniIletisim,
                                y => y.YakiniOgrenciYakiniIletisim);

                            if (ogrenciModel == null)
                                break;

                            var replacedVeliAdSoyad = "";

                            switch (smsTelefonBilgi.Bilgi)
                            {
                                case Bilgi.Kendisi:
                                    {

                                        if (ogrenciModel.AnneOgrenciYakiniIletisim != null)
                                            replacedVeliAdSoyad = ogrenciModel.AnneOgrenciYakiniIletisim.AdSoyad;
                                        else if (ogrenciModel.BabaOgrenciYakiniIletisim != null)
                                            replacedVeliAdSoyad = ogrenciModel.BabaOgrenciYakiniIletisim.AdSoyad;
                                        else if (ogrenciModel.YakiniOgrenciYakiniIletisim != null)
                                            replacedVeliAdSoyad = ogrenciModel.YakiniOgrenciYakiniIletisim.AdSoyad;

                                        break;
                                    }
                                case Bilgi.Anne:
                                    {
                                        replacedVeliAdSoyad = ogrenciModel.AnneOgrenciYakiniIletisim.AdSoyad;
                                        break;
                                    }
                                case Bilgi.Baba:
                                    {
                                        replacedVeliAdSoyad = ogrenciModel.BabaOgrenciYakiniIletisim.AdSoyad;
                                        break;
                                    }
                                case Bilgi.Yakini:
                                    {
                                        replacedVeliAdSoyad = ogrenciModel.YakiniOgrenciYakiniIletisim.AdSoyad;
                                        break;
                                    }
                            }

                            mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSROgrenciAdSoyad }}}", ogrenciModel.AdSoyad);
                            mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSRVeliAdSoyad }}}", replacedVeliAdSoyad);

                            mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSRSinavAd }}}", sinavAdi);
                            mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSRSinavTarihi }}}", sinavTarihi);

                            var ogrenciSinavKontrolModel = ogrenciSinavKontrolService.Get(x =>
                                x.SinavKitapcik.SinavId == viewModel.SelectedSinavId &&
                                x.OgrenciId == ogrenciModel.OgrenciId,
                                y => y.OgrenciSinavKontrolPuanTurPuanlar.Select(z => z.PuanTur));

                            if (ogrenciSinavKontrolModel != null)
                            {
                                mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSROgrenciNet }}}", ogrenciSinavKontrolModel.NetFormatted);

                                var puanTurler = "";

                                if (ogrenciSinavKontrolModel.OgrenciSinavKontrolPuanTurPuanlar != null && ogrenciSinavKontrolModel.OgrenciSinavKontrolPuanTurPuanlar.Any())
                                {
                                    foreach (var ogrenciSinavKontrolPuanTurPuan in ogrenciSinavKontrolModel.OgrenciSinavKontrolPuanTurPuanlar)
                                    {
                                        puanTurler = $"{puanTurler} {ogrenciSinavKontrolPuanTurPuan.PuanTur.PuanTurAd} : {ogrenciSinavKontrolPuanTurPuan.Puan}, ";
                                    }

                                    puanTurler = puanTurler.Substring(0, puanTurler.Length - 1);

                                    mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSROgrenciPuanlar }}}", puanTurler);
                                }
                            }
                        }
                        else
                        {
                            var personelService = serviceFactory.CreateService<IPersonelService>();
                            var personelModel = personelService.Get(x => x.PersonelId == smsTelefonBilgi.Id);

                            if (personelModel == null)
                                break;

                            mesaj = mesaj.Replace($"{{{Resources.LangResources.SMSRPersonelAdSoyad }}}", personelModel.AdSoyad);
                        }
                    }
                    else
                    {
                        mesaj = viewModel.Model.Mesaj;
                    }

                    var sms = new Sms
                    {
                        AdSoyad = smsTelefonBilgi.AdSoyad,
                        SmsDurumId = -1,
                        SmsHesapId = viewModel.SelectedSmsHesapId,
                        TelefonNo = smsTelefonBilgi.Telefon,
                        Mesaj = mesaj,
                        GonderilecegiTarih = viewModel.Model.GonderilecegiTarih,
                        SmsMetinSablonId = viewModel.SelectedSmsSablonId,
                        Tip = smsTelefonBilgi.Tip,
                        Bilgi = smsTelefonBilgi.Bilgi
                    };

                    if (smsTelefonBilgi.Tip == Tip.Ogrenci)
                        sms.OgrenciId = smsTelefonBilgi.Id;

                    if (smsTelefonBilgi.Tip == Tip.Personel)
                        sms.PersonelId = smsTelefonBilgi.Id;

                    smsler.Add(sms);
                }

                var toplamKredi = smsler.Sum(x => x.MesajOlcum);

                if (smsHesapModel.Kredi < toplamKredi)
                {
                    TempData["OperationResult"] = new EntityOperationResult<Sms>
                    {
                        Status = false,
                        MessageInfos = new List<MessageInfo>() {
                            new MessageInfo {
                                Message = $"{Resources.LangResources.SmsKrediYeterliDegil} {toplamKredi - smsHesapModel.Kredi}",
                                MessageInfoType = MessageInfoType.Error } },

                    };

                    GetLists(viewModel);

                    return View(viewModel);
                }

                for (int i = 0; i < smsler.Count; i++)
                {
                    var sms = smsler[i];

                    var smsGonderildi = SmsHelper.SmsGonder(smsHesapModel.Baslik, sms.TelefonNo, sms.Mesaj);

                    if (smsGonderildi.Item1)
                    {
                        var smsDurum = smsDurumlar.FirstOrDefault(x => x.Kod == smsGonderildi.Item2);

                        if (smsDurum != null)
                        {
                            sms.SmsDurumId = smsDurum.SmsDurumId;
                            sms.DlrId = smsGonderildi.Item3;
                        }

                        var smsHareketTip = sms.Tip == Tip.Personel
                            ? 6
                            : sms.Tip == Tip.Ogrenci &&
                              sms.Bilgi == Bilgi.Kendisi
                                ? 2
                                : sms.Tip == Tip.Ogrenci &&
                                  sms.Bilgi == Bilgi.Anne
                                    ? 3
                                    : sms.Tip == Tip.Ogrenci &&
                                      sms.Bilgi == Bilgi.Baba
                                        ? 4
                                        : sms.Tip == Tip.Ogrenci &&
                                          sms.Bilgi == Bilgi.Yakini
                                            ? 5
                                            : 0;

                        var smsHesapHareket = new SmsHesapHareket
                        {
                            SmsHesapId = sms.SmsHesapId,
                            PersonelId = Identity.PersonelId,
                            Kredi = -sms.KrediAdet,
                            Telefon = sms.TelefonNo,
                            HareketTarihi = DateTime.Now,
                            SmsHesapHareketTipId = smsHareketTip
                        };

                        sms.SmsHesapHareket = smsHesapHareket;

                        var operationResult = service.Add(sms);

                        if (!operationResult.Status)
                            viewModel.OperationResult.Status = false;

                        viewModel.OperationResult.MessageInfos.AddRange(operationResult.MessageInfos);
                    }
                    else
                    {
                        viewModel.OperationResult.Status = false;
                    }
                }
            }

            viewModel.OperationResult.MessageInfos = viewModel.OperationResult.MessageInfos.Distinct().ToList();

            TempData["OperationResult"] = viewModel.OperationResult;

            if (viewModel.OperationResult.Status)
                return Redirect("/Sms/Duzenle");

            GetLists(viewModel);

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ActionResult Filtrele(
            int GrupId,
            int? SinavId,
            string SubeIdler,
            string OgrenciGrupIdler,
            string SezonIdler,
            string SinifIdler,
            string PersonelGrupIdler)
        {
            var smsTelefonBilgiler = GetSmsTelefonBilgiler(
                GrupId,
                SinavId,
                SubeIdler,
                OgrenciGrupIdler,
                SezonIdler,
                SinifIdler,
                PersonelGrupIdler);

            var viewModel = new SmsDuzenleViewModel
            {
                SmsTelefonBilgiler = smsTelefonBilgiler
            };

            return PartialView("~/Views/Shared/_SmsTelefonBilgiListele.cshtml", viewModel);
        }
    }
}