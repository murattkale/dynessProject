using System.Collections.Generic;
using Core.Entities.Dto;
using Entities.Concrete;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using System.Data;
using System;
using Core.Properties;

namespace WebUI.Controllers
{
    public class SinifController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public SinifController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(SinifDuzenleViewModel viewModel)
        {
            viewModel.SubeSelectList = selectListHelper.SubeSelectList();
            viewModel.SezonSelectList = selectListHelper.SezonSelectList();
            viewModel.BransSelectList = selectListHelper.BransSelectList();
            viewModel.SinifTurSelectList = selectListHelper.SinifTurSelectList();
            viewModel.SinifSeansSelectList = selectListHelper.SinifSeansSelectList();
            viewModel.SinifSeviyeSelectList = selectListHelper.SinifSeviyeSelectList();
            viewModel.DerslikSelectList = selectListHelper.DerslikSelectList();
            viewModel.SinifSelectList = selectListHelper.SinifSelectList();
            viewModel.PersonelSelectList = selectListHelper.PersonelSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new SinifDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Sinif>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<ISinifService>();
                var model = service.Get(x => x.SinifId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;
            }
            else
            {
                viewModel.Model = new Sinif();
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(SinifDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinifService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.SinifId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.SinifId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.SinifId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult TopluOgrenciSozlesmeSinifGuncelle(int? id)
        {
            var viewModel = new TopluOgrenciSozlesmeSinifGuncelleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<List<OgrenciSozlesme>>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            viewModel.SubeSelectList = selectListHelper.SubeSelectList(true);

            if (id != null)
            {
                var subeId = (int)id;

                var service = serviceFactory.CreateService<ISinifService>();
                var siniflar = service.List(x => x.EtkinMi && x.SubeId == subeId).OrderBy(x => x.SinifAd).ToList();

                var sezonService = serviceFactory.CreateService<ISezonService>();
                var sezon = sezonService.List(x => x.EtkinMi).OrderByDescending(x => x.BaslangicTarihi).FirstOrDefault();

                var ogrenciSozlesmeService = serviceFactory.CreateService<IOgrenciSozlesmeService>();
                var sozlesmeler = ogrenciSozlesmeService.List(x => x.SezonId == sezon.SezonId && x.SubeId == subeId, y => y.Ogrenci);
                sozlesmeler = sozlesmeler.OrderBy(x => x.OgrenciAdSoyad).ToList();

                viewModel.Siniflar = siniflar;
                viewModel.OgrenciSozlesmeSiniflar = new List<OgrenciSozlesmeSinif>();
                viewModel.SubeId = subeId;

                for (var i = 0; i < siniflar.Count; i++)
                {
                    var ogrenciSozlesmeler = sozlesmeler.Where(x => x.SinifId == siniflar[i].SinifId).Select(x => x.OgrenciSozlesmeId).ToArray();

                    var ogenciSozlesmeSinif = new OgrenciSozlesmeSinif
                    {
                        SinifId = siniflar[i].SinifId,
                        OgrenciSozlesmeIdler = ogrenciSozlesmeler,
                        OgrenciSozlesmeSelectList = selectListHelper.ToSelectList(sozlesmeler, x => x.OgrenciSozlesmeId, x => x.OgrenciAdSoyad, selectedItems: ogrenciSozlesmeler)
                    };


                    viewModel.OgrenciSozlesmeSiniflar.Add(ogenciSozlesmeSinif);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult TopluOgrenciSozlesmeSinifGuncelle(TopluOgrenciSozlesmeSinifGuncelleViewModel viewModel)
        {
            ModelState.Clear();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        var ogrenciSozlesmeler = new List<OgrenciSozlesme>();

                        var service = serviceFactory.CreateService<IOgrenciSozlesmeService>();

                        if (viewModel.OgrenciSozlesmeSiniflar != null && viewModel.OgrenciSozlesmeSiniflar.Count > 0)
                        {
                            for (int i = 0; i < viewModel.OgrenciSozlesmeSiniflar.Count; i++)
                            {
                                var ogrenciSozlesmeSinif = viewModel.OgrenciSozlesmeSiniflar[i];

                                var sinifId = ogrenciSozlesmeSinif.SinifId;

                                if (ogrenciSozlesmeSinif.OgrenciSozlesmeIdler != null && ogrenciSozlesmeSinif.OgrenciSozlesmeIdler.Length > 0)
                                {
                                    for (int j = 0; j < ogrenciSozlesmeSinif.OgrenciSozlesmeIdler.Length; j++)
                                    {
                                        var ogrenciSozlesmeId = ogrenciSozlesmeSinif.OgrenciSozlesmeIdler[j];

                                        var ogrenciSozlesme = service.Get(x => x.OgrenciSozlesmeId == ogrenciSozlesmeId);

                                        if (ogrenciSozlesme != null)
                                        {
                                            ogrenciSozlesme.SinifId = sinifId;
                                            ogrenciSozlesmeler.Add(ogrenciSozlesme);
                                        }
                                    }
                                }
                            }
                        }

                        viewModel.OperationResult = service.UpdateList(ogrenciSozlesmeler);

                        break;
                    }
                case "Iptal":
                    {
                        return Redirect("/");
                    }
            }

            return RedirectToAction("TopluOgrenciSozlesmeSinifGuncelle");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<ISinifService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SinifListeleViewModel
            {
                Model = new Sinif()
            };
            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(SinifListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<ISinifService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SinifListele(int? subeId, int? sezonId, int? bransId)
        {
            if (subeId == null) subeId = 0;
            if (sezonId == null) sezonId = 0;
            if (bransId == null) bransId = 0;

            var selectList = selectListHelper.SinifSelectList((int)subeId, (int)sezonId, (int)bransId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Seciniz,
                Value = ""
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SiniflarListele(string subeIdler, string sezonIdler)
        {
            var selectList = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(subeIdler) && !string.IsNullOrEmpty(sezonIdler))
            {
                var service = serviceFactory.CreateService<ISinifService>();

                var parameters = new List<Parameter>
                {
                    new Parameter("SubeIdler", subeIdler),
                    new Parameter("SezonIdler", sezonIdler)
                };

                var siniflar = service.SinifListele(parameters).ToList();

                for (int i = 0; i < siniflar.Count; i++)
                {
                    selectList.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = siniflar[i].SinifAd,
                        Value = siniflar[i].SinifId.ToString(),
                    });
                }
            }

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult ExcelIndir(string filter)
        {
            var sezonService = serviceFactory.CreateService<ISezonService>();
            var sonSezonlar = sezonService.List(x => x.EtkinMi && x.BaslangicTarihi <= DateTime.Now && x.BitisTarihi >= DateTime.Now).ToList();

            var siniflar = new List<Tuple<Sinif, int>>();

            var sinifService = serviceFactory.CreateService<ISinifService>();
            var sozlesmeService = serviceFactory.CreateService<IOgrenciSozlesmeService>();

            foreach (var sonSezon in sonSezonlar)
            {
                var sonSezonSiniflar = sinifService.List(x =>
                    x.EtkinMi &&
                    x.SezonId == sonSezon.SezonId,
                    y => y.Sube,
                    y => y.Sezon,
                    y => y.SinifSeviye,
                    y => y.Brans,
                    y => y.SinifSeans,
                    y => y.Derslik,
                    y => y.SinifTur);

                if (sonSezonSiniflar != null && sonSezonSiniflar.Any())
                {
                    foreach (var sonSezonSinif in sonSezonSiniflar)
                    {
                        var ogrenciCount = sozlesmeService.GetCount(x => x.SinifId == sonSezonSinif.SinifId);
                        var tuple = new Tuple<Sinif, int>(sonSezonSinif, ogrenciCount);

                        siniflar.Add(tuple);
                    }
                }
            }

            var dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[13] {
                      new DataColumn(FieldNameResources.Sube),
                      new DataColumn(FieldNameResources.Sezon),
                      new DataColumn(FieldNameResources.SinifSeviye),
                      new DataColumn(FieldNameResources.Brans),
                      new DataColumn(FieldNameResources.SinifAd),
                      new DataColumn(FieldNameResources.SinifTur),
                      new DataColumn(FieldNameResources.ToplamDersSaati),
                      new DataColumn(FieldNameResources.KayitUcreti),
                      new DataColumn(FieldNameResources.Kapasite),
                      new DataColumn(FieldNameResources.Mevcut),
                      new DataColumn(FieldNameResources.SinifSeans),
                      new DataColumn(FieldNameResources.Derslik),
                      new DataColumn(FieldNameResources.Durum)
            });

            foreach (var sinif in siniflar)
            {
                dt.Rows.Add(
                    sinif.Item1.SubeAd,
                    sinif.Item1.SezonAd,
                    sinif.Item1.SinifSeviyeAd,
                    sinif.Item1.BransAd,
                    sinif.Item1.SinifAd,
                    sinif.Item1.SinifTurAd,
                    sinif.Item1.ToplamDersSaat,
                    sinif.Item1.KayitUcretiFormatted,
                    sinif.Item1.SinifKapasite,
                    sinif.Item2,
                    sinif.Item1.SinifSeansAd,
                    sinif.Item1.DerslikAd,
                    sinif.Item1.Durum);
            }

            dt.TableName = "SinifOgrenciler";
            var file = ExcelHelper.ExcelIndir(new List<DataTable> { dt }, "SinifOgrenciler");

            return File(file.FileContents, file.ContentType, file.FileDownLoadName);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SubeSezonBransSinifListele(string SubeIdler, string SezonIdler, string BransIdler)
        {
            var subeIdler = new int[SubeIdler.Split(',').Length - 1];

            for (int i = 0; i < SubeIdler.Split(',').Length - 1; i++)
            {
                subeIdler[i] = Convert.ToInt32(SubeIdler.Split(',')[i]);
            }

            var sezonIdler = new int[SezonIdler.Split(',').Length - 1];

            for (int i = 0; i < SezonIdler.Split(',').Length - 1; i++)
            {
                sezonIdler[i] = Convert.ToInt32(SezonIdler.Split(',')[i]);
            }

            var bransIdler = new int[BransIdler.Split(',').Length - 1];

            for (int i = 0; i < BransIdler.Split(',').Length - 1; i++)
            {
                bransIdler[i] = Convert.ToInt32(BransIdler.Split(',')[i]);
            }

            var selectList = selectListHelper.SubeSezonBransSiniflarSelectList(subeIdler, sezonIdler, bransIdler);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}