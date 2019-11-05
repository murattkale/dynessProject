using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class EskiKayitVeriGuncelleViewModel
    {
        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public int SubeId { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dosya")]
        public HttpPostedFileBase PostedFileVeri { get; set; }

        public EskiKayit Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<EskiKayit> OperationResult { get; set; }
    }
}