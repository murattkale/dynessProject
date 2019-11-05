using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KullaniciDuzenleViewModel : IViewModelDuzenle<Kullanici>
    {
        public string Command { get; set; }

        public HttpPostedFileBase PostedFileGorselDosyaAd { get; set; }

        public SelectList KullaniciSelectList { get; set; }

        public SelectList KullaniciGrupSelectList { get; set; }

        public SelectList SubeSelectList { get; set; }

        public SelectList UlkeSelectList { get; set; }

        public Kullanici Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Kullanici> OperationResult { get; set; }
    }
}