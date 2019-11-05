using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class PersonelYetki 
    {
        public int PersonelYetkiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Metod")]
        public int YetkiMethodId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        public int? PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelYetkiGrup")]
        public int? PersonelYetkiGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yetki")]
        public bool Yetki { get; set; }

        public virtual YetkiMethod YetkiMethod { get; set; }

        public virtual Personel Personel { get; set; }

        public virtual PersonelYetkiGrup PersonelYetkiGrup { get; set; }
    }
}
