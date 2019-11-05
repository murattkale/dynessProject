using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class BankaDuzenleViewModel : IViewModelDuzenle<Banka>
    {
        public string Command { get; set; }

        public List<SelectListItem> BankaSelectList { get; set; }

        public Banka Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Banka> OperationResult { get; set; }
    }
}