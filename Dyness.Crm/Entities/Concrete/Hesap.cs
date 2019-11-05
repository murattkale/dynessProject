using Core.General;
using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Entities.Concrete
{
    public class Hesap
    {
        public int HesapId { get; set; }

        public int? BagliKurumId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sube")]
        public int? UstHesapId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapTur")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        public int? HesapTurId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TransferTip")]
        public int? TransferTipId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        public int ParaBirimId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Hesap")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsRequired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string HesapBaslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kod")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string HesapKod { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        public string ToplamBorcFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamBorc);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamBorc")]
        [NotMapped]
        public decimal ToplamBorc => HesapBilgiler != null
            ? HesapBilgiler.Where(x => x.Ay == null && x.Yil == null).Sum(x => x.ToplamBorc)
            : 0;

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamAlacak")]
        public string ToplamAlacakFormatted => AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamAlacak);

        [Display(ResourceType = typeof(FieldNameResources), Name = "ToplamAlacak")]
        [NotMapped]
        public decimal ToplamAlacak => HesapBilgiler != null
            ? HesapBilgiler.Where(x => x.Ay == null && x.Yil == null).Sum(x => x.ToplamAlacak)
            : 0;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Toplam")]
        public string ToplamFormatted => UstHesapId != null
            ? string.Empty
            : AyarlarService.ParaFormat(GecerliParaBirimCulture, ToplamAlacak - ToplamBorc);

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        [NotMapped]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Kurum")]
        [NotMapped]
        public string BagliKurumAd => BagliKurum != null ? BagliKurum.KurumAd : string.Empty;

        [ForeignKey("BagliKurumId")]
        public virtual Kurum BagliKurum { get; set; }

        [ForeignKey("UstHesapId")]
        public virtual Hesap UstHesap { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "HesapTur")]
        [NotMapped]
        public string HesapTurAd => HesapTur != null ? HesapTur.HesapTurAd : string.Empty;

        public virtual HesapTur HesapTur { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "ParaBirim")]
        [NotMapped]
        public string ParaBirimAd => ParaBirim != null ? ParaBirim.ParaBirimAd : string.Empty;

        public virtual ParaBirim ParaBirim { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TransferTip")]
        [NotMapped]
        public string TransferTipAd => TransferTip != null ? TransferTip.TransferTipAd : string.Empty;

        public virtual TransferTip TransferTip { get; set; }

        public virtual Personel Personel { get; set; }

        public virtual Ogrenci Ogrenci { get; set; }

        public virtual Sube Sube { get; set; }

        public virtual BankaHesap BankaHesap { get; set; }

        public virtual List<Hesap> AltHesaplar { get; set; }

        [NotMapped]
        public List<HesapHareket> HesapHareketler { get; set; }

        [NotMapped]
        public List<HesapBilgi> HesapBilgiler { get; set; }

        [NotMapped]
        public CultureInfo GecerliParaBirimCulture => ParaBirim != null
            ? CultureInfo.GetCultureInfo(ParaBirim.KulturKod)
            : CultureInfo.GetCultureInfo("tr-TR");

        [NotMapped]
        public bool Odenebilir { get; set; }
    }
}
