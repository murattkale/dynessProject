namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update038 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OgrenciSinavKontrolDersPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropForeignKey("dbo.OgrenciSinavKontrolDersPuan", "PuanTurId", "dbo.PuanTur");
            DropIndex("dbo.OgrenciSinavKontrolDersPuan", new[] { "OgrenciSinavKontrolId" });
            DropIndex("dbo.OgrenciSinavKontrolDersPuan", new[] { "PuanTurId" });
            CreateTable(
                "dbo.OgrenciSinavKontrolPuanTurPuan",
                c => new
                    {
                        OgrenciSinavKontrolPuanTurPuanId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        PuanTurId = c.Int(nullable: false),
                        Puan = c.Double(nullable: false),
                        ToplamPuan = c.Double(nullable: false),
                        SinifSira = c.Int(nullable: false),
                        SubeSira = c.Int(nullable: false),
                        GenelSira = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolPuanTurPuanId)
                .ForeignKey("dbo.OgrenciSinavKontrol", t => t.OgrenciSinavKontrolId, cascadeDelete: true)
                .ForeignKey("dbo.PuanTur", t => t.PuanTurId, cascadeDelete: true)
                .Index(t => t.OgrenciSinavKontrolId)
                .Index(t => t.PuanTurId);
            
            DropTable("dbo.OgrenciSinavKontrolDersPuan");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OgrenciSinavKontrolDersPuan",
                c => new
                    {
                        OgrenciSinavKontrolDersPuanId = c.Int(nullable: false, identity: true),
                        OgrenciSinavKontrolId = c.Int(nullable: false),
                        PuanTurId = c.Int(nullable: false),
                        Puan = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OgrenciSinavKontrolDersPuanId);
            
            DropForeignKey("dbo.OgrenciSinavKontrolPuanTurPuan", "PuanTurId", "dbo.PuanTur");
            DropForeignKey("dbo.OgrenciSinavKontrolPuanTurPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol");
            DropIndex("dbo.OgrenciSinavKontrolPuanTurPuan", new[] { "PuanTurId" });
            DropIndex("dbo.OgrenciSinavKontrolPuanTurPuan", new[] { "OgrenciSinavKontrolId" });
            DropTable("dbo.OgrenciSinavKontrolPuanTurPuan");
            CreateIndex("dbo.OgrenciSinavKontrolDersPuan", "PuanTurId");
            CreateIndex("dbo.OgrenciSinavKontrolDersPuan", "OgrenciSinavKontrolId");
            AddForeignKey("dbo.OgrenciSinavKontrolDersPuan", "PuanTurId", "dbo.PuanTur", "PuanTurId", cascadeDelete: true);
            AddForeignKey("dbo.OgrenciSinavKontrolDersPuan", "OgrenciSinavKontrolId", "dbo.OgrenciSinavKontrol", "OgrenciSinavKontrolId", cascadeDelete: true);
        }
    }
}
