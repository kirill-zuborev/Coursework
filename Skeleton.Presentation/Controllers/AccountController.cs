using Skeleton.Presentation.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Skeleton.Presentation.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(loginModel.UserName, loginModel.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sorry the login or password is invalid");
                }
            }
            return View(loginModel); 
        }
        
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(registrationModel.UserName, registrationModel.Password);
                    if (ReturnUrl != null)
                    { 
                        Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch(MembershipCreateUserException)
                {
                    ModelState.AddModelError("", "Sorry the login lready exists");
                }
            }
            return View(registrationModel);
        }
        
        public ActionResult LogOut()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }
	}
}