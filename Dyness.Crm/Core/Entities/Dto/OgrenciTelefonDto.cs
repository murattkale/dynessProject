namespace Core.Entities.Dto
{
    public class OgrenciTelefonDto
    {
        public int OgrenciId { get; set; }

        public int SubeId { get; set; }

        public int SezonId { get; set; }

        public int SinifId { get; set; }

        public string OgrenciAdSoyad { get; set; }

        public string SubeAd { get; set; }

        public string SezonAd { get; set; }

        public string SinifAd { get; set; }

        public string OgrenciCepTelefon { get; set; }

        public string AnneAdSoyad { get; set; }

        public string AnneCepTelefon1 { get; set; }

        public string AnneCepTelefon2 { get; set; }

        public string BabaAdSoyad { get; set; }

        public string BabaCepTelefon1 { get; set; }

        public string BabaCepTelefon2 { get; set; }

        public string YakiniAdSoyad { get; set; }

        public string YakiniCepTelefon1 { get; set; }

        public string YakiniCepTelefon2 { get; set; }

        public bool IletisimKendi { get; set; }

        public bool IletisimAnne { get; set; }

        public bool IletisimBaba { get; set; }

        public bool IletisimYakini { get; set; }
    }
}
