using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Dao
{
    public static class OptionDao
    {
        public static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetOption(string key, string defaultValue = "")
        {
            try
            {
                var option = db.Options.Where(o => o.Name == key).SingleOrDefault();
                return option != null ? option.Value : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static void SetOption(string key, string Value)
        {
            try
            {
                var option = db.Options.Where(o => o.Name == key).SingleOrDefault();
                if (option == null)
                {
                    option = new Entities.Option { Name = key, Value = Value };
                    db.Options.Add(option);
                }
                else
                {
                    option.Value = Value;
                    db.Entry(option).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
            catch { }
             
        }
    }
}