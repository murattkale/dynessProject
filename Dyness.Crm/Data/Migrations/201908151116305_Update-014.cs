namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update014 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sinav", "KurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sinav", "KurumId");
            AddForeignKey("dbo.Sinav", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sinav", "KurumId", "dbo.Kurum");
            DropIndex("dbo.Sinav", new[] { "KurumId" });
            DropColumn("dbo.Sinav", "KurumId");
        }
    }
}
