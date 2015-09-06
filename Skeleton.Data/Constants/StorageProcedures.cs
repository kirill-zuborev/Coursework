using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data.Constants
{
    public static class StorageProcedures
    {
        public static readonly string GetFolders = "dbo.GET_FOLDERS";

        public static readonly string GetGoals = "dbo.GET_GOALS";

        public static readonly string GetSubgoals = "dbo.GET_SUBGOALS";

        public static readonly string SearchGoalsByName = "dbo.SEARCH_GOALS_BY_NAME";
    }
}
