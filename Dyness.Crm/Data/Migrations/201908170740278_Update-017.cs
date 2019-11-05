namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update017 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OptikFormTanimlama", "OptikFormTanimlamaId", "dbo.SinavKitapcik");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama");
            DropIndex("dbo.OptikFormTanimlama", new[] { "OptikFormTanimlamaId" });
            RenameColumn(table: "dbo.OptikFormTanimalamaDersBilgi", name: "OptikFormTanimlamaId", newName: "SinavId");
            RenameIndex(table: "dbo.OptikFormTanimalamaDersBilgi", name: "IX_OptikFormTanimlamaId", newName: "IX_SinavId");
            DropPrimaryKey("dbo.OptikFormTanimlama");
            AddColumn("dbo.OptikFormTanimlama", "SinavId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.OptikFormTanimlama", "SinavId");
            CreateIndex("dbo.OptikFormTanimlama", "SinavId");
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.Sinav", "SinavId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimlama", "SinavId", "dbo.Sinav", "SinavId");
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.OptikFormTanimlama", "SinavId", cascadeDelete: true);
            DropColumn("dbo.OptikFormTanimlama", "OptikFormTanimlamaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OptikFormTanimlama", "OptikFormTanimlamaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.OptikFormTanimlama");
            DropForeignKey("dbo.OptikFormTanimlama", "SinavId", "dbo.Sinav");
            DropForeignKey("dbo.OptikFormTanimalamaDersBilgi", "SinavId", "dbo.Sinav");
            DropIndex("dbo.OptikFormTanimlama", new[] { "SinavId" });
            DropPrimaryKey("dbo.OptikFormTanimlama");
            DropColumn("dbo.OptikFormTanimlama", "SinavId");
            AddPrimaryKey("dbo.OptikFormTanimlama", "OptikFormTanimlamaId");
            RenameIndex(table: "dbo.OptikFormTanimalamaDersBilgi", name: "IX_SinavId", newName: "IX_OptikFormTanimlamaId");
            RenameColumn(table: "dbo.OptikFormTanimalamaDersBilgi", name: "SinavId", newName: "OptikFormTanimlamaId");
            CreateIndex("dbo.OptikFormTanimlama", "OptikFormTanimlamaId");
            AddForeignKey("dbo.OptikFormTanimalamaDersBilgi", "OptikFormTanimlamaId", "dbo.OptikFormTanimlama", "OptikFormTanimlamaId", cascadeDelete: true);
            AddForeignKey("dbo.OptikFormTanimlama", "OptikFormTanimlamaId", "dbo.SinavKitapcik", "SinavKitapcikId");
        }
    }
}
