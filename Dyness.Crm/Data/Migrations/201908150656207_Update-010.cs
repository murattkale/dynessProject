namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Konu",
                c => new
                    {
                        KonuId = c.Int(nullable: false, identity: true),
                        DersId = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 200),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KonuId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.OgrenciSinavKontrol",
                c => new
                    {
                        OgrenciSinavKontrolId = c.Int(nullable: false, identity: true),
                        OgrenciId = c.Int(),
                        SinavKitapcikId = c.Int(nullable: false),
                        TcKimlikNo = c.String(maxLength: 11),
                        OgrenciNo = c.String(maxLength: 20),
                        Ad = c.String(maxLength: 50),
                        Soyad = c.String(maxLength: 50),
                        Sinif = c.String(maxLength: 10),
                        KadinMi = c.Boolean(nullable: false),
                        SoruCevaplar = c.String(),
                        DogruCevapAdet = c.Int(nullable: false),
                        YanlisCevapAdet = c.Int(nullable: false),
                        BosCevapAdet = c.Int(nullable: false),
                        Net = c.Double(nullable: false),
                        TabanPuan = c.Double(nullable: false),
                        ToplamPuan = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolId)
                .ForeignKey("dbo.Ogrenci", t => t.OgrenciId)
                .ForeignKey("dbo.SinavKitapcik", t => t.SinavKitapcikId, cascadeDelete: true)
                .Index(t => t.OgrenciId)
                .Index(t => t.SinavKitapcikId);
            
            CreateTable(
                "dbo.SinavKitapcik",
                c => new
                    {
                        SinavKitapcikId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.SinavKitapcikId)
                .ForeignKey("dbo.Sinav", t => t.SinavId, cascadeDelete: true)
                .Index(t => t.SinavId);
            
            CreateTable(
                "dbo.Sinav",
                c => new
                    {
                        SinavId = c.Int(nullable: false, identity: true),
                        SinavTurId = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 200),
                        SinavTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SinavId)
                .ForeignKey("dbo.SinavTur", t => t.SinavTurId, cascadeDelete: true)
                .Index(t => t.SinavTurId);
            
            CreateTable(
                "dbo.SinavTur",
                c => new
                    {
                        SinavTurId = c.Int(nullable: false, identity: true),
                        SinavTurAd = c.String(nullable: false, maxLength: 50),
                        KacYanlisBirDogruyuGoturur = c.Int(nullable: false),
                        TabanPuan = c.Double(nullable: false),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SinavTurId);
            
            CreateTable(
                "dbo.OptikFormTanimalamaDersBilgi",
                c => new
                    {
                        OptikFormTanimalamaDersBilgiId = c.Int(nullable: false, identity: true),
                        OptikFormTanimlamaId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        Bilgi = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimalamaDersBilgiId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.OptikFormTanimlama", t => t.OptikFormTanimlamaId, cascadeDelete: true)
                .Index(t => t.OptikFormTanimlamaId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.OptikFormTanimlama",
                c => new
                    {
                        OptikFormTanimlamaId = c.Int(nullable: false, identity: true),
                        SinavKitapcikId = c.Int(nullable: false),
                        TcNo = c.String(nullable: false, maxLength: 10),
                        OgrenciNo = c.String(nullable: false, maxLength: 10),
                        Ad = c.String(nullable: false, maxLength: 10),
                        Soyad = c.String(nullable: false, maxLength: 10),
                        Sinif = c.String(nullable: false, maxLength: 10),
                        KitapcikTur = c.String(nullable: false, maxLength: 10),
                        Cinsiyet = c.String(nullable: false, maxLength: 10),
                        AyracKarakter = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimlamaId)
                .ForeignKey("dbo.SinavKitapcik", t => t.SinavKitapcikId, cascadeDelete: true)
                .Index(t => t.SinavKitapcikId);
            
            CreateTable(
                "dbo.SinavDers",
                c => new
                    {
                        SinavDersId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        DersGrupId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        Sira = c.Int(nullable: false),
                        SoruSayi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SinavDersId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: false)
                .ForeignKey("dbo.DersGrup", t => t.DersGrupId, cascadeDelete: false)
                .ForeignKey("dbo.Sinav", t => t.SinavId, cascadeDelete: true)
                .Index(t => t.SinavId)
                .Index(t => t.DersGrupId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.SinavSoru",
                c => new
                    {
                        SinavSoruId = c.Int(nullable: false, identity: true),
                        SinavKitapcikId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        KonuId = c.Int(nullable: false),
                        Soru = c.Int(nullable: false),
                        Dogru = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SinavSoruId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: false)
                .ForeignKey("dbo.Konu", t => t.KonuId, cascadeDelete: false)
                .ForeignKey("dbo.SinavKitapcik", t => t.SinavKitapcikId, cascadeDelete: true)
                .Index(t => t.SinavKitapcikId)
                .Index(t => t.DersId)
                .Index(t => t.KonuId);
            
            CreateTable(
                "dbo.SinavTurDersKatSayi",
                c => new
                    {
                        SinavTurDersKatSayiId = c.Int(nullable: false, identity: true),
                        SinavTurId = c.Int(nullable: false),
                        DersGrupId = c.Int(nullable: false),
                        KatSayi = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SinavTurDersKatSayiId)
                .ForeignKey("dbo.DersGrup", t => t.DersGrupId, cascadeDelete: false)
                .ForeignKey("dbo.SinavTur", t => t.SinavTurId, cascadeDelete: true)
                .Index(t => t.SinavTurId)
                .Index(t => t.DersGrupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavTurDersKatSayi", "SinavTurId", "dbo.SinavTur");
            DropForeignKey("dbo.SinavTurDersKatSayi", "DersGrupId", "dbo.DersGrup");
            DropForeignKey("dbo.SinavSoru", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu");
            DropForeignKey("dbo.SinavSoru", "DersId", "dbo.Ders");
            DropForeignKey("dbo.SinavDers", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.SinavDers", "DersGrupId", "dbo.DersGrup");
            DropForeignKey("dbo.SinavDers", "DersId", "dbo.Ders");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama");
            DropForeignKey("dbo.OptikFormTanimlama", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders");
            DropForeignKey("dbo.OgrenciSinavKontrol", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.SinavKitapcik", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.Sinav", "SinavTurId", "dbo.SinavTur");
            DropForeignKey("dbo.OgrenciSinavKontrol", "OgrenciId", "dbo.Ogrenci");
            DropForeignKey("dbo.Konu", "DersId", "dbo.Ders");
            DropIndex("dbo.SinavTurDersKatSayi", new[] { "DersGrupId" });
            DropIndex("dbo.SinavTurDersKatSayi", new[] { "SinavTurId" });
            DropIndex("dbo.SinavSoru", new[] { "KonuId" });
            DropIndex("dbo.SinavSoru", new[] { "DersId" });
            DropIndex("dbo.SinavSoru", new[] { "SinavKitapcikId" });
            DropIndex("dbo.SinavDers", new[] { "DersId" });
            DropIndex("dbo.SinavDers", new[] { "DersGrupId" });
            DropIndex("dbo.SinavDers", new[] { "SinavId" });
            DropIndex("dbo.OptikFormTanimlama", new[] { "SinavKitapcikId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "DersId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "OptikFormTanimlamaId" });
            DropIndex("dbo.Sinav", new[] { "SinavTurId" });
            DropIndex("dbo.SinavKitapcik", new[] { "SinavId" });
            DropIndex("dbo.OgrenciSinavKontrol", new[] { "SinavKitapcikId" });
            DropIndex("dbo.OgrenciSinavKontrol", new[] { "OgrenciId" });
            DropIndex("dbo.Konu", new[] { "DersId" });
            DropTable("dbo.SinavTurDersKatSayi");
            DropTable("dbo.SinavSoru");
            DropTable("dbo.SinavDers");
            DropTable("dbo.OptikFormTanimlama");
            DropTable("dbo.OptikFormTanimalamaDersBilgi");
            DropTable("dbo.SinavTur");
            DropTable("dbo.Sinav");
            DropTable("dbo.SinavKitapcik");
            DropTable("dbo.OgrenciSinavKontrol");
            DropTable("dbo.Konu");
        }
    }
}
