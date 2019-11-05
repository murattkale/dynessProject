using Core.Entities.Dto;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciSozlesmeListeleViewModel : ViewModelListele<OgrenciSozlesmeDto>
    {
        public List<SelectListItem> OgrenciSozlesmeTurSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> SinifSelectList { get; set; }

        public int OgrenciSozlesmeTurId { get; set; }

        public int SubeId { get; set; }

        public int SezonId { get; set; }

        public int SinifId { get; set; }
    }
}