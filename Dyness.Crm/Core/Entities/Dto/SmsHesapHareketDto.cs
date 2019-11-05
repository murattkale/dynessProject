using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Dto
{
    public class SmsHesapHareketDto
    {
        public int SmsHesapHareketId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHareketTip")]
        public string SmsHesapHareketTipAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public string SubeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        public string PersonelAdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kredi")]
        public int Kredi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        public string Telefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        public DateTime HareketTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        public string HareketTarihiFormatted => HareketTarihi != null
         ? HareketTarihi.ToString(AyarlarService.Get().GecerliTarihSaatFormati)
         : string.Empty;
    }
}
