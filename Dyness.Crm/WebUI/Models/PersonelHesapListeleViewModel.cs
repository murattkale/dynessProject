using Core.Entities.Dto;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class PersonelHesapListeleViewModel : ViewModelListele<HesapDto>
    {
        public List<SelectListItem> AySelectList { get; set; }

        public List<SelectListItem> YilSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public int Ay { get; set; }

        public int Yil { get; set; }

        public int SubeId { get; set; }

        public bool Borclu { get; set; }

        public bool EtkinMi { get; set; }
    }
}