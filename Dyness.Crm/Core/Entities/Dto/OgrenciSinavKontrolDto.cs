using Core.Properties;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Dto
{
    public class OgrenciSinavKontrolDto
    {
        public int OgrenciSinavKontrolId { get; set; }

        public int? OgrenciId { get; set; }

        public int? OnKayitId { get; set; }

        public int SubeId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public string SubeAd { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kayit")]
        public bool OgrenciMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public string Durum => OgrenciMi ? FieldNameResources.Ogrenci : FieldNameResources.OnKayit;

        [Display(ResourceType = typeof(FieldNameResources), Name = "AdSoyad")]
        public string AdSoyad { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        public string TcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciNo")]
        public string OgrenciNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sinif")]
        public string Sinif { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogruCevapAdet")]
        public int DogruCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "YanlisCevapAdet")]
        public int YanlisCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "BosCevapAdet")]
        public int BosCevapAdet { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Net")]
        public double Net { get; set; }
    }
}
