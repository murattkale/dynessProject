namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update065 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Video", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Video", new[] { "KurumId" });
            AddColumn("dbo.Ogrenci", "OgrenciSifre", c => c.String(maxLength: 20));
            AddColumn("dbo.Ogrenci", "VeliSifre", c => c.String(maxLength: 20));
            AddColumn("dbo.Ogrenci", "OgrenciSonGirisTarihi", c => c.DateTime());
            AddColumn("dbo.Ogrenci", "VeliSonGirisTarihi", c => c.DateTime());
            DropColumn("dbo.Video", "KurumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "KurumId", c => c.Int());
            DropColumn("dbo.Ogrenci", "VeliSonGirisTarihi");
            DropColumn("dbo.Ogrenci", "OgrenciSonGirisTarihi");
            DropColumn("dbo.Ogrenci", "VeliSifre");
            DropColumn("dbo.Ogrenci", "OgrenciSifre");
            CreateIndex("dbo.Video", "KurumId");
            AddForeignKey("dbo.Video", "KurumId", "dbo.Kurum", "KurumId");
        }
    }
}
