using Core.General;
using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Concrete
{
    public class Sinif 
    {
        public int SinifId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezon")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SezonId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        public int? BransId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifTur")]
        public int? SinifTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        public int? SinifSeviyeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeans")]
        public int? SinifSeansId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Derslik")]
        public int? DerslikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimKocu")]
        public int? PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SinifAd { get; set; }

        public string BransSinifAd => Brans != null
            ? $"{Brans.BransAd} - {SinifAd}"
            : $"Genel - {SinifAd}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamDersSaati")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? ToplamDersSaat { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifKapasite")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? SinifKapasite { get; set; }

        public string KayitUcretiFormatted => KayitUcreti != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, KayitUcreti)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KayitUcreti")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal? KayitUcreti { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimSüre")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? EgitimSüre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [NotMapped]
        public string SubeAd => Sube != null ? Sube.SubeAd : string.Empty;

        public virtual Sube Sube { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezon")]
        [NotMapped]
        public string SezonAd => Sezon != null ? Sezon.SezonAd : string.Empty;

        public virtual Sezon Sezon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        [NotMapped]
        public string BransAd => Brans != null ? Brans.BransAd : string.Empty;

        public virtual Brans Brans { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifTur")]
        [NotMapped]
        public string SinifTurAd => SinifTur != null ? SinifTur.SinifTurAd : string.Empty;

        public virtual SinifTur SinifTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        [NotMapped]
        public string SinifSeviyeAd => SinifSeviye != null ? SinifSeviye.SinifSeviyeAd : string.Empty;

        public virtual SinifSeviye SinifSeviye { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeans")]
        [NotMapped]
        public string SinifSeansAd => SinifSeans != null ? SinifSeans.SinifSeansAd : string.Empty;

        public virtual SinifSeans SinifSeans { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Derslik")]
        [NotMapped]
        public string DerslikAd => Derslik != null ? Derslik.DerslikAd : string.Empty;

        public virtual Derslik Derslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimKocu")]
        [NotMapped]
        public string EgitimKocuPersonelAd => Personel != null ? Personel.AdSoyad : string.Empty;

        public virtual Personel Personel { get; set; }

        public CultureInfo GecerliParaBirimCulture => Sube?.ParaBirim != null
            ? CultureInfo.GetCultureInfo(Sube.ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
