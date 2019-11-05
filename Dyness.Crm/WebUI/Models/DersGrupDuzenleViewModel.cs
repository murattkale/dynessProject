using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class DersGrupDuzenleViewModel : IViewModelDuzenle<DersGrup>
    {
        public string Command { get; set; }

        public List<SelectListItem> DersGrupSelectList { get; set; }

        public DersGrup Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<DersGrup> OperationResult { get; set; }
    }
}