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
    public class DersDuzenleViewModel : IViewModelDuzenle<Ders>
    {
        public string Command { get; set; }

        public List<SelectListItem> DersSelectList { get; set; }

        public List<SelectListItem> DersGrupSelectList { get; set; }

        public List<SelectListItem> BransSelectList { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SeciliBranslar")]
        public int[] SelectedBranslar { get; set; }

        public Ders Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Ders> OperationResult { get; set; }
    }
}