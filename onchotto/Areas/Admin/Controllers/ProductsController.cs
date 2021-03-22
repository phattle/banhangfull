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
using HtmlAgilityPack;
using OnChotto.Models.Dao;
using OnChotto.Commons;
using System.Globalization;
using System.Threading;
using System.Text;
using System.IO;

namespace OnChotto.Areas.Admin.Controllers
{
    public class ProductsController : AdminController
    {
        List<int> LstCatID;
        private IList<ProductTaxHscode> _lst = null;
        // GET: Admin/Products
        public ActionResult Index()
        {
            string strUserID = GetUserID();
            var LstProductSite = db.UserProductSites.Where(p => p.UsersId == strUserID).ToList().Select(p => p.ProductSiteId);
            if (LstProductSite.Count() > 0)
            {

                LstCatID = new List<int>();
                AddParentNode(LstProductSite.ToList());
                IEnumerable<Product> LstProduct = from pro in db.Products
                                                  where LstCatID.Contains((int)pro.CatId)
                                                  select pro;
                return View(LstProduct);
            }
            else
            {
                var products = db.Products.Include(p => p.User).Include(p => p.Manufact).Include(p => p.ProductCategory).Include(p => p.ProductType);
                return View(products.ToList());
            }
        }
        public ProductsController()
        {
            if (_lst == null)
            {
                _lst = new List<ProductTaxHscode>();
                var lst = (from p in db.ProductTaxHscodes select p).AsEnumerable();
                _lst = lst.ToList<ProductTaxHscode>();
            }
        }
        public string GetUserID()
        {
            return db.Users.Where(p => p.Email == User.Identity.Name).Select(p => p.Id).FirstOrDefault().ToString();
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ManufactId = new SelectList(db.Manufacts, "id", "name");
            var lst = (from p in db.ProductTaxHscodes select p).AsEnumerable();
            _lst = lst.ToList();
            ViewBag.HsCodeId = new SelectList(db.ProductTaxHscodes, "HsCodeId", "PoductName", "HsCode");
            ViewBag.lst = lst;

            //GetBy Theo USERID
            string strUserID = GetUserID();
            var LstProductSite = db.UserProductSites.Where(p => p.UsersId == strUserID).ToList().Select(p => p.ProductSiteId);
            IEnumerable<ProductCategory> LstProductCategories;
            if (LstProductSite.Count() > 0)
            {
                LstCatID = new List<int>();
                AddParentNode(LstProductSite.ToList());
                LstProductCategories = from pro in db.ProductCategories
                                       where LstCatID.Contains(pro.CatId)
                                       select pro;


            }
            else
            {
                LstProductCategories = from pro in db.ProductCategories
                                       select pro;
            }
            ViewBag.CatId = new SelectList(LstProductCategories, "CatId", "Name");
            ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name");
            //ViewBag.UnitCurrRank = new SelectList(db.PriceCurrencys, "CurrencyName", "CurrencyName");
            ViewBag.UnitCurrRank = db.PriceCurrencys;
            ViewBag.currenrak = Commons.DataHelper.CurrRank.ToString("#,##0");
            //LstCatID.Clear();
            return View();
        }


        private void AddParentNode(List<string> LstProductSite)
        {
            try
            {
                foreach (string der in LstProductSite)
                {
                    int iCatID = int.Parse(der);
                    var lstDepartment = db.ProductCategories.Where(p => p.ParentId == iCatID).ToList();
                    foreach (var item in lstDepartment)
                    {
                        AddChidrenNode(item.CatId);
                    }
                    if (lstDepartment.Count == 0)
                        LstCatID.Add(iCatID);
                }

            }
            catch (Exception)
            {

            }

        }

        private void AddChidrenNode(int CatId)
        {
            try
            {
                int parentID = (int)CatId;
                var lstDepartment = db.ProductCategories.Where(p => p.ParentId == parentID).ToList();//Danh sách các nút con có ID của cha là parentID
                foreach (ProductCategory der in lstDepartment)
                {
                    int iNode = der.CatId;
                    LstCatID.Add(iNode);

                }
            }
            catch (Exception)
            {

            }

        }

        public string GetProductZone(int iCartID)
        {
            string strProductZone = string.Empty, strParentId = string.Empty, strProductSiteID = string.Empty;

            var strItems = db.ProductCategories.Where(p => p.CatId == iCartID);
            List<ProductSite> LstProductSites = db.ProductSites.ToList();
            foreach (var item in strItems)
            {
                strParentId = item.ParentId.ToString();
                strProductSiteID = GetProductSiteName(LstProductSites, item.Name);
            }
            if (strParentId == string.Empty && strProductSiteID == string.Empty)
                strProductZone = "US";
            else
            {
                var strItemsParent = db.ProductCategories.Where(p => p.CatId.ToString() == strParentId);
                foreach (var item in strItemsParent)
                {
                    foreach (var item1 in db.ProductCategories.Where(p => p.CatId == item.ParentId))
                    {
                        strProductZone = GetProductSiteName(LstProductSites, item1.Name);
                    }

                }

            }
            return strProductZone != string.Empty ? strProductZone : strProductSiteID;
        }

        public string GetProductSiteName(List<ProductSite> LstProductSites, string strName)
        {

            string strProductSiteID = string.Empty;
            foreach (var itemSites in LstProductSites)
            {
                if (strName.Trim().ToUpper() == itemSites.ProductSiteName.Trim().ToUpper())
                {
                    strProductSiteID = itemSites.IdCode;
                }
            }
            return strProductSiteID;
        }
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
        private static string retrieveData(string url)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(url); 
            HttpWebResponse response = (HttpWebResponse)
            request.GetResponse();
            Stream resStream = response.GetResponseStream();
            string tempString = null;
            int count = 0;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    tempString = Encoding.ASCII.GetString(buf, 0, count);
                    sb.Append(tempString);
                }
            }
            while (count > 0);
            return sb.ToString();
        }
         
        private void GetDataByAsin(string aSIN, int catid, string productname)
        {
            OnChotto.Models.Dao.CaptureEvent ev = new OnChotto.Models.Dao.CaptureEvent();
            string strURL = string.Empty, CategoryName = "", KeywordSeach = "", SearchID = "";
            int CaptureID = 0;
            HtmlWeb web = new HtmlWeb();
            Product item = new OnChotto.Models.Entities.Product();

            if (ev.GetExitsASin(item.ASIN) <= 0)
            {
                DataTable tbl = ev.GetCaptureByASIN(aSIN);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    CategoryName = tbl.Rows[0]["CategoryName"].ToString();
                    KeywordSeach = tbl.Rows[0]["KeywordSeach"].ToString();
                    SearchID = tbl.Rows[0]["SearchID"].ToString();
                    CaptureID = Common.CorrectValue(tbl.Rows[0]["CaptureID"], int.MinValue);
                }

                strURL = "https://www.amazon.com/dp/" + aSIN; 
                string htmlSource = retrieveData(strURL);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmlSource);
                item.DetailPageURL = strURL;
                if (IsElementPresent(document, "titleSection"))
                {
                    
                    string name = "";
                    if (document.DocumentNode.SelectSingleNode("//body") != null)
                    {
                        HtmlNode node = document.DocumentNode.SelectNodes("//div[@id='titleSection']").First();
                        name = node.InnerText;
                    }
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
                    {
                        string [] pr = price.ToString().Split('-');
                        price = price[0].ToString().Trim();
                        item.Price = Decimal.Parse(price, CultureInfo.InvariantCulture);
                    }

                    item.Amount = 0;
                    if (item.Price > 0)
                    {
                        item.Price = item.Price * DataHelper.getRate();
                        item.PriceAfter = item.Price;
                    }
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
                item.CatId = catid;
                item.ASIN = aSIN;
                if (!string.IsNullOrEmpty(productname))
                    item.Name = productname;
                ev.SaveProductcapture(item, CaptureID, CategoryName, KeywordSeach);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateByAsin(Product product)
        {
            string asin = product.ASIN;
            int catid = (int)product.CatId;
            string productname = product.Name;
            int productID = 0;
            if (asin.Length > 0)
            {
                GetDataByAsin(asin, catid, productname);
                try
                {
                    productID = db.Products.Where(p => p.ASIN == asin).FirstOrDefault().Id;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                if (productID <= 0)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Edit/" + productID);
            }
            return View(product);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToAscii();
                product.ProductZone = GetProductZone(int.Parse(product.CatId.ToString()));

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", product.UserId);
            ViewBag.ManufactId = new SelectList(db.Manufacts, "id", "name", product.ManufactId);
            ViewBag.CatId = new SelectList(db.ProductCategories, "CatId", "name", product.CatId);
            ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name", product.TypeId);
            ViewBag.HsCodeId = new SelectList(db.ProductTaxHscodes, "HsCodeId", "PoductName", "HsCode", product.HsCodeId);
            //ViewBag.UnitCurrRank = new SelectList(db.PriceCurrencys, "Id", "CurrencyName", product.UnitCurrRank);


            ////GetBy Theo USERID
            //string strUserID = GetUserID();
            //var LstProductSite = db.UserProductSites.Where(p => p.UsersId == strUserID).ToList().Select(p => p.ProductSiteId);
            //IEnumerable<ProductCategory> LstProductCategories;
            //if (LstProductSite.Count() > 0)
            //{
            //    LstProductCategories = from pro in db.ProductCategories
            //                           where LstProductSite.Contains(pro.CatId.ToString())
            //                           select pro;
            //}
            //else
            //{
            //    LstProductCategories = from pro in db.ProductCategories
            //                           select pro;
            //}
            //ViewBag.CatId = new SelectList(LstProductCategories, "CatId", "Name");
            //ViewBag.CatId = new SelectList(LstProductCategories, "CatId", "Name", product.CatId);
            //ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name", product.TypeId);

            return View(product);
        }



        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", product.UserId);
            ViewBag.ManufactId = new SelectList(db.Manufacts, "id", "name", product.ManufactId);
            //ViewBag.CatId = new SelectList(db.ProductCategories, "CatId", "name", product.CatId);
            ViewBag.HsCodeId = new SelectList(db.ProductTaxHscodes, "HsCodeId", "PoductName", "HsCode", product.HsCodeId);
            var lst = (from p in db.ProductTaxHscodes select p).AsEnumerable();
            _lst = lst.ToList();
            ViewBag.lst = lst;

            //GetBy Theo USERID
            string strUserID = GetUserID();
            var LstProductSite = db.UserProductSites.Where(p => p.UsersId == strUserID).ToList().Select(p => p.ProductSiteId);
            IEnumerable<ProductCategory> LstProductCategories;
            if (LstProductSite.Count() > 0)
            {
                LstCatID = new List<int>();
                AddParentNode(LstProductSite.ToList());
                LstProductCategories = from pro in db.ProductCategories
                                       where LstCatID.Contains(pro.CatId)
                                       select pro;
            }
            else
            {
                LstProductCategories = from pro in db.ProductCategories
                                       select pro;
            }
            ViewBag.CatId = new SelectList(LstProductCategories, "CatId", "Name", product.CatId);
            ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name", product.TypeId);
            ViewBag.UnitCurrRank = new SelectList(db.PriceCurrencys, "CurrencyName", "CurrencyName", product.UnitCurrRank);
            ViewBag.currenrak = Commons.DataHelper.CurrRank.ToString("#,##0");
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                product.Slug = product.Name.ToAscii();
                product.ProductZone = GetProductZone(int.Parse(product.CatId.ToString()));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", product.UserId);
            ViewBag.ManufactId = new SelectList(db.Manufacts, "id", "name", product.ManufactId);
            ViewBag.CatId = new SelectList(db.ProductCategories, "CatId", "name", product.CatId);
            ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name", product.TypeId);
            ViewBag.HsCodeId = new SelectList(db.ProductTaxHscodes, "HsCodeId", "PoductName", "HsCode", product.HsCodeId);

            ////GetBy Theo USERID
            //string strUserID = GetUserID();
            //var LstProductSite = db.UserProductSites.Where(p => p.UsersId == strUserID).ToList().Select(p => p.ProductSiteId);
            //IEnumerable<ProductCategory> LstProductCategories;
            //if (LstProductSite.Count() > 0)
            //{
            //    LstProductCategories = from pro in db.ProductCategories
            //                           where LstProductSite.Contains(pro.CatId.ToString())
            //                           select pro;
            //}
            //else
            //{
            //    LstProductCategories = from pro in db.ProductCategories
            //                           select pro;
            //}
            //ViewBag.CatId = new SelectList(LstProductCategories, "CatId", "Name");

            //ViewBag.TypeId = new SelectList(db.ProductTypes, "id", "name", product.TypeId);

            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {

                return HttpNotFound();
            }

            db.Products.Remove(product);
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

        [HttpPost]
        public JsonResult GetData(string keyword, int? page)
        {
            var currentPage = page.HasValue ? page.Value - 1 : 0;
            var query = _lst.Where(n => n.PoductName.ToLower().Contains(keyword.ToLower()));
            var lst = query.Skip(currentPage * 10).Take(10);
            int total = query.Count();
            return Json(new
            {
                data = lst,
                total = total,
                JsonRequestBehavior.AllowGet
            });
        }

        public ActionResult FillPriceCurrency(int LocationId)
        {
            var CurrencyName = db.PriceCurrencys.Where(c => c.Id == LocationId).Select(p=>p.CurrencyName).FirstOrDefault();
            return Json(CurrencyName, JsonRequestBehavior.AllowGet);
        }
    }
}
