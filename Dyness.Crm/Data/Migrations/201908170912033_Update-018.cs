namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update018 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OptikFormTanimlama", "AyracKarakter", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OptikFormTanimlama", "AyracKarakter", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
