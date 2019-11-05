using Entities.Concrete;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Data.Concrete.EntityFramework.Context
{
    public class EFContext : DbContext
    {
        public EFContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<EFContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = false;

            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Hesap>().HasOptional(s => s.BankaHesap).WithRequired(ad => ad.Hesap);
        }

        public DbSet<Banka> Bankalar { get; set; }
        public DbSet<BankaHesap> BankaHesaplar { get; set; }
        public DbSet<Brans> Branslar { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<DersGrup> DersGruplar { get; set; }
        public DbSet<Derslik> Derslikler { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<DuyuruPersonelBilgi> DuyuruPersonelBilgiler { get; set; }
        public DbSet<EhliyetTur> EhliyetTurler { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<EskiKayit> EskiKayitlar { get; set; }
        public DbSet<FaturaBilgi> FaturaBilgiler { get; set; }
        public DbSet<Hesap> Hesaplar { get; set; }
        public DbSet<HesapBilgi> HesapBilgiler { get; set; }
        public DbSet<HesapHareket> HesapHareketler { get; set; }
        public DbSet<HesapTur> HesapTurler { get; set; }
        public DbSet<HesapTurGrup> HesapTurGruplar { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Kiyafet> Kiyafetler { get; set; }
        public DbSet<KiyafetBeden> KiyafetBedenler { get; set; }
        public DbSet<KiyafetTur> KiyafetTurlar { get; set; }
        public DbSet<Konu> Konular { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kurum> Kurumlar { get; set; }
        public DbSet<KurumOgrenciSozlesmeMetin> KurumOgrenciSozlesmeMetinler { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }
        public DbSet<NeredenDuydunuz> NeredenDuydunuzlar { get; set; }
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<OgrenciFaturaBilgi> OgrenciFaturaBilgiler { get; set; }
        public DbSet<OgrenciSozlesme> OgrenciSozlesmeler { get; set; }
        public DbSet<OgrenciSinavKontrol> OgrenciSinavKontrollar { get; set; }
        public DbSet<OgrenciSinavKontrolPuanTurPuan> OgrenciSinavKontrolPuanTurPuanlar { get; set; }
        public DbSet<OgrenciSinavKontrolDersBilgi> OgrenciSinavKontrolDersBilgiler { get; set; }
        public DbSet<OgrenciSozlesmeOdemeBilgi> OgrenciSozlesmeOdemeBilgiler { get; set; }
        public DbSet<OgrenciSozlesmeOdemeBilgiSenetImzalayan> OgrenciSozlesmeOdemeBilgiSenetImzalayanlar { get; set; }
        public DbSet<OgrenciSozlesmeDersSecim> OgrenciSozlesmeDersSecimler { get; set; }
        public DbSet<OgrenciSozlesmeHesapHareket> OgrenciSozlesmeHesapHareketler { get; set; }
        public DbSet<OgrenciSozlesmeKiyafetDurum> OgrenciSozlesmeKiyafetDurumlar { get; set; }
        public DbSet<OgrenciSozlesmeTur> OgrenciSozlesmeTurlar { get; set; }
        public DbSet<OgrenciSozlesmeYayin> OgrenciSozlesmeYayinlar { get; set; }
        public DbSet<OgrenciYakiniIletisim> OgrenciYakınIletisimler { get; set; }
        public DbSet<OkulTur> OkulTurler { get; set; }
        public DbSet<OptikForm> OptikFormlar { get; set; }
        public DbSet<OptikFormDersGrupBilgi> OptikFormDersGrupBilgiler { get; set; }
        public DbSet<OzelDersDurum> OzelDersDurumlar { get; set; }
        public DbSet<ParaBirim> ParaBirimler { get; set; }
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<PersonelGrup> PersonelGruplar { get; set; }
        public DbSet<PersonelSubeDers> PersonelSubeDersler { get; set; }
        public DbSet<PersonelSubeUcret> PersonelSubeUcretler { get; set; }
        public DbSet<PersonelSubeYetki> PersoneliSubeYetkiler { get; set; }
        public DbSet<PersonelYetki> PersonelYetkiler { get; set; }
        public DbSet<PersonelYetkiGrup> PersonelYetkiGruplar { get; set; }
        public DbSet<PersonelPuantaj> PersonelPuantajlar { get; set; }
        public DbSet<PersonelPuantajGunluk> PersonelPuantajGunlukler { get; set; }
        public DbSet<PersonelPuantajGunlukDurum> PersonelPuantajGunlukDurumlar { get; set; }
        public DbSet<PuanTur> PuanTurler { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }
        public DbSet<Servis> Servisler { get; set; }
        public DbSet<Sezon> Sezonlar { get; set; }
        public DbSet<Sinav> Sinavlar { get; set; }
        public DbSet<SinavKitapcik> SinavKitapciklar { get; set; }
        public DbSet<SinavKitapcikDersBilgi> SinavKitapcikDersBilgiler { get; set; }
        public DbSet<SinavSube> SinavSubeler { get; set; }
        public DbSet<SinavTurDersKatSayi> SinavTurDersKatSayilar { get; set; }
        public DbSet<SinavTur> SinavTurler { get; set; }
        public DbSet<SinavTurDers> SinavTurDersler { get; set; }
        public DbSet<Sinif> Siniflar { get; set; }
        public DbSet<SinifSeans> SinifSeanslar { get; set; }
        public DbSet<SinifSeviye> SinifSeviyeler { get; set; }
        public DbSet<SinifTur> SinifTurler { get; set; }
        public DbSet<Sms> Smsler { get; set; }
        public DbSet<SmsDurum> SmsDurumlar { get; set; }
        public DbSet<SmsHesap> SmsHesaplar { get; set; }
        public DbSet<SmsHesapHareket> SmsHesapHareketler { get; set; }
        public DbSet<SmsHesapHareketTip> SmsHesapHareketTipler { get; set; }
        public DbSet<SmsHesapDosya> SmsHesapDosyalar { get; set; }
        public DbSet<SmsHesapDurum> SmsHesapDurumlar { get; set; }
        public DbSet<SmsMetinSablon> SmsMetinSablonlar { get; set; }
        public DbSet<Sube> Subeler { get; set; }
        public DbSet<TransferTip> TransferTipler { get; set; }
        public DbSet<Ulke> Ulkeler { get; set; }
        public DbSet<Video> Videolar { get; set; }
        public DbSet<VideoKategori> VideoKategoriler { get; set; }
        public DbSet<VideoKonu> VideoKonular { get; set; }
        public DbSet<VideoKurumYetki> VideoKurumYetkiler { get; set; }
        public DbSet<VideoSinifYetki> VideoSinifYetkiler { get; set; }
        public DbSet<VideoSubeYetki> VideoSubeYetkiler { get; set; }
        public DbSet<VideoVideoKategori> VideoVideoKategoriler { get; set; }
        public DbSet<Yayin> Yayinlar { get; set; }
        public DbSet<YetkiMethod> YetkilendirmeMethodlar { get; set; }
    }
}
