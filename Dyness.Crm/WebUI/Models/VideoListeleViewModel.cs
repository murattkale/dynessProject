using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class VideoListeleViewModel : ViewModelListele<Video>
    {
        public List<SelectListItem> DersSelectList { get; set; }

        public List<SelectListItem> VideoKategoriSelectList { get; set; }

        public List<SelectListItem> KonuSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int SelectedDers { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kategoriler")]
        public int[] SelectedKategoriler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Konular")]
        public int[] SelectedKonular { get; set; }
    }
}