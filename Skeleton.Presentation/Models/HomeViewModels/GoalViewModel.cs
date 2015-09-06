using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skeleton.Presentation.Models.HomeViewModels
{
    public class GoalViewModel
    {
        public List<Domain.Entities.Goal> Goals { get; set; }
    }
}