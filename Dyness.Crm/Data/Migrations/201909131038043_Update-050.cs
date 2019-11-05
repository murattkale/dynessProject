namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update050 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmsDurum",
                c => new
                    {
                        SmsDurumId = c.Int(nullable: false, identity: true),
                        SmsDurumAd = c.String(nullable: false, maxLength: 100),
                        Kod = c.String(nullable: false, maxLength: 5),
                        Aciklama = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.SmsDurumId);
            
            CreateTable(
                "dbo.Sms",
                c => new
                    {
                        SmsId = c.Int(nullable: false, identity: true),
                        SmsDurumId = c.Int(nullable: false),
                        SmsHesapId = c.Int(nullable: false),
                        DlrId = c.Int(nullable: false),
                        GecerlilikSuresi = c.Int(nullable: false),
                        SmsAdet = c.Int(nullable: false),
                        KrediAdet = c.Int(nullable: false),
                        TelefonNo = c.String(nullable: false, maxLength: 20),
                        Mesaj = c.String(nullable: false),
                        GonderilecegiTarih = c.DateTime(),
                    })
                .PrimaryKey(t => t.SmsId)
                .ForeignKey("dbo.SmsDurum", t => t.SmsDurumId, cascadeDelete: false)
                .ForeignKey("dbo.SmsHesap", t => t.SmsHesapId, cascadeDelete: true)
                .Index(t => t.SmsDurumId)
                .Index(t => t.SmsHesapId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sms", "SmsHesapId", "dbo.SmsHesap");
            DropForeignKey("dbo.Sms", "SmsDurumId", "dbo.SmsDurum");
            DropIndex("dbo.Sms", new[] { "SmsHesapId" });
            DropIndex("dbo.Sms", new[] { "SmsDurumId" });
            DropTable("dbo.Sms");
            DropTable("dbo.SmsDurum");
        }
    }
}
