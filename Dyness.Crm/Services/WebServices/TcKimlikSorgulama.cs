namespace Services.WebServices
{
    public static class TcKimlikSorgulama
    {
        public static bool Dogrula(string tcKimlikNo, string ad, string soyad, int dogumYili)
        {
            if (tcKimlikNo.Length != 11) return false;
            if (string.IsNullOrEmpty(ad)) return false;
            if (string.IsNullOrEmpty(soyad)) return false;
            if (dogumYili < 1900) return false;

            var upperAd = ad.Trim().ToUpper();
            var upperSoyad = soyad.Trim().ToUpper();

            long.TryParse(tcKimlikNo, out long longTcKimlikNo);

            try
            {
                using (KPSPublic.KPSPublic service = new KPSPublic.KPSPublic())
                {
                    return service.TCKimlikNoDogrula(longTcKimlikNo, upperAd, upperSoyad, dogumYili);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
