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
using System.Collections.Generic;
using System.Globalization;
using PagedList;


namespace OnChotto.Controllers
{
    public class StoreDiffController : BaseController
    {

        public StoreDiffCart cart = StoreDiffCart.Cart;

        [HttpGet]
        public ActionResult Index()
        {
            //if (cart.Count == 0)
            //{
            //    Warning(string.Format("<b><h5>{0}</h4></b>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
            //    return RedirectToAction("Index", "Home");
            //}

            ViewBag.OtherProvinceId = ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull");
            ViewBag.OtherDistrictId = ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "-1").Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull");

            var model = new OrderDiffViewModel();

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
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(OrderDiffViewModel modelVm, FormCollection form)
        {
            var sms = new SpeedSMSAPI();
            int rs = 0;
            //Lấy formcollection dữ liệu từ client ////////////////
            var productLinks = form["ProductLink"];
            var orderNo = form["OrderNo"];
            var orderTrackingNo = form["OrderTrackingNo"];
            var storeName = form["StoreName"];
            var productName = form["ProductName"];
            var size = form["Size"];
            var amount = form["Amount"];            
            var weight = form["Weight"];
            var price = form["Price"];
            var discount = form["Discount"];
            var priceAfter = form["PriceAfter"];
            var note = form["Note"];
            var productStatus = form["ProductStatus"];
            var weightUnit = form["WeightUnit"];
            var currency = form["Currency"];
            var transtype = form["TransType"];

            if (string.IsNullOrEmpty(productLinks))
            {
                ModelState.AddModelError("ProductLinks", "Bạn cần phải nhập tối thiểu 1 link sản phẩm");
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //Cắt các dữ liệu (*các dữ liệu được phân cách bằng dấu ',' *) ////////////////////////////////////
            string[] productLinksArr = productLinks.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] orderNosArr = orderNo.Split(',').ToArray();
            string[] orderTrackingNoArr = orderTrackingNo.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] storeNameArr = storeName.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] productNameArr = productName.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] sizeArr = size.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] amountArr = amount.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] weightArr = weight.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] priceArr = price.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] discountArr = discount.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] priceAfterArr = priceAfter.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] noteArr = note.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] weUnit = weightUnit.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] curr = currency.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] trans = transtype.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //string[] productStatusArr = productStatus.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //Đẩy dữ liệu vào list//////////////////////////////////////////////////////
            List<string> listamountArr = new List<string>(amountArr);
            List<string> listweightArr = new List<string>(weightArr);
            List<string> listpriceArr = new List<string>(priceArr);
            List<string> listdiscountArr = new List<string>(discountArr);
            List<string> listpriceAfterArr = new List<string>(priceAfterArr);
            List<string> listweightUnitArr = new List<string>(weUnit);
            List<string> listcurrencyArr = new List<string>(curr);
            List<string> listtransArr = new List<string>(trans);
            ////////////////////////////////////////////////////////////////////////////
            ////Dữ liệu bao gồm div templete nên nếu có sản phẩm cần remove dữ liệu div templete ////

            if (listamountArr.Count > 1)
            {
                listamountArr.RemoveAt(1);
            }
            if (listweightArr.Count > 1)
            {
                listweightArr.RemoveAt(1);
            }
            if (listpriceArr.Count > 1)
            {
                listpriceArr.RemoveAt(1);
            }
            if (listdiscountArr.Count > 1)
            {
                listdiscountArr.RemoveAt(1);
            }
            if (listpriceAfterArr.Count > 1)
            {
                listpriceAfterArr.RemoveAt(1);
            }
            if (listweightUnitArr.Count > 1)
            {
                listweightUnitArr.RemoveAt(1);
            }
            if (listcurrencyArr.Count > 1)
            {
                listcurrencyArr.RemoveAt(1);
            }
            if (listtransArr.Count > 1)
            {
                listtransArr.RemoveAt(1);
            }
            ///////////////////////////////////////////////////////////////////////////
            if (ModelState.IsValid && string.IsNullOrEmpty(modelVm.UserId))
            {
                var password = Xstring.GeneratePassword();
                var newUser = new ApplicationUser
                {
                    FullName = modelVm.ReceiveName,
                    UserName = modelVm.ReceiveEmail,
                    Email = modelVm.ReceiveEmail,
                    PhoneNumber = modelVm.ReceivePhone,
                    //DistrictId = modelVm.DistrictId,
                    Address = modelVm.ReceiveAddress,
                    PasswordHash = password,
                };
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var item = await userManager.FindByEmailAsync(newUser.Email);
                if (item == null)
                {
                    await userManager.CreateAsync(newUser, password);
                }
                item = await userManager.FindByEmailAsync(newUser.Email);
                if (item.Email.Length > 0)
                {
                    var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                    await signInManager.SignInAsync(item, isPersistent: false, rememberBrowser: false);
                    modelVm.UserId = item.Id;

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

                    ModelState.AddModelError("", @"Hệ thống quá tải. không tạo được người dùng !");
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
                   
                    //user.DistrictId = cart.Transport.DistrictId;
                    db.Entry(user).State = EntityState.Modified;
                }

                //Update order info 
                var model = new OrderDiff();
                model.UpdateDataFrom(modelVm);
                model.IsDeposit = modelVm.IsDeposit;
                model.DistrictId = modelVm.DistrictId;
                model.ProvinceId = modelVm.ProvinceId;
                model.PaymentMethodId = modelVm.PaymentMethodId;
                model.MAWB = "";
                model.OrderDate = DateTime.Now;
                model.RequireDate = DateTime.Now.AddDays(20);
                /*model.ShippingInLand = modelVm.ShippingInLand;*///Phí
                model.StatusId = 1;
                //model.OrderStatus
                //Update thông tin người nhận hàng
                if (modelVm.OtherAddress)
                {
                    model.ReceiveName = modelVm.OtherReceiveName;
                    model.ReceiveEmail = modelVm.OtherEmail;
                    model.ReceivePhone = modelVm.OtherReceivePhone;
                    model.ReceiveAddress = modelVm.OtherReceiveAddress;
                }
                db.OrderDiffs.Add(model);
                rs = db.SaveChanges();
                int id = model.Id;
                decimal totalAm = 0;
                decimal totalWei = 0;
                try
                {

                    for (int i = 0; i < productLinksArr.Length; i++)
                    {

                        if (modelVm.ProductLink.Length > 0)
                        {

                            var dis = listdiscountArr.Count;
                            int l = listpriceAfterArr.Count;
                            var priceOr = listpriceArr.Count;
                            var d = new OrderDetailDiff
                            {
                                OrderDiffId = model.Id,
                                OrderNo = orderNosArr[i],
                                OrderTrackingNo = orderTrackingNoArr[i],
                                ProductLink = productLinksArr[i],
                                StoreName = storeNameArr[i],
                                ProductName = productNameArr[i],
                                Size = sizeArr[i],
                                Amount = Convert.ToInt32(amountArr[i]),
                                Weight = Convert.ToDecimal(weightArr[i], System.Globalization.CultureInfo.InvariantCulture),
                                Price = (priceOr != 0)?Convert.ToDecimal(priceArr[i], System.Globalization.CultureInfo.InvariantCulture) : 0,
                                Discount = (dis!=0)?Convert.ToDecimal(discountArr[i], System.Globalization.CultureInfo.InvariantCulture) : 0,
                                PriceAfter = (l != 0)?Convert.ToDecimal(priceAfterArr[i], System.Globalization.CultureInfo.InvariantCulture) : 0,
                                Note = modelVm.Notes,
                                ProductStatus = modelVm.ProductStatus,
                                WeightUnit = listweightUnitArr[i],
                                Currency = listcurrencyArr[i],
                                TransType = listtransArr[i],
                            };
                            totalAm += (Convert.ToDecimal(amountArr[i]) * ((l != 0) ? Convert.ToDecimal(priceAfterArr[i], System.Globalization.CultureInfo.InvariantCulture) : 0));
                            totalWei += (decimal)d.Amount * (decimal)d.Weight /*Convert.ToDecimal(weightArr[i], System.Globalization.CultureInfo.InvariantCulture)*/;
                            db.OrderDetailDiff.Add(d);
                        }
                    }
                    model.TotalWeight = totalWei;
                    model.TotalAmount = totalAm;
                    modelVm.TotalWeight = totalWei;
                    modelVm.TotalAmount = totalAm;
                    modelVm.Note = model.Note;
                    //Lưu total oderdiff amount và weight////////////////////
                    var orderdif = db.OrderDiffs.Where(p => p.Id == id);
                    orderdif.FirstOrDefault().TotalAmount = totalAm;
                    orderdif.FirstOrDefault().TotalWeight = totalWei;
                    /////////////////////////////////////////////////////////
                    rs += db.SaveChanges();
                    if (rs > 0)
                    {
                        //cart.Clear();
                        Success(string.Format("<b><h5>{0}</h4></b>", "Gửi đơn hàng thành công, chúng tôi sẽ liên hệ lại với bạn để xác nhận giá mua ship hộ đơn hàng này. Trân trọng phục vụ!"), true);

                        //Gửi SMS xác nhận và báo tin cho Sale
                        var customerMsg = $"OnChotto TB: gui thanh cong MaDH:#{model.Id}, Tong Tien: {model.TotalAmount:0,0}";
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
                        Msg += "<br>Đơn hàng MaDH:#" + model.Id + ", Với số tiền: " + string.Format("{0:0,0}", model.TotalAmount);
                        Msg += "<br>Khách hàng: " + model.ReceiveName;
                        Msg += "<br>Địa chỉ: " + model.ReceiveAddress;
                        Msg += "<br>Điện thoại: " + model.ReceivePhone;
                        Msg += "<br>Vui lòng liên hệ với khách hàng để xác nhận";
                        Msg += "<br>Chúc Bạn một ngày tốt lành, tràn đầy năng lượng";
                        Msg += "<p></p><p></p>-BQT OnChotto!.</p>";

                        XMail.Send("cskh@onchotto.com", Subject, Msg);

                        //Gửi mail tài khoản cho khách hàng
                        modelVm.UserId = user.Id;
                        var subject = "Thông báo gửi đơn hàng thành công!";
                        var msg = "Xin chào, " + model.ReceiveName;
                        msg += "<br>OnChotto thông báo: Gửi đơn hàng thành công MaDH:#" + model.Id + ", Với số tiền: " + string.Format("{0:0,0}", model.TotalAmount);
                        msg += "<br>Bạn đăng nhập tài khoản tại: <a href='http://onchotto.com/Account/Login'>onchotto.com</a> để kiểm tra danh sách tình trạng đơn hàng";
                        msg += "<br>Cảm ơn Bạn đã quan tâm sử dụng dịch vụ của OnChotto. mọi thắc mắc xin liên hệ hotline: 1800-545480 - 0903.896741.";
                        msg += "<br>OnChotto hân hạnh được phục vụ Bạn.";
                        msg += "<br>Chúc Bạn một ngày tốt lành.";
                        msg += "<p></p><p></p>-BQT OnChotto!.</p>";
                        XMail.Send(user.Email, subject, msg);
                        return RedirectToAction("Detail", new { id = model.Id });
                    }

                }
                catch (Exception ex)
                {
                    Danger($"-{ex.Message}<br>", true);
                    ModelState.AddModelError("", ex.InnerException);
                }
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }

            ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name })
                , "ProvinceId", "NameFull", "01");
            ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "001").Select(x => new
            {
                DistrictId = x.DistrictId,
                NameFull = x.Type + " " + x.Name
            }), "DistrictId", "NameFull", "001");
            ViewBag.OtherProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }),
                "ProvinceId", "NameFull", modelVm.OtherProvinceId);
            ViewBag.OtherDistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == modelVm.OtherProvinceId).Select(x => new
            {
                DistrictId = x.DistrictId,
                NameFull = x.Type + " " + x.Name
            }), "DistrictId", "NameFull", modelVm.OtherDistrictId);

            return View(modelVm);
        }


        public ActionResult Detail(int id)
        {
            List<OrderDiffViewModel> lst = new List<Models.ViewModel.OrderDiffViewModel>();
            var user = db.Users.Find(User.Identity.GetUserId());
            var order = (from p in db.OrderDiffs
                         join d in db.OrderDetailDiff on p.Id equals d.OrderDiffId
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
                             Note = p.Note,
                             DetailID = d.Id,
                             ProductStatus = d.ProductStatus,
                             WeightUnit = d.WeightUnit,
                             Currency = d.Currency
                         });
            foreach (var item in order)
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
                // it.Ispayenough = item.Ispayenough;
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
                //it.DetailID = item.Id;
                it.ProductStatus = item.ProductStatus;
                it.WeightUnit = item.WeightUnit;
                it.Currency = item.Currency;    
                it.OtherReceiveName = user.UserName;
                it.OtherReceivePhone = user.PhoneNumber;
                it.OtherReceiveAddress = user.Address;
                it.OtherEmail = user.Email;
                lst.Add(it);
            }
            ViewBag.Total = order.FirstOrDefault().TotalAmount;
            return View(lst);
        }

        public ActionResult List(int? page)
        {
            string currentUserId = User.Identity.GetUserId();
            List<OrderDiffViewModel> lst = new List<Models.ViewModel.OrderDiffViewModel>();
            var order = (from p in db.OrderDiffs
                         join d in db.OrderDetailDiff on p.Id equals d.OrderDiffId
                         join s in db.OrderStatus on p.StatusId equals s.Id
                         select new
                         {
                             Id = p.Id,
                             DistrictId = p.DistrictId,
                             ProvinceId = p.ProvinceId,
                             OrderDate = p.OrderDate,
                             TotalWeight = p.TotalWeight,
                             TotalAmount = p.TotalAmount,
                             StatusId = p.StatusId,
                             ProductLink = d.ProductLink,
                             ProductName = d.ProductName,
                             Status = s.Name,
                             PriceAfter = d.PriceAfter,
                             WeightUnit = d.WeightUnit,
                             Currency = d.Currency,
                             Amount = d.Amount


                         });
            foreach (var item in order)
            {
                OrderDiffViewModel it = new OrderDiffViewModel();
                it.OrderDiffId = item.Id;
                it.OrderDate = item.OrderDate;
                it.TotalAmount = item.TotalAmount;
                it.ProductLink = item.ProductLink;
                it.ProductName = item.ProductName;
                it.OrderStatusName = item.Status;
                it.PriceAfter = item.PriceAfter.ToString();
                it.WeightUnit = item.WeightUnit.ToString();
                it.Currency = item.Currency.ToString();
                it.ProductLink = item.ProductLink.ToString();
                it.Amount = item.Amount;
                lst.Add(it);
            }
            //var orders = db.Orders.Where(o => o.UserId == currentUserId).ToList();
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(lst.ToPagedList(pageNumber, pageSize));
        }


        //public bool UpdateTransport(int transportId)
        //{
        //    var transport = db.Transports.Find(transportId);
        //    if (transport == null)
        //    {
        //        return false;
        //    }

        //    //Update cart Transport
        //    cart.UpdateTransport(transport);

        //    return true;
        //}


        //public bool UpdateCoupon(string code)
        //{
        //    var coupon = db.Coupons.Find(code);
        //    if (coupon == null)
        //    {
        //        return false;
        //    }
        //    //Update cart Transport
        //    cart.UpdateCoupon(coupon);

        //    return true;
        //}


        //public ActionResult AjaxGetTransport(string districtId)
        //{
        //    var transports = db.Transports.Where(t => t.DistrictId == districtId).ToList();
        //    if (transports.Count() > 0)
        //    {
        //        UpdateTransport(transports.First().Id);
        //    }
        //    return PartialView(transports);
        //}

        //[HttpPost]

        //public ActionResult AjaxUpdateCoupon(string couponCode)
        //{
        //    var info = new
        //    {
        //        Status = 0,
        //        Msg = "Coupon không tồn tại hoặc đã hết hạn dùng.!"
        //    };

        //    if (UpdateCoupon(couponCode))
        //    {
        //        info = new
        //        {
        //            Status = 1,
        //            Msg = "Update thành công!"
        //        };
        //    }

        //    return Json(info, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult getOrderInfo()
        //{
        //    var info = new
        //    {
        //        TransportCost = cart.TransportCost,
        //        Discount = cart.Discount,
        //        TotalWeight = cart.TotalWeight,
        //        DiscountDescription = cart.discountDescription,
        //        OrderTotal = cart.OrderTotal
        //    };
        //    return Json(info, JsonRequestBehavior.AllowGet);
        //}
    }
}