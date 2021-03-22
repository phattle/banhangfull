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
    public class ContactsController : AdminController
    {
        // GET: Admin/Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // POST: Admin/Contact/update
        [HttpPost]
        public JsonResult Update(int id, string newStatus)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return Json(new { status = 0, msg = "Không tìm thấy liên hệ." });
            }

            contact.Status = newStatus;
            db.Entry(contact).State = EntityState.Modified;

            if (db.SaveChanges() == 0)
            {
                return Json(new { status = 0, msg = "lỗi update" });
            }

            return Json(new { status = 1, msg = "Update thành công" });
        }

        // GET: Admin/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                Danger("Không thấy thông tin!");
                return RedirectToAction("Index");
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();
            Success("Xóa thành công!");
            return RedirectToAction("Index");
        }

    }
}
