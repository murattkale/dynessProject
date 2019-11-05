using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class EskiKayitDuzenleViewModel : IViewModelDuzenle<EskiKayit>
    {
        public string Command { get; set; }
   
        public EskiKayit Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<EskiKayit> OperationResult { get; set; }
    }
}