namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update051 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SmsDurum", "Aciklama", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SmsDurum", "Aciklama", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
