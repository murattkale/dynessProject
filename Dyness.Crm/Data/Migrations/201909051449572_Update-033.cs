namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update033 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sinav", "SonuclarGoruntulenebilir", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sinav", "SonuclarGoruntulenebilir");
        }
    }
}
