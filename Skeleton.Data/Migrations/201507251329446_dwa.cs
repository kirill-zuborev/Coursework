namespace Skeleton.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dwa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DueDate = c.DateTime(),
                        IsDone = c.Boolean(nullable: false),
                        IsStarred = c.Boolean(nullable: false),
                        FolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.FolderId, cascadeDelete: true)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.Subgoals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDone = c.Boolean(nullable: false),
                        GoalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goals", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.GoalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subgoals", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.Goals", "FolderId", "dbo.Folders");
            DropIndex("dbo.Subgoals", new[] { "GoalId" });
            DropIndex("dbo.Goals", new[] { "FolderId" });
            DropTable("dbo.Subgoals");
            DropTable("dbo.Goals");
            DropTable("dbo.Folders");
        }
    }
}
