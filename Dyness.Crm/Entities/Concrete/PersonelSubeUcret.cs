using Core.General;
using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class PersonelSubeUcret
    {
        public int PersonelSubeUcretId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SubeId { get; set; }

        [NotMapped]
        public bool Silinecek { get; set; }

        public string UcretFormatted => Ucret != null && Personel != null
            ? AyarlarService.ParaFormat(Personel.GecerliParaBirimCulture, Ucret)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ucret")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal? Ucret { get; set; }

        public virtual Personel Personel { get; set; }

        public virtual Sube Sube { get; set; }
    }
}
