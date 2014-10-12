using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
      
        [JsonObject]
        class result
        {
            [JsonObject]
            public class id_
            {
                [JsonProperty("id")]
                public int id = 0;
            }

            [JsonProperty("array")]
            public id_[] array = null;
        }

        [HttpPost]
        public ActionResult AddDebt(string summ,  int group_id, string count, string comment)
        {           
            var t = JsonConvert.DeserializeObject<result>(count);
            var div = t.array.Count();
            var Sum = Convert.ToDecimal(summ.Replace('.',','));
            Decimal debt = Sum / div;           
            foreach (var id in t.array)
            {
                var user = db.users.Where((x) => x.id == id.id).First();
                VDolgah.debt first = null;
                if ((first = user.debts1.Where((x) => x.column == (Session["user"] as VDolgah.user).id).FirstOrDefault()) != null)
                {
                    first.value += debt;
                    addLog(debt, group_id, comment, user.id);
                }
                else if ((first = user.debts.Where((x) => x.row == (Session["user"] as VDolgah.user).id).FirstOrDefault()) != null)
                {
                    first.value -= debt;
                    if (first.value == 0)
                        db.debts.Remove(first);
                    if (first.value < 0)
                    {
                        debt tmp = new VDolgah.debt();
                        tmp.row = first.column;
                        tmp.column = first.row;
                        tmp.value = - first.value;
                        db.debts.Add(tmp);
                        addLog(debt, group_id, comment, tmp.row);
                        db.debts.Remove(first);
                    }
                    else
                        addLog(debt, group_id, comment, user.id);
                }
                else if (id.id != (Session["user"] as VDolgah.user).id)
                {
                    var d = new debt();
                    d.row = id.id;
                    d.column = (Session["user"] as VDolgah.user).id;
                    d.value = debt;
                    db.debts.Add(d);
                    addLog(debt, group_id, comment, user.id);
                }                       
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { group_id = group_id });
        }

        private void addLog(decimal debt, int group_id, string comment, int user )
        {
            debt_log log = new debt_log();
            log.time = DateTime.Now;
            log.user = Session["user"] as user;
            log.value = debt;
            log.groups_idgroups = group_id;
            log.comment = comment;
            log.debtor = user;
            db.debt_log.Add(log); 
        }

        public ActionResult Minimize(int group_id)
        {
            VDolgah.Models.Minimizer min = new Minimizer();
            min.MinGroup(group_id);
            return RedirectToAction("Index", new { group_id = group_id });
        }

        public ActionResult ChangeDebt(decimal change_value, int row, int column, int group_id)
        {
            var debt = db.debts.Where((x) => x.row == row && x.column == column).FirstOrDefault();
            if (debt != null)
                if (debt.value - change_value <= 0)
                    db.debts.Remove(debt);
                else
                    debt.value -= change_value;
            addLog(-change_value, group_id, "Вернул долг", row);
            db.SaveChanges();
            return RedirectToAction("Index", new { group_id = group_id });
        }
    }
}
