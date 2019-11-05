namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update042 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SmsHesap", "SmsHesapDurum_SmsHesapDurumId", "dbo.SmsHesapDurum");
            DropIndex("dbo.SmsHesap", new[] { "SmsHesapDurum_SmsHesapDurumId" });
            RenameColumn(table: "dbo.SmsHesap", name: "SmsHesapDurum_SmsHesapDurumId", newName: "SmsHesapDurumId");
            AlterColumn("dbo.SmsHesap", "SmsHesapDurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.SmsHesap", "SmsHesapDurumId");
            AddForeignKey("dbo.SmsHesap", "SmsHesapDurumId", "dbo.SmsHesapDurum", "SmsHesapDurumId", cascadeDelete: true);
            DropColumn("dbo.SmsHesap", "SubeSmsHesapDurumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SmsHesap", "SubeSmsHesapDurumId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SmsHesap", "SmsHesapDurumId", "dbo.SmsHesapDurum");
            DropIndex("dbo.SmsHesap", new[] { "SmsHesapDurumId" });
            AlterColumn("dbo.SmsHesap", "SmsHesapDurumId", c => c.Int());
            RenameColumn(table: "dbo.SmsHesap", name: "SmsHesapDurumId", newName: "SmsHesapDurum_SmsHesapDurumId");
            CreateIndex("dbo.SmsHesap", "SmsHesapDurum_SmsHesapDurumId");
            AddForeignKey("dbo.SmsHesap", "SmsHesapDurum_SmsHesapDurumId", "dbo.SmsHesapDurum", "SmsHesapDurumId");
        }
    }
}
