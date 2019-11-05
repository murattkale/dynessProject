using Core.Entities.Dto;
using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeOdemeBilgi
    {
        [Key, ForeignKey("OgrenciSozlesme")]
        public int OgrenciSozlesmeOdemeBilgiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public int ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatHesap")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PesinatHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SenetiImzalayan")]
        public int? OgrenciSozlesmeOdemeBilgiSenetImzalayanId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGuncelleyenPersonel")]
        public int? SonGuncelleyenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatTutar")]
        public string PesinatTutarFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, PesinatTutar);

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatTutar")]
        public decimal? PesinatTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitTutar")]
        public string TaksitTutarFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, TaksitTutar);

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitTutar")]
        public decimal? TaksitTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitAdet")]
        public int? TaksitAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Not { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitBaslangic")]
        [NotMapped]
        public string TaksitBaslangicTarihiFormatted => TaksitBaslangicTarihi != null
           ? TaksitBaslangicTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitBaslangic")]
        [DataType(DataType.Date)]
        public DateTime? TaksitBaslangicTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGuncellenmeTarihi")]
        [NotMapped]
        public string SonGuncellenmeTarihiFormatted => SonGuncellenmeTarihi != null
       ? SonGuncellenmeTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
       : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGuncellenmeTarihi")]
        public DateTime? SonGuncellenmeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public virtual ParaBirim ParaBirim { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatHesap")]
        [ForeignKey("PesinatHesapId")]
        public virtual Hesap PesinatHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SenetiImzalayan")]
        public virtual OgrenciSozlesmeOdemeBilgiSenetImzalayan OgrenciSozlesmeOdemeBilgiSenetImzalayan { get; set; }

        public virtual OgrenciSozlesme OgrenciSozlesme { get; set; }

        [ForeignKey("SonGuncelleyenPersonelId")]
        [Display(ResourceType = typeof(FieldNameResources), Name = "SonGuncelleyenPersonel")]
        public virtual Personel SonGuncelleyenPersonel { get; set; }

        public CultureInfo GecerliParaBirimCulture => ParaBirim != null
            ? CultureInfo.GetCultureInfo(ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");

        [NotMapped]
        public List<AylikTaksitBilgi> AylikTaksitBilgiler { get; set; }
    }
}
