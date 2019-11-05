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
    public class PersonelPuantajDuzenleViewModel : IViewModelDuzenle<List<PersonelPuantaj>>
    {
        public string Command { get ; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yil")]
        [Required(ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? Yil { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ay")]
        [Required(ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? Ay { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public int? SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelGrup")]
        public int? PersonelGrupId { get; set; }

        public bool TabloyuGoster { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<SelectListItem> PersonelSelectList { get; set; }

        public List<GunDto> GunDtolar { get; set; }

        public List<PersonelPuantajGunlukDurum> PersonelPuantajGunlukDurumlar { get; set; }

        public List<PersonelPuantaj> Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<List<PersonelPuantaj>> OperationResult { get; set; }
    }
}