using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class BankaHesap
    {
        [Key, ForeignKey("Hesap")]
        public int BankaHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Banka")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int BankaId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public int ParaBirimId { get; set; }

        [ForeignKey("UstHesap")]
        [Display(ResourceType = typeof(FieldNameResources), Name = "BagliHesap")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int UstHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Aciklama")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Aciklama { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(FieldNameResources), Name = "Aciklama")]
        public string AciklamaFormatted => Banka != null ? $"{Banka.BankaAd} - {Aciklama}" : Aciklama;

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapSahibi")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string HesapSahibi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Sube { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SubeNo")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SubeNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapNo")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string HesapNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Iban")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Iban { get; set; }

        public int Sira { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [NotMapped]
        public string UstHesapBaslik => UstHesap != null ? UstHesap.HesapBaslik : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        [NotMapped]
        public string HesapBaslik => Hesap != null ? Hesap.HesapBaslik : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Banka")]
        [NotMapped]
        public string BankaAd => Banka != null ? Banka.BankaAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        [NotMapped]
        public string ParaBirimAd => ParaBirim != null ? ParaBirim.ParaBirimAd : string.Empty;

        public virtual Hesap Hesap { get; set; }

        [ForeignKey("UstHesapId")]
        public virtual Hesap UstHesap { get; set; }

        public virtual Banka Banka { get; set; }

        public virtual ParaBirim ParaBirim { get; set; }
    }
}
