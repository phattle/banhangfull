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
    public class FunctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Functions
        public ActionResult Index()
        {
            return View(db.Function.ToList());
        }

        // GET: Admin/Functions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Function.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function);
        }

        // GET: Admin/Functions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Functions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FunctionId,TitleName,ViewName,ModelId,ClassId,OnclickId,Description,Status")] Function function)
        {
            if (ModelState.IsValid)
            {
                Function ItemFunction = db.Function.Where(p => p.FunctionId == function.FunctionId).FirstOrDefault();
               
                if (ItemFunction == null)
                {
                    db.Function.Add(function);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(function);
        }

        // GET: Admin/Functions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Function.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function);
        }

        // POST: Admin/Functions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FunctionId,TitleName,ViewName,ModelId,ClassId,OnclickId,Description,Status")] Function function)
        {
            if (ModelState.IsValid)
            {
                db.Entry(function).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(function);
        }

        // GET: Admin/Functions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Function.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            UsersFunctionDetail ItemUserFunctionDetail = db.UsersFunctionDetail.Where(p => p.FunctionId == id).FirstOrDefault();
            if(ItemUserFunctionDetail==null)
            {                
                db.Function.Remove(function);
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
