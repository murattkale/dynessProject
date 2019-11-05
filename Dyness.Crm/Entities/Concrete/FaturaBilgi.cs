using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class FaturaBilgi 
    {
        public int FaturaBilgiId { get; set; }
     
        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VergiDairesi")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VergiDairesi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VergiNo")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VergiNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Adres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Adres { get; set; }
    }
}
