namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update028 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ogrenci", "DogumTarihi", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ogrenci", "DogumTarihi", c => c.DateTime(nullable: false));
        }
    }
}
