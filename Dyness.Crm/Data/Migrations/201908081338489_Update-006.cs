namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update006 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube");
            DropIndex("dbo.Ogrenci", new[] { "SubeId" });
            AlterColumn("dbo.Ogrenci", "SubeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ogrenci", "SubeId");
            AddForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube", "SubeId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube");
            DropIndex("dbo.Ogrenci", new[] { "SubeId" });
            AlterColumn("dbo.Ogrenci", "SubeId", c => c.Int());
            CreateIndex("dbo.Ogrenci", "SubeId");
            AddForeignKey("dbo.Ogrenci", "SubeId", "dbo.Sube", "SubeId");
        }
    }
}
