using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciSozlesmeGuncelleViewModel : IViewModelDuzenle<OgrenciSozlesme>
    {
        public string Command { get; set; }

        public int? SinifId { get; set; }

        public List<SelectListItem> OgrenciSozlesmeTurSelectList { get; set; }

        public List<SelectListItem> PersonelSelectList { get; set; }

        public List<SelectListItem> DanismanPersonelSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> SinifSeviyeSelectList { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        public List<SelectListItem> OkulTurSelectList { get; set; }

        public List<SelectListItem> ServisSelectList { get; set; }

        public List<SelectListItem> EtkinlikSelectList { get; set; }

        public List<SelectListItem> EhliyetTurSelectList { get; set; }

        public OgrenciSozlesme Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<OgrenciSozlesme> OperationResult { get; set; }
    }
}