using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class PersonelPuantajGunluk
    {
        public int PersonelPuantajGunlukId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelPuantaj")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelPuantajId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelPuantajGunlukDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yil")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Yil { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ay")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Ay { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gun")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Gun { get; set; }

        public virtual PersonelPuantaj PersonelPuantaj { get; set; }

        public virtual PersonelPuantajGunlukDurum PersonelPuantajGunlukDurum { get; set; }
    }
}
