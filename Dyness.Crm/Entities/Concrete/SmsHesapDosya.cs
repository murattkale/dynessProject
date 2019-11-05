using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SmsHesapDosya
    {
        public int SmsHesapDosyaId { get; set; }

        public int SmsHesapId { get; set; }

        [NotMapped]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dosya")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DosyaAd { get; set; }

        public string DosyaYol => !string.IsNullOrEmpty(DosyaAd)
            ? $"{AyarlarService.Get().SmsHesapDosyaYol}{(SmsHesap != null ? SmsHesap.SubeId : SubeId)}/{DosyaAd}"
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "YuklenmeTarihi")]
        public DateTime? YuklenmeTarihi { get; set; }

        public virtual SmsHesap SmsHesap { get; set; }
    }
}
