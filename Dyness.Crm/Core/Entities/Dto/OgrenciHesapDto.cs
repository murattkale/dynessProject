using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Dto
{
    public class OgrenciHesapDto : HesapDto
    {

        [Display(ResourceType = typeof(FieldNameResources), Name = "GecikenTaksit")]
        public int GecikenTaksit { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VeliAdSoyad")]
        public string VeliAdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VeliTelefon")]
        public string VeliTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciTelefon")]
        public string OgrenciTelefon { get; set; }
    }
}
