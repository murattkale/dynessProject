using Core.General;
using Core.Properties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class HesapBilgi
    {
        public int HesapBilgiId { get; set; }

        public int HesapId { get; set; }

        public int? Yil { get; set; }

        public int? Ay { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        public string ToplamBorcFormatted => AyarlarService.ParaFormat(Hesap.GecerliParaBirimCulture, ToplamBorc);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamAlacak")]
        [DefaultValue(0)]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal ToplamBorc { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        [DefaultValue(0)]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public decimal ToplamAlacak { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        public string ToplamAlacakFormatted => AyarlarService.ParaFormat(Hesap.GecerliParaBirimCulture, ToplamAlacak);

        public virtual Hesap Hesap { get; set; }
    }
}
