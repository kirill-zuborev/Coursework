namespace Skeleton.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class GoalContext : DbContext
    { 
        public GoalContext()
            : base("name=GoalContext")
        {
        }

        public DbSet<Model.Goal> Goals { get; set; }
        public DbSet<Model.Subgoal> Subgoals { get; set; }
        public DbSet<Model.Folder> Folders { get; set; }

        public DbSet<Model.User> Users { get; set; }
    } 
}