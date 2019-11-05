namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update031 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OptikForm",
                c => new
                    {
                        OptikFormId = c.Int(nullable: false, identity: true),
                        OptikFormAd = c.String(nullable: false, maxLength: 50),
                        TcNoBasla = c.Int(nullable: false),
                        TcNoAdet = c.Int(nullable: false),
                        OgrenciNoBasla = c.Int(nullable: false),
                        OgrenciNoAdet = c.Int(nullable: false),
                        AdBasla = c.Int(nullable: false),
                        AdAdet = c.Int(nullable: false),
                        SoyadBasla = c.Int(nullable: false),
                        SoyadAdet = c.Int(nullable: false),
                        SinifBasla = c.Int(nullable: false),
                        SinifAdet = c.Int(nullable: false),
                        KitapcikTurBasla = c.Int(nullable: false),
                        KitapcikTurAdet = c.Int(nullable: false),
                        CinsiyetBasla = c.Int(nullable: false),
                        CinsiyetAdet = c.Int(nullable: false),
                        AyracKarakter = c.String(),
                    })
                .PrimaryKey(t => t.OptikFormId);
            
            AddColumn("dbo.Sinav", "OptikFormId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sinav", "OptikFormId");
            AddForeignKey("dbo.Sinav", "OptikFormId", "dbo.OptikForm", "OptikFormId", cascadeDelete: true);
            DropColumn("dbo.SinavTur", "TcNoBasla");
            DropColumn("dbo.SinavTur", "TcNoAdet");
            DropColumn("dbo.SinavTur", "OgrenciNoBasla");
            DropColumn("dbo.SinavTur", "OgrenciNoAdet");
            DropColumn("dbo.SinavTur", "AdBasla");
            DropColumn("dbo.SinavTur", "AdAdet");
            DropColumn("dbo.SinavTur", "SoyadBasla");
            DropColumn("dbo.SinavTur", "SoyadAdet");
            DropColumn("dbo.SinavTur", "SinifBasla");
            DropColumn("dbo.SinavTur", "SinifAdet");
            DropColumn("dbo.SinavTur", "KitapcikTurBasla");
            DropColumn("dbo.SinavTur", "KitapcikTurAdet");
            DropColumn("dbo.SinavTur", "CinsiyetBasla");
            DropColumn("dbo.SinavTur", "CinsiyetAdet");
            DropColumn("dbo.SinavTur", "AyracKarakter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SinavTur", "AyracKarakter", c => c.String());
            AddColumn("dbo.SinavTur", "CinsiyetAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "CinsiyetBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "KitapcikTurAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "KitapcikTurBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SinifAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SinifBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SoyadAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "SoyadBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "AdAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "AdBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "OgrenciNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "OgrenciNoBasla", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "TcNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.SinavTur", "TcNoBasla", c => c.Int(nullable: false));
            DropForeignKey("dbo.Sinav", "OptikFormId", "dbo.OptikForm");
            DropIndex("dbo.Sinav", new[] { "OptikFormId" });
            DropColumn("dbo.Sinav", "OptikFormId");
            DropTable("dbo.OptikForm");
        }
    }
}
