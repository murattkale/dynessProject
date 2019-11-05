using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class OgrenciSinavKontrol
    {
        public int OgrenciSinavKontrolId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ogrenci")]
        public int? OgrenciId { get; set; }

        public int? OnKayitId { get; set; }

        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavKitapcik")]
        public int SinavKitapcikId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Ad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Soyad")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Soyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [NotMapped]
        public string AdSoyadSon => Ogrenci != null ? Ogrenci.AdSoyad : AdSoyad;

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string AdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string OgrenciNo { get; set; }

        private string ogrenciNoSon;

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [NotMapped]
        public string OgrenciNoSon
        {
            get
            {
                return ogrenciNoSon;
            }
            set
            {
                if (Ogrenci != null)
                    ogrenciNoSon = Ogrenci.OgrenciNo;
                else
                    ogrenciNoSon = OgrenciNo;
            }
        }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Sinif { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Cinsiyet")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Cinsiyet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SinavKitapcik")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string KitapcikBaslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoruCevaplar")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string SoruCevaplar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Dogrulamalar")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        public string Dogrulamalar { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "SoruAdet")]
        public int SoruAdet => DogruCevapAdet + BosCevapAdet + YanlisCevapAdet;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogruCevapAdet")]
        public int DogruCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YanlisCevapAdet")]
        public int YanlisCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BosCevapAdet")]
        public int BosCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public double Net { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public string NetFormatted => string.Format("{0:0.000}", Net);

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public double Basari => 100 * DogruCevapAdet / SoruAdet;

        [Display(ResourceType = typeof(FieldNameResources), Name = "TabanPuan")]
        public double TabanPuan { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public string Durum => OgrenciId != null ? FieldNameResources.Ogrenci : FieldNameResources.OnKayit;

        public virtual Ogrenci Ogrenci { get; set; }

        public virtual OnKayit OnKayit { get; set; }

        public virtual Sube Sube { get; set; }

        public virtual SinavKitapcik SinavKitapcik { get; set; }

        public virtual List<OgrenciSinavKontrolDersBilgi> OgrenciSinavKontrolDersBilgiler { get; set; }

        public virtual List<OgrenciSinavKontrolPuanTurPuan> OgrenciSinavKontrolPuanTurPuanlar { get; set; }

        [NotMapped]
        public virtual List<OgrenciSinavKontrolSoru> Sorular { get; set; }
    }
}
