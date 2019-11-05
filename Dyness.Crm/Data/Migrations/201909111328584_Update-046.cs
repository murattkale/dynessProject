namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update046 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OptikFormDersGrupBilgi", "DersGrupBasla", c => c.Int());
            AlterColumn("dbo.OptikFormDersGrupBilgi", "DersGrupAdet", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OptikFormDersGrupBilgi", "DersGrupAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikFormDersGrupBilgi", "DersGrupBasla", c => c.Int(nullable: false));
        }
    }
}
