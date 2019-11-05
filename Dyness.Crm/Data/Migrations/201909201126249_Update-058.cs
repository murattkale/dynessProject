namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update058 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OnKayit", "TcKimlikNo", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OnKayit", "TcKimlikNo", c => c.String(nullable: false, maxLength: 11));
        }
    }
}
