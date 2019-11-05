using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeKiyafetDurum
    {
        public int OgrenciSozlesmeKiyafetDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sozlesme")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int OgrenciSozlesmeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kiyafet")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int KiyafetId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SecildiMi")]
        [NotMapped]
        public bool SecildiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TeslimDurumu")]
        [NotMapped]
        public string TeslimDurum => TeslimEdildiMi ? FieldNameResources.TeslimEdildi : FieldNameResources.TeslimEdilmedi;

        [Display(ResourceType = typeof(FieldNameResources), Name = "TeslimEdildiMi")]
        public bool TeslimEdildiMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sozlesme")]
        public virtual OgrenciSozlesme OgrenciSozlesme { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kiyafet")]
        [NotMapped]
        public string KiyafetAd => Kiyafet != null ? Kiyafet.KiyafetAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Yayin")]
        public virtual Kiyafet Kiyafet { get; set; }
    }
}
