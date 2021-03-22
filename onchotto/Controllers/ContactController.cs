using OnChotto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        // POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ContactName,ContactEmail,ContactPhone,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.Now;
                contact.Status = "New";
                db.Contacts.Add(contact);
                db.SaveChanges();
                Success("Cảm ơn bạn đã gửi góp ý, chúng tôi sẽ liên lạc với bạn ngay khi có thể.!", true);
            }
            return View();
        }
    }
}