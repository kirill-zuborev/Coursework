namespace Skeleton.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsertable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Folders", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Folders", "UserId");
            AddForeignKey("dbo.Folders", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Folders", "UserId", "dbo.Users");
            DropIndex("dbo.Folders", new[] { "UserId" });
            DropColumn("dbo.Folders", "UserId");
            DropTable("dbo.Users");
        }
    }
}
