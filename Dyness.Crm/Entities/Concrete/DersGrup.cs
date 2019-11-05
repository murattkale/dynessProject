using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class DersGrup
    {
        public int DersGrupId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrupAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DersGrupAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Kurum Kurum { get; set; }

        public virtual List<Ders> Dersler { get; set; }
    }
}
