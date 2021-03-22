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

namespace OnChotto.Controllers
{
    public class QuotationsController : BaseController
    {
        // GET: Quotations
        public ActionResult Index()
        {
            return View();
        }

        // POST: Quotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "CustomerName,CustomerEmail,CustomerPhone,AdditionalInformation")] Quotation quotation, FormCollection form)
        {

            var productLinks = form["ProductLinks[]"];
            var productNumbers = form["ProductNumbers[]"];

            if (string.IsNullOrEmpty(productLinks))
            {
                ModelState.AddModelError("ProductLinks", "Bạn cần phải nhập tối thiểu 1 link sản phẩm");
            }

            string[] productLinksArr = productLinks.Split(',');
            string[] productNumbersArr = productNumbers.Split(',');

            quotation.ProductLinks = string.Empty;
            if (productLinksArr.Length > 0 && productLinksArr.Length == productNumbersArr.Length)
            {
                for (var i = 0; i < productLinksArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(productLinksArr[i]))
                    {
                        if (!string.IsNullOrEmpty(quotation.ProductLinks))
                        {
                            quotation.ProductLinks += ",";
                        }

                        quotation.ProductLinks += productLinksArr[i] + "|" + productNumbersArr[i];
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại sản phẩm và số lượng tương ứng, chưa khớp.");
            }

            if (ModelState.IsValid)
            {
                quotation.CreatedAt = DateTime.Now;
                quotation.Status = "New";

                db.Quotations.Add(quotation);
                db.SaveChanges();
                Success("Gửi yêu cầu thành công!");
            }

            return View(quotation);
        }

    }
}
