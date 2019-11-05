using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Core.Entities.Dto;

namespace WebUI.Models
{
    public class OgrenciSozlesmeOdemeBilgiGuncelleViewModel
    {
        public int OgrenciId { get; set; }

        public int OgrenciSozlesmeId { get; set; }

        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatHesap")]
        public int PesinatHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public int ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SenetiImzalayan")]
        public int? OgrenciSozlesmeOdemeBilgiSenetImzalayanId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "EgitimTutar")]
        public decimal? EgitimTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YayinTutar")]
        public decimal? YayinTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ServisTutar")]
        public decimal? ServisTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTutar")]
        public decimal? KiyafetTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YemekTutar")]
        public decimal? YemekTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamTutar")]
        public decimal? ToplamTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PesinatTutar")]
        public decimal? PesinatTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OdenenTutar")]
        public decimal? OdenenTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KalanTutar")]
        public decimal? KalanTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitTutar")]
        public decimal? TaksitTutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitAdet")]
        public int? TaksitAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitBaslangic")]
        public DateTime? TaksitBaslangicTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonTaksitTarihi")]
        public DateTime? SonTaksitTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OdenenlerSilinsinMi")]
        public bool OdenenlerSilinsinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        public string Not { get; set; }

        public List<SelectListItem> PesinatHesapSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public List<SelectListItem> OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList { get; set; }

        public List<AylikTaksitBilgi> AylikTaksitBilgiler { get; set; }
    }
}