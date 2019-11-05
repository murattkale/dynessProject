namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update047 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Konu", "Kod", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Konu", "Kod");
        }
    }
}
