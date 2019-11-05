namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update003 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Yayin", "BransId", "dbo.Brans");
            DropIndex("dbo.Yayin", new[] { "BransId" });
            AlterColumn("dbo.Yayin", "BransId", c => c.Int());
            CreateIndex("dbo.Yayin", "BransId");
            AddForeignKey("dbo.Yayin", "BransId", "dbo.Brans", "BransId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Yayin", "BransId", "dbo.Brans");
            DropIndex("dbo.Yayin", new[] { "BransId" });
            AlterColumn("dbo.Yayin", "BransId", c => c.Int(nullable: false));
            CreateIndex("dbo.Yayin", "BransId");
            AddForeignKey("dbo.Yayin", "BransId", "dbo.Brans", "BransId", cascadeDelete: true);
        }
    }
}
