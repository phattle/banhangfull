using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnChotto.Models.Dao;
using OnChotto.Models.Entities;

namespace OnChotto.Models.Dao
{
    public static class QuotationDao
    {
        public static ApplicationDbContext db = ApplicationDbContext.Create();
        public static int count()
        {
            return db.Quotations.Count(x => x.Status == "New");
        }

        public static string activeClass(string step, Quotation quota)
        {
            if (quota != null && step == "step2")
                return "active";

            if (quota == null && step == "step1")
                return "active";

            return "disabled";
        }
    }
}