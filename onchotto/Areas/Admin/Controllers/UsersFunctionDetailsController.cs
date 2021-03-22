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
    public class UsersFunctionDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/UsersFunctionDetails
        public ActionResult Index()
        {
            IEnumerable<UsersFunctionDetail> UserList = (from func in db.UsersFunctionDetail
                                                             from users in db.Users
                                                             from menuadmin in db.Function
                                                             where func.UsersId == users.Id && func.FunctionId == menuadmin.FunctionId.ToString()
                                                             select new
                                                             {
                                                                 Id = func.Id,
                                                                 UsersId = users.FullName,
                                                                 FunctionId = menuadmin.TitleName
                                                             }).AsEnumerable()
                             .Select(x => new UsersFunctionDetail
                             {
                                 Id = x.Id,
                                 UsersId = x.UsersId,
                                 FunctionId = x.FunctionId
                             });
            return View (UserList);
        }

        // GET: Admin/UsersFunctionDetails/Details/5
        public ActionResult Details(int Id)
        {
            if (Id == 0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersFunctionDetail usersFunctionDetail = db.UsersFunctionDetail.Find(Id);
            if (usersFunctionDetail == null)
            {
                return HttpNotFound();
            }
            return View(usersFunctionDetail);
        }

        // GET: Admin/UsersFunctionDetails/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.FunctionId = new SelectList(db.Function, "FunctionId", "TitleName");
            return View();
        }

        // POST: Admin/UsersFunctionDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsersId,FunctionId")] UsersFunctionDetail usersFunctionDetail)
        {
            if (ModelState.IsValid)
            {
                List<UsersFunctionDetail> LstUser = db.UsersFunctionDetail.Where(r => r.UsersId == usersFunctionDetail.UsersId && r.FunctionId == usersFunctionDetail.FunctionId).ToList();

                if (LstUser.Count == 0)
                {
                    db.UsersFunctionDetail.Add(usersFunctionDetail);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(usersFunctionDetail);
        }

        // GET: Admin/UsersFunctionDetails/Edit/5
        public ActionResult Edit(int Id)
        {
            if (Id == 0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           UsersFunctionDetail usersFunctionDetail = db.UsersFunctionDetail.Find(Id);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "Name", usersFunctionDetail.UsersId);
            ViewBag.FunctionId = new SelectList(db.Function, "FunctionId", "TitleName", usersFunctionDetail.FunctionId);
            if (usersFunctionDetail == null)
            {
                return HttpNotFound();
            }
            return View(usersFunctionDetail);
        }

        // POST: Admin/UsersFunctionDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsersId,FunctionId")] UsersFunctionDetail usersFunctionDetail)
        {
            if (ModelState.IsValid)
            {
                UsersFunctionDetail ItemRolesFunctionDetail = db.UsersFunctionDetail.Find(usersFunctionDetail.Id);
                if (ItemRolesFunctionDetail == null)
                {
                    db.Entry(usersFunctionDetail).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();// (usersFunctionDetail);
        }

        // GET: Admin/UsersFunctionDetails/Delete/5
        public ActionResult Delete(int Id)
        {
            if (Id == 0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersFunctionDetail usersFunctionDetail = db.UsersFunctionDetail.Find(Id);        
            if (usersFunctionDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.UsersFunctionDetail.Remove(usersFunctionDetail);
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
