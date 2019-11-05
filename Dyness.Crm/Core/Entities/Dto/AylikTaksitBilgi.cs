using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Dto
{
    public class AylikTaksitBilgi
    {
        public int HesapHareketId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitNo")]
        public int TaksitNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TaksitTutar")]
        public decimal? TaksitTutari { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VadeTarihi")]
        public DateTime? VadeTarihi { get; set; }
    }
}
