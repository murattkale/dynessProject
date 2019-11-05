using Core.Entities.Dto;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciSinavKontrolListeleViewModel : ViewModelListele<OgrenciSinavKontrolDto>
    {
        public int SinavId { get; set; }

        public int SubeId { get; set; }

        public string SinavBaslik { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }
    }
}