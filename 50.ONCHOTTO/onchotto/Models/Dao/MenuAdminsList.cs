using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnChotto.Models.Entities;

namespace OnChotto.Models.Dao
{
    public static class MenuAdminsList
    {
        public static List<MenuAdmin> LstMenuAdmins(string strUserName)
        {
            var db = new ApplicationDbContext();
            string strUserId = MenuAdminLocationList.GetUserId(strUserName);
            string strRolesId = MenuAdminLocationList.GetRolesName(strUserId);
            List<MenuAdmin> LstMenuAdmin = new List<MenuAdmin>();
            if (strRolesId.ToUpper().ToString() == "Administrator".ToUpper().ToUpper())
                LstMenuAdmin = db.MenuAdmins.ToList();
            else
            {
                List<FunctionMenu> Lstfunctionmenu = MenuAdminLocationList.LstFunctionMenuChild(strUserId);
                foreach (FunctionMenu item in Lstfunctionmenu)
                {
                    foreach (MenuAdmin itemmenu in db.MenuAdmins.ToList())
                    {
                        if (itemmenu.MenuChildId == int.Parse(item.MenuChildId))
                        {
                            MenuAdmin items = new MenuAdmin();
                            items.MenuChildId = itemmenu.MenuChildId;
                            items.LocationId = itemmenu.LocationId;
                            items.Text = itemmenu.Text;
                            items.Description = itemmenu.Description;
                            items.Url = itemmenu.Url;
                            items.STT = itemmenu.STT;
                            items.TitleIcon = itemmenu.TitleIcon;
                            items.PageLayout = itemmenu.PageLayout;
                            items.TableName = itemmenu.TableName;
                            LstMenuAdmin.Add(items);
                            break;
                        }
                    }

                }
            }
            return LstMenuAdmin;
        }
    }
}