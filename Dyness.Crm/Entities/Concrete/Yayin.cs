using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Yayin
    {
        public int YayinId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SinifSeviyeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        public int? BransId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "StokAdet")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? StokAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YayinAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string YayinAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "StokDurumu")]
        [NotMapped]
        public string StokDurumu => StoktaVarmi ? FieldNameResources.StoktaVar : FieldNameResources.StoktaYok;

        [Display(ResourceType = typeof(FieldNameResources), Name = "StokDurumu")]
        [NotMapped]
        public bool StoktaVarmi => StokAdet != null && StokAdet > 0;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public virtual Kurum Kurum { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinifSeviye")]
        [NotMapped]
        public string SinifSeviyeAd => SinifSeviye != null ? SinifSeviye.SinifSeviyeAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        public virtual SinifSeviye SinifSeviye { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        [NotMapped]
        public string BransAd => Brans != null ? Brans.BransAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Brans")]
        public virtual Brans Brans { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        [NotMapped]
        public string DersAd => Ders != null ? Ders.DersAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public virtual Ders Ders { get; set; }
    }
}
