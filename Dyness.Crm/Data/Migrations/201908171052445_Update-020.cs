namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptikFormTanimalamaDersBilgi", "DersBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimalamaDersBilgi", "DersAdet", c => c.Int(nullable: false));
            DropColumn("dbo.OptikFormTanimalamaDersBilgi", "Bilgi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptikFormTanimalamaDersBilgi", "Bilgi", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.OptikFormTanimalamaDersBilgi", "DersAdet");
            DropColumn("dbo.OptikFormTanimalamaDersBilgi", "DersBasla");
        }
    }
}
