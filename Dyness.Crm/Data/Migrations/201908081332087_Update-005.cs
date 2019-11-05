namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ogrenci", "SubeId", c => c.Int());
            CreateIndex("dbo.Ogrenci", "SubeId");
            AddForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube", "SubeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube");
            DropIndex("dbo.Ogrenci", new[] { "SubeId" });
            DropColumn("dbo.Ogrenci", "SubeId");
        }
    }
}
