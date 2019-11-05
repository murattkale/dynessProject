using Core.Properties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Servis
    {
        public int ServisId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Plaka")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string ServisPlaka { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Guzergah")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Guzergah { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Servis")]
        [NotMapped]
        public string ServisAd => $"{Guzergah} - {ServisPlaka}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kapasite")]
        public int? Kapasite { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YolcuAdet")]
        [DefaultValue(0)]
        public int? YolcuAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KapasiteDurumu")]
        [NotMapped]
        public string KapasiteDurumu => KapasiteVarmi ? ((Kapasite ?? 0) - (YolcuAdet ?? 0)).ToString() : FieldNameResources.Dolu;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KapasiteDurumu")]
        [NotMapped]
        public bool KapasiteVarmi => Kapasite != null && (Kapasite ?? 0) - (YolcuAdet ?? 0) > 0;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KapasiteKontrolEdilsinMi")]
        public bool KapasiteKontrolEdilsinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [NotMapped]
        public string SubeAd => Sube != null ? Sube.SubeAd : string.Empty;

        public virtual Sube Sube { get; set; }
    }
}
