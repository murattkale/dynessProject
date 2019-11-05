namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update049 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SmsMetinSablon", "Sablon", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SmsMetinSablon", "Sablon", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
