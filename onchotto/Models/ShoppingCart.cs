namespace OnChotto.Models
{
    using Commons;
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ShoppingCart
    {
        // Chứa các mặt hàng đã chọn
        public List<Product> Items = new List<Product>();

        public int Count { get; set; }

        public decimal? Total { get; set; }

        public decimal? TotalWeight
        {
            get
            {
                decimal iTotal = 0;
                foreach (var item in Items)
                {
                    iTotal += DataHelper.CorrectValue(item.WeightKG * item.Amount, decimal.Zero);
                }
                return iTotal;
            }
        }

        public string WeightUnit { get; set; }

        public decimal? Distance { get; set; }

        public Coupon Coupon { get; set; }

        public Transport Transport { get; set; }

        public ProductTaxHscode ProductTaxHscode { get; set; }

        public decimal? Federaltaxpercentage // Thuế bang % theo product
        {
            get
            {
                decimal fedetaxper = 0;
                var db = new ApplicationDbContext();
                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var Federaltaxpercentages = db.ProductTaxHscodes.Find(itemp.HsCodeId).FederalTaxPercentage;
                        //var dProductTaxHscodes = new ApplicationDbContext().ProductTaxHscodes.Find(itemp.HsCodeId).TaxPercentage;
                        fedetaxper = DataHelper.CorrectValue(Federaltaxpercentages, decimal.Zero) / 100;
                    }
                }
                return fedetaxper;
            }
        }

        public decimal? FederalTaxUS // Thuế bang theo product
        {
            get
            {
                decimal fedfee = 0;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                        fedfee += DataHelper.CorrectValue(((itemp.PriceAfter * Federaltaxpercentage) * itemp.Amount), decimal.Zero);
                    
                }
                return fedfee;
            }
        }
        public decimal? TaxExportVN // Phí nhập khẩu theo product
        {
            get
            {
                decimal TaxEVN = 0;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                        TaxEVN += DataHelper.CorrectValue(((itemp.PriceAfter * Taxpercentage) + ((itemp.PriceAfter + (itemp.PriceAfter * Taxpercentage)) * Vatpercentage)) * itemp.Amount, decimal.Zero);
                }
                return TaxEVN;
            }
        }

        public decimal? Taxpercentage // Thuế xuất % phí nhập khẩu / Sản phẩm
        {
            get
            {
                decimal Taxper = 0;
                var db = new ApplicationDbContext();
                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var Taxpercentages = db.ProductTaxHscodes.Find(itemp.HsCodeId).TaxPercentage;
                        Taxper = DataHelper.CorrectValue(Taxpercentages, decimal.Zero) / 100;
                    }

                }
                return Taxper;
            }
        }

        public decimal? Vatpercentage // Thuế VAT %  cho phí nhập khẩu phí nhập khẩu / Sản phẩm
        {
            get
            {
                decimal Vatperper = 0;
                var db = new ApplicationDbContext();
                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var Vatpercentages = db.ProductTaxHscodes.Find(itemp.HsCodeId).VATPercentage;
                        Vatperper = DataHelper.CorrectValue(Vatpercentages, decimal.Zero) / 100;
                    }

                }
                return Vatperper;
            }
        }

        public decimal? ShippingInLandUS // Phí vận chuyển bang US
        {
            get
            {
                decimal shfee = 0, sshipppinginlandus = 0;
                var db = new ApplicationDbContext();
                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var shipppinginlandus = db.ProductTaxHscodes.Find(itemp.HsCodeId).ShippinglandPercentage;
                        sshipppinginlandus = DataHelper.CorrectValue(shipppinginlandus, decimal.Zero) / 100;
                        shfee += DataHelper.CorrectValue(itemp.PriceAfter * itemp.Amount, decimal.Zero) * sshipppinginlandus;
                    }
                }
                return shfee;
            }
        }
        public decimal? HandlingFeeUS  // Phí (sort, pack, label...) US/HAWB

        {
            get
            {
                decimal handfee = 0, hhandfee = 0;
                var db = new ApplicationDbContext();
                string strProductZone = string.Empty;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                    {
                        strProductZone = itemp.ProductZone;
                        break;
                    }
                }
                if (strProductZone != string.Empty)
                {
                    var handlingfeeus = db.ProductTaxHscodes.Single(x => x.HsCodeId == 1).Pricehandling;
                    hhandfee = DataHelper.CorrectValue(handlingfeeus, decimal.Zero);
                    handfee += DataHelper.CorrectValue(hhandfee * Commons.DataHelper.CurrRank, decimal.Zero);
                }
                //foreach (var itemp in Items)
                //{
                //    var handlingfeeus = db.ProductTaxHscodes.Find(itemp.HsCodeId).Pricehandling;
                //    hhandfee = DataHelper.CorrectValue(handlingfeeus, decimal.Zero);
                //    handfee += DataHelper.CorrectValue(hhandfee * Commons.DataHelper.CurrRank, decimal.Zero);
                //}

                return handfee;
            }

        }
        public decimal? AFFeeVN // Phí vận chuyển hàng hàng không US --> VN
        {
            get

            {
                decimal afee = 0, aaffeevn = 0;
                var db = new ApplicationDbContext();

                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var affeevn = db.ProductTaxHscodes.Find(itemp.HsCodeId).PriceAF;
                        aaffeevn = DataHelper.CorrectValue(affeevn, decimal.Zero);
                        afee += DataHelper.CorrectValue((itemp.WeightKG * itemp.Amount) * (aaffeevn * Commons.DataHelper.CurrRank), decimal.Zero);
                    }
                }

                return afee;
            }

        }
        public decimal? ClearanceFeeVN // Phí thông quan / HAWB
        {
            get
            {
                var db = new ApplicationDbContext();
                decimal cleafee = 0, ccleafee = 0;
                string strProductZone = string.Empty;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                    {
                        strProductZone = itemp.ProductZone;
                        break;
                    }
                }
                if (strProductZone != string.Empty)
                {
                    var clearancefeevn = db.ProductTaxHscodes.Single(x => x.HsCodeId == 1).PriceClearanceFee;
                    ccleafee = DataHelper.CorrectValue(clearancefeevn, decimal.Zero);
                    cleafee += DataHelper.CorrectValue(ccleafee * Commons.DataHelper.CurrRank, decimal.Zero);
                }
                    
                //foreach (var itemp in Items)
                //{
                //    var clearancefeevn = db.ProductTaxHscodes.Find(itemp.HsCodeId).PriceClearanceFee;
                //    ccleafee = DataHelper.CorrectValue(clearancefeevn, decimal.Zero);
                //    cleafee += DataHelper.CorrectValue(ccleafee * Commons.DataHelper.CurrRank, decimal.Zero);
                //}
                return cleafee;
            }
        }
        public decimal? TECSServicesFeeVN //Phí lao vụ
        {
            get
            {
                var db = new ApplicationDbContext();
                decimal tefee = 0, tecs = 0;
                foreach (var itemp in Items)
                {
                    if (itemp.HsCodeId != null)
                    {
                        var tecsservicesfeevn = db.ProductTaxHscodes.Find(itemp.HsCodeId).PriceTECSServicesFee;
                        tecs = DataHelper.CorrectValue(tecsservicesfeevn, decimal.Zero);
                        tefee += DataHelper.CorrectValue((itemp.WeightKG * itemp.Amount) * (tecs), decimal.Zero);
                    }
               }
                return tefee;
            }
        }

        /*public decimal? TransactionFee { get; set; }*/ // Phí xử lý giao dịch
        public decimal? CustomFeeVN // Lệ phí Hải quan
        {

            get
            {
                var db = new ApplicationDbContext();
                decimal cusfee = 0, cusfees = 0;
                string strProductZone = string.Empty;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                    {
                        strProductZone = itemp.ProductZone;
                        break;
                    }
                }
                if(strProductZone != string.Empty)
                {
                    var customfeevnw = db.ProductTaxHscodes.Single(x => x.HsCodeId == 1).PriceCustomFee;
                    cusfee = DataHelper.CorrectValue(customfeevnw, decimal.Zero);
                    cusfees += DataHelper.CorrectValue(cusfee, decimal.Zero);
                }
                
                //foreach (var itemp in Items)
                //{
                //    var customfeevn = db.ProductTaxHscodes.Find(itemp.HsCodeId).PriceCustomFee;
                //    cusfee += DataHelper.CorrectValue(customfeevn, decimal.Zero);
                //}
                return cusfees;
            }
        }

        public decimal? TransactionFeeVN // Phí xử lý giao dịch theo rule deposit ứng 50% thì tính 2% trên đơn hàng và nhân cho VAT 1.1
        {
            get
            {
                var db = new ApplicationDbContext();
                decimal trafee = 0, tranfevn = 0, vattranfevn = 0;
                string strProductZone = string.Empty;
                foreach (var itemp in Items)
                {
                    if (itemp.ProductZone == "US")
                    {
                        strProductZone = itemp.ProductZone;
                        break;
                    }
                }
                if (strProductZone != string.Empty)
                {
                    foreach (var itemp in Items)
                    {
                        var transfee = db.ProductTaxHscodes.Single(x => x.HsCodeId == 1).TransactionPercentage;
                        var transfeevat = db.ProductTaxHscodes.Single(x => x.HsCodeId == 1).VATFeeTransaction;
                        trafee = DataHelper.CorrectValue(transfee, decimal.Zero) / 100;
                        vattranfevn = 0;
                    }
                        
                }
                return tranfevn;
            }
        }

        // Lấy giỏ hàng từ Session
        public static ShoppingCart Cart
        {
            get
            {
                var cart = HttpContext.Current.Session["Cart"] as ShoppingCart;
                // Nếu chưa có giỏ hàng trong session -> tạo mới và lưu vào session
                if (cart == null)
                {
                    cart = new ShoppingCart();
                    cart.Count = 0;
                    cart.Total = 0;
                    HttpContext.Current.Session["Cart"] = cart;
                }
                return cart;
            }
        }

        public string CouponCode
        {
            get
            {
                if (Coupon != null)
                {
                    return Coupon.Code;
                }
                return "";
            }
        }

        public decimal Discount
        {
            get
            {
                var db = new ApplicationDbContext();
                if (Coupon == null)
                {
                    return 0;
                }

                switch (Coupon.DiscountFor)
                {
                    case DiscountObject.Product:
                        if (Coupon.DiscountForId.HasValue)
                        {
                            var product = db.Products.Find(Coupon.DiscountForId.Value);
                            if (product != null && Items.Any(x => x.Id == product.Id))
                            {
                                if (product.PriceAfter != null)
                                    return (decimal)product.PriceAfter.Value * Coupon.Discount / 100;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                        break;
                    case DiscountObject.Order:
                        return (decimal)Total * Coupon.Discount / 100;
                    case DiscountObject.Transport:
                        return (decimal)Transport.Cost * Coupon.Discount / 100;

                }
                db.Dispose();
                return 0;
            }
        }

        public string discountDescription
        {
            get
            {
                if (Coupon == null)
                {
                    return "";
                }

                return "<br><small>(" + Coupon.Name + ")</small";
            }
        }

        public string CouponNameDescription
        {
            get
            {
                if (Coupon == null)
                {
                    return "";
                }
                return Coupon.Name;
            }
        }

        public decimal? OrderTotal
        {
            get
            {
                return (Total + TransportCost - Discount) + FederalTaxUS + ShippingInLandUS + HandlingFeeUS + AFFeeVN + TaxExportVN + ClearanceFeeVN + TECSServicesFeeVN + CustomFeeVN;
            }
        }

        public decimal? OverWeight
        {
            get
            {
                decimal overweight = 0;
                if(TotalWeight < 2)
                {
                    overweight = 0M;
                }
                else
                {
                    overweight = (decimal)TotalWeight - 2M;
                }
                return overweight;
            }
            

        }
        public decimal TransportCost
        {
            get
            {
                var db = new ApplicationDbContext();
                var cost = Transport == null ? 0 : Transport.Cost;
                decimal overweight = 0;
                //Nếu trọng lượng > 2 Kg theo rute tính CostSecondary
                if (Transport == null)
                    return db.Transports.FirstOrDefault().Cost.Value;
                else
                {
                    if (TotalWeight < 2)
                    {
                        cost = cost - 0;
                    }
                    else
                    {
                        overweight += (decimal)TotalWeight - 2M;
                        cost = cost + ((overweight / 0.5M) * DataHelper.CorrectValue(Transport.CostSecondary, decimal.Zero));
                    }

                    return DataHelper.CorrectValue(cost, decimal.Zero);
                }
               

                //var cost = Transport == null ? 0 : Transport.Cost;
                ////Nếu số tiền lớn hơn 300k hoặc số lượng mua từ 3 sản phẩm trở lên thì miễn ph9í vận chuyển
                //if (Total > 300000 || Count > 2)
                //{
                //    cost = cost - 0;
                //}

                //return cost < 0 ? 0 : cost;
            }
        }
        public void Add(int id, int soluong) // Thêm Id product + So Luong và Tổng tiền Trên giỏ hàng
        {
            var db = new ApplicationDbContext();
            Product item = null;
            try // tìm thấy trong giỏ -> tăng số lượng lên 1
            {
                item = Items.Single(i => i.Id == id);
                item.Amount = item.Amount + soluong;
            }
            catch // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
            {

                item = db.Products.Find(id);
                item.Amount = soluong;

                Items.Add(item);
            }
            Total += (item.PriceAfter * soluong);
            Count += soluong;
            db.Dispose();
        }

        public void Remove(int id) // Xóa sản phẩm trong giỏ hàng
        {
            var item = Items.Single(i => i.Id == id);
            Total -= item.PriceAfter * item.Amount;
            Count -= item.Amount;
            Items.Remove(item);
        }
        // Update thay đổi số lượng trên giỏ hàng
        public void Update(int id, int newQuantity)
        {
            var item = Items.Single(i => i.Id == id);
            Total += item.PriceAfter * (newQuantity - item.Amount);
            Count += newQuantity - item.Amount;
            item.Amount = newQuantity;
        }

        public void UpdateCoupon(Coupon coupon)
        {
            Coupon = coupon;
        }

        public void UpdateTransport(Transport transport)
        {
            Transport = transport;
        }

        public int getQuantity(int id)
        {
            var item = Items.Single(i => i.Id == id);
            return item.Amount;
        }

        public void Clear()
        {
            Count = 0;
            Total = 0;
            Items.Clear();
        }

    }
}
