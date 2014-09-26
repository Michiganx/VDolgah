using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDolgah.Tests.Models
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = VDolgah.DbEntities.Instance;
            Assert.AreEqual(db.users.Where((x) => x.first_name == "Андрей").First().id, 4);
         
        }
    }
}
