namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update015 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders");
            DropForeignKey("dbo.OptikFormTanimlama", "SinavKitapcikId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama");
            DropForeignKey("dbo.SinavDers", "DersId", "dbo.Ders");
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "OptikFormTanimlamaId" });
            DropIndex("dbo.OptikFormTanimalamaDersBilgi", new[] { "DersId" });
            DropIndex("dbo.OptikFormTanimlama", new[] { "SinavKitapcikId" });
            DropIndex("dbo.SinavDers", new[] { "DersId" });
            AlterColumn("dbo.SinavDers", "DersId", c => c.Int());
            AlterColumn("dbo.SinavDers", "Sira", c => c.Int());
            AlterColumn("dbo.SinavDers", "SoruSayi", c => c.Int());
            CreateIndex("dbo.SinavDers", "DersId");
            AddForeignKey("dbo.SinavDers", "DersId", "dbo.Ders", "DersId");
            DropTable("dbo.OptikFormTanimalamaDersBilgi");
            DropTable("dbo.OptikFormTanimlama");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OptikFormTanimlama",
                c => new
                    {
                        OptikFormTanimlamaId = c.Int(nullable: false, identity: true),
                        SinavKitapcikId = c.Int(nullable: false),
                        TcNo = c.String(nullable: false, maxLength: 10),
                        OgrenciNo = c.String(nullable: false, maxLength: 10),
                        Ad = c.String(nullable: false, maxLength: 10),
                        Soyad = c.String(nullable: false, maxLength: 10),
                        Sinif = c.String(nullable: false, maxLength: 10),
                        KitapcikTur = c.String(nullable: false, maxLength: 10),
                        Cinsiyet = c.String(nullable: false, maxLength: 10),
                        AyracKarakter = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimlamaId);
            
            CreateTable(
                "dbo.OptikFormTanimalamaDersBilgi",
                c => new
                    {
                        OptikFormTanimalamaDersBilgiId = c.Int(nullable: false, identity: true),
                        OptikFormTanimlamaId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        Bilgi = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.OptikFormTanimalamaDersBilgiId);
            
            DropForeignKey("dbo.SinavDers", "DersId", "dbo.Ders");
            DropIndex("dbo.SinavDers", new[] { "DersId" });
            AlterColumn("dbo.SinavDers", "SoruSayi", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavDers", "Sira", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavDers", "DersId", c => c.Int(nullable: false));
            CreateIndex("dbo.SinavDers", "DersId");
            CreateIndex("dbo.OptikFormTanimlama", "SinavKitapcikId");
            CreateIndex("dbo.OptikFormTanimalamaDersBilgi", "DersId");
            CreateIndex("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId");
            AddForeignKey("dbo.SinavDers", "DersId", "dbo.Ders", "DersId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama", "OptikFormTanimlamaId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimlama", "SinavKitapcikId", "dbo.SinavKitapcik", "SinavKitapcikId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "DersId", "dbo.Ders", "DersId", cascadeDelete: true);
        }
    }
}
