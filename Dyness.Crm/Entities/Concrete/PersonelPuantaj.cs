using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class PersonelPuantaj
    {
        public int PersonelPuantajId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yil")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PuantajYil { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ay")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PuantajAy { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamGun")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int ToplamGun { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gun")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Gun { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hakedis")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int Hakedis { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Maas")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Maas { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gunluk")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Gunluk { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesaplananMaas")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal HesaplananMaas { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Elden")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Elden { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Icra")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Icra { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Bes")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Bes { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Banka")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Banka { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sistem")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal Sistem { get; set; }

        public virtual Personel Personel { get; set; }

        public List<PersonelPuantajGunluk> PersonelPuantajGunlukler { get; set; }
    }
}
