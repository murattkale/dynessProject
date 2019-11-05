using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinavTurDers
    {
        public int SinavTurDersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTur")]
        public int SinavTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sira")]
        public int? Sira { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoruSayi")]
        public int? SoruSayi { get; set; }

        [NotMapped]
        public string OgrenciCevaplar { get; set; }

        public virtual SinavTur SinavTur { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
