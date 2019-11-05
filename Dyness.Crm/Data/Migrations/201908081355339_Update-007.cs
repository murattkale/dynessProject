namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update007 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ogrenci", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Ogrenci", "IX_OgrenciNoKurumIdUnique");
            DropIndex("dbo.Ogrenci", new[] { "SubeId" });
            CreateIndex("dbo.Ogrenci", new[] { "SubeId", "OgrenciNo" }, unique: true, name: "IX_OgrenciNoSubeIdUnique");
            DropColumn("dbo.Ogrenci", "KurumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ogrenci", "KurumId", c => c.Int(nullable: false));
            DropIndex("dbo.Ogrenci", "IX_OgrenciNoSubeIdUnique");
            CreateIndex("dbo.Ogrenci", "SubeId");
            CreateIndex("dbo.Ogrenci", new[] { "KurumId", "OgrenciNo" }, unique: true, name: "IX_OgrenciNoKurumIdUnique");
            AddForeignKey("dbo.Ogrenci", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: true);
        }
    }
}
