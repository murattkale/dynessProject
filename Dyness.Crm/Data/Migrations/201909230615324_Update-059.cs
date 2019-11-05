namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update059 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon");
            DropIndex("dbo.Sms", new[] { "SmsMetinSablonId" });
            AlterColumn("dbo.Sms", "SmsMetinSablonId", c => c.Int());
            CreateIndex("dbo.Sms", "SmsMetinSablonId");
            AddForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon", "SmsMetinSablonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon");
            DropIndex("dbo.Sms", new[] { "SmsMetinSablonId" });
            AlterColumn("dbo.Sms", "SmsMetinSablonId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sms", "SmsMetinSablonId");
            AddForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon", "SmsMetinSablonId", cascadeDelete: true);
        }
    }
}
