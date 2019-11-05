namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update021 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu");
            DropIndex("dbo.SinavSoru", new[] { "KonuId" });
            AddColumn("dbo.OptikFormTanimalamaDersBilgi", "SoruAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.SinavSoru", "KonuId", c => c.Int());
            CreateIndex("dbo.SinavSoru", "KonuId");
            AddForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu", "KonuId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu");
            DropIndex("dbo.SinavSoru", new[] { "KonuId" });
            AlterColumn("dbo.SinavSoru", "KonuId", c => c.Int(nullable: false));
            DropColumn("dbo.OptikFormTanimalamaDersBilgi", "SoruAdet");
            CreateIndex("dbo.SinavSoru", "KonuId");
            AddForeignKey("dbo.SinavSoru", "KonuId", "dbo.Konu", "KonuId", cascadeDelete: true);
        }
    }
}
