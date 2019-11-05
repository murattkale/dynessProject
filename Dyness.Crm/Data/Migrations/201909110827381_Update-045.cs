namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update045 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SmsHesapDurum", "SmsHesapDurumAd", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.SmsHesapDurum", "SubeSmsBilgiDurumAd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SmsHesapDurum", "SubeSmsBilgiDurumAd", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.SmsHesapDurum", "SmsHesapDurumAd");
        }
    }
}
