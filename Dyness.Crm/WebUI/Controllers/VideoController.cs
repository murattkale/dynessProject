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
    public class VideoController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public VideoController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new VideoDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Video>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IVideoService>();
                var serviceVideoVideoKategoriService = serviceFactory.CreateService<IVideoVideoKategoriService>();
                var serviceVideoKonuService = serviceFactory.CreateService<IVideoKonuService>();
                var serviceVideoKurumYetkiService = serviceFactory.CreateService<IVideoKurumYetkiService>();
                var serviceVideoSubeYetkiService = serviceFactory.CreateService<IVideoSubeYetkiService>();
                var serviceVideoSinifYetkiService = serviceFactory.CreateService<IVideoSinifYetkiService>();

                var model = service.Get(x => x.VideoId == id);

                if (model == null)
                    return Redirect("/Error/NotFound");

                model.VideoVideoKategoriler = serviceVideoVideoKategoriService.List(x => x.VideoId == id);
                model.VideoKonular = serviceVideoKonuService.List(x => x.VideoId == id);
                model.VideoKurumYetkiler = serviceVideoKurumYetkiService.List(x => x.VideoId == id);
                model.VideoSubeYetkiler = serviceVideoSubeYetkiService.List(x => x.VideoId == id);
                model.VideoSinifYetkiler = serviceVideoSinifYetkiService.List(x => x.VideoId == id, y => y.Sinif);

                viewModel.Model = model;

                if (model.VideoVideoKategoriler != null)
                    viewModel.SelectedVideoKategoriler = model.VideoVideoKategoriler.Select(x => x.VideoKategoriId).ToArray();

                if (model.VideoKonular != null)
                    viewModel.SelectedKonular = model.VideoKonular.Select(x => x.KonuId).ToArray();

                if (model.VideoKurumYetkiler != null)
                    viewModel.SelectedKurumlar = model.VideoKurumYetkiler.Select(x => x.KurumId).ToArray();

                if (model.VideoSubeYetkiler != null)
                    viewModel.SelectedSubeler = model.VideoSubeYetkiler.Select(x => x.SubeId).ToArray();

                if (model.VideoSinifYetkiler != null)
                {
                    viewModel.SelectedSubeler = model.VideoSinifYetkiler.Select(x => x.Sinif.SubeId).Distinct().ToArray();
                    viewModel.SelectedSezonlar = model.VideoSinifYetkiler.Select(x => x.Sinif.SezonId).Distinct().ToArray();
                    viewModel.SelectedBranslar = model.VideoSinifYetkiler.Where(x => x.Sinif.BransId != null).Select(x => (int)x.Sinif.BransId).Distinct().ToArray();

                    viewModel.SelectedSiniflar = model.VideoSinifYetkiler.Select(x => x.SinifId).ToArray();
                }
            }
            else
            {
                viewModel.Model = new Video();
            }

            viewModel.DersSelectList = selectListHelper.DersSelectList();
            viewModel.KurumSelectList = selectListHelper.KurumSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(VideoDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IVideoService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.Model.VideoId == 0)
                        {
                            #region VideoVideoKategori

                            if (viewModel.SelectedVideoKategoriler != null && viewModel.SelectedVideoKategoriler.Length > 0)
                            {
                                var videoKategoriler = new List<VideoVideoKategori>();

                                for (int i = 0; i < viewModel.SelectedVideoKategoriler.Length; i++)
                                {
                                    var videoKategoriId = viewModel.SelectedVideoKategoriler[i];

                                    if (videoKategoriId == 0)
                                        continue;

                                    var videoKategori = new VideoVideoKategori
                                    {
                                        Video = viewModel.Model,
                                        VideoKategoriId = videoKategoriId
                                    };

                                    videoKategoriler.Add(videoKategori);
                                }

                                viewModel.Model.VideoVideoKategoriler = videoKategoriler;
                            }

                            #endregion

                            #region VideoKonu

                            if (viewModel.SelectedKonular != null && viewModel.SelectedKonular.Length > 0)
                            {
                                var videoKonular = new List<VideoKonu>();

                                for (int i = 0; i < viewModel.SelectedKonular.Length; i++)
                                {
                                    var konuId = viewModel.SelectedKonular[i];

                                    if (konuId == 0)
                                        continue;

                                    var videoKonu = new VideoKonu
                                    {
                                        Video = viewModel.Model,
                                        KonuId = konuId
                                    };

                                    videoKonular.Add(videoKonu);
                                }

                                viewModel.Model.VideoKonular = videoKonular;
                            }

                            #endregion

                            #region VideoKurumYetki

                            if (viewModel.SelectedKurumlar != null && viewModel.SelectedKurumlar.Length > 0)
                            {
                                var videoKurumYetkiler = new List<VideoKurumYetki>();

                                for (int i = 0; i < viewModel.SelectedKurumlar.Length; i++)
                                {
                                    var kurumId = viewModel.SelectedKurumlar[i];

                                    if (kurumId == 0)
                                        continue;

                                    var videoKurumYetki = new VideoKurumYetki
                                    {
                                        Video = viewModel.Model,
                                        KurumId = kurumId
                                    };

                                    videoKurumYetkiler.Add(videoKurumYetki);
                                }

                                viewModel.Model.VideoKurumYetkiler = videoKurumYetkiler;
                            }

                            #endregion

                            #region VideoSubeYetki

                            if (viewModel.SelectedSubeler != null && viewModel.SelectedSubeler.Length > 0)
                            {
                                var videoSubeYetkiler = new List<VideoSubeYetki>();

                                for (int i = 0; i < viewModel.SelectedSubeler.Length; i++)
                                {
                                    var subeId = viewModel.SelectedSubeler[i];

                                    if (subeId == 0)
                                        continue;

                                    var videoSubeYetki = new VideoSubeYetki
                                    {
                                        Video = viewModel.Model,
                                        SubeId = subeId
                                    };

                                    videoSubeYetkiler.Add(videoSubeYetki);
                                }

                                viewModel.Model.VideoSubeYetkiler = videoSubeYetkiler;
                            }

                            #endregion

                            #region VideoSinifYetki

                            if (viewModel.SelectedSiniflar != null && viewModel.SelectedSiniflar.Length > 0)
                            {
                                var videoSinifYetkiler = new List<VideoSinifYetki>();

                                for (int i = 0; i < viewModel.SelectedSiniflar.Length; i++)
                                {
                                    var sinifId = viewModel.SelectedSiniflar[i];

                                    if (sinifId == 0)
                                        continue;

                                    var videoSinifYetki = new VideoSinifYetki
                                    {
                                        Video = viewModel.Model,
                                        SinifId = sinifId
                                    };

                                    videoSinifYetkiler.Add(videoSinifYetki);
                                }

                                viewModel.Model.VideoSinifYetkiler = videoSinifYetkiler;
                            }



                            #endregion
                        }
                        else
                        {
                            var serviceVideoVideoKategoriService = serviceFactory.CreateService<IVideoVideoKategoriService>();
                            var serviceVideoKonuService = serviceFactory.CreateService<IVideoKonuService>();
                            var serviceVideoKurumYetkiService = serviceFactory.CreateService<IVideoKurumYetkiService>();
                            var serviceVideoSubeYetkiService = serviceFactory.CreateService<IVideoSubeYetkiService>();
                            var serviceVideoSinifYetkiService = serviceFactory.CreateService<IVideoSinifYetkiService>();

                            var id = viewModel.Model.VideoId;

                            var model = service.Get(x => x.VideoId == id);

                            if (model == null)
                                return Redirect("/Error/NotFound");

                            var videoVideoKategoriler = serviceVideoVideoKategoriService.List(x => x.VideoId == id).ToList();
                            var videoKonular = serviceVideoKonuService.List(x => x.VideoId == id).ToList();
                            var videoKurumYetkiler = serviceVideoKurumYetkiService.List(x => x.VideoId == id).ToList();
                            var videoSubeYetkiler = serviceVideoSubeYetkiService.List(x => x.VideoId == id).ToList();
                            var videoSinifYetkiler = serviceVideoSinifYetkiService.List(x => x.VideoId == id, y => y.Sinif).ToList();

                            #region VideoVideoKategori

                            if (viewModel.SelectedVideoKategoriler != null && viewModel.SelectedVideoKategoriler.Length > 0)
                            {
                                if (videoVideoKategoriler.Any())
                                {
                                    for (int i = 0; i < videoVideoKategoriler.Count; i++)
                                    {
                                        var exists = viewModel.SelectedVideoKategoriler.Count(x => x == videoVideoKategoriler[i].VideoKategoriId) > 0;

                                        if (!exists)
                                            videoVideoKategoriler[i].Deleted = true;
                                    }
                                }

                                if (viewModel.Model.VideoVideoKategoriler == null)
                                    viewModel.Model.VideoVideoKategoriler = new List<VideoVideoKategori>();

                                for (int i = 0; i < viewModel.SelectedVideoKategoriler.Length; i++)
                                {
                                    var videoKategoriId = viewModel.SelectedVideoKategoriler[i];

                                    if (videoKategoriId == 0)
                                        continue;

                                    if (videoVideoKategoriler.Count(x => x.VideoKategoriId == videoKategoriId) == 0)
                                    {
                                        var videoKategori = new VideoVideoKategori
                                        {
                                            VideoId = id,
                                            VideoKategoriId = videoKategoriId
                                        };

                                        videoVideoKategoriler.Add(videoKategori);
                                    }
                                }

                                if (videoVideoKategoriler.Count > 0)
                                    viewModel.Model.VideoVideoKategoriler = videoVideoKategoriler;
                            }
                            else if (videoVideoKategoriler.Any())
                            {
                                foreach (var videoVideoKategori in videoVideoKategoriler)
                                {
                                    videoVideoKategori.Deleted = true;
                                }

                                viewModel.Model.VideoVideoKategoriler = videoVideoKategoriler;
                            }

                            #endregion

                            #region VideoKonu

                            if (viewModel.SelectedKonular != null && viewModel.SelectedKonular.Length > 0)
                            {
                                if (videoKonular.Any())
                                {
                                    for (int i = 0; i < videoKonular.Count; i++)
                                    {
                                        var exists = viewModel.SelectedKonular.Count(x => x == videoKonular[i].KonuId) > 0;

                                        if (!exists)
                                            videoKonular[i].Deleted = true;
                                    }
                                }

                                if (viewModel.Model.VideoKonular == null)
                                    viewModel.Model.VideoKonular = new List<VideoKonu>();

                                for (int i = 0; i < viewModel.SelectedKonular.Length; i++)
                                {
                                    var konuId = viewModel.SelectedKonular[i];

                                    if (konuId == 0)
                                        continue;

                                    if (videoKonular.Count(x => x.KonuId == konuId) == 0)
                                    {
                                        var videoKonu = new VideoKonu
                                        {
                                            VideoId = id,
                                            KonuId = konuId
                                        };

                                        videoKonular.Add(videoKonu);
                                    }
                                }

                                if (videoKonular.Count > 0)
                                    viewModel.Model.VideoKonular = videoKonular;
                            }
                            else if (videoKonular.Any())
                            {
                                foreach (var videoKonu in videoKonular)
                                {
                                    videoKonu.Deleted = true;
                                }

                                viewModel.Model.VideoKonular = videoKonular;
                            }

                            #endregion

                            #region VideoKurumYetki

                            if (viewModel.SelectedKurumlar != null && viewModel.SelectedKurumlar.Length > 0)
                            {
                                if (videoKurumYetkiler.Any())
                                {
                                    for (int i = 0; i < videoKurumYetkiler.Count; i++)
                                    {
                                        var exists = viewModel.SelectedKurumlar.Count(x => x == videoKurumYetkiler[i].KurumId) > 0;

                                        if (!exists)
                                            videoKurumYetkiler[i].Deleted = true;
                                    }
                                }

                                if (viewModel.Model.VideoKurumYetkiler == null)
                                    viewModel.Model.VideoKurumYetkiler = new List<VideoKurumYetki>();

                                for (int i = 0; i < viewModel.SelectedKurumlar.Length; i++)
                                {
                                    var kurumId = viewModel.SelectedKurumlar[i];

                                    if (kurumId == 0)
                                        continue;

                                    if (videoKurumYetkiler.Count(x => x.KurumId == kurumId) == 0)
                                    {
                                        var videoKurumYetki = new VideoKurumYetki
                                        {
                                            VideoId = id,
                                            KurumId = kurumId
                                        };

                                        videoKurumYetkiler.Add(videoKurumYetki);
                                    }
                                }

                                if (videoKurumYetkiler.Count > 0)
                                    viewModel.Model.VideoKurumYetkiler = videoKurumYetkiler;
                            }
                            else if (videoKurumYetkiler.Any())
                            {
                                foreach (var videoKurumYetki in videoKurumYetkiler)
                                {
                                    videoKurumYetki.Deleted = true;
                                }

                                viewModel.Model.VideoKurumYetkiler = videoKurumYetkiler;
                            }

                            #endregion

                            #region VideoSubeYetki

                            if (viewModel.SelectedSubeler != null && viewModel.SelectedSubeler.Length > 0)
                            {
                                if (videoSubeYetkiler.Any())
                                {
                                    for (int i = 0; i < videoSubeYetkiler.Count; i++)
                                    {
                                        var exists = viewModel.SelectedSubeler.Count(x => x == videoSubeYetkiler[i].SubeId) > 0;

                                        if (!exists)
                                            videoSubeYetkiler[i].Deleted = true;
                                    }
                                }

                                if (viewModel.Model.VideoSubeYetkiler == null)
                                    viewModel.Model.VideoSubeYetkiler = new List<VideoSubeYetki>();

                                for (int i = 0; i < viewModel.SelectedSubeler.Length; i++)
                                {
                                    var subeId = viewModel.SelectedSubeler[i];

                                    if (subeId == 0)
                                        continue;

                                    if (videoSubeYetkiler.Count(x => x.SubeId == subeId) == 0)
                                    {
                                        var videoSubeYetki = new VideoSubeYetki
                                        {
                                            VideoId = id,
                                            SubeId = subeId
                                        };

                                        videoSubeYetkiler.Add(videoSubeYetki);
                                    }
                                }

                                if (videoSubeYetkiler.Count > 0)
                                    viewModel.Model.VideoSubeYetkiler = videoSubeYetkiler;
                            }
                            else if (videoSubeYetkiler.Any())
                            {
                                foreach (var videoSubeYetki in videoSubeYetkiler)
                                {
                                    videoSubeYetki.Deleted = true;
                                }

                                viewModel.Model.VideoSubeYetkiler = videoSubeYetkiler;
                            }

                            #endregion

                            #region VideoSinifYetki

                            if (viewModel.SelectedSiniflar != null && viewModel.SelectedSiniflar.Length > 0)
                            {
                                if (videoSinifYetkiler.Any())
                                {
                                    for (int i = 0; i < videoSinifYetkiler.Count; i++)
                                    {
                                        var exists = viewModel.SelectedSiniflar.Count(x => x == videoSinifYetkiler[i].SinifId) > 0;

                                        if (!exists)
                                            videoSinifYetkiler[i].Deleted = true;
                                    }
                                }

                                if (viewModel.Model.VideoSinifYetkiler == null)
                                    viewModel.Model.VideoSinifYetkiler = new List<VideoSinifYetki>();

                                for (int i = 0; i < viewModel.SelectedSiniflar.Length; i++)
                                {
                                    var sinifId = viewModel.SelectedSiniflar[i];

                                    if (sinifId == 0)
                                        continue;

                                    if (videoSinifYetkiler.Count(x => x.SinifId == sinifId) == 0)
                                    {
                                        var videoSinifYetki = new VideoSinifYetki
                                        {
                                            VideoId = id,
                                            SinifId = sinifId
                                        };

                                        videoSinifYetkiler.Add(videoSinifYetki);
                                    }
                                }

                                if (videoSinifYetkiler.Count > 0)
                                    viewModel.Model.VideoSinifYetkiler = videoSinifYetkiler;
                            }
                            else if (videoSinifYetkiler.Any())
                            {
                                foreach (var videoSinifYetki in videoSinifYetkiler)
                                {
                                    videoSinifYetki.Deleted = true;
                                }

                                viewModel.Model.VideoSinifYetkiler = videoSinifYetkiler;
                            }

                            #endregion
                        }

                        viewModel.OperationResult = viewModel.Model.VideoId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        break;
                    }
                case "Iptal":
                    {
                        RedirectToAction("Duzenle");
                        break;
                    }
            }

            viewModel.DersSelectList = selectListHelper.DersSelectList();
            viewModel.KurumSelectList = selectListHelper.KurumSelectList();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IVideoService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new VideoListeleViewModel
            {
                Model = new Video()
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(VideoListeleViewModel viewModel)
        {
            var service = serviceFactory.CreateService<IVideoService>();

            var entityPagedDataSource = service.EntityPagedDataSource(viewModel.EntityPagedDataSourceFilter);

            var list = JsonHelper.ObjectToJsonString(entityPagedDataSource);

            return Content(list, "application/json");
        }
    }
}