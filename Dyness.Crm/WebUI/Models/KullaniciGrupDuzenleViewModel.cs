using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class KullaniciGrupDuzenleViewModel : IViewModelDuzenle<PersonelYetkiGrup>
    {
        public string Command { get; set; }

        public SelectList KullaniciGrupSelectList { get; set; }

        public PersonelYetkiGrup Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<PersonelYetkiGrup> OperationResult { get; set; }
    }
}