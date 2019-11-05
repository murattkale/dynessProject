using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Kullanici 
    {
        [Key, ForeignKey("Personel")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KullaniciAd")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KullaniciAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SifreTekrar")]
        [Compare("Sifre", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsCompare")]
        [NotMapped]
        public string SifreTekrar { get; set; }
       
        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string Sifre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGirisTarihi")]
        [NotMapped]
        public string SonGirisTarihiFormatted => SonGirisTarihi != null
            ? SonGirisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGirisTarihi")]
        [DataType(DataType.Date)]
        public DateTime? SonGirisTarihi { get; set; }

        public Personel Personel { get; set; }
    }
}
