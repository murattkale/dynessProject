using Entities.Concrete;

namespace WebUI.Models
{
    public class MakbuzYazdirViewModel
    {
        public int HesapHareketId { get; set; }

        public bool KopyaMi { get; set; }

        public Hesap Model { get; set; }

        public Sube Sube { get; set; }

        public Personel Personel { get; set; }
    }
}