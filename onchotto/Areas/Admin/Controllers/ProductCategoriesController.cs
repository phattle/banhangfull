using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;
using OnChotto.Models.Dao;
using OnChotto.Models.Entities;

namespace OnChotto.Areas.Admin.Controllers
{
    public class ProductCategoriesController : AdminController
    {
        ProductCategoryDao dao = new ProductCategoryDao();
        // GET: Admin/ProductCategories
        public ActionResult Index()
        {
            var productCategories = db.ProductCategories.OrderBy(p=>p.DisplayOrder).Include(p => p.ParentCategory);
            return View(productCategories.ToList());
        }

        // GET: Admin/ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = dao.listCategory();
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.Slug = productCategory.Name.ToAscii();
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = ViewBag.ParentId = dao.listCategory(productCategory.ParentId, productCategory.CatId); 
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = dao.listCategory(productCategory.ParentId, productCategory.CatId);
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = dao.listCategory(productCategory.ParentId, productCategory.CatId);
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            List<Product> pro = db.Products.Where(p => p.CatId == id).ToList();
            foreach (Product item in pro)
            {
                List<ProductLinks> lik = db.ProductLinks.Where(p => p.ProductID == item.Id).ToList();
                foreach (ProductLinks ilik in lik)
                {
                    db.ProductLinks.Remove(ilik);
                }

                List<OrderDetail> order = db.OrderDetails.Where(p => p.ProductId == item.Id).ToList();
                foreach (OrderDetail o in order)
                {
                    db.OrderDetails.Remove(o);
                }

                db.Products.Remove(item);               
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
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
