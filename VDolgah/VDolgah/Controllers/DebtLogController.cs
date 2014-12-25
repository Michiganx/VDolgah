using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VDolgah.Controllers
{
    public class DebtLogController : Controller
    {
        DbEntities db = DbEntities.Instance;

        public ActionResult Index(int group_id, int user_id)
        {
            var User = Session["user"] as user;
            if (user_id != User.id)
            {
                var res = db.debt_log.Where((x) => x.groups_idgroups == group_id && 
                        ((x.debtor == user_id && x.payer == User.id) || (x.debtor == User.id && x.payer == user_id)))
                        .OrderByDescending((x) => x.time).ToList();
                return View(res);
            }
            else
	        {
                var res = db.debt_log.Where((x) => x.groups_idgroups == group_id && x.debtor == user_id).OrderByDescending((x) => x.time).ToList();
                return View(res);
            }
        }

    }
}
