using Services.DependencyResolvers;
using System.Web.Mvc;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class IlceSehirUlkeController : Controller
    {
        private IServiceFactory serviceFactory;
        private SelectListHelper selectListHelper;

        public IlceSehirUlkeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            selectListHelper = new SelectListHelper(serviceFactory);
        }

        [CheckAuthorizedActionFilter]
        public ContentResult SehirListele(int? ulkeId)
        {
            if (ulkeId == null) ulkeId = 0;

            var selectList = selectListHelper.SehirSelectList((int)ulkeId);

            var selectItem = new SelectListItem
            {
                Selected = false,
                Text =  Resources.LangResources.Seciniz,
                Value = ""
            };

            selectList.Insert(0, selectItem);

            var jSonlist = JsonHelper.ObjectToJsonString(selectList);

            return Content(jSonlist, "application/json");
        }

        [CheckAuthorizedActionFilter]
        public ContentResult IlceListele(int? sehirId)
        {
            if (sehirId == null) sehirId = 0;

            var selectList = selectListHelper.IlceSelectList((int)sehirId);

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
    }
}