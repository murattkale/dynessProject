using Core.General;
using Core.Properties;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Etkinlik 
    {
        public int EtkinlikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SorumluPersonel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? SorumluPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string EtkinlikAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kontenjan")]
        public int? Kontenjan { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Katilim")]
        [DefaultValue(0)]
        public int? KatilimAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KontenjanDurumu")]
        [NotMapped]
        public string KontenjanDurumu => KontenjanVarmi ? ((Kontenjan ?? 0) - (KatilimAdet ?? 0)).ToString() : FieldNameResources.Dolu;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KapasiteDurumu")]
        [NotMapped]
        public bool KontenjanVarmi => !KontenjanKontrolEdilsinMi || (Kontenjan != null && (Kontenjan ?? 0) - (KatilimAdet ?? 0) > 0);

        [Display(ResourceType = typeof(FieldNameResources), Name = "KontenjanKontrolEdilsinMi")]
        public bool KontenjanKontrolEdilsinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
            ? OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [DataType(DataType.Date)]
        public DateTime? OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikSonBasvuruTarihi")]
        [NotMapped]
        public string EtkinlikSonBasvuruTarihiFormatted => EtkinlikSonBasvuruTarihi != null
           ? EtkinlikSonBasvuruTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikSonBasvuruTarihi")]
        [DataType(DataType.Date)]
        public DateTime? EtkinlikSonBasvuruTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikBaslangicTarihi")]
        [NotMapped]
        public string EtkinlikBaslangicTarihiFormatted => EtkinlikBaslangicTarihi != null
            ? EtkinlikBaslangicTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikBaslangicTarihi")]
        [DataType(DataType.Date)]
        public DateTime? EtkinlikBaslangicTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikBitisTarihi")]
        [NotMapped]
        public string EtkinlikBitisTarihiFormatted => EtkinlikBitisTarihi != null
           ? EtkinlikBitisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "EtkinlikBitisTarihi")]
        [DataType(DataType.Date)]
        public DateTime? EtkinlikBitisTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SorumluPersonel")]
        [NotMapped]
        public string SorumluPersonelAdSoyad => SorumluPersonel != null ? SorumluPersonel.AdSoyad : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SorumluPersonel")]
        [ForeignKey("SorumluPersonelId")]
        public virtual Personel SorumluPersonel { get; set; }
    }
}
