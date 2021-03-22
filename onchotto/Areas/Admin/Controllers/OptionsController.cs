using OnChotto.Models.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Areas.Admin.Controllers
{
    public class OptionsController : AdminController
    {
        // GET: Admin/Options
        public ActionResult General()
        {
            return View();
        }

        // GET: Admin/Options
        public ActionResult HomeSettings()
        {
            return View();
        }

        // GET: Admin/Options
        public ActionResult Socials()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SaveOption(FormCollection formCollection)
        {
            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key].ToString();
                OptionDao.SetOption(key.ToUpper(), value);
            }
            return RedirectToAction(formCollection["return_url"]);
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
