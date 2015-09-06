using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skeleton.Domain;
using Skeleton.Presentation.Models.HomeViewModels;
using Newtonsoft.Json;
using WebMatrix.WebData;

namespace Skeleton.Presentation.Controllers
{
	public class HomeController : Controller
    {
        #region Private Fields

        private readonly IRepository _repo;

        #endregion

        #region Constructors

        public HomeController(IRepository repo)
		{
			if (repo == null)
			{
				throw new ArgumentNullException("repo");
			}
			_repo = repo;
		}

        #endregion

        #region Action metods

        public ActionResult Index()
        {
			return View();
		}

        #endregion

        #region Public metods

        public JsonResult SearchGoalsByName(string substring)
        {
            return Json(_repo.SearchGoalsByName(substring), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoalsByFolderId(int folderId)
        {
            return Json(_repo.GetGoalsByFolderId(folderId), JsonRequestBehavior.AllowGet);
        } 

        public void DeleteSubgoal(int subgoalId)
		{
            _repo.DeleteSubgoal(subgoalId); 
		}

        public void ChangeDescriptionInGoal(int goalId, string description)
        {
            _repo.ChangeDescriptionInGoal(goalId, description);
        }

        public void ChangeDateInGoal(int goalId, string date)
        {
            DateTime goalDate;
            if(DateTime.TryParse(date, out goalDate))
            {
                _repo.ChangeDueDateInGoal(goalId, goalDate);
            }  
        }

        public void ChangeIsDoneInGoal(int goalId)
        {
            _repo.ChangeIsDoneInGoal(goalId);
        }

        public void DeleteFolder(int folderId)
        {
            _repo.DeleteFolder(folderId);
        } 

        public void DeleteGoal(int goalId)
        {
            _repo.DeleteGoal(goalId);
        } 

        public JsonResult GetSubgoalsByGoalId(int goalId)
        {
            return Json(_repo.GetSubgoalsByGoalId(goalId), JsonRequestBehavior.AllowGet);
        }

        public void ChangeIsDoneInSubgoal(int subgoalId)
        {
            _repo.ChangeIsDoneInSubgoal(subgoalId);
        }

        public void ChangeIsStarredInGoal(int goalId)
        {
            _repo.ChangeIsStarredInGoal(goalId);
        }

        public int AddSubgoal(int goalId, string name)
        {
            Domain.Entities.Subgoal subgoal = new Domain.Entities.Subgoal();
            subgoal.GoalId = goalId;
            subgoal.Name = name; 
            return _repo.AddSubgoal(subgoal); 
        }

        public int AddFolder(string name)
        {
            Domain.Entities.Folder folder = new Domain.Entities.Folder();
            folder.Name = name;
            folder.UserId = WebSecurity.CurrentUserId;
            return _repo.AddFolder(folder);
        }

        public int AddGoal(string name, int folderId)
        {
            Domain.Entities.Goal goal = new Domain.Entities.Goal();
            goal.Name = name;
            goal.FolderId = folderId;
             
            return _repo.AddGoal(goal);
        }

        [Authorize]
        public JsonResult GetAllFoldersByUserId()
        {
            return Json(_repo.GetFoldersByUserId(WebSecurity.CurrentUserId), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}