using Core.General;
using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Kurum 
    {
        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KurumAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Adres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Adres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Telefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Eposta")]
        [MaxLength(254, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsEmail")]
        public string Eposta { get; set; }

        [NotMapped]
        public string LogoYol => !string.IsNullOrEmpty(LogoDosyaAd)
         ? $"{ AyarlarService.Get().KurumLogoYol}{LogoDosyaAd}"
         : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Logo")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string LogoDosyaAd { get; set; }

        [NotMapped]
        public string ArkaPlanYol => !string.IsNullOrEmpty(ArkaPlanDosyaAd)
           ? $"{AyarlarService.Get().KurumArkaPlanYol}{ArkaPlanDosyaAd}"
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ArkaPlan")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string ArkaPlanDosyaAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SozlesmedeLogoKullanilsinMi")]
        public bool SozlesmedeLogoKullanilsinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SozlesmedeArkaPlanGorselKullanilsinMi")]
        public bool SozlesmedeArkaPlanGorselKullanilsinMi { get; set; }
    }
}
