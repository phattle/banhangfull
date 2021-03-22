using OnChotto.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Models.Dao
{
    public static class MenuAdminLocationList
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        public static List<MenuAdminLocation> LstMenuAdminLocation(string strUserName)
        {
                      
            string strUserId = GetUserId(strUserName);
            string strRolesId = GetRolesName(strUserId);
            List<MenuAdminLocation> Lstmenuadminlocation = new List<MenuAdminLocation>() ;
            if (strRolesId.ToUpper().ToString() == "Administrator".ToUpper().ToUpper())
                Lstmenuadminlocation = db.MenuAdminLocations.ToList();
            else
            {
                List<string> Lstfunctionmenu = LstFunctionMenu(strUserId);
                foreach (var item in Lstfunctionmenu)
                {
                    foreach (MenuAdminLocation itemmenu in db.MenuAdminLocations.ToList())
                    {
                        if (itemmenu.MenuParentId.ToString() == item.ToString())
                        {
                            MenuAdminLocation items = new MenuAdminLocation();
                            items.MenuParentId = itemmenu.MenuParentId;
                            items.Name = itemmenu.Name;
                            items.STT = itemmenu.STT;
                            items.TitleIcon = itemmenu.TitleIcon;
                            items.PageLayout = itemmenu.PageLayout;
                            items.TableName = itemmenu.TableName;
                            Lstmenuadminlocation.Add(items);
                            break;
                        }
                    }

                }
            }
            return Lstmenuadminlocation;
        }

        public static string GetUserId(string strUserName)
        {
            
            return db.Users.Where(u => u.Email == strUserName).Select(u => u.Id).FirstOrDefault().ToString();
        }

        public static List<string>LstFunctionMenu(string UserId)
        {
          
            var items = (from fun in db.FunctionMenus                         
                         where fun.UserId == UserId
                         select new
                         {
                             MenuParentId = fun.MenuParentId,
                         }).Distinct();
            List<string> LstParentID = new List<string>();
            foreach (var item in items)
            {
                LstParentID.Add(item.MenuParentId.ToString());
            }

            return LstParentID;
        }

        public static List<FunctionMenu> LstFunctionMenuChild(string UserId)
        {           

            return db.FunctionMenus.Where(f => f.UserId == UserId).ToList();
        }

        public static string GetRolesName(string strUserId)
        {
          
            ApplicationUser model = db.Users.Find(strUserId);
            return model.FullName.ToString();
        }
    }
}