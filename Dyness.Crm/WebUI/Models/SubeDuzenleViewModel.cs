using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SubeDuzenleViewModel : IViewModelDuzenle<Sube>
    {
        public string Command { get; set; }

        public List<SelectListItem> KurumSelectList { get; set; }

        public List<SelectListItem> UlkeSelectList { get; set; }
        public List<SelectListItem> SehirSelectList { get; set; }
        public List<SelectListItem> IlceSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public Sube Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Sube> OperationResult { get; set; }
    }
}