using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinavKitapcikDersBilgi
    {
        public int SinavKitapcikDersBilgiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavKitapcik")]
        public int SinavKitapcikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CevapAnahtari")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string CevapAnahtartari { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersKonuBilgi")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DersKonuBilgi { get; set; }

        [NotMapped]
        public int Sira { get; set; }

        public virtual SinavKitapcik SinavKitapcik { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
