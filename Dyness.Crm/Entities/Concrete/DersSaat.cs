using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class DersSaat
    {
        public int DersSaatId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersBaslangicSaat")]
        public int DersBaslangicSaat { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersBaslangicDakika")]
        public int DersBaslangicDakika { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersBitisSaat")]
        public int DersBitisSaat { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersBitisDakika")]
        public int DersBitisDakika { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersNo")]
        public string DersNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }
    }
}
