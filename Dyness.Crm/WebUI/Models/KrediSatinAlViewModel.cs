using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KrediSatinAlViewModel : IViewModelDuzenle<SmsHesap>
    {
        [Display(ResourceType = typeof(FieldNameResources), Name = "Kredi")]
        public int KrediAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BirimFiyat")]
        public decimal KrediBirimFiyat { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamTutar")]
        public decimal KrediToplamTutar { get; set; }

        public string Command { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        public int SelectedSmsHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        public List<SelectListItem> SmsHesapSelectList { get; set; }

        public SmsHesap Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<SmsHesap> OperationResult { get; set; }
    }
}