using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDolgah.Models;

namespace VDolgah.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            user user = new user();
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(user user)
        {
            if (user.email != null && user.password_hesh != null && !user.email.Equals(String.Empty) && !user.password_hesh.Equals(String.Empty))
            {
                AccountChecker checker = new AccountChecker(user.email, user.password_hesh);
                if ((ViewBag.ErrorMessage = checker.CheckData()) == null)
                {
                    user = checker.GetUser();
                    Response.Cookies["user"].Value = user.name;
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }

        public ActionResult Register()
        {
            return RedirectToAction("Register", "Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
