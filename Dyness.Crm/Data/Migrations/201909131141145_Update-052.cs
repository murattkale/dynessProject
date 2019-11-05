namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update052 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmsHesapHareketTip",
                c => new
                    {
                        SmsHesapHareketTipId = c.Int(nullable: false, identity: true),
                        SmsHesapHareketTipAd = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SmsHesapHareketTipId);
            
            AddColumn("dbo.SmsHesapHareket", "SmsHesapHareketTipId", c => c.Int(nullable: false));
            CreateIndex("dbo.SmsHesapHareket", "SmsHesapHareketTipId");
            AddForeignKey("dbo.SmsHesapHareket", "SmsHesapHareketTipId", "dbo.SmsHesapHareketTip", "SmsHesapHareketTipId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SmsHesapHareket", "SmsHesapHareketTipId", "dbo.SmsHesapHareketTip");
            DropIndex("dbo.SmsHesapHareket", new[] { "SmsHesapHareketTipId" });
            DropColumn("dbo.SmsHesapHareket", "SmsHesapHareketTipId");
            DropTable("dbo.SmsHesapHareketTip");
        }
    }
}
