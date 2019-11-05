using Core.Properties;
using Core.General;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public enum SoruDurum
    {
        Dogru,
        Yanlis,
        Bos
    }

    public class OgrenciSinavKontrolSoru
    {
        public int Soru { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "OgrenciCevap")]
        public string OgrenciCevap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogruCevap")]
        public string DogruCevap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public SoruDurum Durum => string.Equals(OgrenciCevap, DogruCevap)
            ? SoruDurum.Dogru
            : string.IsNullOrEmpty(OgrenciCevap.Trim())
                ? SoruDurum.Bos
                : SoruDurum.Yanlis;

        public string Renk => Durum == SoruDurum.Dogru
            ? AyarlarService.Get().SoruDogruRenkKodu
            : Durum == SoruDurum.Yanlis
                ? AyarlarService.Get().SoruYanlisRenkKodu
                : AyarlarService.Get().SoruBosRenkKodu;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public string DurumAd => string.Equals(OgrenciCevap, DogruCevap)
          ? FieldNameResources.Dogru
          : string.IsNullOrEmpty(OgrenciCevap.Trim())
              ? FieldNameResources.Bos
              : FieldNameResources.Yanlis;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Konu")]
        public string KonuBaslik => Konu != null ? Konu.Baslik : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Konu")]
        public virtual Konu Konu { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public string DersAd => Ders != null ? Ders.DersAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Ders")]
        public virtual Ders Ders { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrup")]
        public string DersGrupAd => DersGrup != null ? DersGrup.DersGrupAd : string.Empty;

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersGrup")]
        public virtual DersGrup DersGrup { get; set; }
    }
}
