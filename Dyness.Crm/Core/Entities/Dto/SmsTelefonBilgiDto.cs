using Core.Properties;

namespace Core.Entities.Dto
{
    public enum Tip
    {
        Ogrenci,
        Personel
    }

    public enum Bilgi
    {
        Kendisi,
        Anne,
        Baba,
        Yakini
    }

    public class SmsTelefonBilgiDto
    {
        public int Id { get; set; }

        public int SubeId { get; set; }

        public int SezonId { get; set; }

        public int SinifId { get; set; }

        public int PersonelGrupId { get; set; }

        public string AdSoyad { get; set; }

        public string SubeAd { get; set; }

        public string SezonAd { get; set; }

        public string SinifAd { get; set; }

        public string PersonelGrupAd { get; set; }

        public string Telefon { get; set; }

        public Tip Tip { get; set; }

        public string TipAd => Tip == Tip.Ogrenci 
            ? FieldNameResources.Ogrenci 
            : FieldNameResources.Personel;

        public Bilgi Bilgi { get; set; }

        public string BilgiAd => Bilgi == Bilgi.Kendisi
            ? FieldNameResources.Kendi
            : Bilgi == Bilgi.Anne
                ? FieldNameResources.Anne
                : Bilgi == Bilgi.Baba
                    ? FieldNameResources.Baba
                    : FieldNameResources.Yakini;

        public bool Iletisim { get; set; }

        public bool SmsGonderilebilir => !string.IsNullOrEmpty(Telefon) && Iletisim;

        public bool Checked { get; set; }
    }
}
