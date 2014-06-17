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
            if (u.email == null || u.name == null || u.password_hesh == null || u.confirm_password == null)
            {
                ViewBag.Error = "Не все поля заполнены";
                return View();
            }
            AccountChecker ac = new AccountChecker(u.email, u.password_hesh, u.confirm_password);
            if ((ViewBag.Error = ac.CheckEmail()) == null)
            {
                u.salt = ac.GenerateSalt();
                u.password_hesh = ac.CreateMD5Hash();
                u.last_ip = ac.getLastIP();
                db.users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Thanks");
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

        public ActionResult Thanks()
        {
            return View();
        }
    }
}
