using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Duyuru 
    {
        public int DuyuruId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturanPersonel")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BagliDuyuru")]
        public int? UstDuyuruId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Metin")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public string Metin { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Zorunlumu")]
        public bool ZorunluMu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sabitmi")]
        public bool SabitMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime? OlusturulmaTarihi { get; set; }

        public virtual Personel Personel { get; set; }

        [ForeignKey("UstDuyuruId")]
        public virtual Duyuru UstDuyuru { get; set; }

        public virtual List<Duyuru> AltDuyurular { get; set; }
    }
}
