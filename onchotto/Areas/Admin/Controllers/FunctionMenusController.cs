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
using System.Web.Script.Serialization;

namespace OnChotto.Areas.Admin.Controllers
{
    public class FunctionMenusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/FunctionMenus
        public ActionResult Index()
        {

            IEnumerable<FunctionMenu> staffList = (from func in db.FunctionMenus
                                                   from users in db.Users
                                                   from menuadmin in db.MenuAdminLocations
                                                   from menuadmins in db.MenuAdmins
                                                   where func.UserId == users.Id && func.MenuParentId == menuadmin.MenuParentId.ToString()
                                                   && func.MenuChildId == menuadmins.MenuChildId.ToString()
                                                   select new
                                                   {
                                                       Id = func.Id,
                                                       UserId = users.FullName,
                                                       MenuParentId = menuadmin.Name,
                                                       MenuChildId = menuadmins.Text,
                                                       func.TypeMenu,
                                                       func.STT,
                                                       func.Status
                                                   }).AsEnumerable()
                               .Select(x => new FunctionMenu
                               {
                                   Id = x.Id,
                                   UserId = x.UserId,
                                   MenuParentId = x.MenuParentId,
                                   MenuChildId = x.MenuChildId,
                                   TypeMenu = x.TypeMenu,
                                   STT = x.STT,
                                   Status = x.Status
                               });
            return View(staffList);
        }

        // GET: Admin/FunctionMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionMenu functionMenu = db.FunctionMenus.Find(id);
            if (functionMenu == null)
            {
                return HttpNotFound();
            }
            return View(functionMenu);

        }

        // GET: Admin/FunctionMenus/Create
        public ActionResult Create()
        {
            ViewBag.UserId = db.Users;
            ViewBag.MenuParentId = db.MenuAdminLocations;
            // ViewBag.MenuChildId = new SelectList(db.MenuAdmins.Where(d => d.LocationId.ToString() == "-1").Select(x => new { MenuChildId = x.MenuChildId, Text = x.Text }), "MenuChildId", "Text");

            return View();
        }

        // POST: Admin/FunctionMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,MenuParentId,MenuChildId,TypeMenu,STT,Status")] FunctionMenu functionMenu)
        {
            if (ModelState.IsValid)
            {

                db.FunctionMenus.Add(functionMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MenuParentId = db.MenuAdminLocations;
            return View(functionMenu);
        }

        // GET: Admin/FunctionMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionMenu functionMenu = db.FunctionMenus.Find(id);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", functionMenu.UserId);
            ViewBag.MenuParentId = new SelectList(db.MenuAdminLocations, "MenuParentId", "Name", functionMenu.MenuParentId);
            ViewBag.MenuChildId = new SelectList(db.MenuAdmins, "MenuChildId", "Text", functionMenu.MenuChildId);
            if (functionMenu == null)
            {
                return HttpNotFound();
            }
            return View(functionMenu);
        }

        // POST: Admin/FunctionMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,MenuParentId,MenuChildId,TypeMenu,STT,Status")] FunctionMenu functionMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(functionMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(functionMenu);
        }

        // GET: Admin/FunctionMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionMenu functionMenu = db.FunctionMenus.Find(id);
            if (functionMenu == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.FunctionMenus.Remove(functionMenu);
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

        public ActionResult FillMenuChild(int LocationId)
        {
            var MenuChild = db.MenuAdmins.Where(c => c.LocationId == LocationId);
            return Json(MenuChild, JsonRequestBehavior.AllowGet);
        }
    }
}
