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
    public class QuotationsController : AdminController
    {
        // GET: Admin/Quotations
        public ActionResult Index()
        {
            return View(db.Quotations.ToList());
        }

        // POST: Admin/Quotations/update
        [HttpPost]
        public JsonResult Update(int id, string newStatus)
        {
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return Json(new { status = 0, msg = "Không tìm thấy báo giá." });
            }

            quotation.Status = newStatus;
            db.Entry(quotation).State = EntityState.Modified;

            if (db.SaveChanges() == 0)
            {
                return Json(new { status = 0, msg = "lỗi update" });
            }

            return Json(new { status = 1, msg = "Update thành công" });
        }

        // GET: Admin/Quotations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }

            db.Quotations.Remove(quotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
