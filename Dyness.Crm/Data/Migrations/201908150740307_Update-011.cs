namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Konu", "KurumId", c => c.Int());
            CreateIndex("dbo.Konu", "KurumId");
            AddForeignKey("dbo.Konu", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Konu", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Konu", new[] { "KurumId" });
            DropColumn("dbo.Konu", "KurumId");
        }
    }
}
