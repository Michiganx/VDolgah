using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDolgah.Models
{
    public class GroupWrapper
    {
        public group Group { get; set; }
        public bool creator = false; //current user - creator
    }
}