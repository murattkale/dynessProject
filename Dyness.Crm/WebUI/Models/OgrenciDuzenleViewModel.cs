using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciDuzenleViewModel : IViewModelDuzenle<Ogrenci>
    {
        public string Command { get; set; }

        public HttpPostedFileBase PostedFileGorselDosyaAd { get; set; }

        public List<SelectListItem> UlkeSelectList { get; set; }

        public List<SelectListItem> NeredenDuydunuzSelectList { get; set; }

        public Ogrenci Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Ogrenci> OperationResult { get; set; }
    }
}