using Core.Entities.Dto;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class HesapListeleViewModel : ViewModelListele<HesapDto>
    {
        public bool Gelir => HesapTurGrupId == 1;

        public bool Gider => HesapTurGrupId == 2;

        public int HesapTurGrupId { get; set; }

        public List<SelectListItem> HesapTurGrupSelectList { get; set; }

        public List<SelectListItem> HesapTurSelectList { get; set; }

        public List<SelectListItem> HesapSelectList { get; set; }

        public int HesapTurId { get; set; }

        public int HesapId { get; set; }

        public bool EtkinMi { get; set; }
    }
}