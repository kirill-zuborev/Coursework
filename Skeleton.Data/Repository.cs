using Skeleton.Data.Model;
using Skeleton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data
{
	public class Repository : IRepository
	{ 
        public int AddGoal(Domain.Entities.Goal goal)
        {
            Goal g = new Goal();
            g.Name = goal.Name;
            g.FolderId = goal.FolderId;


            using (GoalContext context = new GoalContext())
            {
                context.Goals.Add(g);
                context.SaveChanges();
            }
            return g.Id;
        }

        public int AddSubgoal(Domain.Entities.Subgoal subgoal)
        {
            Subgoal subg = new Subgoal();
            subg.Name = subgoal.Name;
            subg.GoalId = subgoal.GoalId;

            using (GoalContext context = new GoalContext())
            {
                context.Subgoals.Add(subg);
                context.SaveChanges();
            }
            return subg.Id;
        }

        public IEnumerable<Domain.Entities.Goal> GetGoalsByFolderId(int folderId)
        {
            List<Domain.Entities.Goal> goalsResult = new List<Domain.Entities.Goal>();
            List<Goal> goals = new List<Goal>();
            using (GoalContext context = new GoalContext())
            {
                goals = context.Goals.Where(c => c.FolderId == folderId).ToList();
            }
            foreach (var goal in goals)
            {
                goalsResult.Add(new Domain.Entities.Goal()
                {
                    Id = goal.Id,
                    Name = goal.Name,
                    DueDate = goal.DueDate,
                    Description = goal.Description,
                    IsDone = goal.IsDone,
                    IsStarred = goal.IsStarred,
                    FolderId = goal.FolderId
                });
            }
            return goalsResult;
        }


        public IEnumerable<Domain.Entities.Subgoal> GetSubgoalsByGoalId(int goalId)
        {
            List<Domain.Entities.Subgoal> subgoalsResult = new List<Domain.Entities.Subgoal>();
            List<Subgoal> subgoals = new List<Subgoal>();
            using (GoalContext context = new GoalContext())
            {
                subgoals = context.Subgoals.Where(c => c.GoalId == goalId).ToList();
            }
            foreach (var subgoal in subgoals)
            {
                subgoalsResult.Add(new Domain.Entities.Subgoal()
                {
                    Id = subgoal.Id,
                    Name = subgoal.Name,
                    GoalId = subgoal.GoalId,
                    IsDone = subgoal.IsDone
                });
            }
            return subgoalsResult;
        }


        public void DeleteSubgoal(int subgoalId)
        {
            using (GoalContext context = new GoalContext())
            {
                var subgoal = context.Subgoals.Where(c => c.Id == subgoalId).FirstOrDefault();
                if (subgoal != null)
                {
                    context.Subgoals.Remove(subgoal);
                    context.SaveChanges();
                } 
            }
        }


        public void ChangeIsDoneInGoal(int goalId)
        {
            using (GoalContext context = new GoalContext())
            {
                var goal = context.Goals.Where(c => c.Id == goalId).FirstOrDefault();
                if (goal != null)
                {
                    goal.IsDone = !goal.IsDone;
                    context.SaveChanges();
                } 
            }
        }

        public void ChangeIsDoneInSubgoal(int subgoalId)
        {
            using (GoalContext context = new GoalContext())
            {
                var subgoal = context.Subgoals.Where(c => c.Id == subgoalId).FirstOrDefault();
                if (subgoal != null)
                {
                    subgoal.IsDone = !subgoal.IsDone;
                    context.SaveChanges();
                } 
            }
        }


        public void ChangeDueDateInGoal(int goalId, DateTime date)
        {
            using (GoalContext context = new GoalContext())
            {
                var goal = context.Goals.Where(c => c.Id == goalId).FirstOrDefault();
                if (goal != null)
                {
                    goal.DueDate = date;
                    context.SaveChanges();
                } 
            }
        }


        public IEnumerable<Domain.Entities.Folder> GetAllFolder(int userId)
        {
            List<Domain.Entities.Folder> foldersResult = new List<Domain.Entities.Folder>();
            List<Folder> folders = new List<Folder>();
            using (GoalContext context = new GoalContext())
            {
                folders = context.Folders.ToList();
            }
            foreach (var folder in folders)
            {
                foldersResult.Add(new Domain.Entities.Folder()
                {
                    Id = folder.Id,
                    Name = folder.Name
                });
            }
            return foldersResult;
        }

        public void DeleteGoal(int goalId)
        {
            using (GoalContext context = new GoalContext())
            {
                var goal = context.Goals.Where(c => c.Id == goalId).FirstOrDefault();
                if (goal != null)
                {
                    context.Goals.Remove(goal);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteFolder(int folderId)
        {
            using (GoalContext context = new GoalContext())
            {
                var folder = context.Folders.Where(c => c.Id == folderId).FirstOrDefault();
                if (folder != null)
                {
                    context.Folders.Remove(folder);
                    context.SaveChanges();
                }
            }
        }


        public void ChangeIsStarredInGoal(int goalId)
        {
            using (GoalContext context = new GoalContext())
            {
                var goal = context.Goals.Where(c => c.Id == goalId).FirstOrDefault();
                if (goal != null)
                {
                    goal.IsStarred = !goal.IsStarred;
                    context.SaveChanges();
                } 
            }
        }


        public int AddFolder(Domain.Entities.Folder folder)
        {
            Folder f = new Folder();
            f.Name = folder.Name;
            f.UserId = folder.UserId;
             
            using (GoalContext context = new GoalContext())
            {
                context.Folders.Add(f);
                context.SaveChanges();
            }
            return f.Id;
        }


        public IEnumerable<Domain.Entities.Goal> SearchGoalsByName(string substring)
        {
            List<Skeleton.Domain.Entities.Goal> result = new List<Domain.Entities.Goal>();
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@str", substring);
            param.SqlDbType = System.Data.SqlDbType.NVarChar; 
            using (GoalContext context = new GoalContext())
            {
                var goals = context.Database.SqlQuery<Model.Goal>("select * from goals where name like '%'+@str+'%'", param);
                //var goals = context.Database.SqlQuery<Model.Goal>("SearchSubstring @str", param);
                foreach (var item in goals)
                {
                    
                    result.Add(new Domain.Entities.Goal()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        DueDate = item.DueDate,
                        IsDone = item.IsDone,
                        IsStarred = item.IsStarred,
                        FolderId = item.FolderId
                    });
                }
            } 
            return result;
        }


        public void ChangeDescriptionInGoal(int goalId, string description)
        {
            using (GoalContext context = new GoalContext())
            {
                var goal = context.Goals.Where(c => c.Id == goalId).FirstOrDefault();
                if (goal != null)
                {
                    goal.Description = description;
                    context.SaveChanges();
                }
            }
        }
    }
}
