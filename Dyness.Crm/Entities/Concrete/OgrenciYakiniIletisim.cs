using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciYakiniIletisim 
    {
        public int OgrenciYakiniIletisimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Soyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [NotMapped]
        public string AdSoyad => $"{Ad} {Soyad}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefonu")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string CepTelefon1 { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefonu")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string CepTelefon2 { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EvTelefonu")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string EvTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IsTelefonu")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string IsTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Eposta")]
        [MaxLength(254, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsEmail")]
        public string Eposta { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EvAdresi")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string EvAdres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IsAdresi")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string IsAdres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [NotMapped]
        public string DogumTarihiFormatted => DogumTarihi != null
            ? DogumTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [DataType(DataType.Date)]
        public DateTime? DogumTarihi { get; set; }
    }
}
