namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update055 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum");
            DropIndex("dbo.SinavTur", new[] { "KurumId" });
            AlterColumn("dbo.SinavTur", "KurumId", c => c.Int());
            CreateIndex("dbo.SinavTur", "KurumId");
            AddForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum");
            DropIndex("dbo.SinavTur", new[] { "KurumId" });
            AlterColumn("dbo.SinavTur", "KurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.SinavTur", "KurumId");
            AddForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: true);
        }
    }
}
