//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VDolgah
{
    using System;
    using System.Collections.Generic;
    
    public partial class debt_log
    {
        public debt_log()
        {
            this.users = new HashSet<user>();
        }
    
        public int iddebt_log { get; set; }
        public System.DateTime time { get; set; }
        public int groups_idgroups { get; set; }
        public int payer { get; set; }
        public decimal value { get; set; }
        public string comment { get; set; }
        public Nullable<int> debtor { get; set; }
    
        public virtual group group { get; set; }
        public virtual user user { get; set; }
        public virtual ICollection<user> users { get; set; }
    }
}
