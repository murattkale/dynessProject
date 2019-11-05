namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update054 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum");
            DropIndex("dbo.OptikForm", new[] { "KurumId" });
            AlterColumn("dbo.OptikForm", "KurumId", c => c.Int());
            AlterColumn("dbo.Sms", "Mesaj", c => c.String(nullable: false, maxLength: 917));
            CreateIndex("dbo.OptikForm", "KurumId");
            AddForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum", "KurumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum");
            DropIndex("dbo.OptikForm", new[] { "KurumId" });
            AlterColumn("dbo.Sms", "Mesaj", c => c.String(nullable: false));
            AlterColumn("dbo.OptikForm", "KurumId", c => c.Int(nullable: false));
            CreateIndex("dbo.OptikForm", "KurumId");
            AddForeignKey("dbo.OptikForm", "KurumId", "dbo.Kurum", "KurumId", cascadeDelete: true);
        }
    }
}
