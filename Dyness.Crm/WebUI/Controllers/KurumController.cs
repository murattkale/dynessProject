using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class KurumController : Controller
    {
        private IServiceFactory serviceFactory;

        public KurumController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new KurumDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Kurum>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IKurumService>();
                var model = service.Get(x => x.KurumId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Kurum();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(KurumDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IKurumService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.KurumId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        var ayarlar = AyarlarService.Get();

                        if (viewModel.OperationResult.Status && viewModel.PostedFileLogoDosyaAd != null)
                        {
                            var oldLogoDosyaAd = viewModel.Model.LogoDosyaAd;

                            viewModel.Model.LogoDosyaAd = FileHelper.KurumLogoImageName(
                                viewModel.PostedFileLogoDosyaAd,
                                viewModel.Model.KurumAd);

                            var logoSaveMessageInfo = FileHelper.ImageSave(
                                viewModel.PostedFileLogoDosyaAd,
                                ayarlar.KurumLogoYol,
                                viewModel.Model.LogoYol,
                                viewModel.Model.GetDisplayName(x => x.LogoDosyaAd),
                                ayarlar.KurumLogoSize);

                            viewModel.OperationResult.MessageInfos.Add(logoSaveMessageInfo);

                            if (logoSaveMessageInfo.MessageInfoType == MessageInfoType.Success)
                            {
                                var updateResult = service.Update(viewModel.Model);

                                if (updateResult.Status)
                                {
                                    FileHelper.IfFileExistsDeleteFile(oldLogoDosyaAd);
                                }
                            }
                            else if (!string.Equals(viewModel.Model.LogoDosyaAd, oldLogoDosyaAd))
                            {
                                viewModel.Model.LogoDosyaAd = oldLogoDosyaAd;
                                service.Update(viewModel.Model);
                            }
                        }

                        if (viewModel.OperationResult.Status && viewModel.PostedFileArkaPlanDosyaAd != null)
                        {
                            var oldArkaPlanDosyaAd = viewModel.Model.ArkaPlanDosyaAd;

                            viewModel.Model.ArkaPlanDosyaAd = FileHelper.KurumArkaPlanImageName(
                                viewModel.PostedFileArkaPlanDosyaAd,
                                viewModel.Model.KurumAd);

                            var arkaPlanSaveMessageInfo = FileHelper.ImageSave(
                                viewModel.PostedFileArkaPlanDosyaAd,
                                ayarlar.KurumArkaPlanYol,
                                viewModel.Model.ArkaPlanYol,
                                viewModel.Model.GetDisplayName(x => x.ArkaPlanDosyaAd),
                                ayarlar.KurumArkaPlanSize);

                            viewModel.OperationResult.MessageInfos.Add(arkaPlanSaveMessageInfo);

                            if (arkaPlanSaveMessageInfo.MessageInfoType == MessageInfoType.Success)
                            {
                                var updateResult = service.Update(viewModel.Model);

                                if (updateResult.Status)
                                {
                                    FileHelper.IfFileExistsDeleteFile(oldArkaPlanDosyaAd);
                                }
                            }
                            else if (!string.Equals(viewModel.Model.ArkaPlanDosyaAd, oldArkaPlanDosyaAd))
                            {
                                viewModel.Model.ArkaPlanDosyaAd = oldArkaPlanDosyaAd;
                                service.Update(viewModel.Model);
                            }
                        }

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.KurumId > 0
                          ? RedirectToAction("Duzenle", new { id = viewModel.Model.KurumId })
                          : RedirectToAction("Duzenle");
                    }
                case "LogoSil":
                    {
                        FileHelper.IfFileExistsDeleteFile(viewModel.Model.LogoYol);

                        viewModel.Model.LogoDosyaAd = string.Empty;

                        viewModel.OperationResult = service.Update(viewModel.Model);

                        break;
                    }
                case "ArkaPlanSil":
                    {
                        FileHelper.IfFileExistsDeleteFile(viewModel.Model.ArkaPlanYol);

                        viewModel.Model.ArkaPlanDosyaAd = string.Empty;

                        viewModel.OperationResult = service.Update(viewModel.Model);

                        break;
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

            var service = serviceFactory.CreateService<IKurumService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new KurumListeleViewModel
            {
                Model = new Kurum()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(KurumListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IKurumService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonConvert.SerializeObject(entityPagedDataSource, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }
    }
}