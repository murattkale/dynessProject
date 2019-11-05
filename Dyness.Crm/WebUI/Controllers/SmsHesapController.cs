using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SmsHesapController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SmsHesapController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SmsHesapDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SmsHesap>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISmsHesapService>();
                var model = service.Get(x =>
                x.SmsHesapId == id,
                y => y.SmsHesapDosyalar,
                y => y.SmsHesapDurum);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new SmsHesap();
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.SmsHesapDurumSelectList = selectListHelper.SmsHesapDurumSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SmsHesapDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISmsHesapService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.Model.SmsHesapId > 0)
                        {
                            var model = service.Get(x =>
                            x.SmsHesapId == viewModel.Model.SmsHesapId,
                            y => y.SmsHesapDosyalar);

                            model.Baslik = viewModel.Model.Baslik;
                            model.SmsHesapDurumId = viewModel.Model.SmsHesapDurumId;
                            viewModel.Model = model;
                        }

                        if (viewModel.PostedFilesDosyalar != null && viewModel.PostedFilesDosyalar[0] != null && viewModel.Model != null)
                        {
                            if (viewModel.Model.SmsHesapDosyalar == null)
                                viewModel.Model.SmsHesapDosyalar = new List<SmsHesapDosya>();

                            for (int i = 0; i < viewModel.PostedFilesDosyalar.Length; i++)
                            {
                                var date = DateTime.Now.ToShortDateString().Split('.');
                                var time = DateTime.Now.ToString("HH:mm:ss:fff").Split(':');
                                var dosyaAd = $"{date[0]}{date[1]}{date[2]}{time[0]}{time[1]}{time[2]}{time[3]}";

                                var smsHesapDosya = new SmsHesapDosya
                                {
                                    YuklenmeTarihi = DateTime.Now,
                                    DosyaAd = $"{dosyaAd}{Path.GetExtension(viewModel.PostedFilesDosyalar[i].FileName)}"
                                };

                                if (viewModel.Model.SmsHesapId > 0)
                                {
                                    smsHesapDosya.SubeId = viewModel.Model.SubeId;
                                    smsHesapDosya.SmsHesapId = viewModel.Model.SmsHesapId;
                                }
                                else
                                    smsHesapDosya.SmsHesap = viewModel.Model;

                                var imageSaveResult = FileHelper.FileSave(
                                    viewModel.PostedFilesDosyalar[i],
                                    smsHesapDosya.DosyaYol.Replace(smsHesapDosya.DosyaAd, ""),
                                    smsHesapDosya.DosyaYol,
                                    Resources.LangResources.Dosya);

                                if (imageSaveResult.MessageInfoType == MessageInfoType.Error)
                                {
                                    if (viewModel.OperationResult?.MessageInfos == null)
                                    {
                                        viewModel.OperationResult = new EntityOperationResult<SmsHesap>();
                                    }

                                    viewModel.OperationResult.MessageInfos.Add(imageSaveResult);
                                }

                                viewModel.Model.SmsHesapDosyalar.Add(smsHesapDosya);
                            }
                        }

                        viewModel.OperationResult = viewModel.Model.SmsHesapId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        if (viewModel.OperationResult.Status)
                            viewModel.Model = service.Get(x =>
                            x.SmsHesapId == viewModel.Model.SmsHesapId,
                            y => y.SmsHesapDosyalar,
                            y => y.SmsHesapDurum);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SmsHesapId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SmsHesapId })
                            : RedirectToAction("Duzenle");
                    }
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.SmsHesapDurumSelectList = selectListHelper.SmsHesapDurumSelectList();

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult KrediSatinAl(int? id)
        {
            var viewModel = new KrediSatinAlViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SmsHesap>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.KrediBirimFiyat = AyarlarService.Get().SmsKrediTutar;
            viewModel.SmsHesapSelectList = selectListHelper.SmsHesapSelectList(1, true);

            if (id != null)
            {
                viewModel.SelectedSmsHesapId = (int)id;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult KrediSatinAl(KrediSatinAlViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISmsHesapHareketService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        // Ödeme burada olacak

                        var smsHesapHareket = new SmsHesapHareket
                        {
                            SmsHesapId = viewModel.SelectedSmsHesapId,
                            PersonelId = Identity.PersonelId,
                            Kredi = viewModel.KrediAdet,
                            HareketTarihi = DateTime.Now,
                            SmsHesapHareketTipId = 1
                        };

                        var operationResult = service.Add(smsHesapHareket);

                        TempData["OperationResult"] = new EntityOperationResult<SmsHesap>
                        {
                            MessageInfos = operationResult.MessageInfos,
                            Status = operationResult.Status
                        };

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SmsHesapId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SmsHesapId })
                            : RedirectToAction("Duzenle");
                    }
            }

            viewModel.SmsHesapSelectList = selectListHelper.SmsHesapSelectList();

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SmsHesapListeleViewModel
            {
                Model = new SmsHesap()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(SmsHesapListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<ISmsHesapService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }
    }
}