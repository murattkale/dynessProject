using Core.Entities.Dto;
using Newtonsoft.Json;
using Services.Abstract;
using Services.DependencyResolvers;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Models.Abstract;

namespace WebUI.Controllers
{
    public class SmsHesapHareketController : Controller
    {
        private IServiceFactory serviceFactory;

        public SmsHesapHareketController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [HttpGet]
        [CheckAuthorizedActionFilter]
        public ViewResult Listele()
        {
            var viewModel = new SmsHesapHareketListeleViewModel
            {
                Model = new SmsHesapHareketDto(),
                EntityPagedDataSource = new EntityPagedDataSource<SmsHesapHareketDto>(),
                Search = new Search(),
            };

            return View(viewModel);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult Listele(HesapHareketListeleViewModel viewModel)
        {
            var paremeters = new List<Parameter>
            {
                new Parameter("IlkTarih", viewModel.IlkTarih),
                new Parameter("SonTarih", viewModel.SonTarih)
            };

            var service = serviceFactory.CreateService<ISmsHesapHareketService>();

            var pagedDataSource = service.DtoPagedDataSource(viewModel.EntityPagedDataSourceFilter, paremeters);

            return Content(JsonConvert.SerializeObject(pagedDataSource), "application/json");
        }
    }
}