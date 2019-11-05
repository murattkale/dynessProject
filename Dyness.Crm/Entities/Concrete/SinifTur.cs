using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class SinifTur 
    {
        public int SinifTurId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifTurAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SinifTurAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OnlineEtutVerebilirMi")]
        public bool OnlineEtutVerebilirMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Kurum Kurum { get; set; }
    }
}
    