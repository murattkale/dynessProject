using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class OptikForm
    {
        public int OptikFormId { get; set; }

        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OptikFormAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OptikFormAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        public string TcNo => $"{TcNoBasla},{TcNoAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNoBasla")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? TcNoBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNoToplam")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? TcNoAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        public string OgrenciNo => $"{OgrenciNoBasla},{OgrenciNoAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNoBasla")]
        public int? OgrenciNoBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNoToplam")]
        public int? OgrenciNoAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        public string Ad => $"{AdBasla},{AdAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdBasla")]
        public int? AdBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdToplam")]
        public int? AdAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        public string Soyad => $"{SoyadBasla},{SoyadAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoyadBasla")]
        public int? SoyadBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoyadToplam")]
        public int? SoyadAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        public string AdSoyad => $"{AdSoyadBasla},{AdSoyadAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyadBasla")]
        public int? AdSoyadBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyadToplam")]
        public int? AdSoyadAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        public string Sinif => $"{SinifBasla},{SinifAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifBasla")]
        public int? SinifBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifToplam")]
        public int? SinifAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KitapcikTur")]
        public string KitapcikTur => $"{KitapcikTurBasla},{KitapcikTurAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "KitapcikTurBasla")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? KitapcikTurBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KitapcikTurToplam")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? KitapcikTurAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public string Cinsiyet => $"{CinsiyetBasla},{CinsiyetAdet}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "CinsiyetBasla")]
        public int? CinsiyetBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CinsiyetToplam")]
        public int? CinsiyetAdet { get; set; }

        public virtual Kurum Kurum { get; set; }

        public virtual List<OptikFormDersGrupBilgi> OptikFormDersGrupBilgiler { get; set; }
    }
}
