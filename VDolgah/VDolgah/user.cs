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
    
    public partial class user
    {
        public user()
        {
            this.debts = new HashSet<debt>();
            this.debts1 = new HashSet<debt>();
            this.debt_log = new HashSet<debt_log>();
            this.groups = new HashSet<group>();
            this.groups1 = new HashSet<group>();
            this.debt_log1 = new HashSet<debt_log>();
        }
    
        public int id { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string password_hash { get; set; }
        public string confirm_password { get; set; }
        public string salt { get; set; }
        public byte[] avatar { get; set; }
    
        public virtual ICollection<debt> debts { get; set; }
        public virtual ICollection<debt> debts1 { get; set; }
        public virtual ICollection<debt_log> debt_log { get; set; }
        public virtual ICollection<group> groups { get; set; }
        public virtual ICollection<group> groups1 { get; set; }
        public virtual ICollection<debt_log> debt_log1 { get; set; }
    }
}
