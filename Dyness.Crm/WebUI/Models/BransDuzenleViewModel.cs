using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class BransDuzenleViewModel : IViewModelDuzenle<Brans>
    {
        public string Command { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        public Brans Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Brans> OperationResult { get; set; }
    }
}