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
    public class ProductTaxHscodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductTaxHscodes
        public ActionResult Index()
        {
            return View(db.ProductTaxHscodes.ToList());
        }

        // GET: Admin/ProductTaxHscodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTaxHscode productTaxHscode = db.ProductTaxHscodes.Find(id);
            if (productTaxHscode == null)
            {
                return HttpNotFound();
            }
            return View(productTaxHscode);
        }

        // GET: Admin/ProductTaxHscodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductTaxHscodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HsCodeId,HsCode,PoductName,TaxPercentage,VATPercentage,FederalTaxPercentage,Pricehandling,ShippinglandPercentage,PriceClearanceFee,PriceAF,PriceTECSServicesFee,PriceCustomFee,TransactionPercentage,VATFeeTransaction,Note,CreateDate")] ProductTaxHscode productTaxHscode)
        {
            if (ModelState.IsValid)
            {
                db.ProductTaxHscodes.Add(productTaxHscode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productTaxHscode);
        }

        // GET: Admin/ProductTaxHscodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTaxHscode productTaxHscode = db.ProductTaxHscodes.Find(id);
            if (productTaxHscode == null)
            {
                return HttpNotFound();
            }
            return View(productTaxHscode);
        }

        // POST: Admin/ProductTaxHscodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HsCodeId,HsCode,PoductName,TaxPercentage,VATPercentage,FederalTaxPercentage,Pricehandling,ShippinglandPercentage,PriceClearanceFee,PriceAF,PriceTECSServicesFee,PriceCustomFee,TransactionPercentage,VATFeeTransaction,Note,CreateDate")] ProductTaxHscode productTaxHscode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productTaxHscode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productTaxHscode);
        }

        // GET: Admin/ProductTaxHscodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductTaxHscode productTaxHscode = db.ProductTaxHscodes.Find(id);
            if (productTaxHscode == null)
            {
                return HttpNotFound();
            }
            return View(productTaxHscode);
        }

        // POST: Admin/ProductTaxHscodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductTaxHscode productTaxHscode = db.ProductTaxHscodes.Find(id);
            db.ProductTaxHscodes.Remove(productTaxHscode);
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
