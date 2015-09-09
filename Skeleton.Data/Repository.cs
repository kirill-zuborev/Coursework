using Skeleton.Data.Model;
using Skeleton.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Skeleton.Data.Constants;

namespace Skeleton.Data
{
	public class Repository : IRepository
    {
        #region Private fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Repository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["GoalContext"].ConnectionString;
        }

        #endregion

        #region IRepository members

        public IEnumerable<Domain.Entities.Goal> GetGoalsByFolderId(int folderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (IDbCommand command = new SqlCommand(StorageProcedures.GetGoals, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter()
                    {
                        DbType = DbType.Int64,
                        ParameterName = StorageProceduresParameters.FolderId,
                        Value = folderId
                    });

                    List<Domain.Entities.Goal> goals = new List<Domain.Entities.Goal>();

                    connection.Open();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        goals = ParseGoals(reader).ToList();
                    }

                    connection.Close();

                    return goals;
                }
            }
        }

        public IEnumerable<Domain.Entities.Subgoal> GetSubgoalsByGoalId(int goalId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (IDbCommand command = new SqlCommand(StorageProcedures.GetSubgoals, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter()
                    {
                        DbType = DbType.Int64,
                        ParameterName = StorageProceduresParameters.GoalId,
                        Value = goalId
                    });

                    List<Domain.Entities.Subgoal> subgoals = new List<Domain.Entities.Subgoal>();

                    connection.Open();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        subgoals = ParseSubgoals(reader).ToList();
                    }

                    connection.Close();

                    return subgoals;
                }
            }
        }

        public IEnumerable<Domain.Entities.Folder> GetFoldersByUserId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (IDbCommand command = new SqlCommand(StorageProcedures.GetFolders, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter()
                    {
                        DbType = DbType.Int64,
                        ParameterName = StorageProceduresParameters.UserId,
                        Value = userId
                    });

                    List<Domain.Entities.Folder> folders = new List<Domain.Entities.Folder>();

                    connection.Open();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        folders = ParseFolders(reader).ToList();
                    }

                    connection.Close();

                    return folders;
                }
            }
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

        public IEnumerable<Domain.Entities.User> GetAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (IDbCommand command = new SqlCommand("SELECT * FROM dbo.Users", connection))
                {
                    List<Domain.Entities.User> folders = new List<Domain.Entities.User>();

                    connection.Open();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        folders = ParseUsers(reader).ToList();
                    }

                    connection.Close();

                    return folders;
                }
            }
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

        #endregion

        #region Private metods

        private IEnumerable<Skeleton.Domain.Entities.Goal> ParseGoals(IDataReader reader)
        {
            List<Domain.Entities.Goal> goals = new List<Domain.Entities.Goal>();

            while (reader.Read())
            {
                goals.Add(ParseGoal(reader));
            }

            return goals;
        }

        private Domain.Entities.Goal ParseGoal(IDataReader reader)
        {
            return new Domain.Entities.Goal()
            {
                Id = (int)reader[ReaderColumnNames.Id],
                Description = reader[ReaderColumnNames.Description] is DBNull ? null : (string)reader[ReaderColumnNames.Description],
                DueDate = reader[ReaderColumnNames.DueDate] is DBNull ? null : (DateTime?)reader[ReaderColumnNames.DueDate],
                FolderId = (int)reader[ReaderColumnNames.FolderId],
                IsDone = (Boolean)reader[ReaderColumnNames.IsDone],
                IsStarred = (Boolean)reader[ReaderColumnNames.IsStarred],
                Name = (string)reader[ReaderColumnNames.Name]
            };
        }

        private IEnumerable<Skeleton.Domain.Entities.Folder> ParseFolders(IDataReader reader)
        {
            List<Domain.Entities.Folder> folders = new List<Domain.Entities.Folder>();

            while (reader.Read())
            {
                folders.Add(ParseFolder(reader));
            }

            return folders;
        }

        private Domain.Entities.Folder ParseFolder(IDataReader reader)
        {
            return new Domain.Entities.Folder()
            {
                Id = (int)reader[ReaderColumnNames.Id],
                Name = (string)reader[ReaderColumnNames.Name],
                UserId = (int)reader[ReaderColumnNames.UserId]
            };
        }

        private IEnumerable<Skeleton.Domain.Entities.Subgoal> ParseSubgoals(IDataReader reader)
        {
            List<Domain.Entities.Subgoal> subgoals = new List<Domain.Entities.Subgoal>();

            while (reader.Read())
            {
                subgoals.Add(ParseSubgoal(reader));
            }

            return subgoals;
        }

        private Domain.Entities.Subgoal ParseSubgoal(IDataReader reader)
        {
            return new Domain.Entities.Subgoal()
            {
                Id = (Int32)reader[ReaderColumnNames.Id],
                Name = (string)reader[ReaderColumnNames.Name],
                IsDone = (Boolean)reader[ReaderColumnNames.IsDone],
                GoalId = (int)reader[ReaderColumnNames.GoalId]
            };
        }

        private IEnumerable<Skeleton.Domain.Entities.User> ParseUsers(IDataReader reader)
        {
            List<Domain.Entities.User> users = new List<Domain.Entities.User>();

            while (reader.Read())
            {
                users.Add(ParseUser(reader));
            }

            return users;
        }

        private Domain.Entities.User ParseUser(IDataReader reader)
        {
            return new Domain.Entities.User()
            {
                Id = (Int32)reader[ReaderColumnNames.Id],
                Login = (string)reader[ReaderColumnNames.Login]
            };
        }

        #endregion
    }
}
