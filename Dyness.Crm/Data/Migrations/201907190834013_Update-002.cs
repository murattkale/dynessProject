namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update002 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sezon", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Sezon", new[] { "KurumId" });
            AlterColumn("dbo.Sezon", "KurumId", c => c.Int());
            CreateIndex("dbo.Sezon", "KurumId");
            AddForeignKey("dbo.Sezon", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sezon", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Sezon", new[] { "KurumId" });
            AlterColumn("dbo.Sezon", "KurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sezon", "KurumId");
            AddForeignKey("dbo.Sezon", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: true);
        }
    }
}
