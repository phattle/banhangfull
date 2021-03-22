using OnChotto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnChotto.Areas.Admin.Models.Dao
{
    public class FunctionList
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        public static List<OnChotto.Models.Entities.Function> GetLstFunction(string strUserName)
        {
            List<OnChotto.Models.Entities.Function> lstItem = new List<OnChotto.Models.Entities.Function>();
            string strUserId = GetUserId(strUserName);
            string strRolesId = GetRolesName(strUserId);
            if (strRolesId.ToUpper().ToString() == "Administrator".ToUpper()|| strRolesId.ToUpper().ToString() != "Administrator".ToUpper())
            {
                lstItem = db.Function.Where(f=>f.Status==true).ToList();
            }
            else
            {
                List<string> Lstfunctionmenu = LstFunctions(strUserId);
                foreach (var item in Lstfunctionmenu)
                {
                    foreach (var itemFunction in GetLstFunctionByID(item.ToString()))
                    {
                        OnChotto.Models.Entities.Function items = new OnChotto.Models.Entities.Function();
                        items.FunctionId = itemFunction.FunctionId;
                        items.TitleName = itemFunction.TitleName;
                        items.ViewName = itemFunction.ViewName;
                        items.ModelId = itemFunction.ModelId;
                        items.ClassId = itemFunction.ClassId;
                        items.OnclickId = itemFunction.OnclickId;
                        items.Description = itemFunction.Description;
                        items.Status = itemFunction.Status;
                        lstItem.Add(items);
                    }
                }

            }
            return lstItem;
        }
        public static string GetUserId(string strUserName)
        {
            var db = new ApplicationDbContext();
            return db.Users.Where(u => u.Email == strUserName).Select(u => u.Id).FirstOrDefault().ToString();
        }

        public static string GetRolesName(string strUserId)
        {
            var db = new ApplicationDbContext();
            ApplicationUser model = db.Users.Find(strUserId);
            return model.FullName.ToString();
        }

        public static List<string> LstFunctions(string UserId)
        {

            var items = (from fun in db.UsersFunctionDetail
                         where fun.UsersId == UserId
                         select new
                         {
                             FunctionId = fun.FunctionId,
                         }).Distinct();
            List<string> LstParentID = new List<string>();
            foreach (var item in items)
            {
                LstParentID.Add(item.FunctionId.ToString());
            }

            return LstParentID;
        }

        public static List<OnChotto.Models.Entities.Function> GetLstFunctionByID(string strFunctionId)
        {
            List<OnChotto.Models.Entities.Function> LstItems = new List<OnChotto.Models.Entities.Function>();
            LstItems = db.Function.Where(f => f.FunctionId == strFunctionId).ToList();
            return LstItems;
        }
    }
}