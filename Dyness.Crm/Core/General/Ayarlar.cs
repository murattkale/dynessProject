using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Core.General
{
    public class Ayarlar
    {
        public string GeciciYol { get; set; }

        public string KurumAd { get; set; }

        public Size KurumLogoSize { get; set; }

        public string KurumLogoYol { get; set; }

        public Size KurumArkaPlanSize { get; set; }

        public string KurumArkaPlanYol { get; set; }

        public Size PersonelGorselSize { get; set; }

        public string PersonelGorselYol { get; set; }

        public Size OgrenciGorselSize { get; set; }

        public string OgrenciGorselYol { get; set; }

        public string SubeSinavDosyaYol { get; set; }

        public string OgrenciSozlesmeDosyaYol { get; set; }

        public string SmsHesapDosyaYol { get; set; }

        public string GecerliTarihFormati { get; set; }

        public string GecerliTarihFormatiString { get; set; }

        public string GecerliTarihSaatFormati { get; set; }

        public string GecerliTarihSaatFormatiString { get; set; }

        public string PersonelErkekDefaultGorselYol { get; set; }

        public string PersonelKadinDefaultGorselYol { get; set; }

        public string SoruDogruRenkKodu { get; set; }

        public string SoruYanlisRenkKodu { get; set; }

        public string SoruBosRenkKodu { get; set; }

        public string SmsUserName { get; set; }

        public string SmsPassword { get; set; }

        public string SmsUrl { get; set; }

        public decimal SmsKrediTutar { get; set; }

        public int MinimumPesinatOrani { get; set; }

        public int MaksimumTaksitAdeti { get; set; }

        public string PayuMerchantName { get; set; }

        public string PayuSecretKey { get; set; }

        public string PayuMerchantName3D { get; set; }

        public string PayuSecretKey3D { get; set; }

        public string SiteUrl { get; set; }

        public string SiteUrlLocal { get; set; }
    }

    public static class AyarlarService
    {
        static string defaultPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\WebUI\\TumAyarlar\\Ayarlar"));

        static Ayarlar _ayarlar;

        static Ayarlar Read()
        {
            try
            {
        

                string ayarlarString;

                using (TextReader reader = File.OpenText(defaultPath))
                {
                    ayarlarString = reader.ReadToEnd();
                }

                _ayarlar = JsonConvert.DeserializeObject<Ayarlar>(ayarlarString);

                return _ayarlar;
            }
            catch (Exception ex)
            {
                return new Ayarlar();
            }
        }

        public static Ayarlar Get()
        {
            return _ayarlar ?? Read();
        }

        public static Ayarlar Update(Ayarlar ayarlar)
        {
            try
            {
                File.WriteAllText(defaultPath, JsonConvert.SerializeObject(ayarlar));
            }
            catch
            {
                // ignored
            }

            return ayarlar;
        }

        public static string ParaFormat(CultureInfo culture, decimal? tutar)
        {
            return string.Format(culture, "{0:C2}", tutar ?? 0).Replace(" ", "");
        }
    }

    public class PersonelSubeYetkiDto
    {
        public int PersonelId { get; set; }

        public int SubeId { get; set; }
    }

    public static class PersonelSubeYetkiService
    {
        static  string DefaultPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\WebUI\\TumAyarlar\\PersonelSubeYetki"));

        static List<PersonelSubeYetkiDto> _personelSubeYetkiler;

        static List<PersonelSubeYetkiDto> Read(int personelId)
        {
            try
            {
                string personelSubeYetkiString;

                var path = $"{DefaultPath}PersonelSubeYetki{personelId}";

                using (TextReader reader = File.OpenText(path))
                {
                    personelSubeYetkiString = reader.ReadToEnd();
                }

                _personelSubeYetkiler = JsonConvert.DeserializeObject<List<PersonelSubeYetkiDto>>(personelSubeYetkiString);

                return _personelSubeYetkiler;
            }
            catch (Exception)
            {
                return new List<PersonelSubeYetkiDto>();
            }
        }

        public static List<PersonelSubeYetkiDto> Get(int personelId)
        {
            _personelSubeYetkiler = Read(personelId);

            return _personelSubeYetkiler;
        }

        public static List<PersonelSubeYetkiDto> Update(List<PersonelSubeYetkiDto> personelSubeYetkiler, int personelId)
        {
            try
            {
                var path = $"{DefaultPath}PersonelSubeYetki{personelId}";

                File.WriteAllText(path, JsonConvert.SerializeObject(personelSubeYetkiler));
            }
            catch
            {
                // ignored
            }

            return personelSubeYetkiler;
        }
    }
}
