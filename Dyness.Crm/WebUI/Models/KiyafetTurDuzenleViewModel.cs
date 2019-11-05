using System.Collections.Generic;
using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KiyafetTurDuzenleViewModel : IViewModelDuzenle<KiyafetTur>
    {
        public string Command { get; set; }

        public List<SelectListItem> KiyafetTurSelectList { get; set; }

        public KiyafetTur Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<KiyafetTur> OperationResult { get; set; }
    }
}