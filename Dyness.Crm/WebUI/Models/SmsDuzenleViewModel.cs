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
    public class SmsDuzenleViewModel : IViewModelDuzenle<Sms>
    {
        public string Command { get; set; }

        public List<SelectListItem> SmsHesapSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        public int SelectedSmsHesapId { get; set; }

        public List<SelectListItem> GonderilenGrupSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Grup")]
        public int? SelectedGonderilenGrup { get; set; }

        public List<SelectListItem> OgrenciGonderilenGrupSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DahilEt")]
        public int[] SelectedOgrenciGonderilenGrup { get; set; }

        public List<SelectListItem> SmsSablonSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsMetinSablon")]
        public int? SelectedSmsSablonId { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Subeler")]
        public int[] SelectedSubeler { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezon")]
        public int[] SelectedSezonlar { get; set; }

        public List<SelectListItem> SinavSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinav")]
        public int? SelectedSinavId { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Siniflar")]
        public int[] SelectedSiniflar { get; set; }

        public List<SelectListItem> PersonelGrupSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelGruplar")]
        public int[] SelectedPersonelGruplar { get; set; }

        public List<SmsTelefonBilgiDto> SmsTelefonBilgiler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TelefonNumaralar")]
        public string TelefonNumaralar { get; set; }

        public Sms Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Sms> OperationResult { get; set; }
    }
}