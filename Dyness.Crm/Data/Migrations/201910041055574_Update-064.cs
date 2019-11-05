namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update064 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoKategori", "KurumId", c => c.Int());
            CreateIndex("dbo.VideoKategori", "KurumId");
            AddForeignKey("dbo.VideoKategori", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoKategori", "KurumId", "dbo.Kurum");
            DropIndex("dbo.VideoKategori", new[] { "KurumId" });
            DropColumn("dbo.VideoKategori", "KurumId");
        }
    }
}
