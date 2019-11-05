using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Linq;
using WebUI.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class OgrenciSifreDegistirViewModel : IViewModelDuzenle<Ogrenci>
    {
        public string Command { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(8, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string Sifre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SifreTekrar")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(8, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string SifreTekrar { get; set; }

        public Ogrenci Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Ogrenci> OperationResult { get; set; }
    }
}