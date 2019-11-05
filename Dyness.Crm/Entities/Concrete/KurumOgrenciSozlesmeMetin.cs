using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class KurumOgrenciSozlesmeMetin
    {
        public int KurumOgrenciSozlesmeMetinId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SozlesmeMetin")]
        public string Metin { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        public virtual Kurum Kurum { get; set; }
    }
}
