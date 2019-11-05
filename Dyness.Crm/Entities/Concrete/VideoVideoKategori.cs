using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoVideoKategori
    {
        public int VideoVideoKategoriId { get; set; }

        public int VideoId { get; set; }

        public int VideoKategoriId { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }

        public virtual Video Video { get; set; }

        public virtual VideoKategori VideoKategori { get; set; }
    }
}
