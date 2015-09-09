namespace Skeleton.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreatorIdinFoldertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Folders", "CreatorId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Folders", "CreatorId");
        }
    }
}
