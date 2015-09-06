using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data.Model
{
    public class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsDone { get; set; }
        public bool IsStarred { get; set; }

        public virtual List<Subgoal> Subgoals { get; set; }

        public int FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}
