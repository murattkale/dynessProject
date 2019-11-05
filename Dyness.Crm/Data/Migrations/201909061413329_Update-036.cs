namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update036 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnKayit",
                c => new
                    {
                        OnKayitId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.OnKayitId);
            
            AddColumn("dbo.OgrenciSinavKontrol", "OnKayitId", c => c.Int());
            AddColumn("dbo.OgrenciSinavKontrol", "Dogrulamalar", c => c.String(maxLength: 500));
            AddColumn("dbo.OgrenciSinavKontrolPuan", "SinifSira", c => c.Int(nullable: false));
            AddColumn("dbo.OgrenciSinavKontrolPuan", "SubeSira", c => c.Int(nullable: false));
            AddColumn("dbo.OgrenciSinavKontrolPuan", "GenelSira", c => c.Int(nullable: false));
            AddColumn("dbo.SinavKitapcik", "CevapAnahtari", c => c.String(maxLength: 500));
            AlterColumn("dbo.OgrenciSinavKontrol", "SoruCevaplar", c => c.String(maxLength: 500));
            AlterColumn("dbo.SinavKitapcikDersBilgi", "CevapAnahtartari", c => c.String(maxLength: 500));
            AlterColumn("dbo.SinavKitapcikDersBilgi", "DersKonuBilgi", c => c.String(maxLength: 1000));
            CreateIndex("dbo.OgrenciSinavKontrol", "OnKayitId");
            AddForeignKey("dbo.OgrenciSinavKontrol", "OnKayitId", "dbo.OnKayit", "OnKayitId");
            DropColumn("dbo.OgrenciSinavKontrol", "TcKimlikNo");
            DropColumn("dbo.OgrenciSinavKontrol", "OgrenciNo");
            DropColumn("dbo.OgrenciSinavKontrol", "Ad");
            DropColumn("dbo.OgrenciSinavKontrol", "Soyad");
            DropColumn("dbo.OgrenciSinavKontrol", "Sinif");
            DropColumn("dbo.OgrenciSinavKontrol", "KadinMi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OgrenciSinavKontrol", "KadinMi", c => c.Boolean(nullable: false));
            AddColumn("dbo.OgrenciSinavKontrol", "Sinif", c => c.String(maxLength: 10));
            AddColumn("dbo.OgrenciSinavKontrol", "Soyad", c => c.String(maxLength: 50));
            AddColumn("dbo.OgrenciSinavKontrol", "Ad", c => c.String(maxLength: 50));
            AddColumn("dbo.OgrenciSinavKontrol", "OgrenciNo", c => c.String(maxLength: 20));
            AddColumn("dbo.OgrenciSinavKontrol", "TcKimlikNo", c => c.String(maxLength: 11));
            DropForeignKey("dbo.OgrenciSinavKontrol", "OnKayitId", "dbo.OnKayit");
            DropIndex("dbo.OgrenciSinavKontrol", new[] { "OnKayitId" });
            AlterColumn("dbo.SinavKitapcikDersBilgi", "DersKonuBilgi", c => c.String());
            AlterColumn("dbo.SinavKitapcikDersBilgi", "CevapAnahtartari", c => c.String());
            AlterColumn("dbo.OgrenciSinavKontrol", "SoruCevaplar", c => c.String());
            DropColumn("dbo.SinavKitapcik", "CevapAnahtari");
            DropColumn("dbo.OgrenciSinavKontrolPuan", "GenelSira");
            DropColumn("dbo.OgrenciSinavKontrolPuan", "SubeSira");
            DropColumn("dbo.OgrenciSinavKontrolPuan", "SinifSira");
            DropColumn("dbo.OgrenciSinavKontrol", "Dogrulamalar");
            DropColumn("dbo.OgrenciSinavKontrol", "OnKayitId");
            DropTable("dbo.OnKayit");
        }
    }
}
