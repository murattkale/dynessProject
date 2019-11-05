namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update041 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmsHesapDosya",
                c => new
                    {
                        SmsHesapDosyaId = c.Int(nullable: false, identity: true),
                        SmsHesapId = c.Int(nullable: false),
                        DosyaAd = c.String(maxLength: 100),
                        YuklenmeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.SmsHesapDosyaId)
                .ForeignKey("dbo.SmsHesap", t => t.SmsHesapId, cascadeDelete: true)
                .Index(t => t.SmsHesapId);
            
            CreateTable(
                "dbo.SmsHesap",
                c => new
                    {
                        SmsHesapId = c.Int(nullable: false, identity: true),
                        SubeId = c.Int(nullable: false),
                        SubeSmsHesapDurumId = c.Int(nullable: false),
                        Kredi = c.Int(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 11),
                        OlusturulmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(nullable: false),
                        SonHareketTarihi = c.DateTime(),
                        SmsHesapDurum_SmsHesapDurumId = c.Int(),
                    })
                .PrimaryKey(t => t.SmsHesapId)
                .ForeignKey("dbo.SmsHesapDurum", t => t.SmsHesapDurum_SmsHesapDurumId)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SubeId)
                .Index(t => t.SmsHesapDurum_SmsHesapDurumId);
            
            CreateTable(
                "dbo.SmsHesapDurum",
                c => new
                    {
                        SmsHesapDurumId = c.Int(nullable: false, identity: true),
                        SubeSmsBilgiDurumAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SmsHesapDurumId);
            
            CreateTable(
                "dbo.SmsHesapHareket",
                c => new
                    {
                        SmsHesapHareketId = c.Int(nullable: false, identity: true),
                        SmsHesapId = c.Int(nullable: false),
                        PersonelId = c.Int(nullable: false),
                        Kredi = c.Int(nullable: false),
                        Telefon = c.String(maxLength: 20),
                        HareketTarihi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SmsHesapHareketId)
                .ForeignKey("dbo.Personel", t => t.PersonelId, cascadeDelete: true)
                .ForeignKey("dbo.SmsHesap", t => t.SmsHesapId, cascadeDelete: false)
                .Index(t => t.SmsHesapId)
                .Index(t => t.PersonelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SmsHesapHareket", "SmsHesapId", "dbo.SmsHesap");
            DropForeignKey("dbo.SmsHesapHareket", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.SmsHesap", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.SmsHesap", "SmsHesapDurum_SmsHesapDurumId", "dbo.SmsHesapDurum");
            DropForeignKey("dbo.SmsHesapDosya", "SmsHesapId", "dbo.SmsHesap");
            DropIndex("dbo.SmsHesapHareket", new[] { "PersonelId" });
            DropIndex("dbo.SmsHesapHareket", new[] { "SmsHesapId" });
            DropIndex("dbo.SmsHesap", new[] { "SmsHesapDurum_SmsHesapDurumId" });
            DropIndex("dbo.SmsHesap", new[] { "SubeId" });
            DropIndex("dbo.SmsHesapDosya", new[] { "SmsHesapId" });
            DropTable("dbo.SmsHesapHareket");
            DropTable("dbo.SmsHesapDurum");
            DropTable("dbo.SmsHesap");
            DropTable("dbo.SmsHesapDosya");
        }
    }
}
