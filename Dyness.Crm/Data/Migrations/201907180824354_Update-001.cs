namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankaHesap",
                c => new
                    {
                        BankaHesapId = c.Int(nullable: false),
                        BankaId = c.Int(nullable: false),
                        ParaBirimId = c.Int(nullable: false),
                        UstHesapId = c.Int(nullable: false),
                        Aciklama = c.String(nullable: false, maxLength: 200),
                        HesapSahibi = c.String(maxLength: 200),
                        Sube = c.String(maxLength: 50),
                        SubeNo = c.String(maxLength: 10),
                        HesapNo = c.String(maxLength: 30),
                        Iban = c.String(maxLength: 32),
                        Sira = c.Int(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BankaHesapId)
                .ForeignKey("dbo.Banka", t => t.BankaId, cascadeDelete: true)
                .ForeignKey("dbo.Hesap", t => t.BankaHesapId)
                .ForeignKey("dbo.ParaBirim", t => t.ParaBirimId, cascadeDelete: true)
                .ForeignKey("dbo.Hesap", t => t.UstHesapId, cascadeDelete: true)
                .Index(t => t.BankaHesapId)
                .Index(t => t.BankaId)
                .Index(t => t.ParaBirimId)
                .Index(t => t.UstHesapId);
            
            CreateTable(
                "dbo.Banka",
                c => new
                    {
                        BankaId = c.Int(nullable: false, identity: true),
                        BankaAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BankaId);
            
            CreateTable(
                "dbo.Hesap",
                c => new
                    {
                        HesapId = c.Int(nullable: false, identity: true),
                        BagliKurumId = c.Int(),
                        UstHesapId = c.Int(),
                        HesapTurId = c.Int(nullable: false),
                        ParaBirimId = c.Int(nullable: false),
                        HesapBaslik = c.String(nullable: false, maxLength: 100),
                        HesapKod = c.String(maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.HesapId)
                .ForeignKey("dbo.Kurum", t => t.BagliKurumId)
                .ForeignKey("dbo.HesapTur", t => t.HesapTurId, cascadeDelete: true)
                .ForeignKey("dbo.ParaBirim", t => t.ParaBirimId, cascadeDelete: false)
                .ForeignKey("dbo.Hesap", t => t.UstHesapId)
                .Index(t => t.BagliKurumId)
                .Index(t => t.UstHesapId)
                .Index(t => t.HesapTurId)
                .Index(t => t.ParaBirimId);
            
            CreateTable(
                "dbo.Kurum",
                c => new
                    {
                        KurumId = c.Int(nullable: false, identity: true),
                        KurumAd = c.String(nullable: false, maxLength: 200),
                        Adres = c.String(maxLength: 300),
                        Telefon = c.String(maxLength: 20),
                        Eposta = c.String(maxLength: 254),
                        LogoDosyaAd = c.String(maxLength: 100),
                        ArkaPlanDosyaAd = c.String(maxLength: 100),
                        SozlesmedeLogoKullanilsinMi = c.Boolean(nullable: false),
                        SozlesmedeArkaPlanGorselKullanilsinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KurumId);
            
            CreateTable(
                "dbo.HesapTur",
                c => new
                    {
                        HesapTurId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        HesapTurGrupId = c.Int(nullable: false),
                        HesapTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.HesapTurId)
                .ForeignKey("dbo.HesapTurGrup", t => t.HesapTurGrupId, cascadeDelete: true)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId)
                .Index(t => t.HesapTurGrupId);
            
            CreateTable(
                "dbo.HesapTurGrup",
                c => new
                    {
                        HesapTurGrupId = c.Int(nullable: false, identity: true),
                        HesapTurGrupAd = c.String(nullable: false, maxLength: 20),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.HesapTurGrupId);
            
            CreateTable(
                "dbo.Ogrenci",
                c => new
                    {
                        OgrenciId = c.Int(nullable: false),
                        KurumId = c.Int(nullable: false),
                        EkleyenPersonelId = c.Int(nullable: false),
                        UlkeId = c.Int(nullable: false),
                        SehirId = c.Int(),
                        IlceId = c.Int(),
                        AnneOgrenciYakiniIletisimId = c.Int(),
                        BabaOgrenciYakiniIletisimId = c.Int(),
                        YakiniOgrenciYakiniIletisimId = c.Int(),
                        NeredenDuydunuzId = c.Int(),
                        Ad = c.String(nullable: false, maxLength: 50),
                        Soyad = c.String(nullable: false, maxLength: 50),
                        AdSoyad = c.String(maxLength: 100),
                        TcKimlikNo = c.String(nullable: false, maxLength: 11),
                        OgrenciNo = c.String(nullable: false, maxLength: 20),
                        CepTelefon = c.String(nullable: false, maxLength: 20),
                        Telefon = c.String(maxLength: 20),
                        Eposta = c.String(maxLength: 254),
                        Adres = c.String(maxLength: 300),
                        PostaKodu = c.String(maxLength: 10),
                        GorselDosyaAd = c.String(maxLength: 100),
                        Not = c.String(maxLength: 500),
                        KadinMi = c.Boolean(nullable: false),
                        IletisimKendi = c.Boolean(nullable: false),
                        IletisimAnne = c.Boolean(nullable: false),
                        IletisimBaba = c.Boolean(nullable: false),
                        IletisimYakini = c.Boolean(nullable: false),
                        DogumTarihi = c.DateTime(nullable: false),
                        OlusturulmaTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.OgrenciId)
                .ForeignKey("dbo.OgrenciYakiniIletisim", t => t.AnneOgrenciYakiniIletisimId)
                .ForeignKey("dbo.OgrenciYakiniIletisim", t => t.BabaOgrenciYakiniIletisimId)
                .ForeignKey("dbo.Personel", t => t.EkleyenPersonelId, cascadeDelete: true)
                .ForeignKey("dbo.Hesap", t => t.OgrenciId)
                .ForeignKey("dbo.Ilce", t => t.IlceId)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: true)
                .ForeignKey("dbo.NeredenDuydunuz", t => t.NeredenDuydunuzId)
                .ForeignKey("dbo.Sehir", t => t.SehirId)
                .ForeignKey("dbo.Ulke", t => t.UlkeId, cascadeDelete: true)
                .ForeignKey("dbo.OgrenciYakiniIletisim", t => t.YakiniOgrenciYakiniIletisimId)
                .Index(t => t.OgrenciId)
                .Index(t => new { t.KurumId, t.OgrenciNo }, unique: true, name: "IX_OgrenciNoKurumIdUnique")
                .Index(t => t.EkleyenPersonelId)
                .Index(t => t.UlkeId)
                .Index(t => t.SehirId)
                .Index(t => t.IlceId)
                .Index(t => t.AnneOgrenciYakiniIletisimId)
                .Index(t => t.BabaOgrenciYakiniIletisimId)
                .Index(t => t.YakiniOgrenciYakiniIletisimId)
                .Index(t => t.NeredenDuydunuzId);
            
            CreateTable(
                "dbo.OgrenciYakiniIletisim",
                c => new
                    {
                        OgrenciYakiniIletisimId = c.Int(nullable: false, identity: true),
                        Ad = c.String(maxLength: 50),
                        Soyad = c.String(maxLength: 50),
                        TcKimlikNo = c.String(maxLength: 11),
                        CepTelefon1 = c.String(maxLength: 20),
                        CepTelefon2 = c.String(maxLength: 20),
                        EvTelefon = c.String(maxLength: 20),
                        IsTelefon = c.String(maxLength: 20),
                        Eposta = c.String(maxLength: 254),
                        EvAdres = c.String(maxLength: 300),
                        IsAdres = c.String(maxLength: 300),
                        DogumTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.OgrenciYakiniIletisimId);
            
            CreateTable(
                "dbo.Personel",
                c => new
                    {
                        PersonelId = c.Int(nullable: false),
                        PersonelGrupId = c.Int(nullable: false),
                        PersonelYetkiGrupId = c.Int(),
                        SubeId = c.Int(nullable: false),
                        DersId = c.Int(),
                        UlkeId = c.Int(nullable: false),
                        Ad = c.String(nullable: false, maxLength: 50),
                        Soyad = c.String(nullable: false, maxLength: 50),
                        TcKimlikNo = c.String(maxLength: 11),
                        CepTelefon = c.String(maxLength: 20),
                        Telefon = c.String(maxLength: 20),
                        Eposta = c.String(maxLength: 254),
                        Adres = c.String(maxLength: 300),
                        GorselDosyaAd = c.String(maxLength: 100),
                        Not = c.String(maxLength: 500),
                        YemekKartNo = c.String(maxLength: 100),
                        DersUcreti = c.Decimal(precision: 18, scale: 2),
                        Maas = c.Decimal(precision: 18, scale: 2),
                        GunlukYemekUcreti = c.Decimal(precision: 18, scale: 2),
                        KadinMi = c.Boolean(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                        PrimdenFaydalansinMi = c.Boolean(nullable: false),
                        DogumTarihi = c.DateTime(),
                        OlusturulmaTarihi = c.DateTime(),
                        IseBaslamaTarihi = c.DateTime(),
                        IstenAyrilmaTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonelId)
                .ForeignKey("dbo.Ders", t => t.DersId)
                .ForeignKey("dbo.Hesap", t => t.PersonelId)
                .ForeignKey("dbo.PersonelGrup", t => t.PersonelGrupId, cascadeDelete: true)
                .ForeignKey("dbo.PersonelYetkiGrup", t => t.PersonelYetkiGrupId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .ForeignKey("dbo.Ulke", t => t.UlkeId, cascadeDelete: false)
                .Index(t => t.PersonelId)
                .Index(t => t.PersonelGrupId)
                .Index(t => t.PersonelYetkiGrupId)
                .Index(t => t.SubeId)
                .Index(t => t.DersId)
                .Index(t => t.UlkeId);
            
            CreateTable(
                "dbo.Ders",
                c => new
                    {
                        DersId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        DersAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DersId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.BransDers",
                c => new
                    {
                        BransDersId = c.Int(nullable: false, identity: true),
                        BransId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BransDersId)
                .ForeignKey("dbo.Brans", t => t.BransId, cascadeDelete: true)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .Index(t => t.BransId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.Brans",
                c => new
                    {
                        BransId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        BransAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BransId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.Kullanici",
                c => new
                    {
                        PersonelId = c.Int(nullable: false),
                        KullaniciAd = c.String(maxLength: 100),
                        Sifre = c.String(maxLength: 20),
                        EtkinMi = c.Boolean(nullable: false),
                        SonGirisTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonelId)
                .ForeignKey("dbo.Personel", t => t.PersonelId)
                .Index(t => t.PersonelId);
            
            CreateTable(
                "dbo.PersonelGrup",
                c => new
                    {
                        PersonelGrupId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        PersonelGrupAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelGrupId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.PersonelSubeDers",
                c => new
                    {
                        PersonelSubeDersId = c.Int(nullable: false, identity: true),
                        PersonelId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelSubeDersId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: false)
                .Index(t => t.PersonelId)
                .Index(t => t.SubeId);
            
            CreateTable(
                "dbo.Sube",
                c => new
                    {
                        SubeId = c.Int(nullable: false),
                        KurumId = c.Int(nullable: false),
                        SehirId = c.Int(),
                        ParaBirimId = c.Int(nullable: false),
                        SubeAd = c.String(nullable: false, maxLength: 200),
                        Kod = c.String(maxLength: 50),
                        Yetkili = c.String(maxLength: 100),
                        Adres = c.String(maxLength: 300),
                        Telefon = c.String(maxLength: 20),
                        Eposta = c.String(maxLength: 254),
                        Not = c.String(maxLength: 500),
                        Harita = c.String(maxLength: 500),
                        SenetDuzenlemeYerBilgisi = c.String(maxLength: 100),
                        MinimumPesinatOrani = c.Int(),
                        MaksimumTaksitAdeti = c.Int(),
                        EtkinMi = c.Boolean(nullable: false),
                        UzaktanEgitimMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SubeId)
                .ForeignKey("dbo.Hesap", t => t.SubeId)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: false)
                .ForeignKey("dbo.ParaBirim", t => t.ParaBirimId, cascadeDelete: true)
                .ForeignKey("dbo.Sehir", t => t.SehirId)
                .Index(t => t.SubeId)
                .Index(t => t.KurumId)
                .Index(t => t.SehirId)
                .Index(t => t.ParaBirimId);
            
            CreateTable(
                "dbo.ParaBirim",
                c => new
                    {
                        ParaBirimId = c.Int(nullable: false, identity: true),
                        ParaBirimAd = c.String(nullable: false, maxLength: 50),
                        Kod = c.String(nullable: false, maxLength: 3),
                        KulturKod = c.String(nullable: false, maxLength: 10),
                        Tutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ParaBirimId);
            
            CreateTable(
                "dbo.Sehir",
                c => new
                    {
                        SehirId = c.Int(nullable: false, identity: true),
                        UlkeId = c.Int(nullable: false),
                        SehirAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SehirId)
                .ForeignKey("dbo.Ulke", t => t.UlkeId, cascadeDelete: true)
                .Index(t => t.UlkeId);
            
            CreateTable(
                "dbo.Ilce",
                c => new
                    {
                        IlceId = c.Int(nullable: false, identity: true),
                        SehirId = c.Int(nullable: false),
                        IlceAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IlceId)
                .ForeignKey("dbo.Sehir", t => t.SehirId, cascadeDelete: true)
                .Index(t => t.SehirId);
            
            CreateTable(
                "dbo.Ulke",
                c => new
                    {
                        UlkeId = c.Int(nullable: false, identity: true),
                        UlkeAd = c.String(nullable: false, maxLength: 100),
                        Kod = c.String(nullable: false, maxLength: 3),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UlkeId);
            
            CreateTable(
                "dbo.PersonelSubeUcret",
                c => new
                    {
                        PersonelSubeUcretId = c.Int(nullable: false, identity: true),
                        PersonelId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                        Ucret = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PersonelSubeUcretId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: false)
                .Index(t => t.PersonelId)
                .Index(t => t.SubeId);
            
            CreateTable(
                "dbo.PersonelSubeYetki",
                c => new
                    {
                        PersonelSubeYetkiId = c.Int(nullable: false, identity: true),
                        PersonelId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelSubeYetkiId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: false)
                .Index(t => t.PersonelId)
                .Index(t => t.SubeId);
            
            CreateTable(
                "dbo.PersonelYetkiGrup",
                c => new
                    {
                        PersonelYetkiGrupId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        PersonelYetkiGrupAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelYetkiGrupId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.NeredenDuydunuz",
                c => new
                    {
                        NeredenDuydunuzId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        NeredenDuydunuzBaslik = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NeredenDuydunuzId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.OgrenciFaturaBilgi",
                c => new
                    {
                        OgrenciFaturaBilgiId = c.Int(nullable: false, identity: true),
                        OgrenciId = c.Int(nullable: false),
                        FaturaBilgiId = c.Int(nullable: false),
                        GecerliMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciFaturaBilgiId)
                .ForeignKey("dbo.FaturaBilgi", t => t.FaturaBilgiId, cascadeDelete: true)
                .ForeignKey("dbo.Ogrenci", t => t.OgrenciId, cascadeDelete: true)
                .Index(t => t.OgrenciId)
                .Index(t => t.FaturaBilgiId);
            
            CreateTable(
                "dbo.FaturaBilgi",
                c => new
                    {
                        FaturaBilgiId = c.Int(nullable: false, identity: true),
                        AdSoyad = c.String(maxLength: 100),
                        VergiDairesi = c.String(maxLength: 100),
                        VergiNo = c.String(maxLength: 50),
                        Adres = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.FaturaBilgiId);
            
            CreateTable(
                "dbo.OgrenciSozlesme",
                c => new
                    {
                        OgrenciSozlesmeId = c.Int(nullable: false, identity: true),
                        OgrenciId = c.Int(nullable: false),
                        EkleyenPersonelId = c.Int(nullable: false),
                        OgrenciSozlesmeTurId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                        SezonId = c.Int(nullable: false),
                        BransId = c.Int(),
                        SinifSeviyeId = c.Int(),
                        SinifId = c.Int(),
                        OkulTurId = c.Int(),
                        ServisId = c.Int(),
                        EhliyetTurId = c.Int(),
                        OzelDersDurumId = c.Int(),
                        EtkinlikId = c.Int(),
                        DanismanPersonelId = c.Int(),
                        GorusenPersonelId = c.Int(nullable: false),
                        KurumaGetirenPersonelId = c.Int(nullable: false),
                        FaturaBilgiId = c.Int(),
                        Referans = c.String(maxLength: 100),
                        Not = c.String(maxLength: 500),
                        YemekDahilMi = c.Boolean(nullable: false),
                        DersAdeti = c.Int(),
                        DersBirimFiyat = c.Int(),
                        EgitimTutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YayinTutar = c.Decimal(precision: 18, scale: 2),
                        ServisTutar = c.Decimal(precision: 18, scale: 2),
                        KiyafetTutar = c.Decimal(precision: 18, scale: 2),
                        YemekTutar = c.Decimal(precision: 18, scale: 2),
                        ToplamUcret = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ToplamOdenen = c.Decimal(precision: 18, scale: 2),
                        OlusturulmaTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeId)
                .ForeignKey("dbo.Brans", t => t.BransId)
                .ForeignKey("dbo.Personel", t => t.DanismanPersonelId)
                .ForeignKey("dbo.EhliyetTur", t => t.EhliyetTurId)
                .ForeignKey("dbo.Personel", t => t.EkleyenPersonelId, cascadeDelete: true)
                .ForeignKey("dbo.Etkinlik", t => t.EtkinlikId)
                .ForeignKey("dbo.FaturaBilgi", t => t.FaturaBilgiId)
                .ForeignKey("dbo.Personel", t => t.GorusenPersonelId, cascadeDelete: false)
                .ForeignKey("dbo.Personel", t => t.KurumaGetirenPersonelId, cascadeDelete: false)
                .ForeignKey("dbo.Ogrenci", t => t.OgrenciId, cascadeDelete: false)
                .ForeignKey("dbo.OgrenciSozlesmeTur", t => t.OgrenciSozlesmeTurId, cascadeDelete: true)
                .ForeignKey("dbo.OkulTur", t => t.OkulTurId)
                .ForeignKey("dbo.OzelDersDurum", t => t.OzelDersDurumId)
                .ForeignKey("dbo.Servis", t => t.ServisId)
                .ForeignKey("dbo.Sezon", t => t.SezonId, cascadeDelete: true)
                .ForeignKey("dbo.Sinif", t => t.SinifId)
                .ForeignKey("dbo.SinifSeviye", t => t.SinifSeviyeId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: false)
                .Index(t => t.OgrenciId)
                .Index(t => t.EkleyenPersonelId)
                .Index(t => t.OgrenciSozlesmeTurId)
                .Index(t => t.SubeId)
                .Index(t => t.SezonId)
                .Index(t => t.BransId)
                .Index(t => t.SinifSeviyeId)
                .Index(t => t.SinifId)
                .Index(t => t.OkulTurId)
                .Index(t => t.ServisId)
                .Index(t => t.EhliyetTurId)
                .Index(t => t.OzelDersDurumId)
                .Index(t => t.EtkinlikId)
                .Index(t => t.DanismanPersonelId)
                .Index(t => t.GorusenPersonelId)
                .Index(t => t.KurumaGetirenPersonelId)
                .Index(t => t.FaturaBilgiId);
            
            CreateTable(
                "dbo.EhliyetTur",
                c => new
                    {
                        EhliyetTurId = c.Int(nullable: false, identity: true),
                        EhliyetTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EhliyetTurId);
            
            CreateTable(
                "dbo.Etkinlik",
                c => new
                    {
                        EtkinlikId = c.Int(nullable: false, identity: true),
                        SorumluPersonelId = c.Int(nullable: false),
                        EtkinlikAd = c.String(nullable: false, maxLength: 100),
                        Kontenjan = c.Int(),
                        KatilimAdet = c.Int(),
                        KontenjanKontrolEdilsinMi = c.Boolean(nullable: false),
                        OlusturulmaTarihi = c.DateTime(),
                        EtkinlikSonBasvuruTarihi = c.DateTime(),
                        EtkinlikBaslangicTarihi = c.DateTime(),
                        EtkinlikBitisTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.EtkinlikId)
                .ForeignKey("dbo.Personel", t => t.SorumluPersonelId, cascadeDelete: true)
                .Index(t => t.SorumluPersonelId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeDersSecim",
                c => new
                    {
                        OgrenciSozlesmeDersSecimId = c.Int(nullable: false, identity: true),
                        OgrenciSozlesmeId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeDersSecimId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.OgrenciSozlesme", t => t.OgrenciSozlesmeId, cascadeDelete: true)
                .Index(t => t.OgrenciSozlesmeId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeHesapHareket",
                c => new
                    {
                        HesapHareketId = c.Int(nullable: false),
                        OgrenciSozlesmeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HesapHareketId)
                .ForeignKey("dbo.HesapHareket", t => t.HesapHareketId)
                .ForeignKey("dbo.OgrenciSozlesme", t => t.OgrenciSozlesmeId, cascadeDelete: true)
                .Index(t => t.HesapHareketId)
                .Index(t => t.OgrenciSozlesmeId);
            
            CreateTable(
                "dbo.HesapHareket",
                c => new
                    {
                        HesapHareketId = c.Int(nullable: false, identity: true),
                        ParaBirimId = c.Int(nullable: false),
                        BorcluHesapId = c.Int(),
                        AlacakliHesapId = c.Int(),
                        PersonelId = c.Int(),
                        Tutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Aciklama = c.String(nullable: false, maxLength: 500),
                        IslemGerceklestiMi = c.Boolean(nullable: false),
                        VadeTarihi = c.DateTime(),
                        HareketTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.HesapHareketId)
                .ForeignKey("dbo.Hesap", t => t.AlacakliHesapId)
                .ForeignKey("dbo.Hesap", t => t.BorcluHesapId)
                .ForeignKey("dbo.ParaBirim", t => t.ParaBirimId, cascadeDelete: true)
                .ForeignKey("dbo.Personel", t => t.PersonelId)
                .Index(t => t.ParaBirimId)
                .Index(t => t.BorcluHesapId)
                .Index(t => t.AlacakliHesapId)
                .Index(t => t.PersonelId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeKiyafetDurum",
                c => new
                    {
                        OgrenciSozlesmeKiyafetDurumId = c.Int(nullable: false, identity: true),
                        OgrenciSozlesmeId = c.Int(nullable: false),
                        KiyafetId = c.Int(nullable: false),
                        TeslimEdildiMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeKiyafetDurumId)
                .ForeignKey("dbo.Kiyafet", t => t.KiyafetId, cascadeDelete: true)
                .ForeignKey("dbo.OgrenciSozlesme", t => t.OgrenciSozlesmeId, cascadeDelete: true)
                .Index(t => t.OgrenciSozlesmeId)
                .Index(t => t.KiyafetId);
            
            CreateTable(
                "dbo.Kiyafet",
                c => new
                    {
                        KiyafetId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(nullable: false),
                        KiyafetTurId = c.Int(nullable: false),
                        KiyafetBedenId = c.Int(nullable: false),
                        StokAdet = c.Int(nullable: false),
                        KiyafetAd = c.String(nullable: false, maxLength: 200),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KiyafetId)
                .ForeignKey("dbo.KiyafetBeden", t => t.KiyafetBedenId, cascadeDelete: true)
                .ForeignKey("dbo.KiyafetTur", t => t.KiyafetTurId, cascadeDelete: true)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: true)
                .Index(t => t.KurumId)
                .Index(t => t.KiyafetTurId)
                .Index(t => t.KiyafetBedenId);
            
            CreateTable(
                "dbo.KiyafetBeden",
                c => new
                    {
                        KiyafetBedenId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        KiyafetBedenAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KiyafetBedenId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.KiyafetTur",
                c => new
                    {
                        KiyafetTurId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        KiyafetTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KiyafetTurId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeOdemeBilgi",
                c => new
                    {
                        OgrenciSozlesmeOdemeBilgiId = c.Int(nullable: false),
                        ParaBirimId = c.Int(nullable: false),
                        PesinatHesapId = c.Int(nullable: false),
                        OdemeTurId = c.Int(nullable: false),
                        OgrenciSozlesmeOdemeBilgiSenetImzalayanId = c.Int(),
                        PesinatTutar = c.Decimal(precision: 18, scale: 2),
                        TaksitTutar = c.Decimal(precision: 18, scale: 2),
                        TaksitAdet = c.Int(),
                        Not = c.String(maxLength: 500),
                        TaksitBaslangicTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeOdemeBilgiId)
                .ForeignKey("dbo.OdemeTur", t => t.OdemeTurId, cascadeDelete: true)
                .ForeignKey("dbo.OgrenciSozlesme", t => t.OgrenciSozlesmeOdemeBilgiId)
                .ForeignKey("dbo.OgrenciSozlesmeOdemeBilgiSenetImzalayan", t => t.OgrenciSozlesmeOdemeBilgiSenetImzalayanId)
                .ForeignKey("dbo.ParaBirim", t => t.ParaBirimId, cascadeDelete: true)
                .ForeignKey("dbo.Hesap", t => t.PesinatHesapId, cascadeDelete: true)
                .Index(t => t.OgrenciSozlesmeOdemeBilgiId)
                .Index(t => t.ParaBirimId)
                .Index(t => t.PesinatHesapId)
                .Index(t => t.OdemeTurId)
                .Index(t => t.OgrenciSozlesmeOdemeBilgiSenetImzalayanId);
            
            CreateTable(
                "dbo.OdemeTur",
                c => new
                    {
                        OdemeTurId = c.Int(nullable: false, identity: true),
                        OdemeTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OdemeTurId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeOdemeBilgiSenetImzalayan",
                c => new
                    {
                        OgrenciSozlesmeOdemeBilgiSenetImzalayanId = c.Int(nullable: false, identity: true),
                        OgrenciSozlesmeOdemeBilgiSenetImzalayanBilgi = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeOdemeBilgiSenetImzalayanId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeTur",
                c => new
                    {
                        OgrenciSozlesmeTurId = c.Int(nullable: false, identity: true),
                        OgrenciSozlesmeTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeTurId);
            
            CreateTable(
                "dbo.OgrenciSozlesmeYayin",
                c => new
                    {
                        OgrenciSozlesmeYayinId = c.Int(nullable: false, identity: true),
                        OgrenciSozlesmeId = c.Int(nullable: false),
                        YayinId = c.Int(nullable: false),
                        TeslimEdildiMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSozlesmeYayinId)
                .ForeignKey("dbo.OgrenciSozlesme", t => t.OgrenciSozlesmeId, cascadeDelete: true)
                .ForeignKey("dbo.Yayin", t => t.YayinId, cascadeDelete: true)
                .Index(t => t.OgrenciSozlesmeId)
                .Index(t => t.YayinId);
            
            CreateTable(
                "dbo.Yayin",
                c => new
                    {
                        YayinId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(nullable: false),
                        SinifSeviyeId = c.Int(nullable: false),
                        BransId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        StokAdet = c.Int(nullable: false),
                        YayinAd = c.String(nullable: false, maxLength: 200),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.YayinId)
                .ForeignKey("dbo.Brans", t => t.BransId, cascadeDelete: true)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: true)
                .ForeignKey("dbo.SinifSeviye", t => t.SinifSeviyeId, cascadeDelete: true)
                .Index(t => t.KurumId)
                .Index(t => t.SinifSeviyeId)
                .Index(t => t.BransId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.SinifSeviye",
                c => new
                    {
                        SinifSeviyeId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        SinifSeviyeAd = c.String(nullable: false, maxLength: 50),
                        Aciklama = c.String(maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SinifSeviyeId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.OkulTur",
                c => new
                    {
                        OkulTurId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        OkulTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OkulTurId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.OzelDersDurum",
                c => new
                    {
                        OzelDersDurumId = c.Int(nullable: false, identity: true),
                        OzelDersDurumAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OzelDersDurumId);
            
            CreateTable(
                "dbo.Servis",
                c => new
                    {
                        ServisId = c.Int(nullable: false, identity: true),
                        SubeId = c.Int(nullable: false),
                        ServisPlaka = c.String(nullable: false, maxLength: 15),
                        Guzergah = c.String(maxLength: 50),
                        Kapasite = c.Int(),
                        YolcuAdet = c.Int(),
                        KapasiteKontrolEdilsinMi = c.Boolean(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServisId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SubeId);
            
            CreateTable(
                "dbo.Sezon",
                c => new
                    {
                        SezonId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(nullable: false),
                        SezonAd = c.String(nullable: false, maxLength: 50),
                        Kod = c.String(nullable: false, maxLength: 20),
                        BaslangicTarihi = c.DateTime(nullable: false),
                        BitisTarihi = c.DateTime(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SezonId)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: false)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.Sinif",
                c => new
                    {
                        SinifId = c.Int(nullable: false, identity: true),
                        SubeId = c.Int(nullable: false),
                        SezonId = c.Int(nullable: false),
                        BransId = c.Int(),
                        SinifTurId = c.Int(),
                        SinifSeviyeId = c.Int(),
                        SinifSeansId = c.Int(),
                        DerslikId = c.Int(),
                        PersonelId = c.Int(),
                        SinifAd = c.String(nullable: false, maxLength: 200),
                        ToplamDersSaat = c.Int(nullable: false),
                        SinifKapasite = c.Int(nullable: false),
                        KayitUcreti = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EgitimSüre = c.Int(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SinifId)
                .ForeignKey("dbo.Brans", t => t.BransId)
                .ForeignKey("dbo.Derslik", t => t.DerslikId)
                .ForeignKey("dbo.Personel", t => t.PersonelId)
                .ForeignKey("dbo.Sezon", t => t.SezonId, cascadeDelete: true)
                .ForeignKey("dbo.SinifSeans", t => t.SinifSeansId)
                .ForeignKey("dbo.SinifSeviye", t => t.SinifSeviyeId)
                .ForeignKey("dbo.SinifTur", t => t.SinifTurId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SubeId)
                .Index(t => t.SezonId)
                .Index(t => t.BransId)
                .Index(t => t.SinifTurId)
                .Index(t => t.SinifSeviyeId)
                .Index(t => t.SinifSeansId)
                .Index(t => t.DerslikId)
                .Index(t => t.PersonelId);
            
            CreateTable(
                "dbo.Derslik",
                c => new
                    {
                        DerslikId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        DerslikAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DerslikId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.SinifSeans",
                c => new
                    {
                        SinifSeansId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        SinifSeansAd = c.String(nullable: false, maxLength: 50),
                        Aciklama = c.String(maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SinifSeansId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.SinifTur",
                c => new
                    {
                        SinifTurId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        SinifTurAd = c.String(nullable: false, maxLength: 50),
                        OnlineEtutVerebilirMi = c.Boolean(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SinifTurId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.Duyuru",
                c => new
                    {
                        DuyuruId = c.Int(nullable: false, identity: true),
                        PersonelId = c.Int(nullable: false),
                        UstDuyuruId = c.Int(),
                        Baslik = c.String(nullable: false, maxLength: 100),
                        Metin = c.String(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                        ZorunluMu = c.Boolean(nullable: false),
                        SabitMi = c.Boolean(nullable: false),
                        OlusturulmaTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DuyuruId)
                .ForeignKey("dbo.Duyuru", t => t.UstDuyuruId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .Index(t => t.PersonelId)
                .Index(t => t.UstDuyuruId);
            
            CreateTable(
                "dbo.DuyuruPersonelBilgi",
                c => new
                    {
                        DuyuruPersonelBilgiId = c.Int(nullable: false, identity: true),
                        DuyuruId = c.Int(nullable: false),
                        PersonelId = c.Int(nullable: false),
                        GorulmeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.DuyuruPersonelBilgiId)
                .ForeignKey("dbo.Duyuru", t => t.DuyuruId, cascadeDelete: true)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: false)
                .Index(t => t.DuyuruId)
                .Index(t => t.PersonelId);
            
            CreateTable(
                "dbo.HesapBilgi",
                c => new
                    {
                        HesapBilgiId = c.Int(nullable: false, identity: true),
                        HesapId = c.Int(nullable: false),
                        Yil = c.Int(),
                        Ay = c.Int(),
                        ToplamBorc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ToplamAlacak = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HesapBilgiId)
                .ForeignKey("dbo.Hesap", t => t.HesapId, cascadeDelete: true)
                .Index(t => t.HesapId);
            
            CreateTable(
                "dbo.KurumOgrenciSozlesmeMetin",
                c => new
                    {
                        KurumOgrenciSozlesmeMetinId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(nullable: false),
                        Metin = c.String(),
                    })
                .PrimaryKey(t => t.KurumOgrenciSozlesmeMetinId)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: true)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.Mesaj",
                c => new
                    {
                        MesajId = c.Int(nullable: false, identity: true),
                        UstMesajId = c.Int(),
                        GonderenPersonelId = c.Int(nullable: false),
                        GonderilenPersonelId = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 100),
                        Metin = c.String(nullable: false, maxLength: 500),
                        GonderilmeTarihi = c.DateTime(nullable: false),
                        OkunmaTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.MesajId)
                .ForeignKey("dbo.Mesaj", t => t.UstMesajId)
                .ForeignKey("dbo.Personel", t => t.GonderenPersonelId, cascadeDelete: false)
                .ForeignKey("dbo.Personel", t => t.GonderilenPersonelId, cascadeDelete: false)
                .Index(t => t.UstMesajId)
                .Index(t => t.GonderenPersonelId)
                .Index(t => t.GonderilenPersonelId);
            
            CreateTable(
                "dbo.PersonelPuantajGunlukDurum",
                c => new
                    {
                        PersonelPuantajGunlukDurumId = c.Int(nullable: false, identity: true),
                        PersonelPuantajGunlukDurumAd = c.String(nullable: false, maxLength: 50),
                        PersonelPuantajGunlukDurumKisatlma = c.String(nullable: false, maxLength: 3),
                        PersonelPuantajGunlukDurumRenk = c.String(nullable: false, maxLength: 10),
                        Sira = c.Int(nullable: false),
                        CalistiMi = c.Boolean(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelPuantajGunlukDurumId);
            
            CreateTable(
                "dbo.PersonelPuantajGunluk",
                c => new
                    {
                        PersonelPuantajGunlukId = c.Int(nullable: false, identity: true),
                        PersonelPuantajId = c.Int(nullable: false),
                        PersonelPuantajGunlukDurumId = c.Int(nullable: false),
                        Yil = c.Int(nullable: false),
                        Ay = c.Int(nullable: false),
                        Gun = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelPuantajGunlukId)
                .ForeignKey("dbo.PersonelPuantaj", t => t.PersonelPuantajId, cascadeDelete: true)
                .ForeignKey("dbo.PersonelPuantajGunlukDurum", t => t.PersonelPuantajGunlukDurumId, cascadeDelete: true)
                .Index(t => t.PersonelPuantajId)
                .Index(t => t.PersonelPuantajGunlukDurumId);
            
            CreateTable(
                "dbo.PersonelPuantaj",
                c => new
                    {
                        PersonelPuantajId = c.Int(nullable: false, identity: true),
                        PersonelId = c.Int(nullable: false),
                        PuantajYil = c.Int(nullable: false),
                        PuantajAy = c.Int(nullable: false),
                        ToplamGun = c.Int(nullable: false),
                        Gun = c.Int(nullable: false),
                        Hakedis = c.Int(nullable: false),
                        Maas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gunluk = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HesaplananMaas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Elden = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Icra = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Banka = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sistem = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PersonelPuantajId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .Index(t => t.PersonelId);
            
            CreateTable(
                "dbo.PersonelYetki",
                c => new
                    {
                        PersonelYetkiId = c.Int(nullable: false, identity: true),
                        YetkiMethodId = c.Int(nullable: false),
                        PersonelId = c.Int(),
                        PersonelYetkiGrupId = c.Int(),
                        Yetki = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonelYetkiId)
                .ForeignKey("dbo.Personel", t => t.PersonelId)
                .ForeignKey("dbo.PersonelYetkiGrup", t => t.PersonelYetkiGrupId)
                .ForeignKey("dbo.YetkiMethod", t => t.YetkiMethodId, cascadeDelete: true)
                .Index(t => t.YetkiMethodId)
                .Index(t => t.PersonelId)
                .Index(t => t.PersonelYetkiGrupId);
            
            CreateTable(
                "dbo.YetkiMethod",
                c => new
                    {
                        YetkiMethodId = c.Int(nullable: false, identity: true),
                        Controller = c.String(nullable: false, maxLength: 100),
                        Method = c.String(nullable: false, maxLength: 100),
                        HttpMethod = c.Int(nullable: false),
                        ReturnType = c.String(nullable: false, maxLength: 50),
                        Sayfa = c.String(nullable: false, maxLength: 50),
                        Islem = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.YetkiMethodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonelYetki", "YetkiMethodId", "dbo.YetkiMethod");
            DropForeignKey("dbo.PersonelYetki", "PersonelYetkiGrupId", "dbo.PersonelYetkiGrup");
            DropForeignKey("dbo.PersonelYetki", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.PersonelPuantajGunluk", "PersonelPuantajGunlukDurumId", "dbo.PersonelPuantajGunlukDurum");
            DropForeignKey("dbo.PersonelPuantajGunluk", "PersonelPuantajId", "dbo.PersonelPuantaj");
            DropForeignKey("dbo.PersonelPuantaj", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Mesaj", "GonderilenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.Mesaj", "GonderenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.Mesaj", "UstMesajId", "dbo.Mesaj");
            DropForeignKey("dbo.KurumOgrenciSozlesmeMetin", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.HesapBilgi", "HesapId", "dbo.Hesap");
            DropForeignKey("dbo.DuyuruPersonelBilgi", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.DuyuruPersonelBilgi", "DuyuruId", "dbo.Duyuru");
            DropForeignKey("dbo.Duyuru", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Duyuru", "UstDuyuruId", "dbo.Duyuru");
            DropForeignKey("dbo.BankaHesap", "UstHesapId", "dbo.Hesap");
            DropForeignKey("dbo.BankaHesap", "ParaBirimId", "dbo.ParaBirim");
            DropForeignKey("dbo.Hesap", "UstHesapId", "dbo.Hesap");
            DropForeignKey("dbo.Hesap", "ParaBirimId", "dbo.ParaBirim");
            DropForeignKey("dbo.Ogrenci", "YakiniOgrenciYakiniIletisimId", "dbo.OgrenciYakiniIletisim");
            DropForeignKey("dbo.Ogrenci", "UlkeId", "dbo.Ulke");
            DropForeignKey("dbo.Ogrenci", "SehirId", "dbo.Sehir");
            DropForeignKey("dbo.OgrenciSozlesme", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.OgrenciSozlesme", "SinifSeviyeId", "dbo.SinifSeviye");
            DropForeignKey("dbo.OgrenciSozlesme", "SinifId", "dbo.Sinif");
            DropForeignKey("dbo.Sinif", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.Sinif", "SinifTurId", "dbo.SinifTur");
            DropForeignKey("dbo.SinifTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Sinif", "SinifSeviyeId", "dbo.SinifSeviye");
            DropForeignKey("dbo.Sinif", "SinifSeansId", "dbo.SinifSeans");
            DropForeignKey("dbo.SinifSeans", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Sinif", "SezonId", "dbo.Sezon");
            DropForeignKey("dbo.Sinif", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Sinif", "DerslikId", "dbo.Derslik");
            DropForeignKey("dbo.Derslik", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Sinif", "BransId", "dbo.Brans");
            DropForeignKey("dbo.OgrenciSozlesme", "SezonId", "dbo.Sezon");
            DropForeignKey("dbo.Sezon", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.OgrenciSozlesme", "ServisId", "dbo.Servis");
            DropForeignKey("dbo.Servis", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.OgrenciSozlesme", "OzelDersDurumId", "dbo.OzelDersDurum");
            DropForeignKey("dbo.OgrenciSozlesme", "OkulTurId", "dbo.OkulTur");
            DropForeignKey("dbo.OkulTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.OgrenciSozlesmeYayin", "YayinId", "dbo.Yayin");
            DropForeignKey("dbo.Yayin", "SinifSeviyeId", "dbo.SinifSeviye");
            DropForeignKey("dbo.SinifSeviye", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Yayin", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Yayin", "DersId", "dbo.Ders");
            DropForeignKey("dbo.Yayin", "BransId", "dbo.Brans");
            DropForeignKey("dbo.OgrenciSozlesmeYayin", "OgrenciSozlesmeId", "dbo.OgrenciSozlesme");
            DropForeignKey("dbo.OgrenciSozlesme", "OgrenciSozlesmeTurId", "dbo.OgrenciSozlesmeTur");
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "PesinatHesapId", "dbo.Hesap");
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "ParaBirimId", "dbo.ParaBirim");
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "OgrenciSozlesmeOdemeBilgiSenetImzalayanId", "dbo.OgrenciSozlesmeOdemeBilgiSenetImzalayan");
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "OgrenciSozlesmeOdemeBilgiId", "dbo.OgrenciSozlesme");
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId", "dbo.OdemeTur");
            DropForeignKey("dbo.OgrenciSozlesmeKiyafetDurum", "OgrenciSozlesmeId", "dbo.OgrenciSozlesme");
            DropForeignKey("dbo.OgrenciSozlesmeKiyafetDurum", "KiyafetId", "dbo.Kiyafet");
            DropForeignKey("dbo.Kiyafet", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Kiyafet", "KiyafetTurId", "dbo.KiyafetTur");
            DropForeignKey("dbo.KiyafetTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Kiyafet", "KiyafetBedenId", "dbo.KiyafetBeden");
            DropForeignKey("dbo.KiyafetBeden", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.OgrenciSozlesmeHesapHareket", "OgrenciSozlesmeId", "dbo.OgrenciSozlesme");
            DropForeignKey("dbo.OgrenciSozlesmeHesapHareket", "HesapHareketId", "dbo.HesapHareket");
            DropForeignKey("dbo.HesapHareket", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.HesapHareket", "ParaBirimId", "dbo.ParaBirim");
            DropForeignKey("dbo.HesapHareket", "BorcluHesapId", "dbo.Hesap");
            DropForeignKey("dbo.HesapHareket", "AlacakliHesapId", "dbo.Hesap");
            DropForeignKey("dbo.OgrenciSozlesmeDersSecim", "OgrenciSozlesmeId", "dbo.OgrenciSozlesme");
            DropForeignKey("dbo.OgrenciSozlesmeDersSecim", "DersId", "dbo.Ders");
            DropForeignKey("dbo.OgrenciSozlesme", "OgrenciId", "dbo.Ogrenci");
            DropForeignKey("dbo.OgrenciSozlesme", "KurumaGetirenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.OgrenciSozlesme", "GorusenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.OgrenciSozlesme", "FaturaBilgiId", "dbo.FaturaBilgi");
            DropForeignKey("dbo.OgrenciSozlesme", "EtkinlikId", "dbo.Etkinlik");
            DropForeignKey("dbo.Etkinlik", "SorumluPersonelId", "dbo.Personel");
            DropForeignKey("dbo.OgrenciSozlesme", "EkleyenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.OgrenciSozlesme", "EhliyetTurId", "dbo.EhliyetTur");
            DropForeignKey("dbo.OgrenciSozlesme", "DanismanPersonelId", "dbo.Personel");
            DropForeignKey("dbo.OgrenciSozlesme", "BransId", "dbo.Brans");
            DropForeignKey("dbo.OgrenciFaturaBilgi", "OgrenciId", "dbo.Ogrenci");
            DropForeignKey("dbo.OgrenciFaturaBilgi", "FaturaBilgiId", "dbo.FaturaBilgi");
            DropForeignKey("dbo.Ogrenci", "NeredenDuydunuzId", "dbo.NeredenDuydunuz");
            DropForeignKey("dbo.NeredenDuydunuz", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Ogrenci", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Ogrenci", "IlceId", "dbo.Ilce");
            DropForeignKey("dbo.Ogrenci", "OgrenciId", "dbo.Hesap");
            DropForeignKey("dbo.Ogrenci", "EkleyenPersonelId", "dbo.Personel");
            DropForeignKey("dbo.Personel", "UlkeId", "dbo.Ulke");
            DropForeignKey("dbo.Personel", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.Personel", "PersonelYetkiGrupId", "dbo.PersonelYetkiGrup");
            DropForeignKey("dbo.PersonelYetkiGrup", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.PersonelSubeYetki", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.PersonelSubeYetki", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.PersonelSubeUcret", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.PersonelSubeUcret", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.PersonelSubeDers", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.Sube", "SehirId", "dbo.Sehir");
            DropForeignKey("dbo.Sehir", "UlkeId", "dbo.Ulke");
            DropForeignKey("dbo.Ilce", "SehirId", "dbo.Sehir");
            DropForeignKey("dbo.Sube", "ParaBirimId", "dbo.ParaBirim");
            DropForeignKey("dbo.Sube", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Sube", "SubeId", "dbo.Hesap");
            DropForeignKey("dbo.PersonelSubeDers", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Personel", "PersonelGrupId", "dbo.PersonelGrup");
            DropForeignKey("dbo.PersonelGrup", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.Kullanici", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Personel", "PersonelId", "dbo.Hesap");
            DropForeignKey("dbo.Personel", "DersId", "dbo.Ders");
            DropForeignKey("dbo.Ders", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.BransDers", "DersId", "dbo.Ders");
            DropForeignKey("dbo.Brans", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.BransDers", "BransId", "dbo.Brans");
            DropForeignKey("dbo.Ogrenci", "BabaOgrenciYakiniIletisimId", "dbo.OgrenciYakiniIletisim");
            DropForeignKey("dbo.Ogrenci", "AnneOgrenciYakiniIletisimId", "dbo.OgrenciYakiniIletisim");
            DropForeignKey("dbo.Hesap", "HesapTurId", "dbo.HesapTur");
            DropForeignKey("dbo.HesapTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.HesapTur", "HesapTurGrupId", "dbo.HesapTurGrup");
            DropForeignKey("dbo.BankaHesap", "BankaHesapId", "dbo.Hesap");
            DropForeignKey("dbo.Hesap", "BagliKurumId", "dbo.Kurum");
            DropForeignKey("dbo.BankaHesap", "BankaId", "dbo.Banka");
            DropIndex("dbo.PersonelYetki", new[] { "PersonelYetkiGrupId" });
            DropIndex("dbo.PersonelYetki", new[] { "PersonelId" });
            DropIndex("dbo.PersonelYetki", new[] { "YetkiMethodId" });
            DropIndex("dbo.PersonelPuantaj", new[] { "PersonelId" });
            DropIndex("dbo.PersonelPuantajGunluk", new[] { "PersonelPuantajGunlukDurumId" });
            DropIndex("dbo.PersonelPuantajGunluk", new[] { "PersonelPuantajId" });
            DropIndex("dbo.Mesaj", new[] { "GonderilenPersonelId" });
            DropIndex("dbo.Mesaj", new[] { "GonderenPersonelId" });
            DropIndex("dbo.Mesaj", new[] { "UstMesajId" });
            DropIndex("dbo.KurumOgrenciSozlesmeMetin", new[] { "KurumId" });
            DropIndex("dbo.HesapBilgi", new[] { "HesapId" });
            DropIndex("dbo.DuyuruPersonelBilgi", new[] { "PersonelId" });
            DropIndex("dbo.DuyuruPersonelBilgi", new[] { "DuyuruId" });
            DropIndex("dbo.Duyuru", new[] { "UstDuyuruId" });
            DropIndex("dbo.Duyuru", new[] { "PersonelId" });
            DropIndex("dbo.SinifTur", new[] { "KurumId" });
            DropIndex("dbo.SinifSeans", new[] { "KurumId" });
            DropIndex("dbo.Derslik", new[] { "KurumId" });
            DropIndex("dbo.Sinif", new[] { "PersonelId" });
            DropIndex("dbo.Sinif", new[] { "DerslikId" });
            DropIndex("dbo.Sinif", new[] { "SinifSeansId" });
            DropIndex("dbo.Sinif", new[] { "SinifSeviyeId" });
            DropIndex("dbo.Sinif", new[] { "SinifTurId" });
            DropIndex("dbo.Sinif", new[] { "BransId" });
            DropIndex("dbo.Sinif", new[] { "SezonId" });
            DropIndex("dbo.Sinif", new[] { "SubeId" });
            DropIndex("dbo.Sezon", new[] { "KurumId" });
            DropIndex("dbo.Servis", new[] { "SubeId" });
            DropIndex("dbo.OkulTur", new[] { "KurumId" });
            DropIndex("dbo.SinifSeviye", new[] { "KurumId" });
            DropIndex("dbo.Yayin", new[] { "DersId" });
            DropIndex("dbo.Yayin", new[] { "BransId" });
            DropIndex("dbo.Yayin", new[] { "SinifSeviyeId" });
            DropIndex("dbo.Yayin", new[] { "KurumId" });
            DropIndex("dbo.OgrenciSozlesmeYayin", new[] { "YayinId" });
            DropIndex("dbo.OgrenciSozlesmeYayin", new[] { "OgrenciSozlesmeId" });
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "OgrenciSozlesmeOdemeBilgiSenetImzalayanId" });
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "OdemeTurId" });
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "PesinatHesapId" });
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "ParaBirimId" });
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "OgrenciSozlesmeOdemeBilgiId" });
            DropIndex("dbo.KiyafetTur", new[] { "KurumId" });
            DropIndex("dbo.KiyafetBeden", new[] { "KurumId" });
            DropIndex("dbo.Kiyafet", new[] { "KiyafetBedenId" });
            DropIndex("dbo.Kiyafet", new[] { "KiyafetTurId" });
            DropIndex("dbo.Kiyafet", new[] { "KurumId" });
            DropIndex("dbo.OgrenciSozlesmeKiyafetDurum", new[] { "KiyafetId" });
            DropIndex("dbo.OgrenciSozlesmeKiyafetDurum", new[] { "OgrenciSozlesmeId" });
            DropIndex("dbo.HesapHareket", new[] { "PersonelId" });
            DropIndex("dbo.HesapHareket", new[] { "AlacakliHesapId" });
            DropIndex("dbo.HesapHareket", new[] { "BorcluHesapId" });
            DropIndex("dbo.HesapHareket", new[] { "ParaBirimId" });
            DropIndex("dbo.OgrenciSozlesmeHesapHareket", new[] { "OgrenciSozlesmeId" });
            DropIndex("dbo.OgrenciSozlesmeHesapHareket", new[] { "HesapHareketId" });
            DropIndex("dbo.OgrenciSozlesmeDersSecim", new[] { "DersId" });
            DropIndex("dbo.OgrenciSozlesmeDersSecim", new[] { "OgrenciSozlesmeId" });
            DropIndex("dbo.Etkinlik", new[] { "SorumluPersonelId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "FaturaBilgiId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "KurumaGetirenPersonelId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "GorusenPersonelId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "DanismanPersonelId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "EtkinlikId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "OzelDersDurumId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "EhliyetTurId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "ServisId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "OkulTurId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "SinifId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "SinifSeviyeId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "BransId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "SezonId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "SubeId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "OgrenciSozlesmeTurId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "EkleyenPersonelId" });
            DropIndex("dbo.OgrenciSozlesme", new[] { "OgrenciId" });
            DropIndex("dbo.OgrenciFaturaBilgi", new[] { "FaturaBilgiId" });
            DropIndex("dbo.OgrenciFaturaBilgi", new[] { "OgrenciId" });
            DropIndex("dbo.NeredenDuydunuz", new[] { "KurumId" });
            DropIndex("dbo.PersonelYetkiGrup", new[] { "KurumId" });
            DropIndex("dbo.PersonelSubeYetki", new[] { "SubeId" });
            DropIndex("dbo.PersonelSubeYetki", new[] { "PersonelId" });
            DropIndex("dbo.PersonelSubeUcret", new[] { "SubeId" });
            DropIndex("dbo.PersonelSubeUcret", new[] { "PersonelId" });
            DropIndex("dbo.Ilce", new[] { "SehirId" });
            DropIndex("dbo.Sehir", new[] { "UlkeId" });
            DropIndex("dbo.Sube", new[] { "ParaBirimId" });
            DropIndex("dbo.Sube", new[] { "SehirId" });
            DropIndex("dbo.Sube", new[] { "KurumId" });
            DropIndex("dbo.Sube", new[] { "SubeId" });
            DropIndex("dbo.PersonelSubeDers", new[] { "SubeId" });
            DropIndex("dbo.PersonelSubeDers", new[] { "PersonelId" });
            DropIndex("dbo.PersonelGrup", new[] { "KurumId" });
            DropIndex("dbo.Kullanici", new[] { "PersonelId" });
            DropIndex("dbo.Brans", new[] { "KurumId" });
            DropIndex("dbo.BransDers", new[] { "DersId" });
            DropIndex("dbo.BransDers", new[] { "BransId" });
            DropIndex("dbo.Ders", new[] { "KurumId" });
            DropIndex("dbo.Personel", new[] { "UlkeId" });
            DropIndex("dbo.Personel", new[] { "DersId" });
            DropIndex("dbo.Personel", new[] { "SubeId" });
            DropIndex("dbo.Personel", new[] { "PersonelYetkiGrupId" });
            DropIndex("dbo.Personel", new[] { "PersonelGrupId" });
            DropIndex("dbo.Personel", new[] { "PersonelId" });
            DropIndex("dbo.Ogrenci", new[] { "NeredenDuydunuzId" });
            DropIndex("dbo.Ogrenci", new[] { "YakiniOgrenciYakiniIletisimId" });
            DropIndex("dbo.Ogrenci", new[] { "BabaOgrenciYakiniIletisimId" });
            DropIndex("dbo.Ogrenci", new[] { "AnneOgrenciYakiniIletisimId" });
            DropIndex("dbo.Ogrenci", new[] { "IlceId" });
            DropIndex("dbo.Ogrenci", new[] { "SehirId" });
            DropIndex("dbo.Ogrenci", new[] { "UlkeId" });
            DropIndex("dbo.Ogrenci", new[] { "EkleyenPersonelId" });
            DropIndex("dbo.Ogrenci", "IX_OgrenciNoKurumIdUnique");
            DropIndex("dbo.Ogrenci", new[] { "OgrenciId" });
            DropIndex("dbo.HesapTur", new[] { "HesapTurGrupId" });
            DropIndex("dbo.HesapTur", new[] { "KurumId" });
            DropIndex("dbo.Hesap", new[] { "ParaBirimId" });
            DropIndex("dbo.Hesap", new[] { "HesapTurId" });
            DropIndex("dbo.Hesap", new[] { "UstHesapId" });
            DropIndex("dbo.Hesap", new[] { "BagliKurumId" });
            DropIndex("dbo.BankaHesap", new[] { "UstHesapId" });
            DropIndex("dbo.BankaHesap", new[] { "ParaBirimId" });
            DropIndex("dbo.BankaHesap", new[] { "BankaId" });
            DropIndex("dbo.BankaHesap", new[] { "BankaHesapId" });
            DropTable("dbo.YetkiMethod");
            DropTable("dbo.PersonelYetki");
            DropTable("dbo.PersonelPuantaj");
            DropTable("dbo.PersonelPuantajGunluk");
            DropTable("dbo.PersonelPuantajGunlukDurum");
            DropTable("dbo.Mesaj");
            DropTable("dbo.KurumOgrenciSozlesmeMetin");
            DropTable("dbo.HesapBilgi");
            DropTable("dbo.DuyuruPersonelBilgi");
            DropTable("dbo.Duyuru");
            DropTable("dbo.SinifTur");
            DropTable("dbo.SinifSeans");
            DropTable("dbo.Derslik");
            DropTable("dbo.Sinif");
            DropTable("dbo.Sezon");
            DropTable("dbo.Servis");
            DropTable("dbo.OzelDersDurum");
            DropTable("dbo.OkulTur");
            DropTable("dbo.SinifSeviye");
            DropTable("dbo.Yayin");
            DropTable("dbo.OgrenciSozlesmeYayin");
            DropTable("dbo.OgrenciSozlesmeTur");
            DropTable("dbo.OgrenciSozlesmeOdemeBilgiSenetImzalayan");
            DropTable("dbo.OdemeTur");
            DropTable("dbo.OgrenciSozlesmeOdemeBilgi");
            DropTable("dbo.KiyafetTur");
            DropTable("dbo.KiyafetBeden");
            DropTable("dbo.Kiyafet");
            DropTable("dbo.OgrenciSozlesmeKiyafetDurum");
            DropTable("dbo.HesapHareket");
            DropTable("dbo.OgrenciSozlesmeHesapHareket");
            DropTable("dbo.OgrenciSozlesmeDersSecim");
            DropTable("dbo.Etkinlik");
            DropTable("dbo.EhliyetTur");
            DropTable("dbo.OgrenciSozlesme");
            DropTable("dbo.FaturaBilgi");
            DropTable("dbo.OgrenciFaturaBilgi");
            DropTable("dbo.NeredenDuydunuz");
            DropTable("dbo.PersonelYetkiGrup");
            DropTable("dbo.PersonelSubeYetki");
            DropTable("dbo.PersonelSubeUcret");
            DropTable("dbo.Ulke");
            DropTable("dbo.Ilce");
            DropTable("dbo.Sehir");
            DropTable("dbo.ParaBirim");
            DropTable("dbo.Sube");
            DropTable("dbo.PersonelSubeDers");
            DropTable("dbo.PersonelGrup");
            DropTable("dbo.Kullanici");
            DropTable("dbo.Brans");
            DropTable("dbo.BransDers");
            DropTable("dbo.Ders");
            DropTable("dbo.Personel");
            DropTable("dbo.OgrenciYakiniIletisim");
            DropTable("dbo.Ogrenci");
            DropTable("dbo.HesapTurGrup");
            DropTable("dbo.HesapTur");
            DropTable("dbo.Kurum");
            DropTable("dbo.Hesap");
            DropTable("dbo.Banka");
            DropTable("dbo.BankaHesap");
        }
    }
}
