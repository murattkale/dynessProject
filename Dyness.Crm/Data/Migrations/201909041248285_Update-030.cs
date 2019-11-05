namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update030 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EskiKayit",
                c => new
                    {
                        EskiKayitId = c.Int(nullable: false, identity: true),
                        SubeId = c.Int(nullable: false),
                        SozlesmeDurumu = c.String(maxLength: 50),
                        OgrenciTckn = c.String(maxLength: 20),
                        OgrenciId = c.String(maxLength: 50),
                        OgrenciNo = c.String(maxLength: 50),
                        OgrenciAdi = c.String(maxLength: 50),
                        OgrenciSoyadi = c.String(maxLength: 50),
                        OgrenciTelefon = c.String(maxLength: 50),
                        OgrenciAdres = c.String(maxLength: 300),
                        OgrenciEposta = c.String(maxLength: 250),
                        ServisBilgisi = c.String(maxLength: 100),
                        Nakit = c.String(maxLength: 50),
                        Cek = c.String(maxLength: 50),
                        KrediKartiPosCihazi = c.String(maxLength: 50),
                        Havale = c.String(maxLength: 50),
                        KrediKartiSanalPos = c.String(maxLength: 50),
                        MailOrder = c.String(maxLength: 50),
                        KayitUcreti = c.String(maxLength: 50),
                        KalanOdeme = c.String(maxLength: 50),
                        OdemeTuru = c.String(maxLength: 50),
                        KayitTarihi = c.String(maxLength: 50),
                        BiziNeredenDuydunuz = c.String(maxLength: 250),
                        SinifSeviyesi = c.String(maxLength: 50),
                        Sinif = c.String(maxLength: 50),
                        Brans = c.String(maxLength: 50),
                        Sezon = c.String(maxLength: 50),
                        SubeBilgi = c.String(maxLength: 50),
                        Adres = c.String(maxLength: 300),
                        Ilce = c.String(maxLength: 50),
                        Il = c.String(maxLength: 50),
                        Ulke = c.String(maxLength: 50),
                        VeliAnne = c.String(maxLength: 100),
                        AnneTckn = c.String(maxLength: 20),
                        AnneTel = c.String(maxLength: 50),
                        VeliBaba = c.String(maxLength: 100),
                        BabaTckn = c.String(maxLength: 20),
                        BabaTel = c.String(maxLength: 50),
                        VeliDiger = c.String(maxLength: 100),
                        DigerTckn = c.String(maxLength: 20),
                        DigerTel = c.String(maxLength: 50),
                        FaturaAdSoyad = c.String(maxLength: 150),
                        VergiDairesi = c.String(maxLength: 100),
                        VergiTckNo = c.String(maxLength: 50),
                        FaturaAdres = c.String(maxLength: 300),
                        FaturaSemt = c.String(maxLength: 50),
                        FaturaIlce = c.String(maxLength: 50),
                        FaturaSehir = c.String(maxLength: 50),
                        FaturaPostaKodu = c.String(maxLength: 50),
                        Gorusen = c.String(maxLength: 100),
                        KaydiYapan = c.String(maxLength: 100),
                        Referans = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.EskiKayitId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SubeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EskiKayit", "SubeId", "dbo.Sube");
            DropIndex("dbo.EskiKayit", new[] { "SubeId" });
            DropTable("dbo.EskiKayit");
        }
    }
}
