using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Data.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public virtual List<Folder> Folders { get; set; }
    }
}
