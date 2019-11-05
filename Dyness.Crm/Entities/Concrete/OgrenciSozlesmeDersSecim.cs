using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeDersSecim 
    {
        public int OgrenciSozlesmeDersSecimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesme")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int OgrenciSozlesmeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int DersId { get; set; }

        public virtual OgrenciSozlesme OgrenciSozlesme { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
