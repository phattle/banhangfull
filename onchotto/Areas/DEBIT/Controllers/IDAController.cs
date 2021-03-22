using Hotdeal.Models.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Hotdeal.Areas.DEBIT.Controllers
{
    public class IDAController : Controller
    {
        // GET: DEBIT/IDA
        public ActionResult Index()
        {
            return View();
        }

        // GET: DEBIT/IDA/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DEBIT/IDA/Create
        public ActionResult Create()
        {
            string s = "";
            ChromeDriver cd = new ChromeDriver(@"D:\4.SBP\4.FEDEX\chromedriver_win32"); 
            cd.Url = @"https://sso.secure.fedex.com/rda";
            cd.Navigate();
            
            IWebElement e = cd.FindElementById("username");
            e.SendKeys("3592371");
            e = cd.FindElementById("password");
            e.SendKeys("Anhanh16");
            e = cd.FindElementById("submit");
            e.Click();
            List<string> lst = new List<string>();
            int iget = 0;
            while (lst.Count == iget)
            {
                Thread.Sleep(500);
                IWebElement f = cd.FindElementById("redesignContent:trkTabLbl");
                f.Click();
                Thread.Sleep(900);

                IWebElement g = cd.FindElementByName("trackingNumberForm:trkNbrTxt");
                g.SendKeys("790813843931");
                g.SendKeys(Keys.Enter);

                Thread.Sleep(900);

                g = cd.FindElementById("trackingNumberForm:crncyTypInTxt");
                s = g.GetAttribute("value");

                if (s.Length > 0)
                    iget++;
            }
            return View();
        }

        // POST: DEBIT/IDA/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here




                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DEBIT/IDA/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DEBIT/IDA/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DEBIT/IDA/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DEBIT/IDA/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
