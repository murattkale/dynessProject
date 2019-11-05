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
    public class DersController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public DersController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(DersDuzenleViewModel viewModel, int selectedItemId)
        {
            viewModel.DersSelectList = selectListHelper.DersSelectList(false);
            viewModel.DersGrupSelectList = selectListHelper.DersGrupSelectList();
            viewModel.BransSelectList = selectListHelper.BransSelectList();
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new DersDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<Ders>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IDersService>();
                var model = service.Get(x => x.DersId == id, y => y.BransDersler);

                if (model == null)
                    return Redirect("/Error/NotFound");

                viewModel.Model = model;

                viewModel.SelectedBranslar = model.BransDersler.Select(x => x.BransId).ToArray();
            }
            else
            {
                viewModel.Model = new Ders
                {
                    BransDersler = new List<BransDers>()
                };
            }

            GetLists(viewModel, id ?? 0);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(DersDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IDersService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        if (viewModel.Model.DersId > 0)
                        {
                            if (viewModel.Model.BransDersler == null)
                                viewModel.Model.BransDersler = new List<BransDers>();

                            var bransDersler = service.Get(x => x.DersId == viewModel.Model.DersId, y => y.BransDersler).BransDersler;

                            foreach (var bransDers in bransDersler)
                            {
                                bransDers.Silinecek = viewModel.SelectedBranslar == null ||
                                    viewModel.SelectedBranslar.Length == 0 ||
                                    viewModel.SelectedBranslar.Count(x => x == bransDers.BransId) == 0;

                                if (bransDers.Silinecek)
                                    viewModel.Model.BransDersler.Add(bransDers);
                            }

                            if (viewModel.SelectedBranslar!= null && viewModel.SelectedBranslar.Any())
                            {
                                foreach (var selectedBrans in viewModel.SelectedBranslar)
                                {
                                    if (bransDersler.Count(x => x.BransId == selectedBrans) > 0)
                                        continue;

                                    viewModel.Model.BransDersler.Add(new BransDers
                                    {
                                        BransId = selectedBrans,
                                        DersId = viewModel.Model.DersId
                                    });
                                }
                            }
                        }

                        if (viewModel.SelectedBranslar != null && viewModel.SelectedBranslar.Any() && viewModel.Model.DersId == 0)
                        {
                            if (viewModel.Model.BransDersler == null)
                                viewModel.Model.BransDersler = new List<BransDers>();

                            foreach (var selectedBrans in viewModel.SelectedBranslar)
                            {
                                viewModel.Model.BransDersler.Add(new BransDers
                                {
                                    BransId = selectedBrans,
                                    Ders = viewModel.Model
                                });
                            }
                        }

                        viewModel.OperationResult = viewModel.Model.DersId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        viewModel.Model = viewModel.OperationResult.Model;

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.DersId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.DersId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.DersId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IDersService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult BransDersListele(int? bransId)
        {
            var selectList = selectListHelper.DersSelectList(bransId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text = Resources.LangResources.Seciniz,
                Value = "0"
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }
    }
}