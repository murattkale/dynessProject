using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Video
    {
        public int VideoId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Baslik { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public string DersAd => Ders != null ? Ders.DersAd : "";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Ders Ders { get; set; }

        public virtual IEnumerable<VideoVideoKategori> VideoVideoKategoriler { get; set; }

        public virtual IEnumerable<VideoKonu> VideoKonular { get; set; }

        public virtual IEnumerable<VideoKurumYetki> VideoKurumYetkiler { get; set; }

        public virtual IEnumerable<VideoSubeYetki> VideoSubeYetkiler { get; set; }

        public virtual IEnumerable<VideoSinifYetki> VideoSinifYetkiler { get; set; }
    }
}
