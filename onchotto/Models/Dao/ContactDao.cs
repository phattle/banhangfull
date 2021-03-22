using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnChotto.Models.Dao;
using OnChotto.Models.Entities;

namespace OnChotto.Models.Dao
{
    public static class ContactDao
    {
        public static ApplicationDbContext db = ApplicationDbContext.Create();
        public static int count()
        {
            return db.Contacts.Count(x => x.Status == "New");
        }

        //public static string activeClass(string step, Contact contact)
        //{
        //    if (contact != null && step == "step2")
        //        return "active";

        //    if (contact == null && step == "step1")
        //        return "active";

        //    return "disabled";
        //}
    }
}