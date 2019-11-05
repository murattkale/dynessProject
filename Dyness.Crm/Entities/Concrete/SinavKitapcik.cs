using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinavKitapcik
    {
        public int SinavKitapcikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinav")]
        public int SinavId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kitapcik")]
        [MaxLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CevapAnahtari")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string CevapAnahtari { get; set; }

        public virtual Sinav Sinav { get; set; }

        public List<SinavKitapcikDersBilgi> SinavKitapcikDersBilgiler { get; set; }

        [NotMapped]
        public List<SinavSoru> SinavSorular { get; set; }

        [NotMapped]
        public List<SinavKonuBilgi> SinavKonuBilgiler { get; set; }
    }
}
