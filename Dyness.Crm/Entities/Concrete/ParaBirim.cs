using Core.Properties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class ParaBirim 
    {
        public int ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirimAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string ParaBirimAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kod")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [DataType("nchar")]
        public string Kod { get; set; }


        [Display(ResourceType = typeof(FieldNameResources), Name = "Kod")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [DataType("nchar")]
        public string KulturKod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Tutar")]
        [Required]
        [DefaultValue(1)]
        public decimal Tutar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }
    }
}
