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
    public class ProductSitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductSites
        public ActionResult Index()
        {
            return View(db.ProductSites.ToList());
        }

        // GET: Admin/ProductSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSite productSite = db.ProductSites.Find(id);
            if (productSite == null)
            {
                return HttpNotFound();
            }
            return View(productSite);
        }

        // GET: Admin/ProductSites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductSiteId,ProductSiteName,IdCode")] ProductSite productSite)
        {
            if (ModelState.IsValid)
            {
                db.ProductSites.Add(productSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productSite);
        }

        // GET: Admin/ProductSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSite productSite = db.ProductSites.Find(id);
            if (productSite == null)
            {
                return HttpNotFound();
            }
            return View(productSite);
        }

        // POST: Admin/ProductSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductSiteId,ProductSiteName,IdCode")] ProductSite productSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productSite);
        }

        // GET: Admin/ProductSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSite productSite = db.ProductSites.Find(id);
            if (productSite == null)
            {
                return HttpNotFound();
            }
            return View(productSite);
        }

        // POST: Admin/ProductSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSite productSite = db.ProductSites.Find(id);
            db.ProductSites.Remove(productSite);
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
