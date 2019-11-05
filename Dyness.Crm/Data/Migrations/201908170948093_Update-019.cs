namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OptikFormTanimlama", "TcNoBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "TcNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "OgrenciNoBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "OgrenciNoAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "AdBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "AdAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "SoyadBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "SoyadAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "SinifBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "SinifAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "KitapcikTurBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "KitapcikTurAdet", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "CinsiyetBasla", c => c.Int(nullable: false));
            AddColumn("dbo.OptikFormTanimlama", "CinsiyetAdet", c => c.Int(nullable: false));
            AlterColumn("dbo.OptikFormTanimlama", "AyracKarakter", c => c.String(nullable: false));
            DropColumn("dbo.OptikFormTanimlama", "TcNo");
            DropColumn("dbo.OptikFormTanimlama", "OgrenciNo");
            DropColumn("dbo.OptikFormTanimlama", "Ad");
            DropColumn("dbo.OptikFormTanimlama", "Soyad");
            DropColumn("dbo.OptikFormTanimlama", "Sinif");
            DropColumn("dbo.OptikFormTanimlama", "KitapcikTur");
            DropColumn("dbo.OptikFormTanimlama", "Cinsiyet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptikFormTanimlama", "Cinsiyet", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "KitapcikTur", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "Sinif", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "Soyad", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "Ad", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "OgrenciNo", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.OptikFormTanimlama", "TcNo", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.OptikFormTanimlama", "AyracKarakter", c => c.String(nullable: false, maxLength: 1));
            DropColumn("dbo.OptikFormTanimlama", "CinsiyetAdet");
            DropColumn("dbo.OptikFormTanimlama", "CinsiyetBasla");
            DropColumn("dbo.OptikFormTanimlama", "KitapcikTurAdet");
            DropColumn("dbo.OptikFormTanimlama", "KitapcikTurBasla");
            DropColumn("dbo.OptikFormTanimlama", "SinifAdet");
            DropColumn("dbo.OptikFormTanimlama", "SinifBasla");
            DropColumn("dbo.OptikFormTanimlama", "SoyadAdet");
            DropColumn("dbo.OptikFormTanimlama", "SoyadBasla");
            DropColumn("dbo.OptikFormTanimlama", "AdAdet");
            DropColumn("dbo.OptikFormTanimlama", "AdBasla");
            DropColumn("dbo.OptikFormTanimlama", "OgrenciNoAdet");
            DropColumn("dbo.OptikFormTanimlama", "OgrenciNoBasla");
            DropColumn("dbo.OptikFormTanimlama", "TcNoAdet");
            DropColumn("dbo.OptikFormTanimlama", "TcNoBasla");
        }
    }
}
