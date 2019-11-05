using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SinifDuzenleViewModel : IViewModelDuzenle<Sinif>
    {
        public string Command { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        public List<SelectListItem> SinifTurSelectList { get; set; }

        public List<SelectListItem> SinifSeviyeSelectList { get; set; }

        public List<SelectListItem> SinifSeansSelectList { get; set; }

        public List<SelectListItem> DerslikSelectList { get; set; }

        public List<SelectListItem> PersonelSelectList { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        public SinifTurDuzenleViewModel SinifTurDuzenleViewModel { get; set; }

        public SinifSeviyeDuzenleViewModel SinifSeviyeDuzenleViewModel { get; set; }

        public SinifSeansDuzenleViewModel SinifSeansDuzenleViewModel { get; set; }

        public DerslikDuzenleViewModel DerslikDuzenleViewModel { get; set; }

        public Sinif Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Sinif> OperationResult { get; set; }
    }
}