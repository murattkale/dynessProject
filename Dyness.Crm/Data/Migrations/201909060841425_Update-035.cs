namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update035 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinavSube", "DegelendirildiMi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SinavSube", "DegelendirildiMi");
        }
    }
}
