using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Dao
{
    public static class UserDao
    {
        public static ApplicationDbContext db = new ApplicationDbContext();


        public static IdentityRole Role(this IdentityUserRole userRole)
        {
            return db.Roles.Find(userRole.RoleId);
        }

        public static string RoleName(this IdentityUserRole userRole)
        {
            var role = db.Roles.Find(userRole.RoleId);
            if (role != null)
            {
                return role.Name;
            }
            return "";
        }

        public static string PrintRoles(this ApplicationUser user)
        {
            if (user.Roles.Count > 0)
            {
                string roles = "";
                foreach (var urole in user.Roles)
                {
                    roles += urole.Role().Name + ",";
                }
                return roles;
            }
            return "";
        }

    }
}