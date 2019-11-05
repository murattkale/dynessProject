namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update012 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinavTur", "KurumId", c => c.Int());
            CreateIndex("dbo.SinavTur", "KurumId");
            AddForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavTur", "KurumId", "dbo.Kurum");
            DropIndex("dbo.SinavTur", new[] { "KurumId" });
            DropColumn("dbo.SinavTur", "KurumId");
        }
    }
}
