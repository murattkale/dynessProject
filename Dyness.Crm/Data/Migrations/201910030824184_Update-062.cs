namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update062 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Video", "KurumId", c => c.Int());
            AddColumn("dbo.Video", "Url", c => c.String(nullable: false, maxLength: 1000));
            CreateIndex("dbo.Video", "KurumId");
            AddForeignKey("dbo.Video", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Video", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Video", new[] { "KurumId" });
            DropColumn("dbo.Video", "Url");
            DropColumn("dbo.Video", "KurumId");
        }
    }
}
