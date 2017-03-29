namespace RandomUserConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RandomUsers",
                c => new
                    {
                        RandomUserId = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255),
                        Phone = c.String(maxLength: 14),
                        Cell = c.String(maxLength: 14),
                        FullName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.RandomUserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RandomUsers");
        }
    }
}
