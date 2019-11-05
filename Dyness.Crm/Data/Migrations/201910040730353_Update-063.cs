namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update063 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Video", "KonuId", "dbo.Konu");
            DropIndex("dbo.Video", new[] { "KonuId" });
            CreateTable(
                "dbo.VideoKategori",
                c => new
                    {
                        VideoKategoriId = c.Int(nullable: false, identity: true),
                        DersId = c.Int(nullable: false),
                        VideoKategoriAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VideoKategoriId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .Index(t => t.DersId);
            
            CreateTable(
                "dbo.VideoKonu",
                c => new
                    {
                        VideoKonuId = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        KonuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoKonuId)
                .ForeignKey("dbo.Konu", t => t.KonuId, cascadeDelete: true)
                .ForeignKey("dbo.Video", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.KonuId);
            
            CreateTable(
                "dbo.VideoVideoKategori",
                c => new
                    {
                        VideoVideoKategoriId = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        VideoKategoriId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoVideoKategoriId)
                .ForeignKey("dbo.Video", t => t.VideoId, cascadeDelete: true)
                .ForeignKey("dbo.VideoKategori", t => t.VideoKategoriId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.VideoKategoriId);
            
            AlterColumn("dbo.Video", "Url", c => c.String(nullable: false, maxLength: 1000));
            DropColumn("dbo.Video", "KonuId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "KonuId", c => c.Int(nullable: false));
            DropForeignKey("dbo.VideoVideoKategori", "VideoKategoriId", "dbo.VideoKategori");
            DropForeignKey("dbo.VideoVideoKategori", "VideoId", "dbo.Video");
            DropForeignKey("dbo.VideoKonu", "VideoId", "dbo.Video");
            DropForeignKey("dbo.VideoKonu", "KonuId", "dbo.Konu");
            DropForeignKey("dbo.VideoKategori", "DersId", "dbo.Ders");
            DropIndex("dbo.VideoVideoKategori", new[] { "VideoKategoriId" });
            DropIndex("dbo.VideoVideoKategori", new[] { "VideoId" });
            DropIndex("dbo.VideoKonu", new[] { "KonuId" });
            DropIndex("dbo.VideoKonu", new[] { "VideoId" });
            DropIndex("dbo.VideoKategori", new[] { "DersId" });
            AlterColumn("dbo.Video", "Url", c => c.String(nullable: false, maxLength: 500));
            DropTable("dbo.VideoVideoKategori");
            DropTable("dbo.VideoKonu");
            DropTable("dbo.VideoKategori");
            CreateIndex("dbo.Video", "KonuId");
            AddForeignKey("dbo.Video", "KonuId", "dbo.Konu", "KonuId", cascadeDelete: true);
        }
    }
}
