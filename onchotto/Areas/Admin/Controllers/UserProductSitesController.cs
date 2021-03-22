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
using OnChotto.Commons;

namespace OnChotto.Areas.Admin.Controllers
{
    public class UserProductSitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/UserProductSites
        public ActionResult Index()
        {
            IEnumerable<UserProductSite> List = (from func in db.UserProductSites
                                                 from users in db.Users
                                                 from pro in db.ProductCategories
                                                 where func.UsersId == users.Id && func.ProductSiteId == pro.CatId.ToString()
                                                 select new
                                                 {
                                                     Id = func.Id,
                                                     UsersId = users.FullName,
                                                     ProductSiteId = pro.Name
                                                 }).AsEnumerable()
                              .Select(x => new UserProductSite
                              {
                                  Id = x.Id,
                                  UsersId = x.UsersId,
                                  ProductSiteId = x.ProductSiteId
                              });
            return View(List);
        }

        // GET: Admin/UserProductSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProductSite userProductSite = db.UserProductSites.Find(id);
            if (userProductSite == null)
            {
                return HttpNotFound();
            }
            return View(userProductSite);
        }

        // GET: Admin/UserProductSites/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ProductSiteId = new SelectList(GetLst(), "CatId", "Name");
            return View();
        }

        public IEnumerable<ProductCategory> GetLst()
        {
            IEnumerable<ProductCategory> ListProductCategory = (from func in db.ProductSites
                                                                from pro in db.ProductCategories
                                                                where func.ProductSiteName.ToUpper() == pro.Name.ToUpper()
                                                                select new
                                                                {
                                                                    catId = pro.CatId,
                                                                    Name = pro.Name
                                                                }).AsEnumerable()
                              .Select(x => new ProductCategory
                              {
                                  CatId = x.catId,
                                  Name = x.Name,
                              });
            return ListProductCategory;
        }

        // POST: Admin/UserProductSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UsersId,ProductSiteId")] UserProductSite userProductSite)
        {
            if (ModelState.IsValid)
            {
                List<UserProductSite> LstUserProductSite = db.UserProductSites.Where(u => u.UsersId == userProductSite.UsersId && u.ProductSiteId == userProductSite.ProductSiteId).ToList();
                if (LstUserProductSite.Count == 0)
                {
                    db.UserProductSites.Add(userProductSite);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(userProductSite);
        }

        // GET: Admin/UserProductSites/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProductSite userProductSite = db.UserProductSites.Find(id);
            if (userProductSite == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FullName", userProductSite.UsersId);
            ViewBag.ProductSiteId = new SelectList(GetLst(), "CatId", "Name", userProductSite.ProductSiteId);
            return View(userProductSite);
        }

        // POST: Admin/UserProductSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UsersId,ProductSiteId")] UserProductSite userProductSite)
        {
            if (ModelState.IsValid)
            {               
                db.Entry(userProductSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userProductSite);
        }

        

        // GET: Admin/UserProductSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProductSite userProductSite = db.UserProductSites.Find(id);          
            if (userProductSite == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.UserProductSites.Remove(userProductSite);
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
