using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class HesapTurDuzenleViewModel : IViewModelDuzenle<HesapTur>
    {
        public bool Gelir => HesapTurGrupId == 1;

        public bool Gider=> HesapTurGrupId == 2;

        public int HesapTurGrupId { get; set; }

        public string Command { get; set; }

        public List<SelectListItem> HesapTurGrupSelectList { get; set; }

        public List<SelectListItem> HesapTurSelectList { get; set; }

        public HesapTur Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<HesapTur> OperationResult { get; set; }
    }
}