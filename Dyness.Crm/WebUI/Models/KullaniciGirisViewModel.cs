using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Linq;
using WebUI.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class KullaniciGirisViewModel : IViewModelDuzenle<Kullanici>
    {
        public string Command { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KullaniciAd")]
        [Required(ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(5, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string KullaniciAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [Required(ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string Sifre { get; set; }

        public Kullanici Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Kullanici> OperationResult { get; set; }
    }
}