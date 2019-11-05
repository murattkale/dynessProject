using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class HesapDuzenleViewModel : IViewModelDuzenle<Hesap>
    {
        public string Command { get; set; }

        public int HesapTurGrupId { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> HesapTurSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public Hesap Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Hesap> OperationResult { get; set; }
    }
}