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
    public class OptikFormController : Controller
    {

        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public OptikFormController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        private void GetLists(OptikFormDuzenleViewModel viewModel, int? selectedItemId)
        {
            viewModel.OptikFormSelectList = selectListHelper.OptikFormSelectList(new int[] { (int)selectedItemId });

            var model = viewModel.Model;

            var serviceDersGrup = serviceFactory.CreateService<IDersGrupService>();
            var dersGruplar = serviceDersGrup.List(x => x.EtkinMi).ToList();

            if (model?.OptikFormDersGrupBilgiler != null && model.OptikFormDersGrupBilgiler.Count > 0)
            {
                var optikFormDersGrupBilgiler = new List<OptikFormDersGrupBilgi>();

                foreach (var optikFormDersGrupBilgi in model.OptikFormDersGrupBilgiler)
                {
                    if (optikFormDersGrupBilgi.DersGrup != null)
                        optikFormDersGrupBilgiler.Add(optikFormDersGrupBilgi);
                }

                model.OptikFormDersGrupBilgiler = optikFormDersGrupBilgiler;
            }

            if (selectedItemId != null && selectedItemId > 0)
            {
                if (dersGruplar != null && dersGruplar != null)
                {
                    var addList = new List<DersGrup>();

                    for (int i = 0; i < dersGruplar.Count; i++)
                    {
                        if (model.OptikFormDersGrupBilgiler.Count(x => x.DersGrupId == dersGruplar[i].DersGrupId) == 0)
                            addList.Add(dersGruplar[i]);
                    }

                    var addOptikFormDersGrupBilgiler = new List<OptikFormDersGrupBilgi>();

                    foreach (var dersGrup in addList)
                    {
                        if (model.OptikFormDersGrupBilgiler.Count(x => x.DersGrupId == dersGrup.DersGrupId) == 0)
                        {
                            addOptikFormDersGrupBilgiler.Add(new OptikFormDersGrupBilgi
                            {
                                DersGrup = dersGrup,
                                DersGrupId = dersGrup.DersGrupId,
                                OptikFormId = viewModel.Model.OptikFormId
                            });
                        }
                    }

                    if (addOptikFormDersGrupBilgiler != null && addOptikFormDersGrupBilgiler.Count > 0)
                        model.OptikFormDersGrupBilgiler.AddRange(addOptikFormDersGrupBilgiler);

                    model.OptikFormDersGrupBilgiler = model.OptikFormDersGrupBilgiler.OrderBy(x => x.DersGrup.DersGrupAd).ToList();

                }
            }
            else
            {
                viewModel.Model = new OptikForm
                {
                    OptikFormDersGrupBilgiler = new List<OptikFormDersGrupBilgi>()
                };

                foreach (var dersGrup in dersGruplar)
                {
                    viewModel.Model.OptikFormDersGrupBilgiler.Add(new OptikFormDersGrupBilgi
                    {
                        DersGrup = dersGrup,
                        DersGrupId = dersGrup.DersGrupId,
                        OptikForm = viewModel.Model
                    });
                }
            }
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ActionResult Duzenle(int? id)
        {
            var viewModel = new OptikFormDuzenleViewModel();

            if (TempData["OperationResult"] != null)
            {
                viewModel.OperationResult = (EntityOperationResult<OptikForm>)TempData["OperationResult"];
                TempData["OperationResult"] = null;
            }

            if (id != null && id > 0)
            {
                var service = serviceFactory.CreateService<IOptikFormService>();
                var model = service.Get(x =>
                    x.OptikFormId == id,
                    y => y.OptikFormDersGrupBilgiler.Select(z => z.DersGrup));

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
        public ActionResult Duzenle(OptikFormDuzenleViewModel viewModel)
        {
            ModelState.Clear();

            if ((viewModel.Model.AdSoyadBasla == null || viewModel.Model.AdSoyadAdet == null) && 
                (viewModel.Model.AdBasla == null || viewModel.Model.AdAdet == null || viewModel.Model.SoyadBasla == null || viewModel.Model.SoyadAdet == null))
            {
                GetLists(viewModel, viewModel.Model.OptikFormId);

                TempData["OperationResult"] = new EntityOperationResult<OptikForm>
                {
                    Status = false,
                    MessageInfos = new List<MessageInfo>() { new MessageInfo { Message = Resources.LangResources.OptikFormAdSoyadBosOlamaz, MessageInfoType = MessageInfoType.Error } }
                };

                return View(viewModel);
            }

            var service = serviceFactory.CreateService<IOptikFormService>();

            switch (viewModel.Command)
            {
                case "Duzenle":
                default:
                    {
                        viewModel.OperationResult = viewModel.Model.OptikFormId == 0
                            ? service.Add(viewModel.Model)
                            : service.Update(viewModel.Model);

                        if (viewModel.OperationResult.Status && viewModel.Model.OptikFormId > 0)
                        {
                            viewModel.Model = service.Get(x =>
                                x.OptikFormId == viewModel.Model.OptikFormId,
                                y => y.OptikFormDersGrupBilgiler.Select(z => z.DersGrup));
                        }

                        break;
                    }
                case "Iptal":
                    {
                        return viewModel.Model.OptikFormId > 0
                            ? RedirectToAction("Duzenle", new { id = viewModel.Model.OptikFormId })
                            : RedirectToAction("Duzenle");
                    }
            }

            GetLists(viewModel, viewModel.Model.OptikFormId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckAuthorizedActionFilter]
        public ActionResult Sil(int id)
        {
            ModelState.Clear();

            var service = serviceFactory.CreateService<IOptikFormService>();

            var operationResult = service.DeleteById(id);

            TempData["OperationResult"] = operationResult;

            return RedirectToAction("Duzenle");
        }
    }
}