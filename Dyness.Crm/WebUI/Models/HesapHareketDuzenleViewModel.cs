using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class HesapHareketDuzenleViewModel : IViewModelDuzenle<HesapHareket>
    {
        public string Command { get; set; }

        public int HesapTurGrupId { get; set; }

        public List<SelectListItem> HesapTurSelectList { get; set; }

        public List<SelectListItem> BorcluHesapSelectList { get; set; }

        public List<SelectListItem> BorcluAltHesapSelectList { get; set; }

        public List<SelectListItem> AlacakliHesapSelectList { get; set; }

        public List<SelectListItem> AlacakliAltHesapSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public int BorcluHesapId { get; set; }

        public int BorcluAltHesapId { get; set; }

        public int AlacakliHesapId { get; set; }

        public int AlacakliAltHesapId { get; set; }

        public HesapHareket Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<HesapHareket> OperationResult { get; set; }
    }
}