namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update040 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OgrenciSinavKontrolPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropForeignKey("dbo.OgrenciSinavKontrolPuan", "PuanTurId", "dbo.PuanTur");
            DropIndex("dbo.OgrenciSinavKontrolPuan", new[] { "OgrenciSinavKontrolId" });
            DropIndex("dbo.OgrenciSinavKontrolPuan", new[] { "PuanTurId" });
            DropTable("dbo.OgrenciSinavKontrolPuan");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OgrenciSinavKontrolPuan",
                c => new
                    {
                        OgrenciSinavKontrolPuanId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        PuanTurId = c.Int(nullable: false),
                        SinifSira = c.Int(nullable: false),
                        SubeSira = c.Int(nullable: false),
                        GenelSira = c.Int(nullable: false),
                        Puan = c.Double(nullable: false),
                        ToplamPuan = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolPuanId);
            
            CreateIndex("dbo.OgrenciSinavKontrolPuan", "PuanTurId");
            CreateIndex("dbo.OgrenciSinavKontrolPuan", "OgrenciSinavKontrolId");
            AddForeignKey("dbo.OgrenciSinavKontrolPuan", "PuanTurId", "dbo.PuanTur", "PuanTurId", cascadeDelete: true);
            AddForeignKey("dbo.OgrenciSinavKontrolPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol", "OgrenciSinavKontrolId", cascadeDelete: true);
        }
    }
}
