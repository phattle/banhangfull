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
    public class RolesFunctionDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/RolesFunctionDetails
        public ActionResult Index()
        {

            IEnumerable<RolesFunctionDetail> FunctionList = (from func in db.RolesFunctionDetail
                                                             from users in db.Roles
                                                             from menuadmin in db.Function
                                                             where func.RolesId == users.Id && func.FunctionId == menuadmin.FunctionId.ToString()
                                                             select new
                                                             {
                                                                 Id = func.Id,
                                                                 RolesId = users.Name,
                                                                 FunctionId = menuadmin.TitleName
                                                             }).AsEnumerable()
                             .Select(x => new RolesFunctionDetail
                             {
                                 Id = x.Id,
                                 RolesId = x.RolesId,
                                 FunctionId = x.FunctionId
                             });


            return View(FunctionList);
        }

        // GET: Admin/RolesFunctionDetails/Details/5
        public ActionResult Details(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolesFunctionDetail rolesFunctionDetail = db.RolesFunctionDetail.Find(Id);
            if (rolesFunctionDetail == null)
            {
                return HttpNotFound();
            }
            return View(rolesFunctionDetail);
        }

        // GET: Admin/RolesFunctionDetails/Create
        public ActionResult Create()
        {
            ViewBag.RolesId = new SelectList(db.Roles, "Id", "Name");
            ViewBag.FunctionId = new SelectList(db.Function, "FunctionId", "TitleName");
            return View();
        }

        // POST: Admin/RolesFunctionDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RolesId,FunctionId")] RolesFunctionDetail rolesFunctionDetail)
        {
            if (ModelState.IsValid)
            {
                List<RolesFunctionDetail> LstRoles = db.RolesFunctionDetail.Where(r => r.RolesId == rolesFunctionDetail.RolesId && r.FunctionId == rolesFunctionDetail.FunctionId).ToList();
                if (LstRoles.Count == 0)
                {
                    db.RolesFunctionDetail.Add(rolesFunctionDetail);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(rolesFunctionDetail);
        }

        // GET: Admin/RolesFunctionDetails/Edit/5
        public ActionResult Edit(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolesFunctionDetail rolesFunctionDetail = db.RolesFunctionDetail.Find(Id);
            ViewBag.RolesId = new SelectList(db.Roles, "Id", "Name", rolesFunctionDetail.RolesId);
            ViewBag.FunctionId = new SelectList(db.Function, "FunctionId", "TitleName", rolesFunctionDetail.FunctionId);
            if (rolesFunctionDetail == null)
            {
                return HttpNotFound();
            }
            return View(rolesFunctionDetail);
        }

        // POST: Admin/RolesFunctionDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RolesId,FunctionId")] RolesFunctionDetail rolesFunctionDetail)
        {
            if (ModelState.IsValid)
            {
                RolesFunctionDetail ItemRolesFunctionDetail = db.RolesFunctionDetail.Find(rolesFunctionDetail.Id);
                if (ItemRolesFunctionDetail == null)
                {
                    db.Entry(rolesFunctionDetail).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();// (rolesFunctionDetail);
        }

        // GET: Admin/RolesFunctionDetails/Delete/5
        public ActionResult Delete(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolesFunctionDetail rolesFunctionDetail = db.RolesFunctionDetail.Find(Id);
            string strRoleId = db.RolesFunctionDetail.Where(p => p.Id == Id).Select(p => p.RolesId).FirstOrDefault().ToString();
            string strFunctionId = db.RolesFunctionDetail.Where(p => p.Id == Id).Select(p => p.FunctionId).FirstOrDefault().ToString();
            ViewBag.RoleName = db.Roles.Where(p => p.Id == strRoleId).Select(p => p.Name).FirstOrDefault().ToString();
            ViewBag.FunctionName = db.Function.Where(p => p.FunctionId == strFunctionId).Select(p => p.TitleName).FirstOrDefault().ToString();
            if (rolesFunctionDetail == null)
            {
                return HttpNotFound();
            }
            return View(rolesFunctionDetail);
        }

        // POST: Admin/RolesFunctionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Id)
        {
            RolesFunctionDetail rolesFunctionDetail = db.RolesFunctionDetail.Find(Id);
            if (rolesFunctionDetail != null)
            {
               
                db.RolesFunctionDetail.Remove(rolesFunctionDetail);
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
