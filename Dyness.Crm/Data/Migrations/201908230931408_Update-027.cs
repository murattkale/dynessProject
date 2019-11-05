namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update027 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransferTip",
                c => new
                    {
                        TransferTipId = c.Int(nullable: false, identity: true),
                        TransferTipAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransferTipId);
            
            AddColumn("dbo.Hesap", "TransferTipId", c => c.Int());
            AlterColumn("dbo.SinavKitapcik", "Baslik", c => c.String(maxLength: 5));
            AlterColumn("dbo.SinavTurDers", "Sira", c => c.Int());
            AlterColumn("dbo.SinavTurDers", "SoruSayi", c => c.Int());
            CreateIndex("dbo.Hesap", "TransferTipId");
            AddForeignKey("dbo.Hesap", "TransferTipId", "dbo.TransferTip", "TransferTipId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hesap", "TransferTipId", "dbo.TransferTip");
            DropIndex("dbo.Hesap", new[] { "TransferTipId" });
            AlterColumn("dbo.SinavTurDers", "SoruSayi", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavTurDers", "Sira", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavKitapcik", "Baslik", c => c.String(nullable: false, maxLength: 5));
            DropColumn("dbo.Hesap", "TransferTipId");
            DropTable("dbo.TransferTip");
        }
    }
}
