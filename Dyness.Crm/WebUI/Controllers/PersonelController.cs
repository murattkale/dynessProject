using Core.Entities.Dto;
using Core.General;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PersonelController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public PersonelController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private Personel GetModel(int id)
        {
            var service = serviceFactory.CreateService<IPersonelService>();
            var model = service.Get(x => x.PersonelId == id,
                 y => y.Kullanici,
                 y => y.Hesap,
                 y => y.Sube.ParaBirim,
                 y => y.PersonelGrup,
                 y => y.PersonelYetkiGrup,
                 y => y.PersonelSubeDersler.Select(z => z.Sube),
                 y => y.PersonelSubeUcretler.Select(z => z.Sube),
                 y => y.PersonelSubeYetkiler.Select(z => z.Sube));

            return model;
        }

        private void GetLists(PersonelDuzenleViewModel viewModel)
        {
            viewModel.PersonelGrupSelectList = selectListHelper.PersonelGrupSelectList();
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.DersSelectList = selectListHelper.DersSelectList();
            viewModel.UlkeSelectList = selectListHelper.UlkeSelectList();
            viewModel.PersonelYetkiGrupSelectList = selectListHelper.PersonelYetkiGrupSelectList();

            if (viewModel.Model == null)
                return;

            #region Ders Şube

            int[] selectedDersSubeItems = viewModel.Model?.PersonelSubeDersler != null && viewModel.Model.PersonelSubeDersler.Count() > 0
                ? viewModel.Model.PersonelSubeDersler.Select(x => x.SubeId).ToArray()
                : null;

            viewModel.DersSubeSelectList = selectListHelper.SubeSelectList(true, selectedDersSubeItems);

            #endregion

            #region Yetki Şube

            int[] selectedYetkiSubeItems = viewModel.Model?.PersonelSubeYetkiler != null && viewModel.Model.PersonelSubeYetkiler.Count() > 0
                ? viewModel.Model.PersonelSubeYetkiler.Select(x => x.SubeId).ToArray()
                : null;

            viewModel.YetkiSubeSelectList = selectListHelper.SubeSelectList(true, selectedYetkiSubeItems);

            #endregion

            #region Ders Şube Ücret

            int[] selectedDersSubeUcretItems = viewModel.Model?.PersonelSubeUcretler != null &&
                    viewModel.Model.PersonelSubeUcretler.Count() > 0
                        ? viewModel.Model.PersonelSubeUcretler.Select(x => x.SubeId).ToArray()
                        : null;

            var subeService = serviceFactory.CreateService<ISubeService>();
            var subeler = subeService.List(x => x.EtkinMi).OrderBy(x => x.SubeAd);

            if (viewModel.Model?.PersonelSubeUcretler == null || viewModel.Model.PersonelSubeUcretler.Count < subeler.Count())
            {
                var personelSubeUcretler = new List<PersonelSubeUcret>();

                foreach (var sube in subeler)
                {
                    var exists = viewModel.Model.PersonelSubeUcretler?.FirstOrDefault(x => x.SubeId == sube.SubeId);

                    if (exists == null)
                    {
                        personelSubeUcretler.Add(new PersonelSubeUcret
                        {
                            SubeId = sube.SubeId,
                            Sube = sube
                        });
                    }
                    else
                    {
                        personelSubeUcretler.Add(exists);
                    }
                }

                viewModel.Model.PersonelSubeUcretler = personelSubeUcretler;
            }

            viewModel.DersSubeUcretSelectList = selectListHelper.SubeSelectList(true, selectedDersSubeUcretItems);

            #endregion
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new PersonelDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Personel>)TempData["OperationResult"];
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
                viewModel.Model = new Personel();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(PersonelDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPersonelService>();

            if (viewModel.Model.PersonelId != 0)
            {
                var model = service.Get(x => x.PersonelId == viewModel.Model.PersonelId, y => y.Kullanici);

                if (viewModel.Model.Kullanici != null && string.IsNullOrEmpty(viewModel.Model.Kullanici.Sifre) && model.Kullanici != null)
                {
                    viewModel.Model.Kullanici.Sifre = model.Kullanici.Sifre;
                    viewModel.Model.Kullanici.SifreTekrar = model.Kullanici.Sifre;
                }
            }

            switch (viewModel.Command)
            {
                case "Duzenle":
                case "DuzenlePersonel":
                default:
                    {
                        var ayarlar = AyarlarService.Get();

                        if (viewModel.PostedFileGorselDosyaAd != null)
                        {
                            var oldGorselDosyaAd = viewModel.Model.GorselDosyaAd;
                            var oldGorselYol = viewModel.Model.GorselYol;

                            viewModel.Model.GorselDosyaAd = FileHelper.PersonelGorselImageName(
                                viewModel.PostedFileGorselDosyaAd,
                                viewModel.Model.AdSoyad);

                            var logoSaveMessageInfo = FileHelper.ImageSave(
                                viewModel.PostedFileGorselDosyaAd,
                                ayarlar.PersonelGorselYol,
                                viewModel.Model.GorselYol,
                                viewModel.Model.GetDisplayName(x => x.GorselDosyaAd),
                                ayarlar.PersonelGorselSize);

                            if (viewModel.OperationResult?.MessageInfos == null)
                            {
                                viewModel.OperationResult = new EntityOperationResult<Personel>
                                {
                                    MessageInfos = new List<MessageInfo>()
                                };
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

                        if (string.Equals(viewModel.Command, "DuzenlePersonel"))
                        {
                            viewModel.Model.Kullanici = null;
                        }

                        if (viewModel.Model?.Kullanici != null && viewModel.Model?.Kullanici.PersonelId == 0)
                        {
                            viewModel.Model.Kullanici.PersonelId = viewModel.Model.PersonelId;
                        }

                        if (viewModel.Model.PersonelId == 0)
                        {
                            if (viewModel.Model.PersonelSubeUcretler != null && viewModel.Model.PersonelSubeUcretler.Count > 0)
                            {
                                viewModel.Model.PersonelSubeUcretler = viewModel.Model.PersonelSubeUcretler.Where(x => x.Ucret != null && x.Ucret > 0).ToList();
                            }

                            if (viewModel.SelectedDersVerdigiSubeler != null && viewModel.SelectedDersVerdigiSubeler.Length > 0)
                            {
                                viewModel.Model.PersonelSubeDersler = new List<PersonelSubeDers>();

                                foreach (var sube in viewModel.SelectedDersVerdigiSubeler)
                                {
                                    viewModel.Model.PersonelSubeDersler.Add(new PersonelSubeDers
                                    {
                                        Personel = viewModel.Model,
                                        SubeId = sube
                                    });
                                }
                            }

                            if (!string.IsNullOrEmpty(viewModel.Model.Kullanici?.KullaniciAd))
                            {
                                if (viewModel.SelectedYetkiliOlduguSubeler != null && viewModel.SelectedYetkiliOlduguSubeler.Length > 0)
                                {
                                    viewModel.Model.PersonelSubeYetkiler = new List<PersonelSubeYetki>();

                                    foreach (var sube in viewModel.SelectedYetkiliOlduguSubeler)
                                    {
                                        viewModel.Model.PersonelSubeYetkiler.Add(new PersonelSubeYetki
                                        {
                                            Personel = viewModel.Model,
                                            SubeId = sube
                                        });
                                    }
                                }
                            }
                            else
                            {
                                viewModel.Model.Kullanici = null;
                            }

                            viewModel.OperationResult = service.Add(viewModel.Model);

                            if (viewModel.Model.PersonelId > 0)
                            {
                                viewModel.Model = GetModel(viewModel.Model.PersonelId);
                            }
                        }
                        else
                        {
                            #region Personel Şube Ücretler

                            if (viewModel.Model.PersonelSubeUcretler != null && viewModel.Model.PersonelSubeUcretler.Count > 0)
                            {
                                foreach (var personelSubeUcret in viewModel.Model.PersonelSubeUcretler)
                                {
                                    if (viewModel.SelectedUcretAldigiSubeler != null &&
                                        viewModel.SelectedUcretAldigiSubeler.Length > 0 &&
                                        viewModel.SelectedUcretAldigiSubeler.Count(x => x == personelSubeUcret.SubeId) > 0 &&
                                        personelSubeUcret.PersonelSubeUcretId > 0)
                                        continue;

                                    personelSubeUcret.Silinecek = personelSubeUcret.PersonelSubeUcretId > 0;
                                }
                            }

                            #endregion

                            #region Personel Şube Dersler

                            if (viewModel.SelectedDersVerdigiSubeler != null && viewModel.SelectedDersVerdigiSubeler.Length > 0)
                            {
                                viewModel.Model.PersonelSubeDersler = new List<PersonelSubeDers>();

                                foreach (var sube in viewModel.SelectedDersVerdigiSubeler)
                                {
                                    viewModel.Model.PersonelSubeDersler.Add(new PersonelSubeDers
                                    {
                                        PersonelId = viewModel.Model.PersonelId,
                                        SubeId = sube
                                    });
                                }
                            }

                            #endregion

                            #region Şube Yetkiler

                            if (viewModel.SelectedYetkiliOlduguSubeler != null && viewModel.SelectedYetkiliOlduguSubeler.Length > 0)
                            {
                                viewModel.Model.PersonelSubeYetkiler = new List<PersonelSubeYetki>();

                                foreach (var sube in viewModel.SelectedYetkiliOlduguSubeler)
                                {
                                    viewModel.Model.PersonelSubeYetkiler.Add(new PersonelSubeYetki
                                    {
                                        Personel = viewModel.Model,
                                        SubeId = sube
                                    });
                                }
                            }

                            #endregion

                            viewModel.OperationResult = service.Update(viewModel.Model);
                            viewModel.Model = GetModel(viewModel.Model.PersonelId);
                        }

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.PersonelId > 0
                          ? RedirectToAction("Duzenle", new { id = viewModel.Model.PersonelId })
                          : RedirectToAction("Duzenle");
                    }
                case "GorselSil":
                    {
                        FileHelper.IfFileExistsDeleteFile(viewModel.Model.GorselYol);

                        viewModel.Model.GorselDosyaAd = string.Empty;

                        viewModel.Model.PersonelSubeDersler = null;
                        viewModel.Model.PersonelSubeUcretler = null;
                        viewModel.Model.PersonelSubeYetkiler = null;

                        viewModel.OperationResult = service.Update(viewModel.Model);
                        viewModel.Model = GetModel(viewModel.Model.PersonelId);

                        break;
                    }
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult YetkiliGiris(int id)
        {
            var service = serviceFactory.CreateService<IKullaniciService>();
            var kullanici = service.Get(x => x.PersonelId == id);

            if (kullanici != null)
            {
                FormsAuthentication.SignOut();

                kullanici = service.Get(
                    x =>
                        x.PersonelId == id,
                    y => y.Personel.PersonelGrup,
                    y => y.Personel.PersonelYetkiGrup,
                    y => y.Personel.Ulke,
                    y => y.Personel.Sube.Kurum,
                    y => y.Personel.PersonelSubeYetkiler,
                    y => y.Personel.PersonelYetkiGrup);

                if (kullanici.Personel?.PersonelSubeYetkiler != null)
                {
                    var personelSubeYetkiler = new List<PersonelSubeYetkiDto>();

                    foreach (var item in kullanici.Personel.PersonelSubeYetkiler)
                    {
                        var personelSubeYetki = new PersonelSubeYetkiDto
                        {
                            PersonelId = item.PersonelId,
                            SubeId = item.SubeId
                        };

                        personelSubeYetkiler.Add(personelSubeYetki);
                    }

                    PersonelSubeYetkiService.Update(personelSubeYetkiler, kullanici.PersonelId);
                }

                string identityParameters = $"{kullanici.PersonelId}|{kullanici.Personel.Sube.KurumId}|{kullanici.Personel.SubeId}|{kullanici.Personel.PersonelYetkiGrupId}|{kullanici.KullaniciAd}|{kullanici.Personel.Eposta}|{kullanici.Personel.AdSoyad}|{kullanici.Personel.GorselYol}|{kullanici.Personel.PersonelGrupAd}|{kullanici.Personel.PersonelYetkiGrupAd}|{kullanici.Personel.YasadigiUlkeAd}|{string.Format(AyarlarService.Get().GecerliTarihSaatFormati, kullanici.SonGirisTarihi)}|{kullanici.Personel.Sube.Kurum.LogoYol}";

                FormsAuthentication.SetAuthCookie(identityParameters, false);
            }

            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPersonelService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Detay(int? id)
        {
            var viewModel = new PersonelDetayViewModel();

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IPersonelService>();
                var model = service.Get(x => x.PersonelId == id,
                    y => y.Kullanici,
                    y => y.Sube,
                    y => y.PersonelGrup,
                    y => y.PersonelYetkiGrup,
                    y => y.PersonelSubeDersler.Select(z => z.Sube),
                    y => y.PersonelSubeUcretler.Select(z => z.Sube),
                    y => y.PersonelSubeYetkiler.Select(z => z.Sube));

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Personel();
            }

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new PersonelListeleViewModel
            {
                Model = new Personel()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(PersonelListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IPersonelService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SubePersonelListele(int? subeId, bool nullable)
        {
            if (subeId == null) subeId = 0;

            var selectList = selectListHelper.SubePersonelSelectList((int)subeId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Seciniz,
                Value = nullable ? "" : "0"
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}