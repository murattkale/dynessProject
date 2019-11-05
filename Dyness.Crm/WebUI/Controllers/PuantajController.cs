using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Models;
using System.Linq;
using WebUI.Helpers;
using Core.Entities.Dto;
using System.Collections.Generic;
using System;
using Resources;
using System.Linq.Expressions;

namespace WebUI.Controllers
{
    public class PuantajController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public PuantajController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(PersonelPuantajGunlukDurumDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.PersonelPuantajGunlukDurumSelectList = selectListHelper.PersonelPuantajGunlukDurumSelectList(false, new[] { selectedItemId });
        }

        private PersonelPuantajDuzenleViewModel TabloyuDoldur(
            PersonelPuantajDuzenleViewModel viewModel,
            IPersonelPuantajService service,
            IOrderedEnumerable<PersonelPuantajGunlukDurum> personelPuantajGunlukDurumlar)
        {
            viewModel.Model = new List<PersonelPuantaj>();

            if (viewModel.Yil == null || viewModel.Ay == null)
            {
                viewModel.OperationResult.MessageInfos.Add(new MessageInfo
                {
                    Message = LangResources.AyYilBosOlamaz,
                    MessageInfoType = MessageInfoType.Error
                });

                return viewModel;
            }

            var daysInMonth = DateTime.DaysInMonth((int)viewModel.Yil, (int)viewModel.Ay);

            viewModel.GunDtolar = new List<GunDto>();

            for (int i = 0; i < daysInMonth; i++)
            {
                var dateTime = new DateTime((int)viewModel.Yil, (int)viewModel.Ay, i + 1);
                var dayOfWeek = dateTime.DayOfWeek;

                var gunDto = new GunDto
                {
                    Id = i + 1,
                    Ay = (int)viewModel.Ay,
                    Yil = (int)viewModel.Yil,
                    TatilMi = dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday
                };

                viewModel.GunDtolar.Add(gunDto);
            }

            viewModel.PersonelPuantajGunlukDurumlar = personelPuantajGunlukDurumlar.ToList();

            Expression<Func<Personel, bool>> expression = null;

            if (viewModel.PersonelGrupId != 0 && viewModel.SubeId != 0)
            {
                expression = x => x.PersonelGrupId == viewModel.PersonelGrupId && x.SubeId == viewModel.SubeId;
            }
            else if (viewModel.PersonelGrupId != 0)
            {
                expression = x => x.PersonelGrupId == viewModel.PersonelGrupId;
            }
            else if (viewModel.SubeId != 0)
            {
                expression = x => x.SubeId == viewModel.SubeId;
            }

            var personelService = serviceFactory.CreateService<IPersonelService>();
            var personeller = personelService.List(expression).ToList();

            foreach (var pers in personeller)
            {
                PersonelPuantaj personelPuantaj = service.Get(
                    x =>
                    x.PersonelId == pers.PersonelId &&
                    x.PuantajYil == viewModel.Yil &&
                    x.PuantajAy == viewModel.Ay,
                    y => y.Personel,
                    y => y.PersonelPuantajGunlukler.Select(z => z.PersonelPuantajGunlukDurum));

                if (personelPuantaj != null)
                {
                    personelPuantaj.PersonelPuantajGunlukler = personelPuantaj.PersonelPuantajGunlukler.OrderBy(x => x.Gun).ToList();

                    viewModel.Model.Add(personelPuantaj);
                }
                else
                {
                    personelPuantaj = new PersonelPuantaj
                    {
                        PuantajAy = (int)viewModel.Ay,
                        PuantajYil = (int)viewModel.Yil,
                        PersonelId = pers.PersonelId,
                        PersonelPuantajGunlukler = new List<PersonelPuantajGunluk>()
                    };

                    foreach (var gunDto in viewModel.GunDtolar)
                    {
                        var personelPuantajGunluk = new PersonelPuantajGunluk
                        {
                            Yil = gunDto.Yil,
                            Ay = gunDto.Ay,
                            Gun = gunDto.Id,
                            PersonelPuantajGunlukDurumId = gunDto.TatilMi ? 2 : 1,
                            PersonelPuantaj = personelPuantaj
                        };

                        personelPuantaj.PersonelPuantajGunlukler.Add(personelPuantajGunluk);
                    }

                    var personelPuantajOperationResult = service.Add(personelPuantaj);

                    if (!personelPuantajOperationResult.Status)
                    {
                        viewModel.OperationResult.MessageInfos.AddRange(personelPuantajOperationResult.MessageInfos);

                        return viewModel;
                    }

                    personelPuantaj.Personel = personelPuantajOperationResult.Model.Personel;

                    foreach (var personelPuantajGunluk in personelPuantaj.PersonelPuantajGunlukler)
                    {
                        if (personelPuantajGunluk.PersonelPuantajGunlukDurum == null)
                            personelPuantajGunluk.PersonelPuantajGunlukDurum = personelPuantajGunlukDurumlar.FirstOrDefault(x => x.PersonelPuantajGunlukDurumId == personelPuantajGunluk.PersonelPuantajGunlukDurumId);
                    }

                    viewModel.Model.Add(personelPuantaj);
                }
            }

            viewModel.TabloyuGoster = true;

            return viewModel;
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle()
        {
            var viewModel = new PersonelPuantajDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<List<PersonelPuantaj>>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.Model = new List<PersonelPuantaj>();
            viewModel.PersonelSelectList = selectListHelper.PersonelGrupSelectList(true);
            viewModel.SubeSelectList = selectListHelper.SubeSelectList(true);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(PersonelPuantajDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPersonelPuantajService>();

            var personelPuantajGunlukDurumService = serviceFactory.CreateService<IPersonelPuantajGunlukDurumService>();
            var personelPuantajGunlukDurumlar = personelPuantajGunlukDurumService.List(x => x.EtkinMi).OrderBy(x => x.Sira);

            switch (viewModel.Command)
            {
                case "Olustur":
                    {
                        viewModel = TabloyuDoldur(viewModel, service, personelPuantajGunlukDurumlar);

                        break;
                    }
                case "Kaydet":
                    {
                        List<EntityOperationResult<PersonelPuantaj>> results = new List<EntityOperationResult<PersonelPuantaj>>();

                        foreach (var model in viewModel.Model)
                        {
                            foreach (var personelPuantajGunluk in model.PersonelPuantajGunlukler)
                            {
                                var personelPuantajGunlukDurum = personelPuantajGunluk.PersonelPuantajGunlukDurum;
                                var durum = personelPuantajGunlukDurumlar.FirstOrDefault(x => x.PersonelPuantajGunlukDurumKisatlma == personelPuantajGunlukDurum.PersonelPuantajGunlukDurumKisatlma);

                                personelPuantajGunluk.PersonelPuantajGunlukDurumId = durum.PersonelPuantajGunlukDurumId;
                                personelPuantajGunluk.PersonelPuantajGunlukDurum = null;
                            }

                            var result = service.Update(model);
                            results.Add(result);
                        }

                        if (results.Count(x => !x.Status) > 0)
                        {
                            viewModel.OperationResult.Status = false;

                            foreach (var item in results)
                            {
                                viewModel.OperationResult.MessageInfos.AddRange(item.MessageInfos);
                            }
                        }
                        else
                        {
                            if (viewModel.OperationResult == null)
                                viewModel.OperationResult = new EntityOperationResult<List<PersonelPuantaj>>();

                            viewModel.OperationResult.Status = true;
                            viewModel.OperationResult.MessageInfos.Add(
                                new MessageInfo
                                {
                                    MessageInfoType = MessageInfoType.Success,
                                    Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
                                });
                        }

                        viewModel = TabloyuDoldur(viewModel, service, personelPuantajGunlukDurumlar);

                        break;
                    }
                case "Iptal":
                    {
                        viewModel.TabloyuGoster = false;

                        RedirectToAction("Duzenle");
                        break;
                    }
            }

            viewModel.PersonelSelectList = selectListHelper.PersonelGrupSelectList(true);
            viewModel.SubeSelectList = selectListHelper.SubeSelectList(true);

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult GunlukDurumDuzenle(int? id)
        {
            var viewModel = new PersonelPuantajGunlukDurumDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<PersonelPuantajGunlukDurum>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IPersonelPuantajGunlukDurumService>();
                var model = service.Get(x => x.PersonelPuantajGunlukDurumId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new PersonelPuantajGunlukDurum();
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult GunlukDurumDuzenle(PersonelPuantajGunlukDurumDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IPersonelPuantajGunlukDurumService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.PersonelPuantajGunlukDurumId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.PersonelPuantajGunlukDurumId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.PersonelPuantajGunlukDurumId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.PersonelPuantajGunlukDurumId);

            return View(viewModel);
        }
    }
}