using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class OgrenciDetayViewModel
    {
        public Ogrenci Model { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamTutar")]
        public decimal? TaksitToplamTutar { get; set; }

        public HesapHareket ModelHesapHareket { get; set; }

        public List<SelectListItem> HesapHareketSelectList { get; set; }

        public List<SelectListItem> HesapSelectList { get; set; }

        public OgrenciSozlesmeSilViewModel OgrenciSozlesmeSilModel { get; set; }

        public OgrenciSozlesme OgrenciSozlesmeModel { get; set; }

        public OgrenciSozlesmeOdemeBilgi OgrenciSozlesmeOdemeBilgiModel { get; set; }

        public FaturaBilgi FaturaBilgiModel { get; set; }

        public bool MessageExists => OperationResult?.MessageInfos != null && OperationResult.MessageInfos.Any();

        public EntityOperationResult<Ogrenci> OperationResult { get; set; }

        public List<Sube> Subeler { get; set; }
    }
}