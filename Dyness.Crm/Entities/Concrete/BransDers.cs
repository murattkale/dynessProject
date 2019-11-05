using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class BransDers
    {
        public int BransDersId { get; set; }

        public int BransId { get; set; }

        public int DersId { get; set; }

        [NotMapped]
        public bool Silinecek { get; set; }

        public virtual Brans Brans { get; set; }

        public virtual Ders Ders { get; set; }
    }
}
