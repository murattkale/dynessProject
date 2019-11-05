namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptikFormTanimlama",
                c => new
                    {
                        OptikFormTanimlamaId = c.Int(nullable: false),
                        TcNo = c.String(nullable: false, maxLength: 10),
                        OgrenciNo = c.String(nullable: false, maxLength: 10),
                        Ad = c.String(nullable: false, maxLength: 10),
                        Soyad = c.String(nullable: false, maxLength: 10),
                        Sinif = c.String(nullable: false, maxLength: 10),
                        KitapcikTur = c.String(nullable: false, maxLength: 10),
                        Cinsiyet = c.String(nullable: false, maxLength: 10),
                        AyracKarakter = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimlamaId)
                .ForeignKey("dbo.SinavKitapcik", t => t.OptikFormTanimlamaId)
                .Index(t => t.OptikFormTanimlamaId);
            
            CreateTable(
                "dbo.OptikFormTanimalamaDersBilgi",
                c => new
                    {
                        OptikFormTanimalamaDersBilgiId = c.Int(nullable: false, identity: true),
                        OptikFormTanimlamaId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        Bilgi = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimalamaDersBilgiId)
                .ForeignKey("dbo.Ders", t => t.DersId, cascadeDelete: true)
                .ForeignKey("dbo.OptikFormTanimlama", t => t.OptikFormTanimlamaId, cascadeDelete: true)
                .Index(t => t.OptikFormTanimlamaId)
                .Index(t => t.DersId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptikFormTanimlama", "OptikFormTanimlamaId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders");
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "DersId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "OptikFormTanimlamaId" });
            DropIndex("dbo.OptikFormTanimlama", new[] { "OptikFormTanimlamaId" });
            DropTable("dbo.OptikFormTanimalamaDersBilgi");
            DropTable("dbo.OptikFormTanimlama");
        }
    }
}
