using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinavSube
    {
        public int SinavSubeId { get; set; }

        public int SinavId { get; set; }

        public int SubeId { get; set; }

        public int? DosyaEkleyenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dosya")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DosyaAd { get; set; }

        public string DosyaYol => !string.IsNullOrEmpty(DosyaAd) ? $"{ AyarlarService.Get().SubeSinavDosyaYol}{SinavId}/{DosyaAd}" : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [NotMapped]
        public string DosyaYuklenmeTarihFormatted => DosyaYuklenmeTarih != null
            ? DosyaYuklenmeTarih.Value.Date.ToString(AyarlarService.Get().GecerliTarihSaatFormati)
            : string.Empty;

        public DateTime? DosyaYuklenmeTarih { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => DegelendirildiMi ? FieldNameResources.Degerlendirildi : FieldNameResources.HenuzDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DegelendirildiMi")]
        public bool DegelendirildiMi { get; set; }

        public virtual Sinav Sinav { get; set; }

        public virtual Sube Sube { get; set; }

        [ForeignKey("DosyaEkleyenPersonelId")]
        public Personel DosyaEkleyenPersonel { get; set; }
    }
}
