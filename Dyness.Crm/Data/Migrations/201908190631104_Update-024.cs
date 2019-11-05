namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update024 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OgrenciSinavKontrol", "SubeId", c => c.Int(nullable: false));
            CreateIndex("dbo.OgrenciSinavKontrol", "SubeId");
            AddForeignKey("dbo.OgrenciSinavKontrol", "SubeId", "dbo.Sube", "SubeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OgrenciSinavKontrol", "SubeId", "dbo.Sube");
            DropIndex("dbo.OgrenciSinavKontrol", new[] { "SubeId" });
            DropColumn("dbo.OgrenciSinavKontrol", "SubeId");
        }
    }
}
