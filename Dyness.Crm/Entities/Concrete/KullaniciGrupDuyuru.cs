using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class KullaniciGrupDuyuru
    {
        public int KullaniciGrupDuyuruId { get; set; }

        public int KullaniciGrupId { get; set; }

        public int DuyuruId { get; set; }

        public virtual KullaniciGrup KullaniciGrup { get; set; }

        public virtual Duyuru Duyuru { get; set; }

        [NotMapped]
        public bool Selected { get; set; }
    }
}
