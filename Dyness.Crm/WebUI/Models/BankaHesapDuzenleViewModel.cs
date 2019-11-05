using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class BankaHesapDuzenleViewModel : IViewModelDuzenle<BankaHesap>
    {
        public string Command { get; set; }

        public List<SelectListItem> BankaSelectList { get; set; }

        public List<SelectListItem> HesapSelectList { get; set; }

        public List<SelectListItem> BankaHesapSelectList { get; set; }

        public List<SelectListItem> ParaBirimSelectList { get; set; }

        public BankaHesap Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<BankaHesap> OperationResult { get; set; }
    }
}