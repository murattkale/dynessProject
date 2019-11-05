using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Kiyafet
    {
        public int KiyafetId { get; set; }

        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTur")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int KiyafetTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetBeden")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int KiyafetBedenId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "StokAdet")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? StokAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetAd")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KiyafetAd { get; set; }

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

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTur")]
        [NotMapped]
        public string KiyafetTurAd => KiyafetTur != null ? KiyafetTur.KiyafetTurAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetTur")]
        public virtual KiyafetTur KiyafetTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetBeden")]
        [NotMapped]
        public string KiyafetBedenAd => KiyafetBeden != null ? KiyafetBeden.KiyafetBedenAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "KiyafetBeden")]
        public virtual KiyafetBeden KiyafetBeden { get; set; }
    }
}
