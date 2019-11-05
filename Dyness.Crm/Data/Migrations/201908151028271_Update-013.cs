namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update013 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SinavTurDersKatSayi", "KatSayi", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SinavTurDersKatSayi", "KatSayi", c => c.Double(nullable: false));
        }
    }
}
