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
    public class ExchangeRatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ExchangeRates
        public ActionResult Index()
        {
            return View(db.ExchangeRates.ToList());
        }

        // GET: Admin/ExchangeRates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExchangeRates exchangeRates = db.ExchangeRates.Find(id);
            if (exchangeRates == null)
            {
                return HttpNotFound();
            }
            return View(exchangeRates);
        }

        // GET: Admin/ExchangeRates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExchangeRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,ExchangeRate,DateTime,FiscalMonth,FiscalYear")] ExchangeRates exchangeRates)
        {
            if (ModelState.IsValid)
            {
                db.ExchangeRates.Add(exchangeRates);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exchangeRates);
        }

        // GET: Admin/ExchangeRates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExchangeRates exchangeRates = db.ExchangeRates.Find(id);
            if (exchangeRates == null)
            {
                return HttpNotFound();
            }
            return View(exchangeRates);
        }

        // POST: Admin/ExchangeRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,ExchangeRate,DateTime,FiscalMonth,FiscalYear")] ExchangeRates exchangeRates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exchangeRates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exchangeRates);
        }

        // GET: Admin/ExchangeRates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExchangeRates exchangeRates = db.ExchangeRates.Find(id);
            if (exchangeRates == null)
            {
                return HttpNotFound();
            }
            return View(exchangeRates);
        }

        // POST: Admin/ExchangeRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExchangeRates exchangeRates = db.ExchangeRates.Find(id);
            db.ExchangeRates.Remove(exchangeRates);
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
