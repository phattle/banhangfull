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
using OnChotto.Commons;
using System.Web.Configuration;

namespace OnChotto.Areas.Admin.Controllers
{
    public class OrdersDiffController : AdminController
    {


        // GET: Admin/Orders
        public ActionResult Index()
        {
            List<OrderDiffViewModel> lst = new List<OrderDiffViewModel>();
            //var ordersdiff = db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User);
            var ordersdiff = (from p in db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User)
                              join s in db.OrderStatus on p.StatusId equals s.Id
                              join m in db.PaymentMethods on p.PaymentMethodId equals m.Id
                              select new
                              {
                                  Id = p.Id,
                                  ReceiveName = p.ReceiveName,
                                  MAWB = p.MAWB,
                                  DistrictId = p.DistrictId,
                                  ProvinceId = p.ProvinceId,
                                  ReceiveEmail = p.ReceiveEmail,
                                  ReceiveAddress = p.ReceiveAddress,
                                  ReceivePhone = p.ReceivePhone,
                                  OrderDate = p.OrderDate,
                                  RequireDate = p.RequireDate,
                                  TotalWeight = p.TotalWeight,
                                  TotalAmount = p.TotalAmount,
                                  IsDeposit = p.IsDeposit,
                                  Ispayenough = p.Ispayenough,
                                  PaymentMethodId = p.PaymentMethodId,
                                  StatusId = p.StatusId,
                                  UserId = p.UserId,
                                  OrderStatusName = s.Name,
                                  PaymentmethodName = m.Name,
                                  Note = p.Note
                              }
                             );
            foreach (var item in ordersdiff)
            {
                OrderDiffViewModel it = new OrderDiffViewModel();

                it.ReceiveName = item.ReceiveName;
                it.MAWB = item.MAWB;
                //it.DistrictId = item.DistrictId;
                //it.ProvinceId = item.ProvinceId;
                it.ReceiveEmail = item.ReceiveEmail;
                it.ReceiveAddress = item.ReceiveAddress;
                it.ReceivePhone = item.ReceivePhone;
                it.OrderDate = item.OrderDate;
                //it.RequireDate = item.RequireDate;
                it.TotalWeight = item.TotalWeight;
                it.TotalAmount = item.TotalAmount;
                it.IsDeposit = item.IsDeposit;
                it.Ispayenough = item.Ispayenough;
                it.PaymentMethodId = item.PaymentMethodId;
                //it.StatusId = item.StatusId;
                it.UserId = item.UserId;
                it.OrderDiffId = item.Id;
                it.OrderStatusName = item.OrderStatusName;
                it.PaymentMethodsName = item.PaymentmethodName;
                it.Note = item.Note;
                lst.Add(it);
            }
            return View(lst);
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<OrderDiffViewModel> lst = new List<OrderDiffViewModel>();
            //var ordersdiff = db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User);
            var ordersdiff = (from p in db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User)
                              join d in db.OrderDetailDiff on p.Id equals d.OrderDiffId
                              join s in db.OrderStatus on p.StatusId equals s.Id
                              join m in db.PaymentMethods on p.PaymentMethodId equals m.Id
                              where p.Id == id && d.OrderDiffId == id
                              select new
                              {
                                  Id = p.Id,
                                  ReceiveName = p.ReceiveName,
                                  MAWB = p.MAWB,
                                  DistrictId = p.DistrictId,
                                  ProvinceId = p.ProvinceId,
                                  ReceiveEmail = p.ReceiveEmail,
                                  ReceiveAddress = p.ReceiveAddress,
                                  ReceivePhone = p.ReceivePhone,
                                  OrderDate = p.OrderDate,
                                  RequireDate = p.RequireDate,
                                  TotalWeight = p.TotalWeight,
                                  TotalAmount = p.TotalAmount,
                                  IsDeposit = p.IsDeposit,
                                  Ispayenough = p.Ispayenough,
                                  PaymentMethodId = p.PaymentMethodId,
                                  StatusId = p.StatusId,
                                  UserId = p.UserId,
                                  OrderDiffId = d.OrderDiffId,
                                  OrderNo = d.OrderNo,
                                  OrderTrackingNo = d.OrderTrackingNo,
                                  StoreName = d.StoreName,
                                  ProductLink = d.ProductLink,
                                  ProductName = d.ProductName,
                                  Size = d.Size,
                                  Amount = d.Amount,
                                  Weight = d.Weight,
                                  Price = d.Price,
                                  PriceAfter = d.PriceAfter,
                                  Discount = d.Discount,
                                  Note = d.Note,
                                  DetailID = d.Id,
                                  ProductStatus = d.ProductStatus,
                                  OrderStatusName = s.Name,
                                  PaymentmethodName = m.Name,
                                  WeightUnit = d.WeightUnit,
                                  Currency = d.Currency,
                                  TransType = d.TransType
                              }

                             );
            var itemc = "";
            if (ordersdiff.FirstOrDefault().OrderDate != null)
            {
                itemc = $"SmuaVN{ordersdiff.FirstOrDefault().OrderDate.Value.Year.ToString("00")}{ordersdiff.FirstOrDefault().OrderDate.Value.Month:00}{ordersdiff.FirstOrDefault().OrderDiffId:000000}";
            }
            else itemc = $"SmuaVN{DateTime.Now.Year:00}{DateTime.Now.Month:00}{ordersdiff.FirstOrDefault().OrderDiffId:000000}";

            foreach (var item in ordersdiff)
            {
                OrderDiffViewModel it = new OrderDiffViewModel();

                it.ReceiveName = item.ReceiveName;
                it.MAWB = item.MAWB;
                it.DistrictId = item.DistrictId;
                it.ProvinceId = item.ProvinceId;
                it.ReceiveEmail = item.ReceiveEmail;
                it.ReceiveAddress = item.ReceiveAddress;
                it.ReceivePhone = item.ReceivePhone;
                it.OrderDate = item.OrderDate;
                //it.RequireDate = item.RequireDate;
                it.TotalWeight = item.TotalWeight;
                it.TotalAmount = item.TotalAmount;
                it.IsDeposit = item.IsDeposit;
                it.Ispayenough = item.Ispayenough;
                it.PaymentMethodId = item.PaymentMethodId;
                //it.StatusId = item.StatusId;
                it.UserId = item.UserId;
                //it.OrderDiffId = item.OrderDiffId;
                it.OrderNo = item.OrderNo;
                it.OrderTrackingNo = item.OrderTrackingNo;
                it.StoreName = item.StoreName;
                it.ProductLink = item.ProductLink;
                it.ProductName = item.ProductName;
                it.Size = item.Size;
                it.Amount = item.Amount;
                it.Weight = item.Weight.ToString();
                it.Price = item.Price.ToString();
                it.PriceAfter = item.PriceAfter.ToString();
                it.Discount = item.Discount.ToString();
                it.Note = item.Note;
                it.OrderDiffId = item.Id;
                it.ProductStatus = item.ProductStatus;
                it.OrderStatusName = item.OrderStatusName;
                it.PaymentMethodsName = item.PaymentmethodName;
                it.ItemCode = itemc;
                it.WeightUnit = item.WeightUnit;
                it.Currency = item.Currency;
                it.TransType = item.TransType;
                lst.Add(it);
            }
            return View(lst);
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
            //Order order = db.Orders.Find(id);
            //if (order == null)
            //{
            //    return HttpNotFound();
            //}
            //var modelVm = new OrderEditViewModel();
            //modelVm.UpdateDataFrom(order);
            //modelVm.ProvinceId = order.Transport.District.ProvinceId;
            //modelVm.DistrictId = order.Transport.DistrictId;
            List<OrderDiffViewModel> lst = new List<OrderDiffViewModel>();
            //var ordersdiff = db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User);
            var ordersdiff = (from p in db.OrderDiffs.OrderByDescending(o => o.OrderDate).Include(o => o.User)
                              join s in db.OrderStatus on p.StatusId equals s.Id
                              join m in db.PaymentMethods on p.PaymentMethodId equals m.Id
                              where p.Id == id
                              select new
                              {
                                  Id = p.Id,
                                  ReceiveName = p.ReceiveName,
                                  MAWB = p.MAWB,
                                  DistrictId = p.DistrictId,
                                  ProvinceId = p.ProvinceId,
                                  ReceiveEmail = p.ReceiveEmail,
                                  ReceiveAddress = p.ReceiveAddress,
                                  ReceivePhone = p.ReceivePhone,
                                  OrderDate = p.OrderDate,
                                  RequireDate = p.RequireDate,
                                  TotalWeight = p.TotalWeight,
                                  TotalAmount = p.TotalAmount,
                                  IsDeposit = p.IsDeposit,
                                  Ispayenough = p.Ispayenough,
                                  PaymentMethodId = p.PaymentMethodId,
                                  StatusId = p.StatusId,
                                  UserId = p.UserId,
                                  OrderStatusName = s.Name,
                                  PaymentmethodName = m.Name,
                                  Note = p.Note
                              }

                             );
            foreach (var item in ordersdiff)
            {
                OrderDiffViewModel it = new OrderDiffViewModel();

                it.ReceiveName = item.ReceiveName;
                it.MAWB = item.MAWB;
                it.DistrictId = item.DistrictId;
                it.ProvinceId = item.ProvinceId;
                it.ReceiveEmail = item.ReceiveEmail;
                it.ReceiveAddress = item.ReceiveAddress;
                it.ReceivePhone = item.ReceivePhone;
                it.OrderDate = item.OrderDate;
                //it.RequireDate = item.RequireDate;
                it.TotalWeight = item.TotalWeight;
                it.TotalAmount = item.TotalAmount;
                it.IsDeposit = item.IsDeposit;
                it.Ispayenough = item.Ispayenough;
                it.PaymentMethodId = item.PaymentMethodId;
                it.StatusId = Convert.ToInt32(item.StatusId);
                it.UserId = item.UserId;
                it.OrderDiffId = item.Id;
                it.OrderStatusName = item.OrderStatusName;
                it.PaymentMethodsName = item.PaymentmethodName;
                it.Note = item.Note;
                lst.Add(it);
            }

            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", ordersdiff.FirstOrDefault().StatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", ordersdiff.FirstOrDefault().UserId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", ordersdiff.FirstOrDefault().PaymentMethodId);
            var a = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", ordersdiff.FirstOrDefault().ProvinceId);
            var b = new SelectList(db.Districts.Where(d => d.ProvinceId == ordersdiff.FirstOrDefault().ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", ordersdiff.FirstOrDefault().DistrictId);
            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", ordersdiff.FirstOrDefault().ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == ordersdiff.FirstOrDefault().ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", ordersdiff.FirstOrDefault().DistrictId);
            //ViewBag.TransportId = new SelectList(db.Transports.Where(d => d.DistrictId == ordersdiff.FirstOrDefault().DistrictId).Select(x => new { TransportId = x.Id, Text = x.Transporter.Name + "-" + x.Cost + "vnd" }), "TransportId", "Text", ordersdiff.FirstOrDefault().TransportId);

            return View(lst);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDiffViewModel modelVm, FormCollection from)
        {

            if (!ModelState.IsValid)
            {
                int approvestatus = 0;
                approvestatus = DataHelper.CorrectValue(WebConfigurationManager.AppSettings["approvestatus"].ToString(), int.MinValue);


                var ordersdiff = db.OrderDiffs.Find(modelVm.OrderDiffId);
                ordersdiff.ReceiveAddress = modelVm.ReceiveAddress;
                ordersdiff.ReceivePhone = modelVm.ReceivePhone;
                ordersdiff.ReceiveName = modelVm.ReceiveName;
                ordersdiff.ProvinceId = modelVm.ProvinceId;
                ordersdiff.DistrictId = modelVm.DistrictId;
                ordersdiff.StatusId = modelVm.StatusId;
                ordersdiff.DistrictId = modelVm.DistrictId;
                ordersdiff.PaymentMethodId = modelVm.PaymentMethodId;
                ordersdiff.Note = modelVm.Note;
                ordersdiff.MAWB = modelVm.MAWB;
                if (ordersdiff == null)
                {
                    return HttpNotFound();
                }
                db.Entry(ordersdiff).State = EntityState.Modified;
                db.SaveChanges();

                if (modelVm.StatusId == approvestatus)
                {
                    OrderDiffs dt = new OrderDiffs();
                    OnChotto.Models.Entities.OrderDiff item = new OnChotto.Models.Entities.OrderDiff();

                    item.ReceiveAddress = modelVm.ReceiveAddress;
                    item.ReceivePhone = modelVm.ReceivePhone;
                    item.ReceiveName = modelVm.ReceiveName;
                    item.ReceiveEmail = modelVm.ReceiveEmail;
                    item.ProvinceId = modelVm.ProvinceId;
                    item.DistrictId = modelVm.DistrictId;
                    item.StatusId = modelVm.StatusId;
                    item.DistrictId = modelVm.DistrictId;
                    item.PaymentMethodId = modelVm.PaymentMethodId;
                    item.Note = modelVm.Note;
                    item.MAWB = modelVm.MAWB;
                    item.OrderDate = ordersdiff.OrderDate;
                    item.RequireDate = ordersdiff.OrderDate.Value.AddDays(20);
                    dt.OrderDiffToBilling(item);

                    List<OnChotto.Models.Entities.OrderDetailDiff> lst = db.OrderDetailDiff.Where(p => p.OrderDiffId == modelVm.OrderDiffId).ToList();
                    OnChotto.Models.Dao.OrderDetailDiff da = new OnChotto.Models.Dao.OrderDetailDiff();
                    da.SendOrderDetailDiff(lst);

                }

                Success("Cập nhật thành công!");
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.OrderStatus, "Id", "Name", modelVm.StatusId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethods, "Id", "Name", modelVm.PaymentMethodId);
            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", modelVm.ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", modelVm.DistrictId);
            //ViewBag.TransportId = new SelectList(db.Transports.Where(d => d.DistrictId == modelVm.DistrictId).Select(x => new { TransportId = x.Id, Text = x.Transporter.Name + "-" + x.Cost + "vnd" }), "TransportId", "Text", modelVm.TransportId);

            return View(modelVm);
        }


        // GET: Edit orderdetail
        public ActionResult EditOrdersDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDiff orderDiff = db.OrderDiffs.Find(id);
            if (orderDiff == null)
            {
                return HttpNotFound();
            }
            //var orderDetails = db.OrderDetails.Include(d => d.Product).Where(d => d.OrderId == orderDiff.Id).ToList();
            var orderDiffDetails = db.OrderDetailDiff.Where(p => p.OrderDiffId == orderDiff.Id).ToList();
            ViewBag.OrderDiffId = orderDiff.Id;
            //var listProductIds = order.OrderDetails.Select(d => d.ProductId).ToList();
            //ViewBag.ProductId = new SelectList(db.Products.Where(p => !listProductIds.Contains(p.Id)).AsEnumerable(), "Id", "Name");
            return View(orderDiffDetails);
        }
        // GET: Change Order Detail

        public JsonResult ChangeOrderDetail(int? id, int? pid, int? qty, string we, decimal? pr, decimal? ds, decimal? pa, string unit, string cur)
        {
            var ordersdiff = db.OrderDiffs.Find(id);
            var orderDiffDetails = db.OrderDetailDiff.FirstOrDefault(d => d.OrderDiffId == id && d.Id == pid);
            var product = db.Products.Find(pid);
            if (ordersdiff == null || orderDiffDetails == null)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            var TotalAmoutold = ((orderDiffDetails.PriceAfter ?? 0) * (orderDiffDetails.Amount ?? 0));
            var TotalAmountnew = (orderDiffDetails.PriceAfter ?? 0) * (qty ?? 1);

            //var TotalOrderold = (orderDetail.PriceAfter ?? 0) * (orderDetail.Amount ?? 0) + (orderDetail.FederalTax ?? 0) + (orderDetail.ShippingInLand ?? 0) + (orderDetail.TaxExport) + (order.Transport.Cost) + (order.HandlingFee) + (order.ClearanceFee) + (order.AFFee) + (order.TECSServicesFee) + (order.CustomFee) - (order.Discount);
            //var TotalOrdernew = (orderDetail.PriceAfter ?? 0) * (qty ?? 1) + (fe ?? 0) + (sh ?? 0) + (ta ?? 0) + (order.Transport.Cost) + (order.HandlingFee) + (order.ClearanceFee) + (order.AFFee) + (order.TECSServicesFee) + (order.CustomFee) - (order.Discount);
            ordersdiff.TotalAmount += TotalAmountnew - TotalAmoutold;
            //ordersdiff.TotalOrder += TotalOrdernew - TotalOrderold;
            ordersdiff.TotalWeight += (orderDiffDetails.Weight) * (qty ?? 1);
            var info = new
            {
                status = 1,
                AmountTotal = (orderDiffDetails.PriceAfter * orderDiffDetails.Amount)
                //Total = order.TotalAmount
            };
            db.Entry(ordersdiff).State = EntityState.Modified;

            orderDiffDetails.Amount = qty ?? 1;
            orderDiffDetails.Weight = Common.CorrectValue(we, decimal.MinValue);
            orderDiffDetails.Price = pr ?? 1;
            orderDiffDetails.Discount = ds ?? 1;
            orderDiffDetails.PriceAfter = pa ?? 1;
            orderDiffDetails.WeightUnit = unit;
            orderDiffDetails.Currency = cur;
            db.Entry(orderDiffDetails).State = EntityState.Modified;
            db.SaveChanges();
            return Json(info, JsonRequestBehavior.AllowGet);
            //return Json(new { status = 1, Total = (order.TotalAmount ?? 0) }, JsonRequestBehavior.AllowGet);

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
            OrderDiff order = db.OrderDiffs.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.Ispayenough == true)
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
