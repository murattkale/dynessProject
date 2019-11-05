using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Sinav
    {
        public int SinavId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        public int KurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTur")]
        public int SinavTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OptikForm")]
        public int OptikFormId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sezon")]
        public int SezonId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        public string KurumAd => Kurum != null ? Kurum.KurumAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTur")]
        public string SinavTurAd => SinavTur != null ? SinavTur.SinavTurAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTarihi")]
        [NotMapped]
        public string SinavTarihiFormatted => SinavTarihi != null
            ? SinavTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavTarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime? SinavTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonuclarGoruntulenebilir")]
        [NotMapped]
        public string SonucGoruntulenmeDurumu => SonuclarGoruntulenebilir ? FieldNameResources.SonuclarGoruntulenir : FieldNameResources.SonuclarGoruntulenmez;

        [Display(ResourceType = typeof(FieldNameResources), Name = "SonuclarGoruntulenebilir")]
        public bool SonuclarGoruntulenebilir { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YetkiliSubeler")]
        [NotMapped]
        public string SinavSubeYetkilerDisplayName { get; set; }

        public virtual Kurum Kurum { get; set; }

        public virtual SinavTur SinavTur { get; set; }

        public virtual OptikForm OptikForm { get; set; }

        public virtual Sezon Sezon { get; set; }

        public virtual List<SinavKitapcik> SinavKitapciklar { get; set; }

        public virtual List<SinavSube> SinavSubeler { get; set; }
    }
}
