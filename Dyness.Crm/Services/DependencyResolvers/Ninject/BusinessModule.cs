using Data.Abstract.Dapper;
using Data.Abstract.EntityFramework;
using Data.Concrete.Dapper;
using Data.Concrete.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Services.Abstract;
using Services.Concrete;
using System.Data.Entity;

namespace Services.DependencyResolvers.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceFactory>().ToFactory();
            Bind<DbContext>().To<EFContext>();

            #region Banka

            Bind<IEfBankaData>().To<EfBankaData>();
            Bind<IBankaService>().To<BankaManager>();

            #endregion

            #region BankaHesap

            Bind<IEfBankaHesapData>().To<EfBankaHesapData>();
            Bind<IBankaHesapService>().To<BankaHesapManager>();

            #endregion

            #region Brans

            Bind<IEfBransData>().To<EfBransData>();
            Bind<IBransService>().To<BransManager>();

            #endregion

            #region Ders

            Bind<IEfDersData>().To<EfDersData>();
            Bind<IDersService>().To<DersManager>();

            #endregion

            #region DersGrup

            Bind<IEfDersGrupData>().To<EfDersGrupData>();
            Bind<IDersGrupService>().To<DersGrupManager>();

            #endregion

            #region Derslik

            Bind<IEfDerslikData>().To<EfDerslikData>();
            Bind<IDerslikService>().To<DerslikManager>();

            #endregion

            #region EhliyetTur

            Bind<IEfEhliyetTurData>().To<EfEhliyetTurData>();
            Bind<IEhliyetTurService>().To<EhliyetTurManager>();

            #endregion

            #region EskiKayit

            Bind<IEfEskiKayitData>().To<EfEskiKayitData>();
            Bind<IEskiKayitService>().To<EskiKayitManager>();

            #endregion


            #region Etkinlik

            Bind<IEfEtkinlikData>().To<EfEtkinlikData>();
            Bind<IEtkinlikService>().To<EtkinlikManager>();

            #endregion

            #region FaturaBilgi

            Bind<IEfFaturaBilgiData>().To<EfFaturaBilgiData>();
            Bind<IFaturaBilgiService>().To<FaturaBilgiManager>();

            #endregion

            #region Hesap

            Bind<IDpHesapData>().To<DpHesapData>();
            Bind<IDpOgrenciHesapData>().To<DpOgrenciHesapData>();
            Bind<IEfHesapData>().To<EfHesapData>();
            Bind<IHesapService>().To<HesapManager>();

            #endregion

            #region HesapBilgi

            Bind<IEfHesapBilgiData>().To<EfHesapBilgiData>();
            Bind<IHesapBilgiService>().To<HesapBilgiManager>();

            #endregion

            #region HesapHareket

            Bind<IEfHesapHareketData>().To<EfHesapHareketData>();
            Bind<IDpHesapHareketData>().To<DpHesapHareketData>();
            Bind<IHesapHareketService>().To<HesapHareketManager>();

            #endregion

            #region HesapTur

            Bind<IEfHesapTurData>().To<EfHesapTurData>();
            Bind<IHesapTurService>().To<HesapTurManager>();

            #endregion

            #region HesapTurGrup

            Bind<IEfHesapTurGrupData>().To<EfHesapTurGrupData>();
            Bind<IHesapTurGrupService>().To<HesapTurGrupManager>();

            #endregion

            #region Ilce

            Bind<IEfIlceData>().To<EfIlceData>();
            Bind<IIlceService>().To<IlceManager>();

            #endregion

            #region Konu

            Bind<IEfKonuData>().To<EfKonuData>();
            Bind<IKonuService>().To<KonuManager>();

            #endregion

            #region Kullanici

            Bind<IEfKullaniciData>().To<EfKullaniciData>();
            Bind<IKullaniciService>().To<KullaniciManager>();

            #endregion

            #region Kurum

            Bind<IEfKurumData>().To<EfKurumData>();
            Bind<IKurumService>().To<KurumManager>();

            #endregion

            #region KiyafetTur

            Bind<IEfKiyafetTurData>().To<EfKiyafetTurData>();
            Bind<IKiyafetTurService>().To<KiyafetTurManager>();

            #endregion

            #region KiyafetTur

            Bind<IEfKiyafetBedenData>().To<EfKiyafetBedenData>();
            Bind<IKiyafetBedenService>().To<KiyafetBedenManager>();

            #endregion

            #region Kiyafet

            Bind<IEfKiyafetData>().To<EfKiyafetData>();
            Bind<IKiyafetService>().To<KiyafetManager>();

            #endregion

            #region NeredenDuydunuz

            Bind<IEfNeredenDuydunuzData>().To<EfNeredenDuydunuzData>();
            Bind<INeredenDuydunuzService>().To<NeredenDuydunuzManager>();

            #endregion

            #region Optik Form

            Bind<IEfOptikFormData>().To<EfOptikFormData>();
            Bind<IOptikFormService>().To<OptikFormManager>();

            #endregion

            #region Ogrenci

            Bind<IEfOgrenciData>().To<EfOgrenciData>();
            Bind<IDpOgrenciData>().To<DpOgrenciData>();
            Bind<IDpOgrenciTelefonData>().To<DpOgrenciTelefonData>();
            Bind<IOgrenciService>().To<OgrenciManager>();

            #endregion

            #region OgrenciOzelDersSecim

            Bind<IEfOgrenciSozlesmeDersSecimData>().To<EfOgrenciSozlesmeDersSecimData>();

            #endregion

            #region OgrenciSinavKontrol

            Bind<IEfOgrenciSinavKontrolData>().To<EfOgrenciSinavKontrolData>();
            Bind<IDpOgrenciSinavKontrolData>().To<DpOgrenciSinavKontrolData>();
            Bind<IOgrenciSinavKontrolService>().To<OgrenciSinavKontrolManager>();

            #endregion

            #region OgrenciSinavKontrolPuanTurPuan

            Bind<IDpOgrenciSinavKontrolPuanTurPuanData>().To<DpOgrenciSinavKontrolPuanTurPuanData>();
            Bind<IOgrenciSinavKontrolPuanTurPuanService>().To<OgrenciSinavKontrolPuanTurPuanManager>();

            #endregion

            #region OgrenciSozlesme

            Bind<IEfOgrenciSozlesmeData>().To<EfOgrenciSozlesmeData>();
            Bind<IOgrenciSozlesmeService>().To<OgrenciSozlesmeManager>();
            Bind<IDpOgrenciSozlesmeData>().To<DpOgrenciSozlesmeData>();

            #endregion

            #region OgrenciSozlesmeHesapHareket

            Bind<IEfOgrenciSozlesmeHesapHareketData>().To<EfOgrenciSozlesmeHesapHareketData>();
            Bind<IOgrenciSozlesmeHesapHareketService>().To<OgrenciSozlesmeHesapHareketManager>();

            #endregion

            #region OgrenciSozlesmeOdemeBilgi

            Bind<IEfOgrenciSozlesmeOdemeBilgiData>().To<EfOgrenciSozlesmeOdemeBilgiData>();
            Bind<IOgrenciSozlesmeOdemeBilgiService>().To<OgrenciSozlesmeOdemeBilgiManager>();

            #endregion

            #region OgrenciSozlesmeOdemeBilgiSenetImzalayan

            Bind<IEfOgrenciSozlesmeOdemeBilgiSenetImzalayanData>().To<EfOgrenciSozlesmeOdemeBilgiSenetImzalayanData>();
            Bind<IOgrenciSozlesmeOdemeBilgiSenetImzalayanService>().To<OgrenciSozlesmeOdemeBilgiSenetImzalayanManager>();

            #endregion

            #region OgrenciSozlesmeOkulDetayKiyafetDurum

            Bind<IEfOgrenciSozlesmeKiyafetDurumData>().To<EfOgrenciSozlesmeKiyafetDurumData>();

            #endregion

            #region OgrenciSozlesmeYayin

            Bind<IEfOgrenciSozlesmeYayinData>().To<EfOgrenciSozlesmeYayinData>();

            #endregion

            #region OgrenciSozlesmeYayin

            Bind<IEfOgrenciSozlesmeTurData>().To<EfOgrenciSozlesmeTurData>();
            Bind<IOgrenciSozlesmeTurService>().To<OgrenciSozlesmeTurManager>();

            #endregion

            #region OkulTur

            Bind<IEfOkulTurData>().To<EfOkulTurData>();
            Bind<IOkulTurService>().To<OkulTurManager>();

            #endregion

            #region ParaBirim

            Bind<IEfParaBirimData>().To<EfParaBirimData>();
            Bind<IParaBirimService>().To<ParaBirimManager>();

            #endregion

            #region Personel

            Bind<IEfPersonelData>().To<EfPersonelData>();
            Bind<IPersonelService>().To<PersonelManager>();
            Bind<IDpPersonelData>().To<DpPersonelData>();
            Bind<IDpPersonelTelefonData>().To<DpPersonelTelefonData>();

            #endregion

            #region PersonelGrup

            Bind<IEfPersonelGrupData>().To<EfPersonelGrupData>();
            Bind<IPersonelGrupService>().To<PersonelGrupManager>();

            #endregion

            #region PersonelPuantaj

            Bind<IEfPersonelPuantajData>().To<EfPersonelPuantajData>();
            Bind<IPersonelPuantajService>().To<PersonelPuantajManager>();

            #endregion

            #region PersonelSubeDers

            Bind<IEfPersonelSubeDersData>().To<EfPersonelSubeDersData>();

            #endregion

            #region PersonelYetkiGrup

            Bind<IEfPersonelYetkiGrupData>().To<EfPersonelYetkiGrupData>();
            Bind<IPersonelYetkiGrupService>().To<PersonelYetkiGrupManager>();

            #endregion

            #region PersonelSubeYetki

            Bind<IEfPersonelSubeYetkiData>().To<EfPersonelSubeYetkiData>();

            #endregion

            #region PersonelPuantajGunlukDurum

            Bind<IEfPersonelPuantajGunlukDurumData>().To<EfPersonelPuantajGunlukDurumData>();
            Bind<IPersonelPuantajGunlukDurumService>().To<PersonelPuantajGunlukDurumManager>();

            #endregion

            #region PuanTur

            Bind<IEfPuanTurData>().To<EfPuanTurData>();
            Bind<IPuanTurService>().To<PuanTurManager>();

            #endregion

            #region Servis

            Bind<IEfServisData>().To<EfServisData>();
            Bind<IServisService>().To<ServisManager>();

            #endregion

            #region Sehir

            Bind<IEfSehirData>().To<EfSehirData>();
            Bind<ISehirService>().To<SehirManager>();

            #endregion


            #region Sms

            Bind<IEfSmsData>().To<EfSmsData>();
            Bind<ISmsService>().To<SmsManager>();

            #endregion

            #region Sms

            Bind<IEfSmsDurumData>().To<EfSmsDurumData>();
            Bind<ISmsDurumService>().To<SmsDurumManager>();

            #endregion

            #region SmsHesap

            Bind<IEfSmsHesapData>().To<EfSmsHesapData>();
            Bind<ISmsHesapService>().To<SmsHesapManager>();

            #endregion


            #region SmsHesapDosya

            Bind<IEfSmsHesapDosyaData>().To<EfSmsHesapDosyaData>();
            Bind<ISmsHesapDosyaService>().To<SmsHesapDosyaManager>();

            #endregion

            #region SmsHesapHareket

            Bind<IEfSmsHesapHareketData>().To<EfSmsHesapHareketData>();
            Bind<IDpSmsHesapHareketData>().To<DpSmsHesapHareketData>();
            Bind<ISmsHesapHareketService>().To<SmsHesapHareketManager>();

            #endregion

            #region SmsHesapDurum

            Bind<IEfSmsHesapDurumData>().To<EfSmsHesapDurumData>();
            Bind<ISmsHesapDurumService>().To<SmsHesapDurumManager>();

            #endregion

            #region SmsMetinSablon

            Bind<IEfSmsMetinSablonData>().To<EfSmsMetinSablonData>();
            Bind<ISmsMetinSablonService>().To<SmsMetinSablonManager>();

            #endregion

            #region Sube

            Bind<IEfSubeData>().To<EfSubeData>();
            Bind<ISubeService>().To<SubeManager>();

            #endregion

            #region Sezon

            Bind<IEfSezonData>().To<EfSezonData>();
            Bind<IDpSezonData>().To<DpSezonData>();
            Bind<ISezonService>().To<SezonManager>();

            #endregion

            #region Sinav

            Bind<IEfSinavData>().To<EfSinavData>();
            Bind<IDpSinavData>().To<DpSinavData>();
            Bind<ISinavService>().To<SinavManager>();

            #endregion

            #region SinavKitapcik

            Bind<IEfSinavKitapcikData>().To<EfSinavKitapcikData>();
            Bind<ISinavKitapcikService>().To<SinavKitapcikManager>();

            #endregion

            #region SinavKitapcik

            Bind<IEfSinavSubeData>().To<EfSinavSubeData>();
            Bind<ISinavSubeService>().To<SinavSubeManager>();

            #endregion

            #region SinavTur

            Bind<IEfSinavTurData>().To<EfSinavTurData>();
            Bind<ISinavTurService>().To<SinavTurManager>();

            #endregion

            #region SinavTurDersKatSayi

            Bind<IEfSinavTurDersKatSayiData>().To<EfSinavTurDersKatSayiData>();
            Bind<ISinavTurDersKatSayiService>().To<SinavTurDersKatSayiManager>();

            #endregion

            #region Sinif

            Bind<IEfSinifData>().To<EfSinifData>();
            Bind<IDpSinifData>().To<DpSinifData>();
            Bind<ISinifService>().To<SinifManager>();

            #endregion

            #region SinifSeviye

            Bind<IEfSinifSeviyeData>().To<EfSinifSeviyeData>();
            Bind<ISinifSeviyeService>().To<SinifSeviyeManager>();

            #endregion

            #region SinifSeans

            Bind<IEfSinifSeansData>().To<EfSinifSeansData>();
            Bind<ISinifSeansService>().To<SinifSeansManager>();

            #endregion

            #region SinifTur

            Bind<IEfSinifTurData>().To<EfSinifTurData>();
            Bind<ISinifTurService>().To<SinifTurManager>();

            #endregion

            #region TransferTip
            Bind<IEfTransferTipData>().To<EfTransferTipData>();
            Bind<ITransferTipService>().To<TransferTipManager>();

            #endregion

            #region Ulke

            Bind<IEfUlkeData>().To<EfUlkeData>();
            Bind<IUlkeService>().To<UlkeManager>();

            #endregion

            #region Video

            Bind<IEfVideoData>().To<EfVideoData>();
            Bind<IDpVideoData>().To<DpVideoData>();
            Bind<IVideoService>().To<VideoManager>();

            #endregion

            #region Video Kategori

            Bind<IEfVideoKategoriData>().To<EfVideoKategoriData>();
            Bind<IVideoKategoriService>().To<VideoKategoriManager>();

            #endregion

            #region Video Video Kategori

            Bind<IEfVideoVideoKategoriData>().To<EfVideoVideoKategoriData>();
            Bind<IVideoVideoKategoriService>().To<VideoVideoKategoriManager>();

            #endregion

            #region Video Konu

            Bind<IEfVideoKonuData>().To<EfVideoKonuData>();
            Bind<IVideoKonuService>().To<VideoKonuManager>();

            #endregion

            #region Video Kurum Yetki

            Bind<IEfVideoKurumYetkiData>().To<EfVideoKurumYetkiData>();
            Bind<IVideoKurumYetkiService>().To<VideoKurumYetkiManager>();

            #endregion

            #region Video Şube Yetki

            Bind<IEfVideoSubeYetkiData>().To<EfVideoSubeYetkiData>();
            Bind<IVideoSubeYetkiService>().To<VideoSubeYetkiManager>();

            #endregion

            #region Video Sinif Yetki

            Bind<IEfVideoSinifYetkiData>().To<EfVideoSinifYetkiData>();
            Bind<IVideoSinifYetkiService>().To<VideoSinifYetkiManager>();

            #endregion

            #region Ulke

            Bind<IEfYayinData>().To<EfYayinData>();
            Bind<IYayinService>().To<YayinManager>();

            #endregion
        }
    }
}
