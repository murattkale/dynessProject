using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinifSeans 
    {
        public int SinifSeansId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeansAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SinifSeansAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Aciklama")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Aciklama { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Kurum Kurum { get; set; }
    }
}
