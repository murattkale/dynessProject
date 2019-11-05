using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class OgrenciSinavKontrolDersBilgi
    {
        public int OgrenciSinavKontrolDersBilgiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSinavKontrol")]
        public int OgrenciSinavKontrolId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoruCevaplar")]
        public string SoruCevaplar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogruCevapAdet")]
        public int DogruCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YanlisCevapAdet")]
        public int YanlisCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BosCevapAdet")]
        public int BosCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public double Net { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public string NetFormatted => string.Format("{0:0.000}", Net);

        public virtual OgrenciSinavKontrol OgrenciSinavKontrol { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
