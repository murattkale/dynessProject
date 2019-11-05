namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update048 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmsMetinSablon",
                c => new
                    {
                        SmsMetinSablonId = c.Int(nullable: false, identity: true),
                        SubeId = c.Int(),
                        Baslik = c.String(nullable: false, maxLength: 100),
                        Sablon = c.String(nullable: false, maxLength: 100),
                        EtkinMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SmsMetinSablonId)
                .ForeignKey("dbo.Sube", t => t.SubeId)
                .Index(t => t.SubeId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SmsMetinSablon", "SubeId", "dbo.Sube");
            DropIndex("dbo.SmsMetinSablon", new[] { "SubeId" });
            DropTable("dbo.SmsMetinSablon");
        }
    }
}
