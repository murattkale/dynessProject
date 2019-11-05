using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;


namespace WebUI.Models
{
    public class KonuDuzenleViewModel : IViewModelDuzenle<Konu>
    {
        public string Command { get; set; }

        public List<SelectListItem> KonuSelectList { get; set; }

        public List<SelectListItem> DersSelectList { get; set; }

        public Konu Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Konu> OperationResult { get; set; }
    }
}