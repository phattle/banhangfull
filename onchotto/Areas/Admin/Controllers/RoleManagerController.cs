using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Areas.Admin.Controllers
{
    public class RoleManagerController : AdminController
    {
        // GET: Admin/RoleManager
        public ActionResult Index()
        {
            var model = db.Roles.AsEnumerable();
            return View(model);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(role);
        }



        public ActionResult Delete(string Id)
        {
            var model = db.Roles.Find(Id);
            if (model != null)
            {
                db.Roles.Remove(model);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}