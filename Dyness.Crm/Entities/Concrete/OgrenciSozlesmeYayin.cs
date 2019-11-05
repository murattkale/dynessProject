using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeYayin 
    {
        public int OgrenciSozlesmeYayinId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesme")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int OgrenciSozlesmeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yayin")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int YayinId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TeslimDurumu")]
        [NotMapped]
        public string TeslimDurum => TeslimEdildiMi ? FieldNameResources.TeslimEdildi : FieldNameResources.TeslimEdilmedi;

        [Display(ResourceType = typeof(FieldNameResources), Name = "TeslimEdildiMi")]
        public bool TeslimEdildiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SecildiMi")]
        [NotMapped]
        public bool SecildiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSozlesme")]
        public OgrenciSozlesme OgrenciSozlesme { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yayin")]

        public Yayin Yayin { get; set; }
    }
}
