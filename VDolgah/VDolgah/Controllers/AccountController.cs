using System.Web.Mvc;
using VDolgah.Models;

namespace VDolgah.Controllers
{
    public class AccountController : Controller
    {
        DbEntities db = DbEntities.Instance;
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
            if (u.email == null || u.first_name == null || u.last_name == null|| u.login == null|| u.password_hash == null || u.confirm_password == null)
            {
                ViewBag.Error = "Не все поля заполнены";
                return View();
            }
            AccountChecker ac = new AccountChecker(u);
            if ((ViewBag.Error = ac.CheckEmail(false)) == null)
            {
                u.salt = ac.GenerateSalt();
                u.password_hash = ac.CreateMD5Hash();
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
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Thanks()
        {
            return View();
        }

        public ActionResult ChangeData()
        {
            return View();
        }
    }
}
