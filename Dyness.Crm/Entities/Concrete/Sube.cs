using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Sube 
    {
        [Key]
        [ForeignKey("Hesap")]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sehir")]
        public int? SehirId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SubeAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SubeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kod")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Kod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yetkili")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Yetkili { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Adres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Adres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Telefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Eposta")]
        [MaxLength(254, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Eposta { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Not { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Harita")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Harita { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SenetDuzenlemeYerBilgisi")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SenetDuzenlemeYerBilgisi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "MinimumPesinatOrani")]
        [Range(0, 100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRange")]
        public int? MinimumPesinatOrani { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "MaksimumTaksitAdeti")]
        public int? MaksimumTaksitAdeti { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Egitim")]
        [NotMapped]
        public string UzaktanEgitim => UzaktanEgitimMi ? "Uzaktan Eğitim" : "Örgün Eğitim";

        [Display(ResourceType = typeof(FieldNameResources), Name = "UzaktanEgitimMi")]
        public bool UzaktanEgitimMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        [NotMapped]
        public string HesapBaslik => Hesap != null ? Hesap.HesapBaslik : string.Empty;

        public virtual Hesap Hesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        [NotMapped]
        public string KurumAd => Kurum != null ? Kurum.KurumAd : string.Empty;

        public virtual Kurum Kurum { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sehir")]
        [NotMapped]
        public string SehirAd => Sehir != null ? Sehir.SehirAd : string.Empty;

        public virtual Sehir Sehir { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        [NotMapped]
        public string ParaBirimAd => ParaBirim != null ? ParaBirim.ParaBirimAd : string.Empty;

        public virtual ParaBirim ParaBirim { get; set; } 
    }
}
