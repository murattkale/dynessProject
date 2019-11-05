using Core.General;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Sezon 
    {
        public int SezonId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        public int? KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SezonAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SezonAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BransAd")]
        public string KurumSezonAd => Kurum != null
            ? $"{Kurum.KurumAd} - {SezonAd}"
            : $"Genel - {SezonAd}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kod")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Kod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BaslangicTarihi")]
        [NotMapped]
        public string BaslangicTarihiFormatted => BaslangicTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "BaslangicTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [DataType(DataType.Date)]
        public DateTime? BaslangicTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BitisTarihi")]
        [NotMapped]
        public string BitisTarihiFormatted => BitisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "BitisTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [DataType(DataType.Date)]
        public DateTime? BitisTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KurumAd")]
        [NotMapped]
        public string KurumAd => Kurum != null ? Kurum.KurumAd : string.Empty;

        public virtual Kurum Kurum { get; set; }
    }
}
