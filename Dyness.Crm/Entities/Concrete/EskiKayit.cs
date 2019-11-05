using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class EskiKayit
    {
        public int EskiKayitId { get; set; }

        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public string SubeAd => Sube != null ? Sube.SubeAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESSozlesmeDurumu")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SozlesmeDurumu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESTckn")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciTckn { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciId")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciNo")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciAdi")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciAdi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciSoyadi")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciSoyadi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciTelefon")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciTelefon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciAdres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciAdres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOgrenciEposta")]
        [MaxLength(250, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciEposta { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESServisBilgisi")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string ServisBilgisi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESNakit")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Nakit { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESCek")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Cek { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKrediKartiPosCihazi")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KrediKartiPosCihazi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESHavale")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Havale { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKrediKartiSanalPos")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KrediKartiSanalPos { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESMailOrder")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string MailOrder { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKayitUcreti")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KayitUcreti { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKalanOdeme")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KalanOdeme { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESOdemeTuru")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OdemeTuru { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKayitTarihi")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KayitTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESBiziNeredenDuydunuz")]
        [MaxLength(250, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string BiziNeredenDuydunuz { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESSinifSeviyesi")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SinifSeviyesi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESSinif")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Sinif { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESBrans")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Brans { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESSezon")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Sezon { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SubeBilgi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESAdres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Adres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESIlce")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ilce { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESIl")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Il { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESUlke")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ulke { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESVeliAnne")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VeliAnne { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESAnneTckn")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AnneTckn { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESAnneTel")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AnneTel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESVeliBaba")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VeliBaba { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESBabaTckn")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string BabaTckn { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESBabaTel")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string BabaTel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESVeliDiger")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VeliDiger { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESDigerTckn")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DigerTckn { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESDigerTel")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string DigerTel { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaAdSoyad")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaAdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESVergiDairesi")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VergiDairesi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESVergiTckNo")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string VergiTckNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaAdres")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaAdres { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaSemt")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaSemt { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaIlce")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaIlce { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaSehir")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaSehir { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESFaturaPostaKodu")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string FaturaPostaKodu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESGorusen")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Gorusen { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESKaydiYapan")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KaydiYapan { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ESReferans")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Referans { get; set; }

        public virtual Sube Sube { get; set; }
    }
}
