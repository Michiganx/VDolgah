using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDolgah.Models
{
    public class PasswordChanger
    {
        public int userId { get; set; }
        public string oldPass { get; set; }
        public string NewPass { get; set; }
        public string confirm { get; set; }
    }
}