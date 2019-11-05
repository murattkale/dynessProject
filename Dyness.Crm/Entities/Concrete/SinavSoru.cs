namespace Entities.Concrete
{
    public class SinavSoru
    {
        public int SinavSoruId { get; set; }

        public int SinavKitapcikId { get; set; }

        public int DersId { get; set; }

        public int? KonuId { get; set; }

        public int Soru { get; set; }

        public string Dogru { get; set; }

        public virtual SinavKitapcik SinavKitapcik { get; set; }

        public virtual Ders Ders { get; set; }

        public virtual Konu Konu { get; set; }
    }
}
