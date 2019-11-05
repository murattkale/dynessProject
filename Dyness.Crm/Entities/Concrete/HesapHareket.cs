using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Concrete
{
    public class HesapHareket
    {
        public int HesapHareketId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public int ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BorcluHesap")]
        public int? BorcluHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AlacakliHesap")]
        public int? AlacakliHesapId { get; set; }

        public int? PersonelId { get; set; }

        [NotMapped]
        public int SubeId { get; set; }

        //ALAN HESAP BORÇLUDUR -> Öğrenci Kayıt, Ödenecek Tutar, -1000 TL
        //VEREN HESAP ALACAKLIDIR -> Öğrenci Ödeme yaptı, 1000 TL
        [Display(ResourceType = typeof(FieldNameResources), Name = "Tutar")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal? Tutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tutar")]
        public string TutarFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, Tutar);

        [Display(ResourceType = typeof(FieldNameResources), Name = "Aciklama")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Aciklama { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IslemGerceklestiMi")]
        public bool IslemGerceklestiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeTarihi")]
        [NotMapped]
        public string VadeTarihiFormatted => VadeTarihi != null
            ? VadeTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeTarihi")]
        [DataType(DataType.Date)]
        public DateTime? VadeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        [NotMapped]
        public string HareketTarihiFormatted => HareketTarihi != null
            ? HareketTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "HareketTarihi")]
        public DateTime? HareketTarihi { get; set; }

        public virtual ParaBirim ParaBirim { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BorcluHesap")]
        [NotMapped]
        public string BorcluHesapBaslik => BorcluHesap != null ? BorcluHesap.HesapBaslik : string.Empty;

        [ForeignKey("BorcluHesapId")]
        public virtual Hesap BorcluHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AlacakliHesap")]
        [NotMapped]
        public string AlacakliHesapBaslik => AlacakliHesap != null ? AlacakliHesap.HesapBaslik : string.Empty;

        [ForeignKey("AlacakliHesapId")]
        public virtual Hesap AlacakliHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IslemiYapanPersonel")]
        [NotMapped]
        public string PersonelAdSoyad => Personel != null ? Personel.AdSoyad : string.Empty;

        public virtual Personel Personel { get; set; }

        public virtual OgrenciSozlesmeHesapHareket OgrenciSozlesmeHesapHareket { get; set; }

        public CultureInfo GecerliParaBirimCulture => ParaBirim != null
            ? CultureInfo.GetCultureInfo(ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
