using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeOdemeBilgiSenetImzalayan 
    {
        public int OgrenciSozlesmeOdemeBilgiSenetImzalayanId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciOdemeBilgiSenetImzalayanBilgi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciSozlesmeOdemeBilgiSenetImzalayanBilgi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }
    }
}
