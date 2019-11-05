namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update043 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptikFormDersGrupBilgi",
                c => new
                    {
                        OptikFormDersGrupBilgiId = c.Int(nullable: false, identity: true),
                        OptikFormId = c.Int(nullable: false),
                        DersGrupId = c.Int(nullable: false),
                        DersBasla = c.Int(nullable: false),
                        DersAdet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptikFormDersGrupBilgiId)
                .ForeignKey("dbo.DersGrup", t => t.DersGrupId, cascadeDelete: true)
                .ForeignKey("dbo.OptikForm", t => t.OptikFormId, cascadeDelete: true)
                .Index(t => t.OptikFormId)
                .Index(t => t.DersGrupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OptikFormDersGrupBilgi", "OptikFormId", "dbo.OptikForm");
            DropForeignKey("dbo.OptikFormDersGrupBilgi", "DersGrupId", "dbo.DersGrup");
            DropIndex("dbo.OptikFormDersGrupBilgi", new[] { "DersGrupId" });
            DropIndex("dbo.OptikFormDersGrupBilgi", new[] { "OptikFormId" });
            DropTable("dbo.OptikFormDersGrupBilgi");
        }
    }
}
