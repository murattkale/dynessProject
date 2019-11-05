namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update022 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SinavSoru", "Dogru", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SinavSoru", "Dogru", c => c.Int(nullable: false));
        }
    }
}
