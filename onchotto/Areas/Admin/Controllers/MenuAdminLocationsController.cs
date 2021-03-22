using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnChotto.Models;
using OnChotto.Models.Entities;

namespace OnChotto.Areas.Admin.Controllers
{
    public class MenuAdminLocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/MenuAdminLocations
        public ActionResult Index()
        {
            return View(db.MenuAdminLocations.ToList());
        }

        // GET: Admin/MenuAdminLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdminLocation menuAdminLocation = db.MenuAdminLocations.Find(id);
            if (menuAdminLocation == null)
            {
                return HttpNotFound();
            }
            return View(menuAdminLocation);
        }

        // GET: Admin/MenuAdminLocations/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Admin/MenuAdminLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuParentId,Name,STT,TitleIcon,PageLayout,TableName")] MenuAdminLocation menuAdminLocation)
        {
            if (ModelState.IsValid)
            {
                db.MenuAdminLocations.Add(menuAdminLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuAdminLocation);
        }

        // GET: Admin/MenuAdminLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdminLocation menuAdminLocation = db.MenuAdminLocations.Find(id);
            if (menuAdminLocation == null)
            {
                return HttpNotFound();
            }
            return View(menuAdminLocation);
        }

        // POST: Admin/MenuAdminLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuParentId,Name,STT,TitleIcon,PageLayout,TableName")] MenuAdminLocation menuAdminLocation)
        {
          
            if (ModelState.IsValid)
            {
                db.Entry(menuAdminLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuAdminLocation);
        }

        // GET: Admin/MenuAdminLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdminLocation menuAdminLocation = db.MenuAdminLocations.Find(id);
            if (menuAdminLocation == null)
            {
                return HttpNotFound();
            }
            else
            {
                var Lstmenuadmin = from lst in db.MenuAdmins
                                   where lst.LocationId == id
                                   select lst;
                db.MenuAdmins.RemoveRange(Lstmenuadmin.ToList());
                db.MenuAdminLocations.Remove(menuAdminLocation);
                db.SaveChanges();
            }          
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
