using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class PersonelPuantajGunlukDurum
    {
        public int PersonelPuantajGunlukDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelPuantajGunlukDurumAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string PersonelPuantajGunlukDurumAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelPuantajGunlukDurumKisatlma")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string PersonelPuantajGunlukDurumKisatlma { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelPuantajGunlukDurumRenk")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string PersonelPuantajGunlukDurumRenk { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sira")]
        public int Sira { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CalistiMi")]
        public bool CalistiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }
    }
}
