using Core.Entities.Dto;
using Core.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities.Concrete
{
    public class Sms
    {
        public int SmsId { get; set; }

        [NotMapped]
        public Tip Tip { get; set; }

        [NotMapped]
        public Bilgi Bilgi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsDurum")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SmsDurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int SmsHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsMetinSablon")]
        public int? SmsMetinSablonId { get; set; }

        public int DlrId { get; set; }

        public int? OgrenciId { get; set; }

        public int? PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GecerlilikSuresi")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [Range(0, 2890)]
        public int GecerlilikSuresi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KarakterSayisi")]
        [NotMapped]
        public int KarakterAdet { get; private set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsAdet")]
        public int SmsAdet { get; private set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "KrediAdet")]
        public int KrediAdet { get; private set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string TelefonNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsMesaj")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(917, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Mesaj { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AdSoyad { get; set; }

        [NotMapped]
        public int MesajOlcum
        {
            get
            {
                var duzenlenenMesaj = Mesaj.Replace("|", "I").Replace("€", "E").Replace("{", "(").Replace("[", "(").Replace("}", ")").Replace("]", ")").Replace("\\", "/");

                var karakterler = "ABCDEFGĞHIİJKLMNOPRSŞTUVYZÖÜQWXÇ abcçdefgğhıijklmnoprsştuvyzöüqwx@*:!$_#()+-;,<>=./'?\"%&0123456789";

                var yeniMesaj = "";

                for (int i = 0; i < duzenlenenMesaj.Length; i++)
                {
                    if (karakterler.IndexOf(duzenlenenMesaj[i].ToString()) == -1)
                        yeniMesaj += " ";
                    else
                        yeniMesaj += duzenlenenMesaj[i].ToString();
                }

                var karakterHesaplaYeniMesaj = yeniMesaj;
                var karakterAdet = 0;
                var ciftKarakterler = "üÜöÖÇ";

                for (int i = 0; i < karakterHesaplaYeniMesaj.Length; i++)
                {
                    if (ciftKarakterler.IndexOf(karakterHesaplaYeniMesaj[i].ToString()) > 0)
                        karakterAdet += 2;
                    else
                        karakterAdet++;

                }

                KarakterAdet = karakterAdet;

                if (!TurkceMi)
                {
                    if (KarakterAdet > 0 && KarakterAdet < 161)
                    {
                        SmsAdet = 1;
                    }
                    else if (KarakterAdet > 160 && KarakterAdet < 307)
                    {
                        SmsAdet = 2;
                    }
                    else if (KarakterAdet > 306 && KarakterAdet < 460)
                    {
                        SmsAdet = 3;
                    }
                    else if (KarakterAdet > 456 && KarakterAdet < 613)
                    {
                        SmsAdet = 4;
                    }
                    else if (KarakterAdet > 612 && KarakterAdet < 766)
                    {
                        SmsAdet = 5;
                    }
                    else if (KarakterAdet > 765 && KarakterAdet < 918)
                    {
                        SmsAdet = 6;
                    }
                    else
                    {
                        SmsAdet = -1;
                    }
                }
                else
                {
                    if (KarakterAdet > 0 && KarakterAdet < 156)
                    {
                        SmsAdet = 1;
                    }
                    else if (KarakterAdet > 155 && KarakterAdet < 293)
                    {
                        SmsAdet = 2;
                    }
                    else if (KarakterAdet > 292 && KarakterAdet < 440)
                    {
                        SmsAdet = 3;
                    }
                    else if (KarakterAdet > 439 && KarakterAdet < 588)
                    {
                        SmsAdet = 4;
                    }
                    else if (KarakterAdet > 587 && KarakterAdet < 736)
                    {
                        SmsAdet = 5;
                    }
                    else if (KarakterAdet > 735 && KarakterAdet < 883)
                    {
                        SmsAdet = 6;
                    }
                    else
                    {
                        SmsAdet = -1;
                    }
                }

                KrediAdet = SmsAdet;
                return KrediAdet;
            }
        }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GonderilecegiTarih")]
        public DateTime? GonderilecegiTarih { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GonderilecegiTarih")]
        [NotMapped]
        public string GonderilecegiTarihFormatted => GonderilecegiTarih != null
            ? $"{GonderilecegiTarih.Value.Year}.{GonderilecegiTarih.Value.Month}.{GonderilecegiTarih.Value.Day}.{GonderilecegiTarih.Value.Hour}.{GonderilecegiTarih.Value.Minute}.{GonderilecegiTarih.Value.Second}"
            : "";

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsTurkceMi")]
        [NotMapped]
        public bool TurkceMi => !string.IsNullOrWhiteSpace(Mesaj) && Mesaj.ToCharArray().Intersect(new char[] { 'ü', 'Ü', 'ö', 'Ö', 'Ç', 'ğ', 'Ğ', 'ş', 'Ş', 'ı', 'İ', 'ç' }).Any();

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsDurum")]
        public virtual SmsDurum SmsDurum { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsHesap")]
        public virtual SmsHesap SmsHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SmsMetinSablon")]
        public virtual SmsMetinSablon SmsMetinSablon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ogrenci")]
        public virtual Ogrenci Ogrenci { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Personel")]
        public virtual Personel Personel { get; set; }

        [NotMapped]
        public virtual SmsHesapHareket SmsHesapHareket { get; set; }

    }
}
