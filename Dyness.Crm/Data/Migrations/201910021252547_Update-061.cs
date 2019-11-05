namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update061 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoKurumYetki",
                c => new
                    {
                        VideoKurumYetkiId = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        KurumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoKurumYetkiId)
                .ForeignKey("dbo.Kurum", t => t.KurumId, cascadeDelete: true)
                .ForeignKey("dbo.Video", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.KurumId);
            
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        VideoId = c.Int(nullable: false, identity: true),
                        DersId = c.Int(nullable: false),
                        KonuId = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 200),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VideoId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: false)
                .ForeignKey("dbo.Konu", t => t.KonuId, cascadeDelete: false)
                .Index(t => t.DersId)
                .Index(t => t.KonuId);
            
            CreateTable(
                "dbo.VideoSinifYetki",
                c => new
                    {
                        VideoSinifYetkiId = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        SinifId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoSinifYetkiId)
                .ForeignKey("dbo.Sinif", t => t.SinifId, cascadeDelete: true)
                .ForeignKey("dbo.Video", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.SinifId);
            
            CreateTable(
                "dbo.VideoSubeYetki",
                c => new
                    {
                        VideoSubeYetkiId = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoSubeYetkiId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .ForeignKey("dbo.Video", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.SubeId);
            
            AlterColumn("dbo.Konu", "Baslik", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoSubeYetki", "VideoId", "dbo.Video");
            DropForeignKey("dbo.VideoSubeYetki", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.VideoSinifYetki", "VideoId", "dbo.Video");
            DropForeignKey("dbo.VideoSinifYetki", "SinifId", "dbo.Sinif");
            DropForeignKey("dbo.VideoKurumYetki", "VideoId", "dbo.Video");
            DropForeignKey("dbo.Video", "KonuId", "dbo.Konu");
            DropForeignKey("dbo.Video", "DersId", "dbo.Ders");
            DropForeignKey("dbo.VideoKurumYetki", "KurumId", "dbo.Kurum");
            DropIndex("dbo.VideoSubeYetki", new[] { "SubeId" });
            DropIndex("dbo.VideoSubeYetki", new[] { "VideoId" });
            DropIndex("dbo.VideoSinifYetki", new[] { "SinifId" });
            DropIndex("dbo.VideoSinifYetki", new[] { "VideoId" });
            DropIndex("dbo.Video", new[] { "KonuId" });
            DropIndex("dbo.Video", new[] { "DersId" });
            DropIndex("dbo.VideoKurumYetki", new[] { "KurumId" });
            DropIndex("dbo.VideoKurumYetki", new[] { "VideoId" });
            AlterColumn("dbo.Konu", "Baslik", c => c.String(nullable: false, maxLength: 200));
            DropTable("dbo.VideoSubeYetki");
            DropTable("dbo.VideoSinifYetki");
            DropTable("dbo.Video");
            DropTable("dbo.VideoKurumYetki");
        }
    }
}
