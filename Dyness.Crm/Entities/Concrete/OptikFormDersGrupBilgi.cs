using Core.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OptikFormDersGrupBilgi
    {
        public int OptikFormDersGrupBilgiId { get; set; }

        public int OptikFormId { get; set; }

        public int DersGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrupBasla")]
        public int? DersGrupBasla { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrupToplam")]
        public int? DersGrupAdet { get; set; }

        [NotMapped]
        public string CevapAnahtari { get; set; }

        [NotMapped]
        public string OgrenciCevaplar { get; set; }

        public virtual OptikForm OptikForm { get; set; }

        public virtual DersGrup DersGrup { get; set; }
    }
}
