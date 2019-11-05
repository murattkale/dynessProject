using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class SinavTurDersKatSayi
    {
        public int SinavTurDersKatSayiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTur")]
        public int SinavTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrup")]
        public int DersGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrup")]
        public int? PuanTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KatSayi")]
        public double? KatSayi { get; set; }

        public virtual SinavTur SinavTur { get; set; }

        public virtual DersGrup DersGrup { get; set; }

        public virtual PuanTur PuanTur { get; set; }
    }
}
