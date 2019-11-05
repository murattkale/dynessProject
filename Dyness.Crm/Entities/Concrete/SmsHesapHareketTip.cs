using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class SmsHesapHareketTip
    {
        public int SmsHesapHareketTipId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesapHareketTipAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SmsHesapHareketTipAd { get; set; }
    }
}
