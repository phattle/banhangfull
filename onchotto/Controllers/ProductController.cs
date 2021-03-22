using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OnChotto.Models.Dao;
using System.Net;
using OnChotto.Models.Entities;
using HtmlAgilityPack;
using System.Data;
using OnChotto.Commons;

namespace OnChotto.Controllers
{
    public class ProductController : BaseController
    {
        //int pageSize = 20;
        public ProductCategoryDao dao = new ProductCategoryDao();

        public List<Product> Products
        {
            get
            {
                // Lấy ra từ Session
                var list = Session["Products"] as List<Product>;
                if (list == null) // chưa có trong Session
                {
                    list = new List<Product>();
                    Session["Products"] = list; // bỏ vào Session
                }
                return list;
            }
        }

        public ActionResult Index()
        {
            var product = db.Products.ToList();
            return View(product);
        }

        public ActionResult Cat(int Id, int? page)
        {
            int pageNumber = (page ?? 1);

            var cat = db.ProductCategories.Find(Id);
            if (cat == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            ViewBag.Category = cat;
            ViewBag.Group = Id.ToString();

            int iCatId = 0;
            int ParentId = 0;
            string url = Request.Url.AbsoluteUri;
            try
            {
                url = url.Substring(url.LastIndexOf("-"));
                url = url.Replace("-", string.Empty);
                iCatId = DataHelper.CorrectValue(url, int.MinValue);

            }
            catch { }
            var model = db.ProductCategories.Where(c => c.ParentId == iCatId).OrderBy(c => c.DisplayOrder);
            if (model.Count() == 0)
            {
                ParentId = DataHelper.CorrectValue(db.ProductCategories.Where(b => b.CatId == iCatId).Select(b => b.ParentId).FirstOrDefault().ToString(), int.MinValue);
                if (ParentId > 0)// hiển thị menu theo gift hàng nhật hàng út
                    model = db.ProductCategories.Where(c => c.ParentId == ParentId).OrderBy(c => c.DisplayOrder);
                else // hiển thị menu theo tất cả categories
                    model = db.ProductCategories.Where(c => c.CatId == iCatId).OrderBy(c => c.DisplayOrder);
            }
            return View(model);

        }

        public ActionResult DetailSearch(string ASIN)
        {
            GetDataByAsin(ASIN);
            var model = (from s in db.Products.ToList()
                         where (s.ASIN.Equals(ASIN))
                         select s).Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate).FirstOrDefault();
            if (model == null)
            {

                return RedirectToAction("NotFound", "Home");
            }
            model.Views++;
            db.SaveChanges();
            ViewBag.PriceOff = model.Price * (100 - model.Discount) / 100;

            //san pham cung chuyen muc
            ViewBag.RelatedProducts = model.ProductCategory.Products.Where(p => p.Actived == true).Where(p => p.Id != model.Id).Take(10);

            //san pham vua xem
            var list = Products;
            var m = list.SingleOrDefault(c => c.ASIN == ASIN);
            if (m == null)
            {
                list.Add(model);
            }

            if (list.Count > 10)
            {
                ViewBag.Views = list.Take(10);
            }
            else
            {
                ViewBag.Views = list;
            }

            ViewBag.PageHelp = db.Pages.SingleOrDefault(p => p.Id == 8);
            //ViewBag.Views = list;
            return View("Detail", model);
        }

        public ActionResult Detail(string ASIN, string Slug)
        {
            var model = db.Products.Where(p => p.Actived == true).SingleOrDefault(p => p.ASIN == ASIN || p.ParentASIN == ASIN);
            var productsize = db.ProductSize.Where(p => p.CatID == model.CatId).Select(p=>p.SizeCode).ToList();
            
            if (model == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            model.Views++;
            db.SaveChanges();
            ViewBag.PriceOff = model.Price * (100 - model.Discount) / 100;

            //san pham cung chuyen muc
            ViewBag.RelatedProducts = model.ProductCategory.Products.Where(p => p.Actived == true).Where(p => p.Id != model.Id).Take(10);

            //san pham vua xem
            var list = Products;
            var m = list.SingleOrDefault(p => p.ASIN == ASIN || p.ParentASIN == ASIN);
            if (m == null)
            {
                list.Add(model);
            }

            if (list.Count > 10)
            {
                ViewBag.Views = list.Take(10);
            }
            else
            {
                ViewBag.Views = list;
            }
            ViewBag.Size = productsize;
            ViewBag.PageHelp = db.Pages.SingleOrDefault(p => p.Id == 8);
            //ViewBag.Views = list;
            return View("Detail", model);
        }



        //public ActionResult Detail(int Id, string Slug)
        //{
        //    var model = db.Products.Where(p => p.Actived == true).SingleOrDefault(p => p.Id == Id);
        //    //var proAttr = db.ProductAttribute.Where(p => p.ProductId == Id).ToList();
        //    //var groupAttrList = proAttr.Select(p => p.GroupAtrributeId).ToList();
        //    ////groupAttribute = 1 => Size
        //    ////groupAttribute = 2 => Màu
        //    //foreach (var i in proAttr)
        //    //{

        //    //}

        //    if (model == null)
        //    {
        //        return RedirectToAction("NotFound", "Home");
        //    }
        //    model.Views++;
        //    db.SaveChanges();
        //    ViewBag.PriceOff = model.Price * (100 - model.Discount) / 100;

        //    //san pham cung chuyen muc
        //    ViewBag.RelatedProducts = model.ProductCategory.Products.Where(p => p.Actived == true).Where(p => p.Id != model.Id).Take(10);

        //    //san pham vua xem
        //    var list = Products;
        //    var m = list.SingleOrDefault(c => c.Id == Id);
        //    if (m == null)
        //    {
        //        list.Add(model);
        //    }

        //    if (list.Count > 10)
        //    {
        //        ViewBag.Views = list.Take(10);
        //    }
        //    else
        //    {
        //        ViewBag.Views = list;
        //    }

        //    ViewBag.PageHelp = db.Pages.SingleOrDefault(p => p.Id == 8);
        //    //ViewBag.Views = list;
        //    return View("Detail", model);
        //}



        public bool IsElementPresent(HtmlDocument driver, string by)
        {
            try
            {
                driver.GetElementbyId(by);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string Selection(HtmlDocument document)
        {
            string strSelection = string.Empty;
            try
            {
                HtmlNode node = document.DocumentNode.SelectNodes("//span[@id='variation_size_name']").First();
                strSelection = node.InnerText;

            }
            catch
            {
            }

            return strSelection;
        }
        public string DetailHTML(HtmlDocument document, string ID)
        {
            string Detail = string.Empty;
            try
            {
                if (IsElementPresent(document, ID))
                {
                    var nodes = document.DocumentNode.SelectNodes("//div[@id='" + ID + "']").First();
                    Detail = nodes.InnerHtml;
                }
                else if (IsElementPresent(document, ID))
                {
                    HtmlNode node = document.DocumentNode.SelectNodes("//div[@id='" + ID + "']").First();
                    Detail = node.InnerHtml;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return Detail;
        }

        public string productDescription(HtmlDocument document)
        {
            string Description = string.Empty;
            try
            {
                if (IsElementPresent(document, "productDescription"))
                {
                    var nodes = document.DocumentNode.SelectNodes("//div[@id='productDescription']").First();
                    Description = nodes.InnerHtml;
                }
                else if (IsElementPresent(document, "productDescription_feature_div"))
                {
                    var nodes = document.DocumentNode.SelectNodes("//div[@id='productDescription_feature_div']").First();
                    Description = nodes.InnerHtml;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return Description;
        }

        public string GetAllaltImages(HtmlDocument document)
        {
            string imageSrc = string.Empty, curimg = "";
            List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//div[@id='altImages']").ToList();
            foreach (HtmlNode node in nodes)
            {
                var nodex = node.SelectNodes("//img/@src");
                foreach (var item in nodex)
                {
                    curimg = item.GetAttributeValue("src", "");
                    if (curimg.EndsWith("SS40_.jpg"))
                    {
                        if (imageSrc.Length <= 1)
                        {
                            imageSrc = curimg.Replace("SS40_.jpg", "XL1500_.jpg");
                        }
                        else
                            imageSrc = curimg.Replace("SS40_.jpg", "XL1500_.jpg") + "," + imageSrc;
                    }
                }
            }
            return imageSrc;
        }

        public string detailBullets(HtmlDocument document)
        {
            string Shipping = string.Empty;
            try
            {
                var nodes = document.DocumentNode.SelectNodes("//table[@id='productDetails_detailBullets_sections1']//tr").ToList();
                foreach (HtmlNode item in nodes)
                {
                    if (item.InnerText.StartsWith("Shipping Weight"))
                    {
                        Shipping = item.InnerText.Substring(item.InnerText.IndexOf("Shipping Weight"), item.InnerText.IndexOf("pounds"));
                    }
                }
                return Shipping;
            }
            catch { return "0"; }
        }
        public string Dimensions(HtmlDocument document)
        {
            string Dimensions = string.Empty;
            try
            {
                if (IsElementPresent(document, "productDetails_detailBullets_sections2"))
                {
                    var nodes = document.DocumentNode.SelectNodes("//table[@id='productDetails_detailBullets_sections1']//tr").ToList();
                    foreach (HtmlNode item in nodes)
                    {
                        if (item.InnerText.StartsWith("Package Dimensions"))
                        {
                            Dimensions = item.InnerText.Substring(item.InnerText.IndexOf("Package Dimensions"), item.InnerText.Length);
                        }
                    }
                }
                else if (IsElementPresent(document, "productDetails_techSpec_section_1"))
                {
                    var nodes = document.DocumentNode.SelectNodes("//table[@id='productDetails_techSpec_section_1']//tr").ToList();
                    foreach (HtmlNode item in nodes)
                    {
                        if (item.InnerText.StartsWith("Package Dimensions"))
                        {
                            Dimensions = item.InnerText.Substring(item.InnerText.IndexOf("Package Dimensions"), item.InnerText.Length);
                        }
                    }
                }
                else if (IsElementPresent(document, "detailBullets_feature_div"))
                {
                    var nodes = document.DocumentNode.SelectNodes("//table[@id='detailBullets_feature_div']//tr").ToList();
                    foreach (HtmlNode item in nodes)
                    {
                        if (item.InnerText.StartsWith("Package Dimensions"))
                        {
                            Dimensions = item.InnerText.Substring(item.InnerText.IndexOf("Package Dimensions"), item.InnerText.Length);
                        }
                    }
                }
                return Dimensions;
            }
            catch { return "0"; }

        }


        private void GetDataByAsin(string aSIN)
        {
            OnChotto.Models.Dao.CaptureEvent ev = new OnChotto.Models.Dao.CaptureEvent();
            string strURL = string.Empty, CategoryName = "", KeywordSeach = "", SearchID = "";
            int CaptureID = 0;
            HtmlWeb web = new HtmlWeb();
            Product item = new Models.Entities.Product();
            try
            {


                if (ev.GetExitsASin(item.ASIN) <= 0)
                {
                    DataTable tbl = ev.GetCaptureByASIN(aSIN);
                    if (tbl != null)
                    {
                        try
                        {
                            CategoryName = tbl.Rows[0]["CategoryName"].ToString();
                            KeywordSeach = tbl.Rows[0]["KeywordSeach"].ToString();
                            SearchID = tbl.Rows[0]["SearchID"].ToString();
                            CaptureID = Common.CorrectValue(tbl.Rows[0]["CaptureID"], int.MinValue);
                        }
                        catch
                        {
                            CategoryName = "";
                            KeywordSeach = "";
                            SearchID = "0";
                            CaptureID = 0;
                        }


                    }
                    strURL = "https://www.amazon.com/dp/" + item.ASIN;
                    HtmlDocument document = web.Load(strURL);
                    item.DetailPageURL = strURL;
                    if (IsElementPresent(document, "titleSection"))
                    {
                        HtmlNode node = document.DocumentNode.SelectNodes("//div[@id='titleSection']").First();
                        string name = node.InnerText;
                        item.Name = name.Replace("\n", "").Trim();
                        item.MetaDescription = item.Name;
                        name = item.Name.Replace('/', ' ');
                        name = name.Replace('|', ' ');
                        name = name.Replace(':', ' ');
                        item.Slug = name.ToAscii();
                        item.MetaKeyword = KeywordSeach + "#" + CategoryName + "#" + SearchID + "#" + item.ASIN + "#" + item.Name;
                    }
                    if (IsElementPresent(document, "priceblock_ourprice"))
                    {
                        HtmlNode node = document.DocumentNode.SelectNodes("//span[@id='priceblock_ourprice']").First();
                        string price = node.InnerText.Substring(1, node.InnerText.Length - 1);
                        if (price.Length > 0)
                            item.Price = (decimal)Common.CorrectValue(price, double.MinValue);
                        item.Amount = Common.CorrectValue(item.Price, int.MinValue);
                    }
                    if (IsElementPresent(document, "variation_size_name"))
                    {
                        item.Size = Selection(document);
                    }
                    //Descption
                    if (IsElementPresent(document, "productDescription"))
                    {
                        item.Description = productDescription(document);
                    }
                    //Detail
                    if (IsElementPresent(document, "feature-bullets"))
                    {
                        item.Featured = DetailHTML(document, "feature-bullets");
                    }
                    //Detail
                    if (IsElementPresent(document, "productDetails_feature_div"))
                    {
                        item.Detail = DetailHTML(document, "productDetails_feature_div");
                    }
                    if (IsElementPresent(document, "landingImage"))
                    {//imgTagWrapperId
                        HtmlNode node = document.DocumentNode.SelectNodes("//img[@id='landingImage']").First();
                        string strzin = node.GetAttributeValue("data-old-hires", "/uploads/files/noimage.gif");
                        item.FeaturedImage = strzin;
                    }
                    //Link Image
                    if (IsElementPresent(document, "altImages"))
                    {
                        item.Images = GetAllaltImages(document);
                        if (item.FeaturedImage.Length < 1)
                            item.FeaturedImage = item.Images.Substring(0, item.Images.LastIndexOf(","));
                    }

                    if (IsElementPresent(document, "productDetails_detailBullets_sections1"))
                    {
                        try
                        {
                            string strWeight = detailBullets(document);
                            item.ChargeWeight = DataHelper.CorrectValue(strWeight, decimal.MinValue);
                            item.ChargeWeightKG = DataHelper.ConvertLbsToKgs(Common.CorrectValue(strWeight, decimal.MinValue));
                            item.ChargeWeightLbs = DataHelper.CorrectValue(strWeight, decimal.MinValue);

                        }
                        catch (Exception)
                        {
                            item.ChargeWeight = 0;
                            item.ChargeWeightKG = 0;
                            item.ChargeWeightLbs = 0;
                        }

                    }
                    if (IsElementPresent(document, "productDetails_detailBullets_sections1"))
                    {
                        item.Dimensions = Dimensions(document);
                    }
                    //productDetails_detailBullets_sections1
                    item.SearchID = SearchID;
                    item.Discount = 0;
                    item.EndDate = DateTime.Now.AddYears(1);
                    item.UserId = "72b39d66-279b-4c6c-a696-1a1c69659ada";
                    item.CreateDate = DateTime.Now;
                    item.ExtraDiscount = 0;
                    item.IsNew = true;
                    item.IsFeatured = true;
                    item.IsSpecial = true;
                    item.PriceAfter = item.Price;
                    item.Actived = true;
                    item.Condition = item.ASIN;
                    item.MetaTitle = item.Name;
                    item.LargeImageURL = item.FeaturedImage;
                    item.ProductZone = "US";
                    item.WeightUnit = "Lbs";
                    item.WeightKG = item.ChargeWeightKG;
                    item.WeightLbs = item.ChargeWeightLbs;
                    item.VLWeightKG = item.ChargeWeightKG;
                    item.VLWeightLbs = item.ChargeWeightLbs;
                    item.ShippingInLand = 0;
                    item.TaxExport = 0;
                    item.PriceTaxPercentage = 0;
                    item.PriceTaxVatPercentage = 0;
                    item.ProductSiteId = 0;
                    ev.SaveProductcapture(item, CaptureID, CategoryName, KeywordSeach);
                }

            }
            catch (Exception)
            {

            }
        }

        public ActionResult Viewed()
        {
            var list = Products;
            return PartialView("Partials/_ProductViewed", list.Take(10));
        }

        public ActionResult AddToWishList(int Id)
        {
            try
            {
                // Lấy cookie cũ tên views
                var wishlist = Request.Cookies["wishlist"];
                var ProductName = db.Products.Where(p => p.Actived == true).SingleOrDefault(p => p.Id == Id);

                if (ProductName == null)
                {
                    return RedirectToAction("NotFound", "Home");
                }

                // Nếu chưa có cookie cũ -> tạo mới
                if (wishlist == null)
                {
                    wishlist = new HttpCookie("wishlist");
                }
                // Bổ sung mặt hàng đã xem vào cookie
                if (wishlist[Id.ToString()] == null)
                {

                    wishlist.Values[Id.ToString()] = Id.ToString();
                    //Success(string.Format("<b><h4>{0}</h4></b> was add to wish list successfully.", ProductName.Name), true);
                }

                // Đặt thời hạn tồn tại của cookie
                wishlist.Expires = DateTime.Now.AddMonths(1);
                // Gửi cookie về client để lưu lại
                Response.Cookies.Add(wishlist);

                // Lấy List<int> chứa mã hàng đã xem từ cookie
                var keys = wishlist.Values
                    .AllKeys.Select(k => int.Parse(k)).ToList();
                // Truy vấn háng đãn xem
                ViewBag.WishList = db.Products
                    .Where(p => keys.Contains(p.Id));
                ViewBag.Count = wishlist.Values.Count;
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MyWishList()
        {
            var wishlist = Request.Cookies["wishlist"];
            // Nếu chưa có cookie cũ -> tạo mới
            if (wishlist == null)
            {
                wishlist = new HttpCookie("wishlist");
            }
            var keys = wishlist.Values
                .AllKeys.Select(k => int.Parse(k)).ToList();
            // Truy vấn háng đãn xem
            ViewBag.WishList = db.Products
                .Where(p => keys.Contains(p.Id))
                .Take(10);
            return View();
        }

        public ActionResult RemoveFromWishList(int Id)
        {
            try
            {
                var ProductName = db.Products.SingleOrDefault(p => p.Id == Id);
                var wishlist = Request.Cookies["wishlist"];

                if (wishlist.HasKeys)
                {
                    wishlist.Values.Remove(Id.ToString());
                }
                Response.SetCookie(wishlist);
                //Success(string.Format("<b><h4>{0}</h4></b> was remove from wish list successfully.", ProductName.Name), true);
            }
            catch
            {

            }


            // Bổ sung mặt hàng đã xem vào cookie
            // wishlist.Values[Id.ToString()].Remove
            return RedirectToAction("MyWishList", "Product");
        }

        public ActionResult LastestProducts(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.label = "Danh sách deal mới nhất";
            var model = db.Products.Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate).ToList();
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BestSaleProducts(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.label = "Danh sách gởi quà tặng";
            var model = db.Products.Where(p => p.Actived == true).OrderByDescending(p => p.Views).ToList();
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetProductSites(string strNameSite, int? page)
        {
            int ProductSiteId = db.ProductSites.Where(p => p.ProductSiteName.ToUpper().Trim() == strNameSite.Trim().ToUpper()).Select(p => p.ProductSiteId).FirstOrDefault();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var model = db.Products.Where(p => p.Actived == true && p.ProductSiteId == ProductSiteId).OrderByDescending(p => p.Views).ToList();
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListBySpecial(string Id, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            List<Product> model = null;
            switch (Id)
            {
                case "All":
                    model = db.Products.Where(p => p.Actived == true).ToList();
                    ViewBag.Id = Id;

                    break;
                case "Latest":
                    model = db.Products.Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate).ToList();
                    ViewBag.label = "Mới";

                    break;
                case "Best":
                    model = db.Products.Where(p => p.Actived == true)
                        .Where(p => p.Views > 0)
                        .OrderByDescending(p => p.Views).ToList();
                    ViewBag.label = "Bán chạy";
                    break;
                case "Special":
                    model = db.Products.Where(p => p.Actived == true)
                        .Where(p => p.IsFeatured.Value).ToList();
                    ViewBag.label = Id;
                    break;
                case "SalesOff":
                    model = db.Products.Where(p => p.Actived == true)
                        .Where(p => p.Discount > 0)
                        .OrderByDescending(p => p.Discount).ToList();
                    ViewBag.label = Id;
                    break;
                case "Favorite":
                    model = db.Products.Where(p => p.Actived == true).ToList();
                    ViewBag.label = Id;
                    break;

                default:
                    model = db.Products.Where(p => p.Actived == true).ToList();
                    //.Where(p => p.ManufactId == Id).ToList();
                    ViewBag.label = Id;
                    break;
            }

            ViewBag.Id = Id;

            // return View(students.ToPagedList(pageNumber, pageSize));
            // return PartialView("Partials/_Search", model.ToPagedList(pageNumber, pageSize));
            return View("ProductList", model.ToPagedList(pageNumber, pageSize));
        }

    }
}