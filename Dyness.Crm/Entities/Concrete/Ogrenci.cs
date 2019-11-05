using Core.General;
using Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Ogrenci
    {
        [Key, ForeignKey("Hesap")]
        public int OgrenciId { get; set; }

        [Index("IX_OgrenciNoSubeIdUnique", 1, IsUnique = true)]
        public int SubeId { get; set; }

        public int EkleyenPersonelId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiUlke")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? UlkeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiSehir")]
        public int? SehirId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiIlce")]
        public int? IlceId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AnneIletisimBilgi")]
        public int? AnneOgrenciYakiniIletisimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BabaIletisimBilgi")]
        public int? BabaOgrenciYakiniIletisimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DigerYakinIletisimBilgi")]
        public int? YakiniOgrenciYakiniIletisimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "NeredenDuydunuz")]
        public int? NeredenDuydunuzId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Soyad { get; set; }

        private string adSoyad;

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AdSoyad
        {
            get
            {
                return string.IsNullOrEmpty(adSoyad) ? $"{Ad} {Soyad}" : adSoyad;
            }
            set
            {
                adSoyad = $"{Ad} {Soyad}";
            }
        }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [Index("IX_OgrenciNoSubeIdUnique", 2, IsUnique = true)]
        public string OgrenciNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        [DefaultValue("****")]
        public string OgrenciSifre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        [DefaultValue("****")]
        public string VeliSifre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "CepTelefonu")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
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

        [Display(ResourceType = typeof(FieldNameResources), Name = "PostaKodu")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string PostaKodu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Gorsel")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string GorselDosyaAd { get; set; }

        public string GorselYol => !string.IsNullOrEmpty(GorselDosyaAd)
            ? $"{ AyarlarService.Get().PersonelGorselYol}{GorselDosyaAd}"
            : $"{ AyarlarService.Get().PersonelGorselYol}{(KadinMi ? AyarlarService.Get().PersonelKadinDefaultGorselYol : AyarlarService.Get().PersonelErkekDefaultGorselYol)}";

        [Display(ResourceType = typeof(FieldNameResources), Name = "Not")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Not { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        [NotMapped]
        public string Cinsiyet => KadinMi ? FieldNameResources.Kadin : FieldNameResources.Erkek;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        public bool KadinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IletisimKendi")]
        public bool IletisimKendi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IletisimAnne")]
        public bool IletisimAnne { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IletisimBaba")]
        public bool IletisimBaba { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "IletisimDigerYakini")]
        public bool IletisimYakini { get; set; }

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

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSonGirisTarihi")]
        [NotMapped]
        public string OgrenciSonGirisTarihiFormatted => OgrenciSonGirisTarihi != null
            ? OgrenciSonGirisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
            : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciSonGirisTarihi")]
        [DataType(DataType.Date)]
        public DateTime? OgrenciSonGirisTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "VeliSonGirisTarihi")]
        [NotMapped]
        public string VeliSonGirisTarihiFormatted => OgrenciSonGirisTarihi != null
           ? VeliSonGirisTarihi.Value.Date.ToString(AyarlarService.Get().GecerliTarihFormati)
           : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "VeliSonGirisTarihi")]
        [DataType(DataType.Date)]
        public DateTime? VeliSonGirisTarihi { get; set; }

        public virtual Sube Sube { get; set; }

        [ForeignKey("EkleyenPersonelId")]
        [Display(ResourceType = typeof(FieldNameResources), Name = "EkleyenPersonel")]
        public virtual Personel EkleyenPersonel { get; set; }

        public virtual Hesap Hesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiUlke")]
        [NotMapped]
        public string YasadigiUlkeAd => Ulke != null ? Ulke.UlkeAd : string.Empty;

        public virtual Ulke Ulke { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiSehir")]
        [NotMapped]
        public string YasadigiSehirAd => Sehir != null ? Sehir.SehirAd : string.Empty;

        public virtual Sehir Sehir { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YasadigiIlce")]
        [NotMapped]
        public string YasadigiIlceAd => Ilce != null ? Ilce.IlceAd : string.Empty;

        public virtual Ilce Ilce { get; set; }

        [ForeignKey("AnneOgrenciYakiniIletisimId")]
        public virtual OgrenciYakiniIletisim AnneOgrenciYakiniIletisim { get; set; }

        [ForeignKey("BabaOgrenciYakiniIletisimId")]
        public virtual OgrenciYakiniIletisim BabaOgrenciYakiniIletisim { get; set; }

        [ForeignKey("YakiniOgrenciYakiniIletisimId")]
        public virtual OgrenciYakiniIletisim YakiniOgrenciYakiniIletisim { get; set; }

        public virtual NeredenDuydunuz NeredenDuydunuz { get; set; }

        public virtual List<OgrenciSozlesme> OgrenciSozlesmeler { get; set; }

        public virtual List<OgrenciFaturaBilgi> OgrenciFaturaBilgiler { get; set; }

        public virtual List<OgrenciSinavKontrol> OgrenciSinavKontroller { get; set; }
    }
}
