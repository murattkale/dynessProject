namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update004 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId", "dbo.OdemeTur");
            DropIndex("dbo.OgrenciSozlesmeOdemeBilgi", new[] { "OdemeTurId" });
            DropColumn("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId");
            DropTable("dbo.OdemeTur");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OdemeTur",
                c => new
                    {
                        OdemeTurId = c.Int(nullable: false, identity: true),
                        OdemeTurAd = c.String(nullable: false, maxLength: 50),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OdemeTurId);
            
            AddColumn("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId", c => c.Int(nullable: false));
            CreateIndex("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId");
            AddForeignKey("dbo.OgrenciSozlesmeOdemeBilgi", "OdemeTurId", "dbo.OdemeTur", "OdemeTurId", cascadeDelete: true);
        }
    }
}
