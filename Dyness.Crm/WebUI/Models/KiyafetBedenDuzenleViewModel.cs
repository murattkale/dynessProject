using System.Collections.Generic;
using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KiyafetBedenDuzenleViewModel : IViewModelDuzenle<KiyafetBeden>
    {
        public string Command { get; set; }

        public List<SelectListItem> KiyafetBedenSelectList { get; set; }

        public KiyafetBeden Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<KiyafetBeden> OperationResult { get; set; }
    }
}