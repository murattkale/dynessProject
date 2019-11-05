using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Mesaj 
    {
        public int MesajId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YanitlananMesaj")]
        public int? UstMesajId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "MesajiGonderen")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int GonderenPersonelId { get; set; }
        
        [Display(ResourceType = typeof(FieldNameResources), Name = "MesajiAlan")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int GonderilenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Metin")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Metin { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GonderilmeTarihi")]
        [NotMapped]
        public string GonderilmeTarihiFormatted =>  GonderilmeTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "GonderilmeTarihi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public DateTime? GonderilmeTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OkunmaTarihi")]
        [NotMapped]
        public string OkunmaTarihiFormatted => OkunmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati);

        [Display(ResourceType = typeof(FieldNameResources), Name = "OkunmaTarihi")]
        public DateTime? OkunmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OkunduMu")]
        [NotMapped]
        public bool OkunduMu => OkunmaTarihi != null;

        [ForeignKey("UstMesajId")]
        public Mesaj UstMesaj { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YanitlananMesaj")]
        [NotMapped]
        public string GonderenPersonelAdSoyad => GonderenPersonel != null ? GonderenPersonel.AdSoyad : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "MesajiGonderen")]
        [ForeignKey("GonderenPersonelId")]
        public virtual Personel GonderenPersonel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "MesajiAlan")]
        [NotMapped]
        public string GonderilenPersonelAdSoyad => GonderilenPersonel != null ? GonderilenPersonel.AdSoyad : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "MesajiAlan")]
        [ForeignKey("GonderilenPersonelId")]
        public virtual Personel GonderilenPersonel { get; set; }

        public virtual List<Mesaj> AltMesajlar { get; set; }
    }
}
