using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skeleton.Domain.Entities;

namespace Skeleton.Domain
{
	public interface IRepository
	{
        int AddGoal(Goal goal);
        int AddSubgoal(Subgoal subgoal);
        int AddFolder(Folder folder);

        IEnumerable<Folder> GetFoldersByUserId(int userId);
        IEnumerable<Goal> GetGoalsByFolderId(int folderId);
        IEnumerable<Subgoal> GetSubgoalsByGoalId(int goalId);

        void DeleteSubgoal(int subgoalId);
        void DeleteGoal(int goalId);
        void DeleteFolder(int folderId);

        void ChangeIsDoneInGoal(int goalId);
        void ChangeIsDoneInSubgoal(int subgoalId);
        void ChangeDueDateInGoal(int goalId, DateTime date);
        void ChangeIsStarredInGoal(int goalId);
        void ChangeDescriptionInGoal(int goalId, string description);

        IEnumerable<Goal> SearchGoalsByName(string substring);
	}
}
