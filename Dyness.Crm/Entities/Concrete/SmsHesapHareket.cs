using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SmsHesapHareket
    {
        public int SmsHesapHareketId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesapHareketTip")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SmsHesapHareketTipId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SmsHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kredi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Kredi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Telefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [NotMapped]
        public string Baslik => SmsHesap != null ? SmsHesap.Baslik : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHareketTipAd")]
        [NotMapped]
        public string SmsHesapHareketTipAd => SmsHesapHareketTip != null ? SmsHesapHareketTip.SmsHesapHareketTipAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        [NotMapped]
        public string HareketTarihiFormatted => HareketTarihi != null
            ? HareketTarihi.Date.ToString(AyarlarService.Get().GecerliTarihSaatFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime HareketTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesapHareketTip")]
        public virtual SmsHesapHareketTip SmsHesapHareketTip { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        public virtual SmsHesap SmsHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        public virtual Personel Personel { get; set; }
    }
}
