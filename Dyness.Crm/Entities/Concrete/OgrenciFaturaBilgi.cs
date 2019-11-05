namespace Entities.Concrete
{
    public class OgrenciFaturaBilgi
    {
        public int OgrenciFaturaBilgiId { get; set; }

        public int OgrenciId { get; set; }

        public int FaturaBilgiId { get; set; }

        public bool GecerliMi { get; set; }

        public Ogrenci Ogrenci { get; set; }

        public FaturaBilgi FaturaBilgi { get; set; }
    }
}
