using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDolgah.Models;

namespace VDolgah.Controllers
{
    public class GroupController : Controller
    {
        DbEntities db = DbEntities.Instance;

        public ActionResult Index(int? group_id)
        {
            group group = db.groups.Where((x) => x.idgroups == group_id).First();
            return View(group);
        }

        public ActionResult Create()
        {
            group gr = new group();
            return View(gr);
        }

        [HttpPost]
        public ActionResult Create(group gr)
        {
            gr.creator = (Session["user"] as user).id;
            gr.users.Add(Session["user"] as user);
            DbEntities.Instance.groups.Add(gr);

            DbEntities.Instance.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeName(string newName, int groupId)
        {
            var group = db.groups.Where((x) => x.idgroups == groupId).First();
            group.name = newName;
            db.SaveChanges();
            return RedirectToAction("Index", "Group", new { group_id = groupId });
        }

        public ActionResult AddUser(int group_id, int user_id)
        {
            var group = db.groups.Where((x) => x.idgroups == group_id).First();
            var user = db.users.Where((x) => x.id == user_id).First();
            group.users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index", new { group_id = group_id });
        }
    }
}
