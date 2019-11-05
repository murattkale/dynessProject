using Core.CrossCuttingConcerns.Security;
using Core.General;
using Services.Abstract;
using Services.DependencyResolvers;
using Services.WebServices;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class KullaniciController : Controller
    {
        private IServiceFactory serviceFactory;

        public KullaniciController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [HttpGet]
        public ActionResult Giris()
        {
            if (Request.IsAuthenticated && Identity.Rol == RolModel.Personel)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var viewModel = new KullaniciGirisViewModel();

                ViewBag.ReturnURL = Request.QueryString["ReturnUrl"];
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Giris(KullaniciGirisViewModel viewModel)
        {
            ModelState.Clear();

            var returnUrl = Request.Form["returnUrl"];

            var service = serviceFactory.CreateService<IKullaniciService>();

            viewModel.OperationResult = service.Giris(viewModel.KullaniciAd, viewModel.Sifre);

            if (viewModel.OperationResult.Status)
            {
                var kullanici = viewModel.OperationResult.Model;

                if (viewModel.OperationResult.Model.Personel?.PersonelSubeYetkiler != null)
                {
                    var personelSubeYetkiler = new List<PersonelSubeYetkiDto>();

                    foreach (var item in viewModel.OperationResult.Model.Personel.PersonelSubeYetkiler)
                    {
                        var personelSubeYetki = new PersonelSubeYetkiDto
                        {
                            PersonelId = item.PersonelId,
                            SubeId = item.SubeId
                        };

                        personelSubeYetkiler.Add(personelSubeYetki);
                    }

                    PersonelSubeYetkiService.Update(personelSubeYetkiler, viewModel.OperationResult.Model.PersonelId);
                }

                string identityParameters =
                    $"{kullanici.PersonelId}|{kullanici.Personel.Sube.KurumId}|{kullanici.Personel.SubeId}|{kullanici.Personel.PersonelYetkiGrupId}|{kullanici.KullaniciAd}|{kullanici.Personel.Eposta ?? ""}|{kullanici.Personel.AdSoyad}|{kullanici.Personel.GorselYol}|{kullanici.Personel.PersonelGrupAd}|{kullanici.Personel.PersonelYetkiGrupAd}|{kullanici.Personel.YasadigiUlkeAd}|{string.Format(AyarlarService.Get().GecerliTarihSaatFormati, kullanici.SonGirisTarihi)}|{kullanici.Personel.Sube.Kurum.LogoYol}|{RolModel.Personel}";

                FormsAuthentication.SetAuthCookie(identityParameters, false);

                if (string.IsNullOrEmpty(returnUrl)) returnUrl = "/";

                return Redirect(returnUrl);
            }

            return View(viewModel);
        }

        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Giris", "Kullanici");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult TcKimlikDogrula(
            string tcKimlikNo,
            string ad,
            string soyad,
            string dogumTarihi)
        {
            object result = null;

            if (string.IsNullOrEmpty(tcKimlikNo) || string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) || string.IsNullOrEmpty(dogumTarihi))
            {
                result = new { StatusClass = "alert alert-info", Message = Resources.LangResources.TcKimlikKontrolEdebilmekIcin };
            }
            else
            {
                var dogumYili = string.IsNullOrEmpty(dogumTarihi)
                    ? 0
                    : DateTime.ParseExact(dogumTarihi, AyarlarService.Get().GecerliTarihFormati, null).Year;

                var response = TcKimlikSorgulama.
                      Dogrula(
                        tcKimlikNo,
                        ad,
                        soyad,
                        dogumYili);

                if (response)
                {
                    result = new { StatusClass = "alert alert-success", Message = Resources.LangResources.TcKimlikNoDogrulandi };
                }
                else
                {
                    result = new { StatusClass = "alert alert-danger", Message = Resources.LangResources.TcKimikNoDogrulanamadi };
                }
            }

            var jSonObject = JsonHelper.ObjectToJsonString(result);

            return Content(jSonObject, "application/json");
        }
    }
}