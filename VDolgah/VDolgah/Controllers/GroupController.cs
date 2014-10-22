using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using VDolgah.Models;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace VDolgah.Controllers
{
    public class GroupController : Controller
    {
        DbEntities db = DbEntities.Instance;

        public ActionResult Index(int? group_id)
        {
            group group = db.groups.Where((x) => x.idgroups == group_id).First();
            if(group.users.Contains(Session["user"] as user))
                return View(group);
            else
                return RedirectToAction("Index", "Error", new { error = "У вас нет доступа к этой группе" });       
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
            Decimal Sum = 0;
            try
            {
                Sum = System.Convert.ToDecimal(Eval.JScriptEvaluate(summ, VsaEngine.CreateEngine()));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { error = "Введено неверное выражение" });  
            }
            if (Sum > 0)
            {
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
                            tmp.value = -first.value;
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
            else
            {
                return RedirectToAction("Index", "Error", new { error = "Введен долг, который равен 0 или отрицательный" });  
            }
        }

        private void addLog(decimal debt, int group_id, string comment, int user )
        {
            if (debt != 0)
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

        bool CheckGroupIdentity(List<user> users)
        {
            for (int i = 0; i < users.Count; i++)
                for (int j = 0; j < users.Count; j++)
                    if (users[i].debts.Where((x) => x.row == users[j].id).ToList().Count() > 0 ||
                        users[i].debts1.Where((x) => x.column == users[j].id).ToList().Count() > 0)
                        return false;
            return true;
        }

        public ActionResult Delete(int group_id)
        {
            var group = db.groups.Where((x) => x.idgroups == group_id).First();
            var users = group.users.ToList();
            if (!CheckGroupIdentity(users))
                return RedirectToAction("Index", "Error", new { error = "Вы не можете удалить эту группу так как в ней есть активные долги" });  
            else
            {
                group.users.Clear();
                var log_list  = db.debt_log.Where((x) => x.groups_idgroups == group.idgroups).ToList();
                foreach (var log in log_list)
                    db.debt_log.Remove(log);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
