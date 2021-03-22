﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Controllers
{
    public class PagesController : BaseController
    {
        // GET: Pages
        //slug.html
        public ActionResult Index(string Slug)
        {
            var page = db.Pages.SingleOrDefault(p => p.Slug == Slug);
            if (page != null)
            {
                return View(page);
            }
            return RedirectToRoute("404");
        }

        //get siderbar
        public ActionResult Sidebar()
        {
            ViewBag.SpecialProducts = db.Products.Take(6).ToList();

            ViewBag.MenuGioiThieu = db.Menus.Where(m => m.LocationId == 1).ToList();
            ViewBag.MenuTroGiup = db.Menus.Where(m => m.LocationId == 2).ToList();

            return PartialView("Partials/_SideBarPage");
        }
    }
}