using System.Linq;
using Core.Entities.Dto;
using Entities.Concrete;
using System.Web.Mvc;
using WebUI.Models.Abstract;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class PersonelGrupDuzenleViewModel : IViewModelDuzenle<PersonelGrup>
    {
        public string Command { get; set; }

        public List<SelectListItem> PersonelGrupSelectList { get; set; }

        public PersonelGrup Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<PersonelGrup> OperationResult { get; set; }
    }
}