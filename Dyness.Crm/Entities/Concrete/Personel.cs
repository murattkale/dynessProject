using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Concrete
{
    public class Personel
    {
        [Key, ForeignKey("Hesap")]
        public int PersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelGrup")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int PersonelGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelYetkiGrup")]
        public int? PersonelYetkiGrupId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "GecerliSube")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public int? DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiUlke")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? UlkeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Soyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [NotMapped]
        public string AdSoyad => $"{Ad} {Soyad}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [NotMapped]
        public string GrupAdSoyad => $"{(PersonelGrup != null ? $"{PersonelGrup.PersonelGrupAd} - " : string.Empty)}{Ad} {Soyad}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefonu")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string CepTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Telefon")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Telefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Eposta")]
        [MaxLength(254, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsEmail")]
        public string Eposta { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Adres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Adres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gorsel")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string GorselDosyaAd { get; set; }

        public string GorselYol => !string.IsNullOrEmpty(GorselDosyaAd)
            ? $"{ AyarlarService.Get().PersonelGorselYol}{GorselDosyaAd}"
            : $"{ AyarlarService.Get().PersonelGorselYol}{(KadinMi ? AyarlarService.Get().PersonelKadinDefaultGorselYol : AyarlarService.Get().PersonelErkekDefaultGorselYol)}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Not { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YemekKartNo")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string YemekKartNo { get; set; }

        public string DersUcretiFormatted => DersUcreti != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, DersUcreti)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersUcreti")]
        public decimal? DersUcreti { get; set; }

        public string MaasFormatted => Maas != null
          ? AyarlarService.ParaFormat(GecerliParaBirimCulture, Maas)
          : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Maas")]
        public decimal? Maas { get; set; }

        public string GunlukYemekUcretiFormatted => Maas != null
            ? AyarlarService.ParaFormat(GecerliParaBirimCulture, GunlukYemekUcreti)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "GunlukYemekUcreti")]
        public decimal? GunlukYemekUcreti { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        [NotMapped]
        public string Cinsiyet => KadinMi ? FieldNameResources.Kadin : FieldNameResources.Erkek;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public bool KadinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PrimdenFaydalansinMi")]
        [NotMapped]
        public string PrimdenFaydalansinMiDurum => PrimdenFaydalansinMi ? FieldNameResources.PrimdenFaydalanabilir : FieldNameResources.PrimdenFaydalanamaz;

        [Display(ResourceType = typeof(FieldNameResources), Name = "PrimdenFaydalansinMi")]
        public bool PrimdenFaydalansinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [NotMapped]
        public string DogumTarihiFormatted => DogumTarihi != null
            ? DogumTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [DataType(DataType.Date)]
        public DateTime? DogumTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [NotMapped]
        public string OlusturulmaTarihiFormatted => OlusturulmaTarihi != null
            ? OlusturulmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OlusturulmaTarihi")]
        [DataType(DataType.Date)]
        public DateTime? OlusturulmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IseBaslamaTarihi")]
        [NotMapped]
        public string IseBaslamaTarihiFormatted => IseBaslamaTarihi != null
            ? IseBaslamaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "IseBaslamaTarihi")]
        [DataType(DataType.Date)]
        public DateTime? IseBaslamaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IstenAyrilmaTarihi")]
        [NotMapped]
        public string IstenAyrilmaTarihiFormatted => IstenAyrilmaTarihi != null
            ? IstenAyrilmaTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "IstenAyrilmaTarihi")]
        [DataType(DataType.Date)]
        public DateTime? IstenAyrilmaTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CalismaDurum")]
        public bool CalisiyorMu => IstenAyrilmaTarihi == null;

        [Display(ResourceType = typeof(FieldNameResources), Name = "CalismaDurum")]
        public string CalismaDurum => CalisiyorMu ? FieldNameResources.Calisiyor : FieldNameResources.Ayrildi;

        [Display(ResourceType = typeof(FieldNameResources), Name = "IstenAyrilmaTarihi")]
        [NotMapped]
        public string SonGirisTarihiFormatted => Kullanici?.SonGirisTarihi != null
           ? Kullanici.SonGirisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kullanici")]
        public bool KullaniciMi => Kullanici != null;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kullanici")]
        public string KullaniciDurum => KullaniciMi ? FieldNameResources.Kullanici : FieldNameResources.KullaniciDegil;

        public virtual Kullanici Kullanici { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        [NotMapped]
        public string HesapBaslik => Hesap != null ? Hesap.HesapBaslik : string.Empty;

        public virtual Hesap Hesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelGrup")]
        [NotMapped]
        public string PersonelGrupAd => PersonelGrup != null ? PersonelGrup.PersonelGrupAd : string.Empty;

        public virtual PersonelGrup PersonelGrup { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "PersonelYetkiGrup")]
        [NotMapped]
        public string PersonelYetkiGrupAd => PersonelYetkiGrup != null ? PersonelYetkiGrup.PersonelYetkiGrupAd : string.Empty;

        public virtual PersonelYetkiGrup PersonelYetkiGrup { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        [NotMapped]
        public string KurumAd => Sube?.Kurum != null ? Sube.Kurum.KurumAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [NotMapped]
        public string SubeAd => Sube != null ? Sube.SubeAd : string.Empty;

        public virtual Sube Sube { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersAd")]
        [NotMapped]
        public string DersAd => Ders != null ? Ders.DersAd : string.Empty;

        public virtual Ders Ders { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiUlke")]
        [NotMapped]
        public string YasadigiUlkeAd => Ulke != null ? Ulke.UlkeAd : string.Empty;

        public virtual Ulke Ulke { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersVerdigiSubeler")]
        [NotMapped]
        public string PersonelSubeDerslerDisplayName { get; set; }

        public virtual List<PersonelSubeDers> PersonelSubeDersler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SubeUcretleri")]
        public virtual List<PersonelSubeUcret> PersonelSubeUcretler { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YetkiliOlduguSubeler")]
        [NotMapped]
        public string PersonelSubeYetkilerDisplayName { get; set; }

        public virtual List<PersonelSubeYetki> PersonelSubeYetkiler { get; set; }

        public CultureInfo GecerliParaBirimCulture => Sube?.ParaBirim != null
            ? CultureInfo.GetCultureInfo(Sube.ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");
    }
}
