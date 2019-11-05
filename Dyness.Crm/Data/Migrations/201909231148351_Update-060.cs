namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update060 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sms", "OgrenciId", c => c.Int());
            AddColumn("dbo.Sms", "PersonelId", c => c.Int());
            AddColumn("dbo.Sms", "AdSoyad", c => c.String(maxLength: 100));
            AlterColumn("dbo.OnKayit", "Ad", c => c.String(maxLength: 50));
            AlterColumn("dbo.OnKayit", "Soyad", c => c.String(maxLength: 50));
            CreateIndex("dbo.Sms", "OgrenciId");
            CreateIndex("dbo.Sms", "PersonelId");
            AddForeignKey("dbo.Sms", "OgrenciId", "dbo.Ogrenci", "OgrenciId");
            AddForeignKey("dbo.Sms", "PersonelId", "dbo.Personel", "PersonelId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sms", "PersonelId", "dbo.Personel");
            DropForeignKey("dbo.Sms", "OgrenciId", "dbo.Ogrenci");
            DropIndex("dbo.Sms", new[] { "PersonelId" });
            DropIndex("dbo.Sms", new[] { "OgrenciId" });
            AlterColumn("dbo.OnKayit", "Soyad", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.OnKayit", "Ad", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Sms", "AdSoyad");
            DropColumn("dbo.Sms", "PersonelId");
            DropColumn("dbo.Sms", "OgrenciId");
        }
    }
}
