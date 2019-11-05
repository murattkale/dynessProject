using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities.Dto
{
    public class OgrenciSozlesmeDto
    {
        public int OgrenciSozlesmeId { get; set; }

        public int OgrenciId { get; set; }

        public int OgrenciSozlesmeTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesmeTurAd")]
        public string OgrenciSozlesmeTurAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SubeAd")]
        public string SubeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SezonAd")]
        public string SezonAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifAd")]
        public string SinifAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        public string AdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        public string OgrenciNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefonu")]
        public string CepTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Eposta")]
        public string Eposta { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gorsel")]
        public string GorselDosyaAd { get; set; }

        public string GorselYol => !string.IsNullOrEmpty(GorselDosyaAd)
            ? $"{ AyarlarService.Get().PersonelGorselYol}{GorselDosyaAd}"
            : $"{ AyarlarService.Get().PersonelGorselYol}{(KadinMi ? AyarlarService.Get().PersonelKadinDefaultGorselYol : AyarlarService.Get().PersonelErkekDefaultGorselYol)}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public string Cinsiyet => KadinMi ? FieldNameResources.Kadin : FieldNameResources.Erkek;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public bool KadinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamUcret")]
        public string ToplamUcretFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamUcret);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamUcret")]
        public decimal? ToplamUcret { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamOdenen")]
        public string ToplamOdenenFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamOdenen);
            
        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamOdenen")]
        public decimal? ToplamOdenen { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamKalan")]
        public string ToplamKalanFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamKalan);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamKalan")]
        public decimal? ToplamKalan => (ToplamUcret ?? 0) - (ToplamOdenen ?? 0);

        public string KulturKod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        public DateTime? OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
           ? OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        public CultureInfo GecerliParaBirimCulture => !string.IsNullOrEmpty(KulturKod)
            ? CultureInfo.GetCultureInfo(KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
