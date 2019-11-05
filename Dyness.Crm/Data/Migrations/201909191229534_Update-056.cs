namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update056 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptikForm", "AdSoyadBasla", c => c.Int());
            AddColumn("dbo.OptikForm", "AdSoyadAdet", c => c.Int());
            AlterColumn("dbo.OptikForm", "OgrenciNoBasla", c => c.Int());
            AlterColumn("dbo.OptikForm", "OgrenciNoAdet", c => c.Int());
            AlterColumn("dbo.OptikForm", "AdBasla", c => c.Int());
            AlterColumn("dbo.OptikForm", "AdAdet", c => c.Int());
            AlterColumn("dbo.OptikForm", "SoyadBasla", c => c.Int());
            AlterColumn("dbo.OptikForm", "SoyadAdet", c => c.Int());
            AlterColumn("dbo.OptikForm", "SinifBasla", c => c.Int());
            AlterColumn("dbo.OptikForm", "SinifAdet", c => c.Int());
            AlterColumn("dbo.OptikForm", "CinsiyetBasla", c => c.Int());
            AlterColumn("dbo.OptikForm", "CinsiyetAdet", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OptikForm", "CinsiyetAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "CinsiyetBasla", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "SinifAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "SinifBasla", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "SoyadAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "SoyadBasla", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "AdAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "AdBasla", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "OgrenciNoAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikForm", "OgrenciNoBasla", c => c.Int(nullable: false));
            DropColumn("dbo.OptikForm", "AdSoyadAdet");
            DropColumn("dbo.OptikForm", "AdSoyadBasla");
        }
    }
}
