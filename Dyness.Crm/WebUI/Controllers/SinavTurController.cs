using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SinavTurController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SinavTurController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SinavTurDuzenleViewModel viewModel, int? selectedItemId)
        {
            viewModel.SinavTurSelectList = selectListHelper.SinavTurSelectList(false);

            var model = viewModel.Model;

            var serviceDers = serviceFactory.CreateService<IDersService>();
            var serviceDersGrup = serviceFactory.CreateService<IDersGrupService>();
            var servicePuanTur = serviceFactory.CreateService<IPuanTurService>();

            var dersler = serviceDers.List(x => x.EtkinMi && x.DersGrupId != null).ToList();
            var dersGruplar = serviceDersGrup.List(x => x.EtkinMi).ToList();
            var puanTurler = servicePuanTur.List(x => x.EtkinMi).ToList();

            if (model?.SinavTurDersKatSayilar != null && model.SinavTurDersKatSayilar.Count > 0)
            {
                var sinavTurDersKatSayilar = new List<SinavTurDersKatSayi>();

                foreach (var sinavTurDersKatSayi in model.SinavTurDersKatSayilar)
                {
                    if (sinavTurDersKatSayi.DersGrup != null)
                        sinavTurDersKatSayilar.Add(sinavTurDersKatSayi);
                }

                model.SinavTurDersKatSayilar = sinavTurDersKatSayilar;
            }

            if (model?.SinavTurDersler != null && model.SinavTurDersler.Count > 0)
            {
                var sinavTurDersler = new List<SinavTurDers>();

                foreach (var sinavTurDers in model.SinavTurDersler)
                {
                    if (sinavTurDers.Ders != null)
                        sinavTurDersler.Add(sinavTurDers);
                }

                model.SinavTurDersler = sinavTurDersler;
            }

            if (selectedItemId != null && selectedItemId > 0)
            {
                if (dersGruplar != null && dersGruplar != null)
                {
                    var addList = new List<DersGrup>();

                    for (int i = 0; i < dersGruplar.Count; i++)
                    {
                        if (puanTurler != null && puanTurler.Count > 0)
                        {
                            for (int j = 0; j < puanTurler.Count; j++)
                            {
                                if (model.SinavTurDersKatSayilar.Count(x => x.DersGrupId == dersGruplar[i].DersGrupId && x.PuanTurId == puanTurler[j].PuanTurId) == 0)
                                {
                                    addList.Add(dersGruplar[i]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (model.SinavTurDersKatSayilar.Count(x => x.DersGrupId == dersGruplar[i].DersGrupId) == 0)
                                addList.Add(dersGruplar[i]);
                        }
                    }

                    var addSinavTurDersKatSayilar = new List<SinavTurDersKatSayi>();

                    foreach (var dersGrup in addList)
                    {
                        if (puanTurler != null && puanTurler.Count > 0)
                        {
                            for (int i = 0; i < puanTurler.Count; i++)
                            {
                                var puanTur = puanTurler[i];

                                if (model.SinavTurDersKatSayilar.Count(x => x.DersGrupId == dersGrup.DersGrupId && x.PuanTurId == puanTur.PuanTurId) == 0)
                                {
                                    addSinavTurDersKatSayilar.Add(new SinavTurDersKatSayi
                                    {
                                        DersGrup = dersGrup,
                                        DersGrupId = dersGrup.DersGrupId,
                                        PuanTur = puanTur,
                                        PuanTurId = puanTur.PuanTurId,
                                        SinavTurId = model.SinavTurId
                                    });
                                }
                            }
                        }
                        else
                        {
                            addSinavTurDersKatSayilar.Add(new SinavTurDersKatSayi
                            {
                                DersGrup = dersGrup,
                                DersGrupId = dersGrup.DersGrupId,
                                SinavTurId = model.SinavTurId
                            });
                        }
                    }

                    if (addSinavTurDersKatSayilar != null && addSinavTurDersKatSayilar.Count > 0)
                        model.SinavTurDersKatSayilar.AddRange(addSinavTurDersKatSayilar);

                    model.SinavTurDersKatSayilar = model.SinavTurDersKatSayilar.OrderBy(x => x.DersGrup.DersGrupAd).ToList();
                }

                if (dersler != null && dersler != null)
                {
                    var addList = new List<Ders>();

                    for (int i = 0; i < dersler.Count; i++)
                    {
                        if (model.SinavTurDersler.Count(x => x.DersId == dersler[i].DersId) == 0)
                            addList.Add(dersler[i]);
                    }

                    var addSinavTurDersler = new List<SinavTurDers>();

                    foreach (var ders in addList)
                    {
                        addSinavTurDersler.Add(new SinavTurDers
                        {
                            DersId = ders.DersId,
                            Ders = ders,
                            SinavTurId = model.SinavTurId
                        });
                    }

                    if (addSinavTurDersler != null && addSinavTurDersler.Count > 0)
                        model.SinavTurDersler.AddRange(addSinavTurDersler);

                    model.SinavTurDersler = model.SinavTurDersler.OrderBy(x => x.Ders.DersAd).ToList();
                }
            }
            else
            {
                viewModel.Model = new SinavTur
                {
                    SinavTurDersKatSayilar = new List<SinavTurDersKatSayi>(),
                    SinavTurDersler = new List<SinavTurDers>()
                };

                foreach (var dersGrup in dersGruplar)
                {
                    if (puanTurler != null && puanTurler.Count > 0)
                    {
                        for (int i = 0; i < puanTurler.Count; i++)
                        {
                            var puanTur = puanTurler[i];

                            viewModel.Model.SinavTurDersKatSayilar.Add(new SinavTurDersKatSayi
                            {
                                DersGrup = dersGrup,
                                DersGrupId = dersGrup.DersGrupId,
                                PuanTur = puanTur,
                                PuanTurId = puanTur.PuanTurId,
                                SinavTur = viewModel.Model
                            });
                        }
                    }
                    else
                    {
                        viewModel.Model.SinavTurDersKatSayilar.Add(new SinavTurDersKatSayi
                        {
                            DersGrup = dersGrup,
                            DersGrupId = dersGrup.DersGrupId,
                            SinavTur = viewModel.Model
                        });
                    }
                }

                viewModel.Model.SinavTurDersKatSayilar = viewModel.Model.SinavTurDersKatSayilar.OrderBy(x => x.DersGrup.DersGrupAd).ToList();

                foreach (var ders in dersler)
                {
                    viewModel.Model.SinavTurDersler.Add(new SinavTurDers
                    {
                        DersId = ders.DersId,
                        Ders = ders,
                        SinavTur = viewModel.Model
                    });
                }

                viewModel.Model.SinavTurDersler = viewModel.Model.SinavTurDersler.OrderBy(x => x.Ders.DersAd).ToList();
            }
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SinavTurDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<SinavTur>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinavTurService>();

                var model = service.Get(
                               x => x.SinavTurId == id,
                               y => y.SinavTurDersKatSayilar.Select(z => z.DersGrup),
                               y => y.SinavTurDersKatSayilar.Select(z => z.PuanTur),
                               y => y.SinavTurDersler.Select(z => z.Ders.DersGrup));

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SinavTurDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinavTurService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SinavTurId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SinavTurId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SinavTurId })
                            : RedirectToAction("Duzenle");
                    }
            }

            var model = service.Get(
                              x => x.SinavTurId == viewModel.Model.SinavTurId,
                              y => y.SinavTurDersKatSayilar.Select(z => z.DersGrup),
                              y => y.SinavTurDersKatSayilar.Select(z => z.PuanTur),
                              y => y.SinavTurDersler.Select(z => z.Ders.DersGrup));
            viewModel.Model = model;

            GetLists(viewModel, viewModel.Model?.SinavTurId ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinavTurService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }
    }
}