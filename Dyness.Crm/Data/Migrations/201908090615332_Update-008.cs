namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update008 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncelleyenPersonelId", c => c.Int());
            AddColumn("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncellenmeTarihi", c => c.DateTime());
            CreateIndex("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncelleyenPersonelId");
            AddForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncelleyenPersonelId", "dbo.Personel", "PersonelId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncelleyenPersonelId", "dbo.Personel");
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "SonGuncelleyenPersonelId" });
            DropColumn("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncellenmeTarihi");
            DropColumn("dbo.OgrenciSozlesmeOdemeBilgi", "SonGuncelleyenPersonelId");
        }
    }
}
