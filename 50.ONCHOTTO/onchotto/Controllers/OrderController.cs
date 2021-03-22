using OnChotto.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Commons.Libs;
using OnChotto.Models.Entities;
using OnChotto.Models.ViewModel;
using PagedList;
using System.Net;

namespace OnChotto.Controllers
{
    public class OrderController : BaseController
    {

        public ShoppingCart cart = ShoppingCart.Cart;

        [HttpGet]
        public ActionResult Checkout()
        {
            if (cart.Count == 0)
            {
                Warning(string.Format("<b><h5>{0}</h4></b>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.OtherProvinceId = ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull");
            ViewBag.OtherDistrictId = ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "-1").Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull");
            ViewBag.OtherWardId = ViewBag.WardId = new SelectList(db.Wards.Where(d => d.districtid == "-1").Select(x => new { WardId = x.WardId, NameFull = x.type + " " + x.Name }), "WardId", "NameFull");



            var model = new OrderViewModel();

            if (Request.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                model.UserId = User.Identity.GetUserId();
                if (user != null)
                {
                    model.ReceiveEmail = user.Email;
                    model.ReceiveName = user.FullName;
                    model.ReceivePhone = user.PhoneNumber;
                    model.ReceiveAddress = user.Address;
                    if (user.District != null)//second
                    {
                        ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", user.District.ProvinceId);
                        ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == user.District.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", user.DistrictId);
                        ViewBag.WardId = new SelectList(db.Wards.Where(d => d.districtid == user.District.DistrictId).Select(x => new { WardId = x.WardId, NameFull = x.type + " " + x.Name }), "WardId", "NameFull", user.WardId);


                    }
                }
            }

            return View(model);
        }

        [HttpPost]

        public async Task<ActionResult> Checkout(OrderViewModel modelVm, FormCollection form)
        {
            var sms = new SpeedSMSAPI();

            //Validate Cart
            if (cart.Count == 0)
            {
                Warning(string.Format("<h5>{0}</h4>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                return RedirectToAction("Index", "Home");
            }
            // Validate Email
            if (!Request.IsAuthenticated && string.IsNullOrEmpty(modelVm.ReceiveEmail))
                ModelState.AddModelError("", "-Bạn chưa nhập email nhận đơn hàng!");

            //Check quận huyện
            if (String.IsNullOrEmpty(modelVm.DistrictId))
                ModelState.AddModelError("", "-Bạn chưa chọn quận huyện nơi chuyển hàng tới!");
            if (String.IsNullOrEmpty(modelVm.WardId))
                ModelState.AddModelError("", "-Bạn chưa chọn xã phường nơi chuyển hàng tới!");

            //Kiểm tra nếu thông tin người nhận hàng khác
            if (modelVm.OtherAddress)
            {
                if (string.IsNullOrEmpty(modelVm.OtherReceiveName)
                    || string.IsNullOrEmpty(modelVm.OtherReceivePhone)
                    || string.IsNullOrEmpty(modelVm.OtherEmail)
                    || string.IsNullOrEmpty(modelVm.OtherReceiveAddress)
                    || string.IsNullOrEmpty(modelVm.OtherProvinceId)
                    || string.IsNullOrEmpty(modelVm.OtherDistrictId)
                    || string.IsNullOrEmpty(modelVm.OtherWardId))
                {
                    ModelState.AddModelError("", "-Vui lòng nhập đầy đủ thông tin người nhận hàng.");
                }
            }

            //Kiểm tra nếu là người dùng mới thì tạo tài khoản
            if (ModelState.IsValid && string.IsNullOrEmpty(modelVm.UserId))
            {
                var password = Xstring.GeneratePassword();
                var newUser = new ApplicationUser
                {
                    FullName = modelVm.ReceiveName,
                    UserName = modelVm.ReceiveEmail,
                    Email = modelVm.ReceiveEmail,
                    PhoneNumber = modelVm.ReceivePhone,
                    DistrictId = modelVm.DistrictId,
                    WardId = modelVm.WardId,
                    Address = modelVm.ReceiveAddress,
                    PasswordHash = password,
                };

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                    await signInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);
                    modelVm.UserId = newUser.Id;

                    //Gửi sms
                    string smsAcc = "OnChotto TB: Tai khoan quan ly don hang cua ban tren OnChotto la: " + modelVm.ReceiveEmail + ", mat khau:" + password;
                    string sent = sms.sendSMS(modelVm.ReceivePhone, smsAcc, 2, "");

                    //Gửi tin nhắn tài khoản cho người dùng.
                    var subject = "Tài khoản quản lý đơn hàng tại OnChotto.";
                    var msg = "Xin chào, " + modelVm.ReceiveName;
                    msg += "<br>Tài khoản quản lý đơn hàng của bạn tại <a href='http://onchotto.com/Account/Login'>onchotto.com</a> là:";
                    msg += "<br>-Tên đăng nhập: " + modelVm.ReceiveEmail;
                    msg += "<br>-Mật khẩu của bạn: " + password;
                    msg += "<br>Bạn có thể sử dụng tài khoản này đăng nhập trên onchotto.com để quản lý đơn hàng và sử dụng các dịch vụ khác do OnChotto cung cấp.!";
                    msg += "<br>Cảm ơn bạn đã quan tâm sử dụng dịch vụ mua hộ của Smua. mọi thắc mắc xin liên hệ hotline: (1800)545480";
                    msg += "<br>OnChotto Hân hạnh được phục vụ bạn.";
                    msg += "<br>Chúc bạn một ngày tốt lành.";
                    msg += "<p></p><p></p>-BQT OnChotto!.</p>";

                    XMail.Send(newUser.Email, subject, msg);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", @"-" + error.Replace("is already taken", "đã tồn tại."));
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var user = db.Users.Find(modelVm.UserId);
                if (user != null)
                {
                    //Update UserInfo
                    user.FullName = modelVm.ReceiveName;
                    user.PhoneNumber = modelVm.ReceivePhone;
                    user.Address = modelVm.ReceiveAddress;
                    user.DistrictId = cart.Transport.DistrictId;
                    user.WardId = cart.Transport.WardId;
                    db.Entry(user).State = EntityState.Modified;
                }

                //Update order info 
                var model = new Order();
                model.UpdateDataFrom(modelVm);

                model.TotalAmount = cart.Total;
                model.TotalWeight = cart.TotalWeight;
                model.TotalOrder = cart.OrderTotal;
                model.HandlingFee = cart.HandlingFeeUS;
                model.AFFee = cart.AFFeeVN;
                model.ClearanceFee = cart.ClearanceFeeVN;
                model.TECSServicesFee = cart.TECSServicesFeeVN;
                model.CustomFee = cart.CustomFeeVN;
                model.ShippingInLand = cart.ShippingInLandUS;
                model.FederalTax = cart.FederalTaxUS;
                model.ImportTax = cart.TaxExportVN;
                if (model.Deposit == 50)
                {
                    model.TransactionFee = cart.TransactionFeeVN;
                }
                else
                {
                    model.TransactionFee = 0;
                }
                //model.TransactionFee = cart.TransactionFeeVN;
                if (cart.Transport != null)
                {
                    model.TransportId = cart.Transport.Id;
                }
                model.Coupon = cart.CouponCode;
                model.CouponDescription = cart.CouponNameDescription;
                model.Discount = cart.Discount;
                model.OrderDate = DateTime.Now;
                model.StatusId = 1;

                //Update thông tin người nhận hàng
                if (modelVm.OtherAddress)
                {
                    model.ReceiveName = modelVm.OtherReceiveName;
                    model.ReceiveEmail = modelVm.OtherEmail;
                    model.ReceivePhone = modelVm.OtherReceivePhone;
                    model.ReceiveAddress = modelVm.OtherReceiveAddress;
                }

                db.Orders.Add(model);
                // Insert order detail
                try
                {
                    foreach (var p in cart.Items)
                    {

                        //ViewBag.ProductDetail = cart.Items;
                        string strTypeProduct = db.Products.Where(fp => fp.Id == p.Id).Select(fp => fp.ProductZone).FirstOrDefault().ToString();
                        var d = new OrderDetail
                        {
                            OrderId = model.Id,
                            ProductId = p.Id,
                            PriceAfter = p.PriceAfter,
                            Discount = p.Discount,
                            Amount = p.Amount,
                            FederalTax = (p.PriceAfter * ShoppingCart.Cart.Federaltaxpercentage) * p.Amount,
                            TaxExport = ((p.PriceAfter * ShoppingCart.Cart.Taxpercentage) + ((p.PriceAfter + (p.PriceAfter * ShoppingCart.Cart.Taxpercentage)) * ShoppingCart.Cart.Vatpercentage)) * p.Amount,
                            OrderType = strTypeProduct
                        };
                        db.OrderDetails.Add(d);
                    }
                    if (db.SaveChanges() > 0)
                    {
                        cart.Clear();
                        Success(string.Format("<b><h5>{0}</h4></b>", "Đặt hàng thành công, chúng tôi sẽ liên hệ lại với bạn để xác nhận giá mua hộ đơn hàng trước khi tiến hành đặt hàng. Trân trọng phục vụ!"), true);

                        //Gửi SMS xác nhận và báo tin cho Sale
                        var customerMsg = $"OnChotto TB: Dat hang thanh cong MaDH:#{model.Id}, Tong Tien: {model.TotalOrder:0,0}vnđ";
                        var saleSms = $"OnChotto TB: Don hang moi #{model.Id} tu KH: {model.ReceiveName} - {model.ReceivePhone}  vui long kiem tra va xn don hang";
                        sms.sendSMS(model.ReceivePhone, customerMsg, 2, "");
                        if (!String.IsNullOrEmpty(modelVm.OtherReceivePhone))
                        {
                            sms.sendSMS(modelVm.OtherReceivePhone, customerMsg, 2, "");
                        }
                        sms.sendSMS("0903896741", saleSms, 2, "");

                        //Gửi mail cho sale.
                        var Subject = "Hệ thống thông báo đơn hàng mới ";
                        var Msg = "Xin chào sale, ";
                        Msg += "<br>BQT muốn nhắc bạn có đơn hàng mới.";
                        Msg += "<br>Đơn hàng MaDH:#" + model.Id + ", Với số tiền: " + string.Format("{0:0,0}vnđ", model.TotalOrder);
                        Msg += "<br>Khách hàng: " + model.ReceiveName;
                        Msg += "<br>Địa chỉ: " + model.ReceiveAddress;
                        Msg += "<br>Điện thoại: " + model.ReceivePhone;
                        Msg += "<br>Vui lòng liên hệ với khách hàng để xác nhận";
                        Msg += "<br>Chúc Bạn một ngày tốt lành, tràn đầy năng lượng";
                        Msg += "<p></p><p></p>-BQT OnChotto!.</p>";

                        XMail.Send("cskh@onchotto.com", Subject, Msg);


                        //Gửi mail tài khoản cho người dùng khi chưa có tài khoản.
                        modelVm.UserId = user.Id;
                        var subject = "Thông báo đặt hàng thành công!";
                        var msg = "Xin chào, " + model.ReceiveName;
                        msg += "<br>OnChotto thông báo: Đặt hàng thành công MaDH:#" + model.Id + ", Với số tiền: " + string.Format("{0:0,0}vnđ", model.TotalOrder);
                        msg += "<br>Bạn đăng nhập tài khoản tại: <a href='http://onchotto.com/Account/Login'>onchotto.com</a> để kiểm tra danh sách tình trạng đơn hàng";
                        msg += "<br>Cảm ơn Bạn đã quan tâm sử dụng dịch vụ của OnChotto. mọi thắc mắc xin liên hệ hotline: 1800-545480 - 0903.896741.";
                        msg += "<br>OnChotto hân hạnh được phục vụ Bạn.";
                        msg += "<br>Chúc Bạn một ngày tốt lành.";
                        msg += "<p></p><p></p>-BQT OnChotto!.</p>";
                        XMail.Send(user.Email, subject, msg);
                        if (!String.IsNullOrEmpty(modelVm.OtherEmail))
                        {
                            XMail.Send(modelVm.OtherEmail, subject, msg);
                        }
                        return RedirectToAction("Detail", new { id = model.Id });
                    }

                }
                catch (Exception ex)
                {
                    Danger($"-{ex.Message}<br>", true);
                    ModelState.AddModelError("", ex.InnerException);
                }
            }

            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", modelVm.ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", modelVm.DistrictId);
            ViewBag.WardId = new SelectList(db.Wards.Where(d => d.districtid == modelVm.DistrictId).Select(x => new { WardId = x.WardId, NameFull = x.type + " " + x.Name }), "WardId", "NameFull", modelVm.WardId);


            ViewBag.OtherProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", modelVm.OtherProvinceId);
            ViewBag.OtherDistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.OtherProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", modelVm.OtherDistrictId);
            ViewBag.OtherWardId = new SelectList(db.Wards.Where(d => d.districtid == modelVm.DistrictId).Select(x => new { WardId = x.WardId, NameFull = x.type + " " + x.Name }), "WardId", "NameFull", modelVm.OtherWardId);

            return View(modelVm);
        }



        public ActionResult Detail(int id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Total = order.StatusId;
            return View(order);
        }

        public ActionResult List(int? page)
        {
            string currentUserId = User.Identity.GetUserId();
            var orders = db.Orders.Where(o => o.UserId == currentUserId).ToList();
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        public bool UpdateTransport(int transportId)
        {
            var transport = db.Transports.Find(transportId);
            if (transport == null)
            {
                return false;
            }

            //Update cart Transport
            cart.UpdateTransport(transport);

            return true;
        }


        public bool UpdateCoupon(string code)
        {
            var coupon = db.Coupons.Find(code);
            if (coupon == null)
            {
                return false;
            }
            //Update cart Transport
            cart.UpdateCoupon(coupon);

            return true;
        }


        public ActionResult AjaxGetTransport(string districtId)
        {
            var transports = db.Transports.Where(t => t.DistrictId == districtId).ToList();
            if (transports.Count() > 0)
            {
                UpdateTransport(transports.First().Id);
            }
            return PartialView(transports);
        }

        [HttpPost]

        public ActionResult AjaxUpdateCoupon(string couponCode)
        {
            var info = new
            {
                Status = 0,
                Msg = "Coupon không tồn tại hoặc đã hết hạn dùng.!"
            };

            if (UpdateCoupon(couponCode))
            {
                info = new
                {
                    Status = 1,
                    Msg = "Update thành công!"
                };
            }

            return Json(info, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getOrderInfo()
        {
            var info = new
            {
                TransportCost = cart.TransportCost,
                Discount = cart.Discount,
                TotalWeight = cart.TotalWeight,
                DiscountDescription = cart.discountDescription,
                OrderTotal = cart.OrderTotal,
                OverWeightCost = cart.OverWeight,
                //Get update phí dịch vụ khi đăng nhập
                FederalTaxUS = cart.FederalTaxUS,
                TaxExportVN = cart.TaxExportVN,
                ShippingInLandUS = cart.ShippingInLandUS,
                AFFeeVN = cart.AFFeeVN,
                TECSServicesFeeVN = cart.TECSServicesFeeVN,
                TransactionFeeVN = cart.TransactionFeeVN
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }


    }
}