using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities.Dto
{
    public class HesapDto
    {
        public int HesapId { get; set; }

        public int? UstHesapId { get; set; }

        public int ParaBirimId { get; set; }

        public int HesapTurGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ay")]
        public int Ay { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yil")]
        public int Yil { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Donem")]
        public string Donem => Ay == 0
            ? $"{FieldNameResources.Toplam} : {Yil}"
            : $"{(Ay < 10 ? "0" + Ay.ToString() : Ay.ToString())}/{Yil}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        public string HesapBaslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BagliHesap")]
        public string UstHesapBaslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public string SubeBaslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapTur")]
        public string HesapTurAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapTurGrup")]
        public string HesapTurGrupAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public string ParaBirimAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        public string ToplamBorcFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamBorc);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        public decimal ToplamBorc { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamAlacak")]
        public string ToplamAlacakFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamAlacak);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamAlacak")]
        public decimal ToplamAlacak { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Toplam")]
        public string ToplamFormatted => $"{(Toplam > 0 ? "+" : Toplam < 0 ? "-" : string.Empty)} {AyarlarService.ParaFormat(GecerliParaBirimCulture, Math.Abs(Toplam))}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Toplam")]
        public decimal Toplam => ToplamAlacak - ToplamBorc;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Toplam")]
        public decimal Bakiye => ToplamBorc - ToplamAlacak;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Toplam")]
        public string BakiyeFormatted => $"{(Bakiye > 0 ? "+" : Bakiye < 0 ? "-" : string.Empty)} { AyarlarService.ParaFormat(GecerliParaBirimCulture, Math.Abs(Bakiye))}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeliBorc")]
        public decimal VadeliBorc { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeliBorc")]
        public string VadeliBorcFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, Math.Abs(VadeliBorc));

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadesiGecenBorc")]
        public decimal VadesiGecenBorc { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadesiGecenBorc")]
        public string VadesiGecenBorcFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, Math.Abs(VadesiGecenBorc));

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tip")]
        public bool GelirGider => HesapTurGrupId == 1 ;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tip")]
        public string GelirGiderDurum => GelirGider ? FieldNameResources.Gelir : FieldNameResources.Gider;

        public string KulturKod { get; set; }

        public CultureInfo GecerliParaBirimCulture => !string.IsNullOrEmpty(KulturKod)
            ? CultureInfo.GetCultureInfo(KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
