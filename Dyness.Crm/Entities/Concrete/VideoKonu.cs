using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoKonu
    {
        public int VideoKonuId { get; set; }

        public int VideoId { get; set; }

        public int KonuId { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }

        public virtual Video Video { get; set; }

        public virtual Konu Konu { get; set; }
    }
}
