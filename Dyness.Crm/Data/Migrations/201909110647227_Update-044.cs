namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update044 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptikFormDersGrupBilgi", "DersGrupBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormDersGrupBilgi", "DersGrupAdet", c => c.Int(nullable: false));
            DropColumn("dbo.OptikFormDersGrupBilgi", "DersBasla");
            DropColumn("dbo.OptikFormDersGrupBilgi", "DersAdet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptikFormDersGrupBilgi", "DersAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormDersGrupBilgi", "DersBasla", c => c.Int(nullable: false));
            DropColumn("dbo.OptikFormDersGrupBilgi", "DersGrupAdet");
            DropColumn("dbo.OptikFormDersGrupBilgi", "DersGrupBasla");
        }
    }
}
