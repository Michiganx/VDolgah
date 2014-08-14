using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
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
            ViewBag.GroupList = GetGroupList(Session["user"] as user);
            return View();
        }

        private List<GroupWrapper> GetGroupList(user user)
        {
            List<GroupWrapper> list = new List<GroupWrapper>();
            foreach (group g in user.groups)
            {
                GroupWrapper wrapper = new GroupWrapper();
                wrapper.Group = g;
                if (g.creator == user.id)
                    wrapper.creator = true;
                list.Add(wrapper);
            }
            return list;
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
            user u = Session["user"] as user;
            return View(u);
        }

        [HttpPost]
        public ActionResult ChangeData(user user)
        {
            user oldData = db.users.Where((x) => x.id == user.id).First();
            oldData.first_name = user.first_name;
            oldData.last_name = user.last_name;
            if (db.users.Where((x) => x.login == user.login).ToList().Count > 0)
            {
                ViewBag.Error = "Пользователь с таким логином уже существует";
                return View(user);
            }
            oldData.login = user.login;
            oldData.email = user.email;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            PasswordChanger pass = new PasswordChanger();
            pass.userId = (Session["user"] as user).id;
            return View(pass);
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordChanger obj)
        {
            user user = db.users.Where((x) => x.id == obj.userId).First();
            string dbHash = user.password_hash;
            user.password_hash = obj.oldPass;
            AccountChecker check = new AccountChecker(user);
            if (dbHash.Equals(check.CreateMD5Hash()))
            {
                if (obj.NewPass.Equals(obj.confirm))
                {
                    user.password_hash = obj.NewPass;
                    check = new AccountChecker(user);
                    if (dbHash.Equals(check.CreateMD5Hash()))
                    {
                        ViewBag.Error = "Cтарый и новый пароль совпадают";
                        return View(obj);
                    }
                    else
                    {
                        user.salt = check.GenerateSalt();
                        check = new AccountChecker(user);
                        user.password_hash = check.CreateMD5Hash();
                        db.SaveChanges();
                        ViewBag.Error = "Пароль изменен";
                        return View(obj);
                    }
                }
                else
                {
                    ViewBag.Error = "Новый пароль и подтверждение не совпадают";
                    return View(obj);
                }
            }
            else
            {
                ViewBag.Error = "Cтарые пароли не совпадают";
                return View(obj);
            }         
        }
    }
}
