using System.Web.Mvc;
using VDolgah.Models;

namespace VDolgah.Controllers
{
    public class AccountController : Controller
    {
        DBEntities db = new DBEntities();
        //
        // GET: /Account/

        public ActionResult Register()
        {
            user r = new user();
            return View(r);
        }

        [HttpPost]
        public ActionResult Register(user u)
        {
            AccountChecker ac = new AccountChecker(u.email, u.password_hesh);
            if (!(ViewBag.Error = ac.CheckEmail()))
            {
                u.salt = ac.GenerateSalt();
                u.password_hesh = ac.CreateMD5Hash();
                u.last_ip = ac.getLastIP();
                db.users.Add(u);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cookies["user"].Value = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
