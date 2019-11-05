using Core.General;
using System;
using System.Threading;

namespace Core.CrossCuttingConcerns.Security
{
    public enum RolModel
    {
        Personel = 1,
        Ogrenci = 2
    }

    public class Identity
    {
        public static int PersonelId
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 0)
                {
                    string value = Thread.CurrentPrincipal.Identity.Name.Split('|')[0];
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int KurumId
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 1)
                {
                    string value = Thread.CurrentPrincipal.Identity.Name.Split('|')[1];
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int SubeId
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 2)
                {
                    string value = Thread.CurrentPrincipal.Identity.Name.Split('|')[2];
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int PersonelYetkiGrupId
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 3)
                {
                    string value = Thread.CurrentPrincipal.Identity.Name.Split('|')[3];
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static string KullaniciAd
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 4)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[4];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string Eposta
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 5)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[5];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string AdSoyad
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 6)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[6];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string GorselYol
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 7)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[7];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string PersonelGrupAd
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 8)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[8];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string PersonelYetkiGrupAd
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 9)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[9];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string UlkeAd
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 10)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[10];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string SonGirisTarihiString
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 11)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[11];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static DateTime? SonGirisTarihi => !string.IsNullOrEmpty(SonGirisTarihiString)
            ? DateTime.ParseExact(SonGirisTarihiString, AyarlarService.Get().GecerliTarihSaatFormati, null)
            : (DateTime?)null;

        public static int CevrimIciDakika => SonGirisTarihi != null
            ? (DateTime.Now - (DateTime)SonGirisTarihi).Minutes
            : 0;

        public int SonKontrolDakika { get; set; }

        public static string KurumLogoYol
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 12)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[12];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static RolModel? Rol
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 13)
                {
                    string value = Thread.CurrentPrincipal.Identity.Name.Split('|')[13];
                    return value == "Ogrenci"
                        ? RolModel.Ogrenci 
                        : value == "Personel"
                            ? RolModel.Personel 
                            : (RolModel?)null;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool VeliMi
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated &&
                    Thread.CurrentPrincipal.Identity.Name.Split('|').Length > 14)
                {
                    return Thread.CurrentPrincipal.Identity.Name.Split('|')[14] == "1";
                }
                else
                {
                    return false;
                }
            }
        }
    }
}