namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update009 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DersGrup",
                c => new
                    {
                        DersGrupId = c.Int(nullable: false, identity: true),
                        KurumId = c.Int(),
                        DersGrupAd = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DersGrupId)
                .ForeignKey("dbo.Kurum", t => t.KurumId)
                .Index(t => t.KurumId);
            
            AddColumn("dbo.Ders", "DersGrupId", c => c.Int());
            CreateIndex("dbo.Ders", "DersGrupId");
            AddForeignKey("dbo.Ders", "DersGrupId", "dbo.DersGrup", "DersGrupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ders", "DersGrupId", "dbo.DersGrup");
            DropForeignKey("dbo.DersGrup", "KurumId", "dbo.Kurum");
            DropIndex("dbo.DersGrup", new[] { "KurumId" });
            DropIndex("dbo.Ders", new[] { "DersGrupId" });
            DropColumn("dbo.Ders", "DersGrupId");
            DropTable("dbo.DersGrup");
        }
    }
}
