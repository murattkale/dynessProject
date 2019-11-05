namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update025 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.OptikFormTanimlama");
            DropForeignKey("dbo.OptikFormTanimlama", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.SinavDers", "DersId", "dbo.Ders");
            DropForeignKey("dbo.SinavDers", "DersGrupId", "dbo.DersGrup");
            DropForeignKey("dbo.SinavDers", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.SinavSoru", "DersId", "dbo.Ders");
            DropForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu");
            DropForeignKey("dbo.SinavSoru", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropIndex("dbo.OptikFormTanimlama", new[] { "SinavId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "SinavId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "DersId" });
            DropIndex("dbo.SinavDers", new[] { "SinavId" });
            DropIndex("dbo.SinavDers", new[] { "DersGrupId" });
            DropIndex("dbo.SinavDers", new[] { "DersId" });
            DropIndex("dbo.SinavSoru", new[] { "SinavKitapcikId" });
            DropIndex("dbo.SinavSoru", new[] { "DersId" });
            DropIndex("dbo.SinavSoru", new[] { "KonuId" });
            CreateTable(
                "dbo.OgrenciSinavKontrolDersBilgi",
                c => new
                    {
                        OgrenciSinavKontrolDersBilgiId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        SoruCevaplar = c.String(),
                        DogruCevapAdet = c.Int(nullable: false),
                        YanlisCevapAdet = c.Int(nullable: false),
                        BosCevapAdet = c.Int(nullable: false),
                        Net = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolDersBilgiId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.OgrenciSinavKontrol", t => t.OgrenciSinavKontrolId, cascadeDelete: true)
                .Index(t => t.OgrenciSinavKontrolId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.OgrenciSinavKontrolDersPuan",
                c => new
                    {
                        OgrenciSinavKontrolDersPuanId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        PuanTurId = c.Int(nullable: false),
                        Puan = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolDersPuanId)
                .ForeignKey("dbo.OgrenciSinavKontrol", t => t.OgrenciSinavKontrolId, cascadeDelete: true)
                .ForeignKey("dbo.PuanTur", t => t.PuanTurId, cascadeDelete: true)
                .Index(t => t.OgrenciSinavKontrolId)
                .Index(t => t.PuanTurId);
            
            CreateTable(
                "dbo.PuanTur",
                c => new
                    {
                        PuanTurId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        PuanTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PuanTurId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.OgrenciSinavKontrolPuan",
                c => new
                    {
                        OgrenciSinavKontrolPuanId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        PuanTurId = c.Int(nullable: false),
                        Puan = c.Double(nullable: false),
                        ToplamPuan = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolPuanId)
                .ForeignKey("dbo.OgrenciSinavKontrol", t => t.OgrenciSinavKontrolId, cascadeDelete: true)
                .ForeignKey("dbo.PuanTur", t => t.PuanTurId, cascadeDelete: true)
                .Index(t => t.OgrenciSinavKontrolId)
                .Index(t => t.PuanTurId);
            
            CreateTable(
                "dbo.SinavTurDers",
                c => new
                    {
                        SinavTurDersId = c.Int(nullable: false, identity: true),
                        SinavTurId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        Sira = c.Int(nullable: false),
                        SoruSayi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SinavTurDersId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.SinavTur", t => t.SinavTurId, cascadeDelete: true)
                .Index(t => t.SinavTurId)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.SinavKitapcikDersBilgi",
                c => new
                    {
                        SinavKitapcikDersBilgiId = c.Int(nullable: false, identity: true),
                        SinavKitapcikId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        CevapAnahtartari = c.String(),
                        DersKonuBilgi = c.String(),
                    })
                .PrimaryKey(t => t.SinavKitapcikDersBilgiId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.SinavKitapcik", t => t.SinavKitapcikId, cascadeDelete: true)
                .Index(t => t.SinavKitapcikId)
                .Index(t => t.DersId);
            
            AddColumn("dbo.SinavTur", "TcNoBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "TcNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "OgrenciNoBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "OgrenciNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "AdBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "AdAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SoyadBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SoyadAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SinifBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SinifAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "KitapcikTurBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "KitapcikTurAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "CinsiyetBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "CinsiyetAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "AyracKarakter", c => c.String());
            AddColumn("dbo.SinavTurDersKatSayi", "PuanTurId", c => c.Int(nullable: false));
            CreateIndex("dbo.SinavTurDersKatSayi", "PuanTurId");
            AddForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur", "PuanTurId", cascadeDelete: true);
            DropColumn("dbo.OgrenciSinavKontrol", "ToplamPuan");
            DropTable("dbo.OptikFormTanimlama");
            DropTable("dbo.OptikFormTanimalamaDersBilgi");
            DropTable("dbo.SinavDers");
            DropTable("dbo.SinavSoru");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SinavSoru",
                c => new
                    {
                        SinavSoruId = c.Int(nullable: false, identity: true),
                        SinavKitapcikId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        KonuId = c.Int(),
                        Soru = c.Int(nullable: false),
                        Dogru = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.SinavSoruId);
            
            CreateTable(
                "dbo.SinavDers",
                c => new
                    {
                        SinavDersId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        DersGrupId = c.Int(nullable: false),
                        DersId = c.Int(),
                        Sira = c.Int(),
                        SoruSayi = c.Int(),
                    })
                .PrimaryKey(t => t.SinavDersId);
            
            CreateTable(
                "dbo.OptikFormTanimalamaDersBilgi",
                c => new
                    {
                        OptikFormTanimalamaDersBilgiId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        DersBasla = c.Int(nullable: false),
                        DersAdet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptikFormTanimalamaDersBilgiId);
            
            CreateTable(
                "dbo.OptikFormTanimlama",
                c => new
                    {
                        SinavId = c.Int(nullable: false),
                        TcNoBasla = c.Int(nullable: false),
                        TcNoAdet = c.Int(nullable: false),
                        OgrenciNoBasla = c.Int(nullable: false),
                        OgrenciNoAdet = c.Int(nullable: false),
                        AdBasla = c.Int(nullable: false),
                        AdAdet = c.Int(nullable: false),
                        SoyadBasla = c.Int(nullable: false),
                        SoyadAdet = c.Int(nullable: false),
                        SinifBasla = c.Int(nullable: false),
                        SinifAdet = c.Int(nullable: false),
                        KitapcikTurBasla = c.Int(nullable: false),
                        KitapcikTurAdet = c.Int(nullable: false),
                        CinsiyetBasla = c.Int(nullable: false),
                        CinsiyetAdet = c.Int(nullable: false),
                        AyracKarakter = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SinavId);
            
            AddColumn("dbo.OgrenciSinavKontrol", "ToplamPuan", c => c.Double(nullable: false));
            DropForeignKey("dbo.SinavKitapcikDersBilgi", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.SinavKitapcikDersBilgi", "DersId", "dbo.Ders");
            DropForeignKey("dbo.SinavTurDers", "SinavTurId", "dbo.SinavTur");
            DropForeignKey("dbo.SinavTurDers", "DersId", "dbo.Ders");
            DropForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur");
            DropForeignKey("dbo.OgrenciSinavKontrolPuan", "PuanTurId", "dbo.PuanTur");
            DropForeignKey("dbo.OgrenciSinavKontrolPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropForeignKey("dbo.OgrenciSinavKontrolDersPuan", "PuanTurId", "dbo.PuanTur");
            DropForeignKey("dbo.PuanTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.OgrenciSinavKontrolDersPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropForeignKey("dbo.OgrenciSinavKontrolDersBilgi", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropForeignKey("dbo.OgrenciSinavKontrolDersBilgi", "DersId", "dbo.Ders");
            DropIndex("dbo.SinavKitapcikDersBilgi", new[] { "DersId" });
            DropIndex("dbo.SinavKitapcikDersBilgi", new[] { "SinavKitapcikId" });
            DropIndex("dbo.SinavTurDers", new[] { "DersId" });
            DropIndex("dbo.SinavTurDers", new[] { "SinavTurId" });
            DropIndex("dbo.SinavTurDersKatSayi", new[] { "PuanTurId" });
            DropIndex("dbo.OgrenciSinavKontrolPuan", new[] { "PuanTurId" });
            DropIndex("dbo.OgrenciSinavKontrolPuan", new[] { "OgrenciSinavKontrolId" });
            DropIndex("dbo.PuanTur", new[] { "KurumId" });
            DropIndex("dbo.OgrenciSinavKontrolDersPuan", new[] { "PuanTurId" });
            DropIndex("dbo.OgrenciSinavKontrolDersPuan", new[] { "OgrenciSinavKontrolId" });
            DropIndex("dbo.OgrenciSinavKontrolDersBilgi", new[] { "DersId" });
            DropIndex("dbo.OgrenciSinavKontrolDersBilgi", new[] { "OgrenciSinavKontrolId" });
            DropColumn("dbo.SinavTurDersKatSayi", "PuanTurId");
            DropColumn("dbo.SinavTur", "AyracKarakter");
            DropColumn("dbo.SinavTur", "CinsiyetAdet");
            DropColumn("dbo.SinavTur", "CinsiyetBasla");
            DropColumn("dbo.SinavTur", "KitapcikTurAdet");
            DropColumn("dbo.SinavTur", "KitapcikTurBasla");
            DropColumn("dbo.SinavTur", "SinifAdet");
            DropColumn("dbo.SinavTur", "SinifBasla");
            DropColumn("dbo.SinavTur", "SoyadAdet");
            DropColumn("dbo.SinavTur", "SoyadBasla");
            DropColumn("dbo.SinavTur", "AdAdet");
            DropColumn("dbo.SinavTur", "AdBasla");
            DropColumn("dbo.SinavTur", "OgrenciNoAdet");
            DropColumn("dbo.SinavTur", "OgrenciNoBasla");
            DropColumn("dbo.SinavTur", "TcNoAdet");
            DropColumn("dbo.SinavTur", "TcNoBasla");
            DropTable("dbo.SinavKitapcikDersBilgi");
            DropTable("dbo.SinavTurDers");
            DropTable("dbo.OgrenciSinavKontrolPuan");
            DropTable("dbo.PuanTur");
            DropTable("dbo.OgrenciSinavKontrolDersPuan");
            DropTable("dbo.OgrenciSinavKontrolDersBilgi");
            CreateIndex("dbo.SinavSoru", "KonuId");
            CreateIndex("dbo.SinavSoru", "DersId");
            CreateIndex("dbo.SinavSoru", "SinavKitapcikId");
            CreateIndex("dbo.SinavDers", "DersId");
            CreateIndex("dbo.SinavDers", "DersGrupId");
            CreateIndex("dbo.SinavDers", "SinavId");
            CreateIndex("dbo.OptikFormTanimalamaDersBilgi", "DersId");
            CreateIndex("dbo.OptikFormTanimalamaDersBilgi", "SinavId");
            CreateIndex("dbo.OptikFormTanimlama", "SinavId");
            AddForeignKey("dbo.SinavSoru", "SinavKitapcikId", "dbo.SinavKitapcik", "SinavKitapcikId", cascadeDelete: true);
            AddForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu", "KonuId");
            AddForeignKey("dbo.SinavSoru", "DersId", "dbo.Ders", "DersId", cascadeDelete: true);
            AddForeignKey("dbo.SinavDers", "SinavId", "dbo.Sinav", "SinavId", cascadeDelete: true);
            AddForeignKey("dbo.SinavDers", "DersGrupId", "dbo.DersGrup", "DersGrupId", cascadeDelete: true);
            AddForeignKey("dbo.SinavDers", "DersId", "dbo.Ders", "DersId");
            AddForeignKey("dbo.OptikFormTanimlama", "SinavId", "dbo.Sinav", "SinavId");
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.OptikFormTanimlama", "SinavId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.Sinav", "SinavId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders", "DersId", cascadeDelete: true);
        }
    }
}
