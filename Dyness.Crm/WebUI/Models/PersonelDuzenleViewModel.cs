using Core.Entities.Dto;
using Entities.Concrete;
using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models.Abstract;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class PersonelDuzenleViewModel : IViewModelDuzenle<Personel>
    {
        public string Command { get; set; }

        public HttpPostedFileBase PostedFileGorselDosyaAd { get; set; }

        public List<SelectListItem> PersonelGrupSelectList { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> DersSelectList { get; set; }

        public List<SelectListItem> UlkeSelectList { get; set; }

        public List<SelectListItem> DersSubeSelectList { get; set; }

        public List<SelectListItem> DersSubeUcretSelectList { get; set; }

        public List<SelectListItem> YetkiSubeSelectList { get; set; }

        public List<SelectListItem> PersonelYetkiGrupSelectList { get; set; }

        public Personel Model { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersVerdigiSubeler")]
        public int[] SelectedDersVerdigiSubeler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YetkiliOlduguSubeler")]
        public int[] SelectedYetkiliOlduguSubeler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "UcretAldigiSubeler")]
        public int[] SelectedUcretAldigiSubeler { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Personel> OperationResult { get; set; }
    }
}