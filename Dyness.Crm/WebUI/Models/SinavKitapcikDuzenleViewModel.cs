using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SinavKitapcikDuzenleViewModel : IViewModelDuzenle<SinavKitapcik>
    {
        public string Command { get; set; }

        public List<Konu> Konular { get; set; }

        public SinavKitapcik Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<SinavKitapcik> OperationResult { get; set; }
    }
}