namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update029 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OgrenciSozlesme", "DosyaAd", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OgrenciSozlesme", "DosyaAd");
        }
    }
}
