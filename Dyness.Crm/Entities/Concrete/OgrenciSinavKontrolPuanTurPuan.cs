using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class OgrenciSinavKontrolPuanTurPuan
    {
        public int OgrenciSinavKontrolPuanTurPuanId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSinavKontrol")]
        public int OgrenciSinavKontrolId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PuanTur")]
        public int PuanTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Puan")]
        public double Puan { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamPuan")]
        public double ToplamPuan { get; set; }

        public int SinifSira { get; set; }

        public int SubeSira { get; set; }

        public int GenelSira { get; set; }

        public virtual OgrenciSinavKontrol OgrenciSinavKontrol { get; set; }

        public virtual PuanTur PuanTur { get; set; }
    }
}
