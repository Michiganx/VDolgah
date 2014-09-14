using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (user.email != null && user.password_hash != null && !user.email.Equals(String.Empty) && !user.password_hash.Equals(String.Empty))
            {
                Regex reg = new Regex(@"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*");
                AccountChecker checker = new AccountChecker(user);
                if ((ViewBag.ErrorMessage = checker.CheckData(reg.Match(user.email).Success)) == null)
                {
                    user = checker.GetUser();
                    checker.db.SaveChanges();
                    Session.Clear();
                    Session["user"] = user;
                    Session.Timeout = 10000;
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
