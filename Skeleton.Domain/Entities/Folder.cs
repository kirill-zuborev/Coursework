﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Domain.Entities
{
    public class Folder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public int CreatorId { get; set; }
    }
}
