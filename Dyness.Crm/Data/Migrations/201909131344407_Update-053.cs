namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update053 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sms", "SmsMetinSablonId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sms", "SmsMetinSablonId");
            AddForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon", "SmsMetinSablonId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sms", "SmsMetinSablonId", "dbo.SmsMetinSablon");
            DropIndex("dbo.Sms", new[] { "SmsMetinSablonId" });
            DropColumn("dbo.Sms", "SmsMetinSablonId");
        }
    }
}
