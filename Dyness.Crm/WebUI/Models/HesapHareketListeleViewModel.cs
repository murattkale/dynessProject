using Core.Entities.Dto;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class HesapHareketListeleViewModel : ViewModelListele<HesapHareketDto>
    {
        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> HareketTurSelectList { get; set; }

        public List<SelectListItem> SorumluPersonelSelectList { get; set; }

        public List<SelectListItem> ZamanSelectList { get; set; }

        public int HesapTurGrupId { get; set; }

        public int SubeId { get; set; }

        public int SorumluPersonelId { get; set; }

        public int Zaman { get; set; }

        public string IlkTarih { get; set; }

        public string SonTarih { get; set; }
    }
}