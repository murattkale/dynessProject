namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update039 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OgrenciSinavKontrol", "Ad", c => c.String(maxLength: 50));
            AddColumn("dbo.OgrenciSinavKontrol", "Soyad", c => c.String(maxLength: 50));
            AddColumn("dbo.OgrenciSinavKontrol", "AdSoyad", c => c.String(maxLength: 100));
            AddColumn("dbo.OgrenciSinavKontrol", "TcKimlikNo", c => c.String(maxLength: 11));
            AddColumn("dbo.OgrenciSinavKontrol", "OgrenciNo", c => c.String(maxLength: 20));
            AddColumn("dbo.OgrenciSinavKontrol", "Sinif", c => c.String(maxLength: 20));
            AddColumn("dbo.OgrenciSinavKontrol", "Cinsiyet", c => c.String(maxLength: 1));
            AddColumn("dbo.OgrenciSinavKontrol", "KitapcikBaslik", c => c.String(maxLength: 1));
            AddColumn("dbo.OnKayit", "Ad", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.OnKayit", "Soyad", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.OnKayit", "AdSoyad", c => c.String(maxLength: 100));
            AddColumn("dbo.OnKayit", "TcKimlikNo", c => c.String(nullable: false, maxLength: 11));
            AddColumn("dbo.OnKayit", "KadinMi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnKayit", "KadinMi");
            DropColumn("dbo.OnKayit", "TcKimlikNo");
            DropColumn("dbo.OnKayit", "AdSoyad");
            DropColumn("dbo.OnKayit", "Soyad");
            DropColumn("dbo.OnKayit", "Ad");
            DropColumn("dbo.OgrenciSinavKontrol", "KitapcikBaslik");
            DropColumn("dbo.OgrenciSinavKontrol", "Cinsiyet");
            DropColumn("dbo.OgrenciSinavKontrol", "Sinif");
            DropColumn("dbo.OgrenciSinavKontrol", "OgrenciNo");
            DropColumn("dbo.OgrenciSinavKontrol", "TcKimlikNo");
            DropColumn("dbo.OgrenciSinavKontrol", "AdSoyad");
            DropColumn("dbo.OgrenciSinavKontrol", "Soyad");
            DropColumn("dbo.OgrenciSinavKontrol", "Ad");
        }
    }
}
