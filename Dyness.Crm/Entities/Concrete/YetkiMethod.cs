using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public enum HTTPRequestMethods
    {
        GET = 1,
        POST = 2,
        HEAD = 3,
        PUT = 4,
        DELETE = 5,
        CONNECT = 6,
        OPTIONS = 7,
        TRACE = 8,
        PATCH = 9
    }

    public class YetkiMethod 
    {
        public int YetkiMethodId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Controller { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Method { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public HTTPRequestMethods HttpMethod { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string ReturnType { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sayfa")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Sayfa { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Islem")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Islem { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }
    }
}
