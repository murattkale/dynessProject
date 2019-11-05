using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class ServisDuzenleViewModel : IViewModelDuzenle<Servis>
    {
        public string Command { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public Servis Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Servis> OperationResult { get; set; }
    }
}