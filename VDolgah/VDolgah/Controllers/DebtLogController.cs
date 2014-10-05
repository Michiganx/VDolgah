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
            var res = db.debt_log.Where((x) => x.groups_idgroups == group_id && x.debtor == user_id).ToList();
            return View(res);
        }

    }
}
