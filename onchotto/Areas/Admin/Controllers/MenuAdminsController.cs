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
    public class MenuAdminsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/MenuAdmins
        public ActionResult Index()
        {

            return View(db.MenuAdmins.ToList());
        }

        // GET: Admin/MenuAdmins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdmin menuAdmin = db.MenuAdmins.Find(id);
            if (menuAdmin == null)
            {
                return HttpNotFound();
            }
            return View(menuAdmin);
        }

        // GET: Admin/MenuAdmins/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.MenuAdminLocations, "MenuParentId", "Name");
            return View();
        }

        // POST: Admin/MenuAdmins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuChildId,LocationId,Text,Description,Url,STT,TitleIcon,PageLayout,TableName")] MenuAdmin menuAdmin)
        {
            if (ModelState.IsValid)
            {
                db.MenuAdmins.Add(menuAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuAdmin);
        }

        // GET: Admin/MenuAdmins/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdmin menuAdmin = db.MenuAdmins.Find(id);
            ViewBag.LocationId = new SelectList(db.MenuAdminLocations, "MenuParentId", "Name",menuAdmin.LocationId);
            if (menuAdmin == null)
            {
                return HttpNotFound();
            }
            return View(menuAdmin);
        }

        // POST: Admin/MenuAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuChildId,LocationId,Text,Description,Url,STT,TitleIcon,PageLayout,TableName")] MenuAdmin menuAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuAdmin);
        }

        // GET: Admin/MenuAdmins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuAdmin menuAdmin = db.MenuAdmins.Find(id);            
            if (menuAdmin == null)
            {
                return HttpNotFound();
            }
            db.MenuAdmins.Remove(menuAdmin);
            db.SaveChanges();
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
