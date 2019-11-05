using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OnKayit
    {
        public int OnKayitId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Soyad { get; set; }

        private string adSoyad;

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AdSoyad
        {
            get
            {
                return string.IsNullOrEmpty(adSoyad) ? $"{Ad} {Soyad}" : adSoyad;
            }
            set
            {
                adSoyad = $"{Ad} {Soyad}";
            }
        }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string TcKimlikNo { get; set; }


        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        [NotMapped]
        public string Cinsiyet => KadinMi ? FieldNameResources.Kadin : FieldNameResources.Erkek;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public bool KadinMi { get; set; }
    }
}
