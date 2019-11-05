using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KurumDuzenleViewModel : IViewModelDuzenle<Kurum>
    {
        public string Command { get; set; }

        public HttpPostedFileBase PostedFileArkaPlanDosyaAd { get; set; }

        public HttpPostedFileBase PostedFileLogoDosyaAd { get; set; }

        public Kurum Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Kurum> OperationResult { get; set; }
    }
}