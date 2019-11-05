namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update032 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum");
            DropIndex("dbo.SinavTur", new[] { "KurumId" });
            CreateTable(
                "dbo.SinavSubeYetki",
                c => new
                    {
                        SinavSubeYetkiId = c.Int(nullable: false, identity: true),
                        SinavId = c.Int(nullable: false),
                        SubeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SinavSubeYetkiId)
                .ForeignKey("dbo.Sinav", t => t.SinavId, cascadeDelete: true)
                .ForeignKey("dbo.Sube", t => t.SubeId, cascadeDelete: true)
                .Index(t => t.SinavId)
                .Index(t => t.SubeId);
            
            AddColumn("dbo.Sinav", "SezonId", c => c.Int(nullable: false));
            AddColumn("dbo.OptikForm", "KurumId", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavTur", "KurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sinav", "SezonId");
            CreateIndex("dbo.OptikForm", "KurumId");
            CreateIndex("dbo.SinavTur", "KurumId");
            AddForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: false);
            AddForeignKey("dbo.Sinav", "SezonId", "dbo.Sezon", "SezonId", cascadeDelete: true);
            AddForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum");
            DropForeignKey("dbo.SinavSubeYetki", "SubeId", "dbo.Sube");
            DropForeignKey("dbo.SinavSubeYetki", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.Sinav", "SezonId", "dbo.Sezon");
            DropForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum");
            DropIndex("dbo.SinavTur", new[] { "KurumId" });
            DropIndex("dbo.SinavSubeYetki", new[] { "SubeId" });
            DropIndex("dbo.SinavSubeYetki", new[] { "SinavId" });
            DropIndex("dbo.OptikForm", new[] { "KurumId" });
            DropIndex("dbo.Sinav", new[] { "SezonId" });
            AlterColumn("dbo.SinavTur", "KurumId", c => c.Int());
            DropColumn("dbo.OptikForm", "KurumId");
            DropColumn("dbo.Sinav", "SezonId");
            DropTable("dbo.SinavSubeYetki");
            CreateIndex("dbo.SinavTur", "KurumId");
            AddForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum", "KurumId");
        }
    }
}
