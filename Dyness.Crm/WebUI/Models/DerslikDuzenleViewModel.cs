using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class DerslikDuzenleViewModel : IViewModelDuzenle<Derslik>
    {
        public List<SelectListItem> DerslikSelectList { get; set; }

        public string Command { get; set; }

        public Derslik Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Derslik> OperationResult { get; set; }
    }
}