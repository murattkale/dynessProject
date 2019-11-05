using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Concrete
{
    public class OgrenciSozlesme
    {
        public int OgrenciSozlesmeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ogrenci")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int OgrenciId { get; set; }

        public int EkleyenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesmeTur")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int OgrenciSozlesmeTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezon")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SezonId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        public int? BransId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        public int? SinifSeviyeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        public int? SinifId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OkulTur")]
        public int? OkulTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Servis")]
        public int? ServisId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EhliyetTur")]
        public int? EhliyetTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OzelDersDurum")]
        public int? OzelDersDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinlik")]
        public int? EtkinlikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DanismanPersonel")]
        public int? DanismanPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GorusenPersonel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? GorusenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumaGetiren")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? KurumaGetirenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "FaturaBilgi")]
        public int? FaturaBilgiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Referans")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Referans { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Not { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YemekVar")]
        public bool YemekDahilMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersAdeti")]
        public int? DersAdeti { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersBirimFiyat")]
        public int? DersBirimFiyat { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimTutar")]
        public string EgitimTutarFormatted => EgitimTutar != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, EgitimTutar)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimTutar")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal? EgitimTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YayinTutar")]
        public string YayinTutarFormatted => YayinTutar != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, YayinTutar)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "YayinTutar")]
        public decimal? YayinTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ServisTutar")]
        public string ServisTutarFormatted => ServisTutar != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, ServisTutar)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ServisTutar")]
        public decimal? ServisTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTutari")]
        public string KiyafetTutarFormatted => KiyafetTutar != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, KiyafetTutar)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTutari")]
        public decimal? KiyafetTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YemekTutar")]
        public string YemekTutarFormatted => YemekTutar != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, YemekTutar)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "YemekTutar")]
        public decimal? YemekTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamUcret")]
        public string ToplamUcretFormatted => ToplamUcret != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture,ToplamUcret)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamUcret")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal? ToplamUcret { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamOdenen")]
        public string ToplamOdenenFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamOdenen);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamOdenen")]
        public decimal? ToplamOdenen { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamKalan")]
        public string ToplamKalanFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamKalan);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamKalan")]
        [NotMapped]
        public decimal? ToplamKalan => (ToplamUcret ?? 0) - (ToplamOdenen ?? 0);

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dosya")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DosyaAd { get; set; }

        public string DosyaYol => !string.IsNullOrEmpty(DosyaAd)
            ? $"{AyarlarService.Get().OgrenciSozlesmeDosyaYol}{DosyaAd}"
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
            ? OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [DataType(DataType.Date)]
        public DateTime? OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ogrenci")]
        [NotMapped]
        public string OgrenciAdSoyad => Ogrenci != null ? Ogrenci.AdSoyad : string.Empty;

        public virtual Ogrenci Ogrenci { get; set; }

        [ForeignKey("EkleyenPersonelId")]
        public virtual Personel EkleyenPersonel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesmeTur")]
        [NotMapped]
        public string OgrenciSozlesmeTurAd => OgrenciSozlesmeTur != null ? OgrenciSozlesmeTur.OgrenciSozlesmeTurAd : string.Empty;

        public virtual OgrenciSozlesmeTur OgrenciSozlesmeTur { get; set; }

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

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        [NotMapped]
        public string SinifSeviyeAd => SinifSeviye != null ? SinifSeviye.SinifSeviyeAd : string.Empty;

        public virtual SinifSeviye SinifSeviye { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        [NotMapped]
        public string SinifAd => Sinif != null ? Sinif.SinifAd : string.Empty;

        public virtual Sinif Sinif { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OkulTur")]
        [NotMapped]
        public string OkulTurAd => OkulTur != null ? OkulTur.OkulTurAd : string.Empty;

        public virtual OkulTur OkulTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Servis")]
        [NotMapped]
        public string ServisAd => Servis != null ? Servis.ServisPlaka : string.Empty;

        public virtual Servis Servis { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EhliyetTur")]
        [NotMapped]
        public string EhliyetTurAd => EhliyetTur != null ? EhliyetTur.EhliyetTurAd : string.Empty;

        public virtual EhliyetTur EhliyetTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OzelDersDurum")]
        [NotMapped]
        public string OzelDersDurumAd => OzelDersDurum != null ? OzelDersDurum.OzelDersDurumAd : string.Empty;

        public virtual OzelDersDurum OzelDersDurum { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinlik")]
        [NotMapped]
        public string EtkinlikAd => Etkinlik != null ? Etkinlik.EtkinlikAd : string.Empty;

        public virtual Etkinlik Etkinlik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DanismanPersonel")]
        [NotMapped]
        public string DanismanPersonelAdSoyad => DanismanPersonel != null ? DanismanPersonel.AdSoyad : string.Empty;

        [ForeignKey("DanismanPersonelId")]
        public virtual Personel DanismanPersonel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GorusenPersonel")]
        [NotMapped]
        public string GorusenPersonelAdSoyad => GorusenPersonel != null ? GorusenPersonel.AdSoyad : string.Empty;

        [ForeignKey("GorusenPersonelId")]
        public virtual Personel GorusenPersonel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumaGetiren")]
        [NotMapped]
        public string KurumaGetirenPersonelAdSoyad => KurumaGetirenPersonel != null ? KurumaGetirenPersonel.AdSoyad : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumaGetiren")]
        [ForeignKey("KurumaGetirenPersonelId")]
        public virtual Personel KurumaGetirenPersonel { get; set; }

        public virtual FaturaBilgi FaturaBilgi { get; set; }

        public virtual OgrenciSozlesmeOdemeBilgi OgrenciSozlesmeOdemeBilgi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciYayinlar")]
        public virtual List<OgrenciSozlesmeYayin> OgrenciSozlesmeYayinlar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciKiyafetler")]
        public virtual List<OgrenciSozlesmeKiyafetDurum> OgrenciSozlesmeKiyafetDurumlar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AlacagiDersler")]
        public virtual List<OgrenciSozlesmeDersSecim> OgrenciSozlesmeDersSecimler { get; set; }

        public virtual List<OgrenciSozlesmeHesapHareket> OgrenciSozlesmeHesapHareketler { get; set; }

        public CultureInfo GecerliParaBirimCulture => Sube?.ParaBirim != null
            ? CultureInfo.GetCultureInfo(Sube.ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
