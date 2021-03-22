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
    public class TransportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Transports
        public ActionResult Index()
        {
            var transports = db.Transports.Include(t => t.District).Include(t => t.Province).Include(t=>t.Wards).Include(t => t.Transporter).Include(t => t.TransportType);
            return View(transports.ToList());// get các col
        }

        // GET: Admin/Transports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transport transport = db.Transports.Find(id);
            if (transport == null)
            {
                return HttpNotFound();
            }
            return View(transport);
        }

        // GET: Admin/Transports/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name");
            ViewBag.Provinceid = new SelectList(db.Provinces, "Provinceid", "Name");//Fix moi them vao
            ViewBag.WardId = new SelectList(db.Wards, "WardId", "Name");
            ViewBag.TransporterId = new SelectList(db.Transporters, "Id", "Name");
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/Transports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TransporterId,TransportTypeId,DistrictId,ProvinceId,WardId,Cost,TransportTime,Note")] Transport transport)
        {
            if (ModelState.IsValid)
            {
                db.Transports.Add(transport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", transport.DistrictId);
            ViewBag.Provinceid = new SelectList(db.Provinces, "Provinceid", "Name", transport.ProvinceId);//Fix moi them vao
            ViewBag.TransporterId = new SelectList(db.Transporters, "Id", "Name", transport.TransporterId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Name", transport.TransportTypeId);
            return View(transport);
        }

        // GET: Admin/Transports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transport transport = db.Transports.Find(id);
            if (transport == null)
            {
                return HttpNotFound();
            }
            ViewBag.valueDistrictId = transport.DistrictId;
            FieldTransport.valueWardId = transport.WardId;
            ViewBag.Provinceid = new SelectList(db.Provinces, "Provinceid", "Name", transport.ProvinceId);//Fix moi them vao
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", transport.DistrictId);
            ViewBag.WardId = new SelectList(db.Wards, "WardId", "Name", transport.WardId);
            ViewBag.TransporterId = new SelectList(db.Transporters, "Id", "Name", transport.TransporterId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Name", transport.TransportTypeId);
          
            return View(transport);
        }

        // POST: Admin/Transports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TransporterId,TransportTypeId,DistrictId,Provinceid,WardId,Cost,TransportTime,Note")] Transport transport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", transport.DistrictId);
            ViewBag.Provinceid = new SelectList(db.Provinces, "Provinceid", "Name", transport.ProvinceId);//Fix moi them vao
            ViewBag.TransporterId = new SelectList(db.Transporters, "Id", "Name", transport.TransporterId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Name", transport.TransportTypeId);
            
            return View(transport);
        }

        // GET: Admin/Transports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transport transport = db.Transports.Find(id);
            if (transport == null)
            {
                return HttpNotFound();
            }
            return View(transport);
        }

        // POST: Admin/Transports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transport transport = db.Transports.Find(id);
            db.Transports.Remove(transport);
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
