using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDolgah.Models
{
    public class Minimizer
    {
        DbEntities db = DbEntities.Instance;

        public void Res(decimal[,] arr)
        {
            int n = arr.GetLength(0), m = arr.GetLength(1);
            if (m != n)
            {
                throw new ArgumentException("Not sqr");
            }
            for (int i = 0; i < n; i++)
            {
                if (arr[i, i] != 0)
                {
                    throw new ArgumentException();
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[i, j] != 0 && arr[j, i] != 0)
                    {
                        if (arr[i, j] - arr[j, i] > 0)
                        {
                            arr[i, j] -= arr[j, i];
                            arr[j, i] = 0;
                        }
                        else
                        {
                            arr[j, i] -= arr[i, j];
                            arr[i, j] = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i == j) || (arr[i, j] == 0))
                    {
                        continue;
                    }
                    else
                    {
                        {

                            for (int y = 0; y < n; y++)
                            {
                                if (arr[y, i] != 0 && arr[y, j] != 0)
                                {
                                    if (arr[i, j] > arr[y, i])
                                    {
                                        arr[i, j] -= arr[y, i];
                                        arr[y, j] += arr[y, i];
                                        arr[y, i] = 0;
                                    }
                                    else
                                    {
                                        arr[y, i] -= arr[i, j];
                                        arr[y, j] += arr[i, j];
                                        arr[i, j] = 0;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void MinGroup(int group_id)
        {
            try
            {
                group group = db.groups.Where((x) => x.idgroups == group_id).First();
                decimal[,] matrix = new decimal[group.users.Count, group.users.Count];
                var list = group.users.ToList();
                for (var i = 0; i < list.Count; i++)
                {
                    var debt_list = list[i].debts.Where((x) => x.column == list[i].id).ToList();
                    foreach (var debt in debt_list)
                    {
                        int user_id = list.IndexOf(db.users.Where((x) => x.id == debt.row).First());
                        if (user_id != -1)
                        {
                            matrix[user_id, i] = debt.value.Value;
                            db.debts.Remove(debt);
                        }
                    }
                }
                //db.SaveChanges();
                Res(matrix);
                for(int i = 0; i < list.Count; i++)
                    for (int j = 0; j < list.Count; j++)
                        if(matrix[i,j] != 0)
                        {
                            debt d = new debt();
                            d.row = list[i].id;
                            d.column = list[j].id;
                            d.value = matrix[i,j];
                            list[i].debts1.Add(d);
                        }
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}