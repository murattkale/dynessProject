using System.Collections.Generic;
using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using WebUI.Models.Abstract;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class PersonelPuantajGunlukDurumDuzenleViewModel : IViewModelDuzenle<PersonelPuantajGunlukDurum>
    {
        public string Command { get; set; }

        public List<SelectListItem> PersonelPuantajGunlukDurumSelectList { get; set; }

        public PersonelPuantajGunlukDurum Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<PersonelPuantajGunlukDurum> OperationResult { get; set; }
    }
}