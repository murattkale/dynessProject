using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoKurumYetki
    {
        public int VideoKurumYetkiId { get; set; }

        public int VideoId { get; set; }

        public int KurumId { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }

        public virtual Video Video { get; set; }

        public virtual Kurum Kurum { get; set; }
    }
}
