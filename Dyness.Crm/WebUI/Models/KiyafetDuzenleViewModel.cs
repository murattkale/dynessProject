using System.Collections.Generic;
using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KiyafetDuzenleViewModel : IViewModelDuzenle<Kiyafet>
    {
        public string Command { get; set; }

        public List<SelectListItem> KiyafetTurSelectList { get; set; }

        public List<SelectListItem> KiyafetBedenSelectList { get; set; }

        public Kiyafet Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Kiyafet> OperationResult { get; set; }
    }
}