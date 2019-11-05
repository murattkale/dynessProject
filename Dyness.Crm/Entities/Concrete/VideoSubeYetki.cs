using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class VideoSubeYetki
    {
        public int VideoSubeYetkiId { get; set; }

        public int VideoId { get; set; }

        public int SubeId { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }
        public virtual Video Video { get; set; }

        public virtual Sube Sube { get; set; }
    }
}
