using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SmsHesap
    {
        public int SmsHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public int SmsHesapDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kredi")]
        public int Kredi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
            ? OlusturulmaTarihi.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GuncellemeTarihi")]
        [NotMapped]
        public string GuncellemeTarihiFormatted => GuncellemeTarihi != null
            ? GuncellemeTarihi.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "GuncellemeTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime GuncellemeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string SonHareketTarihiFormatted => SonHareketTarihi != null
            ? SonHareketTarihi.Value.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonHareketTarihi")]
        public DateTime? SonHareketTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesapDurum")]
        public string SmsHesapDurumAd => SmsHesapDurum != null ? SmsHesapDurum.SmsHesapDurumAd : string.Empty;

        public virtual Sube Sube { get; set; }

        public virtual SmsHesapDurum SmsHesapDurum { get; set; }

        public virtual List<SmsHesapDosya> SmsHesapDosyalar { get; set; }
    }
}
