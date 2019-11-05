using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciSozlesmeHesapHareket
    {
        [Key, ForeignKey("HesapHareket")]
        public int HesapHareketId { get; set; }

        public int OgrenciSozlesmeId { get; set; }

        public virtual HesapHareket HesapHareket { get; set; }

        public virtual OgrenciSozlesme OgrenciSozlesme { get; set; }
    }
}
