namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update037 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OptikForm", "AyracKarakter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptikForm", "AyracKarakter", c => c.String());
        }
    }
}
