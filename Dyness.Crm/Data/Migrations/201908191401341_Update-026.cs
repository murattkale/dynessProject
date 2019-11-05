namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update026 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur");
            DropIndex("dbo.SinavTurDersKatSayi", new[] { "PuanTurId" });
            AlterColumn("dbo.SinavTurDersKatSayi", "PuanTurId", c => c.Int());
            CreateIndex("dbo.SinavTurDersKatSayi", "PuanTurId");
            AddForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur", "PuanTurId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur");
            DropIndex("dbo.SinavTurDersKatSayi", new[] { "PuanTurId" });
            AlterColumn("dbo.SinavTurDersKatSayi", "PuanTurId", c => c.Int(nullable: false));
            CreateIndex("dbo.SinavTurDersKatSayi", "PuanTurId");
            AddForeignKey("dbo.SinavTurDersKatSayi", "PuanTurId", "dbo.PuanTur", "PuanTurId", cascadeDelete: true);
        }
    }
}
