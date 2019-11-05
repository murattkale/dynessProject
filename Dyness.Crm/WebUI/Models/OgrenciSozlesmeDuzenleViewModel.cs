using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciSozlesmeDuzenleViewModel : IViewModelDuzenle<OgrenciSozlesme>
    {
        public string Command { get; set; }

        public HttpPostedFileBase PostedFileGorselDosyaAd { get; set; }

        public List<SelectListItem> OgrenciSozlesmeTurSelectList { get; set; }

        public List<SelectListItem> PersonelSelectList { get; set; }

        public List<SelectListItem> DanismanPersonelSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> SinifSeviyeSelectList { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        public List<SelectListItem> DersSelectList { get; set; }

        public List<SelectListItem> UlkeSelectList { get; set; }

        public List<SelectListItem> OkulTurSelectList { get; set; }

        public List<SelectListItem> ServisSelectList { get; set; }

        public List<SelectListItem> EtkinlikSelectList { get; set; }

        public List<SelectListItem> EhliyetTurSelectList { get; set; }

        public List<SelectListItem> NeredenDuydunuzSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public List<SelectListItem> OgrenciSozlesmeOdemeBilgiSenetImzalayanSelectList { get; set; }

        public List<Sube> Subeler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AlacagiDersler")]
        public int[] SelectedDersler { get; set; }

        public OgrenciSozlesme Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<OgrenciSozlesme> OperationResult { get; set; }
    }
}