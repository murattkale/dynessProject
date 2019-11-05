using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class VideoDuzenleViewModel : IViewModelDuzenle<Video>
    {
        public List<SelectListItem> DersSelectList { get; set; }
        
        public List<SelectListItem> VideoKategoriSelectList { get; set; }

        public List<SelectListItem> KonuSelectList { get; set; }

        public List<SelectListItem> KurumSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VideoKategoriler")]
        public int[] SelectedVideoKategoriler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Konular")]
        public int[] SelectedKonular { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurumlar")]
        public int[] SelectedKurumlar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Subeler")]
        public int[] SelectedSubeler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezonlar")]
        public int[] SelectedSezonlar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Branslar")]
        public int[] SelectedBranslar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Siniflar")]
        public int[] SelectedSiniflar { get; set; }

        public string Command { get; set; }

        public Video Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Video> OperationResult { get; set; }
    }
}