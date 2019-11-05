using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SmsHesapDuzenleViewModel : IViewModelDuzenle<SmsHesap>
    {
        public string Command { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SmsHesapDurumSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dosyalar")]
        public HttpPostedFileBase[] PostedFilesDosyalar { get; set; }

        public SmsHesap Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<SmsHesap> OperationResult { get; set; }
    }
}