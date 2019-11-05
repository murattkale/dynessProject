using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoKategori
    {
        public int VideoKategoriId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VideoKategoriAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VideoKategoriAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public string DersAd => Ders != null ? Ders.DersAd : "";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Kurum Kurum { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
