using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SinavTurDuzenleViewModel : IViewModelDuzenle<SinavTur>
    {
        public string Command { get; set; }

        public List<SelectListItem> SinavTurSelectList { get; set; }

        public SinavTur Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<SinavTur> OperationResult { get; set; }
    }
}