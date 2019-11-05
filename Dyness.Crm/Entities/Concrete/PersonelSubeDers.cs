using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class PersonelSubeDers 
    {
        public int PersonelSubeDersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SubeId { get; set; }

        public virtual Personel Personel { get; set; }

        public virtual Sube Sube { get; set; }
    }
}
