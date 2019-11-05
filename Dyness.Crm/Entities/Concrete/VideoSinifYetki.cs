using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoSinifYetki
    {
        public int VideoSinifYetkiId { get; set; }

        public int VideoId { get; set; }

        public int SinifId { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }
        public virtual Video Video { get; set; }

        public virtual Sinif Sinif { get; set; }
    }
}
