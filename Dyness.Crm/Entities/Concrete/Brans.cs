using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Brans
    {
        public int BransId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BransAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string BransAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BransAd")]
        public string KurumBransAd => Kurum != null
            ? $"{Kurum.KurumAd} - {BransAd}"
            : $"Genel - {BransAd}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumAd")]
        [NotMapped]
        public string KurumAd => Kurum != null ? Kurum.KurumAd : string.Empty;

        public virtual Kurum Kurum { get; set; }

        public List<BransDers> BransDersler { get; set; }
    }
}
