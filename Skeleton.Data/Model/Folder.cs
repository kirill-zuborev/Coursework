using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data.Model
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Goal> Goals { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
