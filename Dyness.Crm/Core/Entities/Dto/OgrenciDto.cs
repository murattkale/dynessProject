using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Dto
{
    public class OgrenciDto
    {
        public int OgrenciId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        public string AdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        public string OgrenciNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcKimlikNo")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefon")]
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

        [Display(ResourceType = typeof(FieldNameResources), Name = "Adres")]
        public string Adres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PostaKodu")]
        public string PostaKodu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IlceAd")]
        public string IlceAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SehirAd")]
        public string SehirAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "UlkeAd")]
        public string UlkeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SubeAd")]
        public string SubeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        public string DogumTarihiFormatted => DogumTarihi != null
            ? DogumTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        public DateTime? DogumTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        public DateTime? OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
           ? OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;
    }
}
