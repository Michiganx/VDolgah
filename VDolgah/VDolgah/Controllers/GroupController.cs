using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VDolgah.Controllers
{
    public class GroupController : Controller
    {

        public ActionResult Index()
        {
            return View();
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
            DbEntities.Instance.groups.Add(gr);

            DbEntities.Instance.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
