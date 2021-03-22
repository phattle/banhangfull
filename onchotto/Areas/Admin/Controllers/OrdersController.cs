using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnChotto.Models;
using OnChotto.Models.Dao;
using OnChotto.Models.Entities;
using OnChotto.Models.ViewModel;
using System.Collections.Generic;
using System;

namespace OnChotto.Areas.Admin.Controllers
{
    public class OrdersController : AdminController
    {


        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.OrderByDescending(o => o.OrderDate).Include(o => o.OrderStatus).Include(o => o.PaymentMethod).Include(o => o.Transport).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public string GetUserID()
        {
            return db.Users.Where(p => p.Email == User.Identity.Name).Select(p => p.Id).FirstOrDefault().ToString();
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name");
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");

            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,StatusId,Coupon,CouponDescription,Discount,ExtraDiscount,TotalWeight,TotalAmount,TotalOrder,Deposit,IsDeposit,TransportId,PaymentMethodId,ReceiveName,ReceiveEmail,ReceiveAddress,ReceivePhone,Note,OrderDate,RequireDate")] Order order)
        {

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", order.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            ViewBag.TransportId = new SelectList(db.Transports, "Id", "DistrictId", order.TransportId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var modelVm = new OrderEditViewModel();
            modelVm.UpdateDataFrom(order);
            modelVm.ProvinceId = order.Transport.District.ProvinceId;
            modelVm.DistrictId = order.Transport.DistrictId;


            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", order.StatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", order.UserId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", modelVm.ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", modelVm.DistrictId);
            ViewBag.TransportId = new SelectList(db.Transports.Where(d => d.DistrictId == modelVm.DistrictId).Select(x => new { TransportId = x.Id, Text = x.Transporter.Name + "-" + x.Cost + "vnd" }), "TransportId", "Text", modelVm.TransportId);

            return View(modelVm);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderEditViewModel modelVm, FormCollection from)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //}
            if (ModelState.IsValid)
            {
                var order = db.Orders.Find(modelVm.Id);

                if (order == null)
                {
                    return HttpNotFound();
                }

                //order.TotalOrder = order.TotalOrder - (modelVm.Discount ?? 0) + (order.Discount ?? 0);

                var newTransport = db.Transports.Find(modelVm.TransportId);

                order.UpdateDataFrom(modelVm);// bỏ tong amount và tong don hang

                if (newTransport != null)
                {
                    order.TotalOrder = order.TotalAmount + ((order.ShippingInLand + order.TECSServicesFee + order.ClearanceFee + order.AFFee + order.CustomFee + order.HandlingFee + order.FederalTax + order.ImportTax + newTransport.Cost + order.TransactionFee) - order.Discount);
                }
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                if (modelVm.StatusId == 01)
                {
                    OrderDao da = new OrderDao();

                    da.OrderToBilling();
                    OrderDetailDao dt = new OrderDetailDao();

                    dt.OrderDetailToBilling();
                }
                Success("Cập nhật thành công!");
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", modelVm.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", modelVm.PaymentMethodId);
            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", modelVm.ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", modelVm.DistrictId);
            ViewBag.TransportId = new SelectList(db.Transports.Where(d => d.DistrictId == modelVm.DistrictId).Select(x => new { TransportId = x.Id, Text = x.Transporter.Name + "-" + x.Cost + "vnd" }), "TransportId", "Text", modelVm.TransportId);

            return View(modelVm);
        }


        // GET: Edit orderdetail
        public ActionResult EditOrdersDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderDetails = db.OrderDetails.Include(d=>d.OrderStatus).Include(d => d.Product).Where(d => d.OrderId == order.Id).ToList();
            ViewBag.OrderId = order.Id;
            var listProductIds = order.OrderDetails.Select(d => d.ProductId).ToList();
            ViewBag.ProductId = new SelectList(db.Products.Where(p => !listProductIds.Contains(p.Id)).AsEnumerable(), "Id", "Name");
            ViewBag.orderStatus = db.OrderStatus;
            return View(orderDetails);
        }
        // GET: Change Order Detail

        public JsonResult ChangeOrderDetail(int? id, int? pid, int? qty, decimal? fe, decimal? ta, string no, decimal? we)
        {
            var order = db.Orders.Find(id);
            var orderDetail = db.OrderDetails.FirstOrDefault(d => d.ProductId == pid && d.OrderId == id);
            var product = db.Products.Find(pid);
            int iHsCodeId = (product.HsCodeId != null) ? (int)product.HsCodeId : 1;
            List<ProductTaxHscode> itemsProductTaxHscode = GetInFor(iHsCodeId);

            if (order == null || orderDetail == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            string Notes = no;
            //var weightKg = we != null ? we : product.WeightKG;
            var weightKg = we == 0 || we == null ? product.WeightKG : we;
            decimal FederalTaxs = 0;
            decimal Taxexports = 0;
            
            order.TotalOrder = ((order.TotalAmount) + (order.FederalTax) + (order.ImportTax) + (order.ShippingInLand) + (order.Transport.Cost) + (order.HandlingFee) + (order.ClearanceFee) + (order.AFFee) + (order.TECSServicesFee) + (order.CustomFee) + order.TransactionFee) - (order.Discount);

            foreach (ProductTaxHscode item in itemsProductTaxHscode)
            {
                order.ShippingInLand = ((orderDetail.PriceAfter ?? 0) * (qty ?? 1)) * (item.ShippinglandPercentage / 100);
            }
            foreach (ProductTaxHscode item in itemsProductTaxHscode)
            {
                order.AFFee = (weightKg * (qty ?? 1)) * (item.PriceAF * Commons.DataHelper.CurrRank);
            }
            foreach (ProductTaxHscode item in itemsProductTaxHscode)
            {
                order.TECSServicesFee = (weightKg * (qty ?? 1)) * item.PriceTECSServicesFee;
            }

            foreach (ProductTaxHscode item in itemsProductTaxHscode)
            {
                FederalTaxs += ((decimal)orderDetail.PriceAfter * (decimal)item.FederalTaxPercentage / 100) * (qty ?? 1);
            }
            foreach (ProductTaxHscode item in itemsProductTaxHscode)
            {
                Taxexports += (((decimal)orderDetail.PriceAfter * ((decimal)item.TaxPercentage) / 100) + (((decimal)orderDetail.PriceAfter + ((decimal)orderDetail.PriceAfter * ((decimal)item.TaxPercentage)) / 100)) * ((decimal)item.VATPercentage) / 100) * (qty ?? 1);
            }
            var info = new
            {
                status = 1,
                AmountTotal = (orderDetail.PriceAfter) * (qty ?? 1),
                Total = order.TotalAmount,
                TotalWeights = (qty ?? 1) * (weightKg ?? 0),
                FederalTaxs = FederalTaxs,
                Taxexports = Taxexports

            };
            db.Entry(order).State = EntityState.Modified;
            orderDetail.Product.WeightKG = weightKg;
            orderDetail.Amount = qty ?? 1;
            orderDetail.FederalTax = FederalTaxs;
            orderDetail.TaxExport = Taxexports;
            orderDetail.Note = Notes;
            db.Entry(orderDetail).State = EntityState.Modified;
            db.SaveChanges();
            UpdateTotal((int)id);
            return Json(info, JsonRequestBehavior.AllowGet);
            //return Json(new { status = 1, Total = (order.TotalAmount ?? 0) }, JsonRequestBehavior.AllowGet);

        }

        public void UpdateTotal(int orderId)// total order details
        {
            // Tổng thuế bang, thue nhap khau
            try
            {
                var order = db.Orders.Find(orderId);
                order.FederalTax = db.OrderDetails.Where(d => d.OrderId == orderId).Sum(p => p.FederalTax).Value;
                order.ImportTax = db.OrderDetails.Where(d => d.OrderId == orderId).Sum(p => p.TaxExport).Value;
                order.TotalWeight = db.OrderDetails.Where(d => d.OrderId == orderId).Sum(p => p.Product.WeightKG * p.Amount).Value;
                order.TotalAmount = db.OrderDetails.Where(d => d.OrderId == orderId).Sum(p => p.Product.PriceAfter * p.Amount).Value;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        // LinQ truy vấn lấy HscodeId bảng ProductTaxHscodes
        public List<ProductTaxHscode> GetInFor(int HsCodeId)
        {

            //int lstOrderDetail = Commons.DataHelper.CorrectValue(db.OrderDetails.Where(or => or.ProductId == ProductID).Select(or => or.ProductId).Distinct(), int.MinValue);
            //var LstProductTaxHscode = from pro in db.Products
            //                          from protax in db.ProductTaxHscodes
            //                          where pro.Id == ProductID && pro.HsCodeId == protax.HsCodeId
            //                          select protax;
            var LstProductTaxHscode = from protax in db.ProductTaxHscodes
                                      where protax.HsCodeId == HsCodeId
                                      select protax;

            return LstProductTaxHscode.ToList();
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            db.OrderDetails.RemoveRange(order.OrderDetails);
            db.Orders.Remove(order);
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
        public ActionResult CheckOut(int? id)
        {

            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.IsDeposit.HasValue && order.IsDeposit.Value)
            {
                Danger("Đơn hàng này đã thanh toán tạm ứng rồi.");
                return RedirectToAction("Details", new { id = order.Id });
            }

            order.IsDeposit = true;
            db.Entry(order).State = EntityState.Modified;

            db.SaveChanges();
            Success($"Update thanh toán thành công cho đơn hàng #{order.Id}");
            return RedirectToAction("Details", new { id = order.Id });
        }
        public ActionResult CheckOutPay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.Ispayenough.HasValue && order.Ispayenough.Value)
            {
                Danger("Đơn hàng này đã thanh toán đủ rồi.");
                return RedirectToAction("Details", new { id = order.Id });
            }

            order.Ispayenough = true;
            db.Entry(order).State = EntityState.Modified;

            db.SaveChanges();
            Success($"Update thanh toán đủ thành công cho đơn hàng #{order.Id}");
            return RedirectToAction("Details", new { id = order.Id });
        }

        public ActionResult AjaxGetTransportToSelect(string districtId)
        {
            var transports = db.Transports.Where(t => t.DistrictId == districtId).ToList();
            return View(transports);
        }
    }
}
