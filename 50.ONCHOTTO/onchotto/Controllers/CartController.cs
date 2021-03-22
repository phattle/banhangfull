using System.Linq;
using System.Web.Mvc;
using OnChotto.Models;

namespace OnChotto.Controllers
{
    public class CartController : BaseController
    {
        public ActionResult Index()
        {
            if (ShoppingCart.Cart.Count == 0)
            {
                Warning(string.Format("<b><h5>{0}</h4></b>", "Bạn chưa có sản phẩm nào trong giỏ hàng, Vui lòng chọn sản phẩm trước khi thanh toán."), true);
                return RedirectToAction("Index", "Home");
            }

            var cart = ShoppingCart.Cart;
            return View(cart.Items);
            
        }
        public ActionResult OrderDetail()
        { 
            var cart = ShoppingCart.Cart;
            return PartialView("Partials/_OrderDetail", cart.Items);
        }
        

        public ActionResult PartialCart()
        {
            var cart = ShoppingCart.Cart;
            return PartialView("Partials/_PartialCart", cart.Items);

        }

        public ActionResult Add(int id, int soluong)
        {
            
            var cart = ShoppingCart.Cart;
            cart.Add(id, soluong);
            var p = cart.Items.Single(i => i.Id == id);
            
            var info = new { cart.Count, cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            var cart = ShoppingCart.Cart;
            cart.Remove(id);

            var info = new { cart.Count, cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        // Cập nhật số lượng trong đơn hàng đặt Order
        public ActionResult Update(int id, int quantity)
        {
            var cart = ShoppingCart.Cart;
            cart.Update(id, quantity);
            var p = cart.Items.Single(i => i.Id == id); // Duyệt lấy id sản phẩm thông qua ShoppingCart.cs
            var info = new
            {
                cart.Count,
                cart.Total,
                cart.ShippingInLandUS,
                cart.AFFeeVN,
                cart.TECSServicesFeeVN,
                cart.TransactionFeeVN,
                quantity=quantity,
                Amount = p.PriceAfter  * (decimal?)p.Amount,
                Weightnet = p.WeightLbs *  (decimal ?) p.Amount,
                FederalTaxUS = (p.PriceAfter * ShoppingCart.Cart.Federaltaxpercentage) * (decimal?)p.Amount,
                TaxExportVN = ((p.PriceAfter * ShoppingCart.Cart.Taxpercentage) + ((p.PriceAfter + (p.PriceAfter * ShoppingCart.Cart.Taxpercentage)) * ShoppingCart.Cart.Vatpercentage)) * (decimal?)p.Amount,
                //ShippingInLandUS = (p.PriceAfter * p.Amount) * 0.01M,
                //AFFeeVN = (p.WeightKG * p.Amount) * 3.2M,
                //TECSServicesFeeVN = (p.WeightKG * p.Amount) * 6000,

            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Clear()
        {
            
            var cart = ShoppingCart.Cart;
            cart.Clear();
            return RedirectToAction("Index");
        }
    }
}