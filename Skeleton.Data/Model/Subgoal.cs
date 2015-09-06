using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data.Model
{
    public class Subgoal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }

        public int GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}
