using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SinavDuzenleViewModel : IViewModelDuzenle<Sinav>
    {
        public string Command { get; set; }

        public List<SelectListItem> SinavTurSelectList { get; set; }

        public List<SelectListItem> SezonSelectList { get; set; }

        public List<SelectListItem> OptikFormSelectList { get; set; }

        public List<SelectListItem> KurumSelectList { get; set; }

        public List<SelectListItem> DersSelectList { get; set; }

        public List<SelectListItem> YetkiSubeSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YetkiliSubeler")]
        public int[] SelectedYetkiliSubeler { get; set; }

        public Sinav Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Sinav> OperationResult { get; set; }
    }
}