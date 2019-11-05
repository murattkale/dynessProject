using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class DuyuruPersonelBilgi 
    {
        public int DuyuruPersonelBilgiId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Duyuru")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int DuyuruId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GorulmeTarihi")]
        [NotMapped]
        public string GorulmeTarihiFormatted => GorulmeTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "GorulmeTarihi")]
        public DateTime? GorulmeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Goruldumu")]
        [NotMapped]
        public bool GorulduMu => GorulmeTarihi != null;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Duyuru")]
        [NotMapped]
        public string DuyuruBaslik => Duyuru != null ? Duyuru.Baslik : string.Empty;

        public virtual Duyuru Duyuru { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [NotMapped]
        public string PersonelAdSoyad => Personel != null ? Personel.AdSoyad : string.Empty;

        public virtual Personel Personel { get; set; }
    }
}
