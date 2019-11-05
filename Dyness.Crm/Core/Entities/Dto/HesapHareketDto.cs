using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Entities.Dto
{
    public class HesapHareketDto
    {
        public int HesapHareketId { get; set; }

        private string AlacakliHesap { get; set; }

        private string AlacakliHesapTur { get; set; }

        private string UstAlacakliHesap { get; set; }

        private string UstAlacakliHesapTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AlacakliHesap")]
        public string AlacakliHesapFormatted => !string.IsNullOrEmpty(UstAlacakliHesap)
            ? $"{UstAlacakliHesap} / {AlacakliHesap}"
            : AlacakliHesap;

        private string BorcluHesap { get; set; }

        private string BorcluHesapTur { get; set; }

        private string UstBorcluHesap { get; set; }

        private string UstBorcluHesapTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BorcluHesap")]
        public string BorcluHesapFormatted => !string.IsNullOrEmpty(UstBorcluHesap)
            ? $"{UstBorcluHesap} / {BorcluHesap}"
            : BorcluHesap;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public string SubeHesap => GelirGider
            ? BorcluHesapFormatted
            : AlacakliHesapFormatted;

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapTur")]
        public string HesapTur => GelirGider
            ? AlacakliHesapTur
            : BorcluHesapTur;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        public string DigerHesap => !GelirGider
          ? BorcluHesapFormatted
          : AlacakliHesapFormatted;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SorumluPersonel")]
        public string PersonelAdSoyad { get; set; }

        public string ParaBirimAd { get; set; }

        public string ParaBirimKulturKod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tutar")]
        public decimal Tutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tutar")]
        public string TutarFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, Tutar);

        [Display(ResourceType = typeof(FieldNameResources), Name = "Aciklama")]
        public string Aciklama { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeTarihi")]
        public string VadeTarihiFormatted => VadeTarihi != null
            ? VadeTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        public DateTime? VadeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IslemTarihi")]
        public string IslemTarihiFormatted => IslemTarihi != null
            ? IslemTarihi.Value.ToString(AyarlarService.Get().GecerliTarihSaatFormati)
            : string.Empty;

        public DateTime? IslemTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IslemGerceklestiMi")]
        public bool IslemGerceklestiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tip")]
        public bool GelirGider { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tip")]
        public string GelirGiderDurum => GelirGider ? FieldNameResources.Gelir : FieldNameResources.Gider;

        public CultureInfo GecerliParaBirimCulture => CultureInfo.GetCultureInfo(ParaBirimKulturKod);
    }
}
