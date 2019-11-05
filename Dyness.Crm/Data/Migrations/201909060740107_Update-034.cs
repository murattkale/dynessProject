namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update034 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinavSubeYetki", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.SinavSubeYetki", "SubeId", "dbo.Sube");
            DropIndex("dbo.SinavSubeYetki", new[] { "SinavId" });
            DropIndex("dbo.SinavSubeYetki", new[] { "SubeId" });
            CreateTable(
                "dbo.SinavSube",
                c => new
                    {
                        SinavSubeId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                        DosyaEkleyenPersonelId = c.Int(),
                        DosyaAd = c.String(maxLength: 100),
                        DosyaYuklenmeTarih = c.DateTime(),
                    })
                .PrimaryKey(t => t.SinavSubeId)
                .ForeignKey("dbo.Personel", t => t.DosyaEkleyenPersonelId)
                .ForeignKey("dbo.Sinav", t => t.SinavId, cascadeDelete: true)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SinavId)
                .Index(t => t.SubeId)
                .Index(t => t.DosyaEkleyenPersonelId);
            
            DropTable("dbo.SinavSubeYetki");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SinavSubeYetki",
                c => new
                    {
                        SinavSubeYetkiId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SinavSubeYetkiId);
            
            DropForeignKey("dbo.SinavSube", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.SinavSube", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.SinavSube", "DosyaEkleyenPersonelId", "dbo.Personel");
            DropIndex("dbo.SinavSube", new[] { "DosyaEkleyenPersonelId" });
            DropIndex("dbo.SinavSube", new[] { "SubeId" });
            DropIndex("dbo.SinavSube", new[] { "SinavId" });
            DropTable("dbo.SinavSube");
            CreateIndex("dbo.SinavSubeYetki", "SubeId");
            CreateIndex("dbo.SinavSubeYetki", "SinavId");
            AddForeignKey("dbo.SinavSubeYetki", "SubeId", "dbo.Sube", "SubeId", cascadeDelete: true);
            AddForeignKey("dbo.SinavSubeYetki", "SinavId", "dbo.Sinav", "SinavId", cascadeDelete: true);
        }
    }
}
