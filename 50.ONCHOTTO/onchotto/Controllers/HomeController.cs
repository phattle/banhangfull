using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OnChotto.Models;
using OnChotto.Models.Entities;
using OnChotto.Filters;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.Collections.Generic;
using OnChotto.Models.Amazon;
using System.Configuration;
using OnChotto.Commons;
using System.Web.Configuration;
using System.Threading;
using System.IO;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace OnChotto.Controllers
{
    public class HomeController : BaseController
    {
        public DateTime now = new DateTime();
        private const string AmazonAccessKey = "AKIAIUCMWIIAWDJGW3UQ";
        private const string AmazonSecretKey = "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";
        private const string DESTINATION = "ecs.amazonaws.com";
        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";
        private const string ITEM_ID = "0545010225";

        private string associateTag = "";
        private AmazonEndpoint EndPointLocation = AmazonEndpoint.US;

        public HomeController()
        {
            this.associateTag = "smuavn-20";
            this.EndPointLocation = AmazonEndpoint.US;
            //string countryCode = DataHelper.GetCountryCode();
            //int countryid = 0;
            //try
            //{
            //    countryid = countryCode == "en-US" ? 1 : 0;
            //    SessionManager.CurrentCulture = countryid;
            //    Session["CurrentCulture"] = countryid;
            //}
            //catch
            //{
            //}
        }
        public ActionResult ChangeLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var httpCookie = Request.Cookies["language"];
                if (httpCookie != null)
                {
                    var cookie = Response.Cookies["language"];
                    if (cookie != null) cookie.Value = culture;
                }

            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
         
        public ActionResult Index()
        {
            this.associateTag = "smuavn-20";
            this.EndPointLocation = AmazonEndpoint.US;
            //Saleoff product
            var saleOffProducts = db.Products.Where(p => p.Actived == true).Where(p => p.IsSpecial == true).Take(20);
            ViewBag.saleOffProducts = saleOffProducts;

            //Lastest Product
            var lastProducts = db.Products.Where(p => p.Actived == true).OrderByDescending(p => p.IsNew).OrderByDescending(p => p.CreateDate).Take(20);
            ViewBag.lastProducts = lastProducts;

            //var model = db.ProductCategories.Where(c => c.ParentId == null).OrderBy(c => c.DisplayOrder);
            var LstParentId = db.ProductCategories.Where(p => p.ParentId !=null).ToList().Select(p => p.ParentId);
            IEnumerable<ProductCategory> ProductCategories = from pro in db.ProductCategories
                                              where !LstParentId.Contains((int)pro.CatId)
                                              select pro;
            //var model = db.ProductCategories.OrderBy(c => c.DisplayOrder);
            return View(ProductCategories.OrderBy(c=>c.DisplayOrder));
        }
        public ActionResult Special()
        {
            var model = db.Products.Where(p => p.Actived == true).Take(10);
            return PartialView("Partials/_Special", model);
        }

        public ActionResult Saleoff()
        {
            var model = db.Products.Where(p => p.Actived == true).Where(p => p.Discount > 0).Take(5);
            return PartialView("Partials/_Saleoff", model);
        }

        public ActionResult _LastNews()
        {
            var model = db.Posts.Where(p => p.Active == true).OrderByDescending(p => p.Id).Take(8);
            return PartialView(model);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ.";

            return View();
        }

        public ActionResult Category()
        {
            int iCatId = 0, iParentID = 0, iResult = 0;
            bool bShowMenu = true;
            string url = Request.Url.AbsoluteUri;
            try
            {
                url = url.Substring(url.LastIndexOf("-"));
                url = url.Replace("-", string.Empty);
                iCatId = DataHelper.CorrectValue(url, int.MinValue);
                var vLstFile = db.ProductCategories.Find(iCatId);
                int iCatID = DataHelper.CorrectValue(vLstFile.ParentId.ToString(), int.MinValue);
                bShowMenu = DataHelper.CorrectValue(vLstFile.ShowOnMenu, true);
                int iCatIDChild = 0;
                if (iCatID < 0)
                {
                    iCatID = DataHelper.CorrectValue(db.Products.Where(b => b.Id == iCatId).Select(b => b.CatId).FirstOrDefault().ToString(), int.MinValue);
                    iCatIDChild = DataHelper.CorrectValue(db.ProductCategories.Where(b => b.CatId == iCatID).Select(b => b.ParentId).FirstOrDefault().ToString(), int.MinValue);

                }
                iParentID = DataHelper.CorrectValue(db.ProductCategories.Where(p => p.CatId == (iCatIDChild > 0 ? iCatIDChild : iCatID)).Select(p => p.ParentId).FirstOrDefault().ToString(), int.MinValue);
            }
            catch { }

            if (!bShowMenu)
            {
                iResult = iParentID > 0 ? iParentID : iCatId;
                var model = db.ProductCategories.Where(P => P.ParentId == iResult).OrderBy(p => p.DisplayOrder);
                return PartialView("Partials/_Category", model);
            }
            else
            {
                var model = db.ProductCategories.Where(P => P.ShowOnMenu == true).OrderBy(p => p.DisplayOrder);
                return PartialView("Partials/_Category", model);
            }

        }

        public ActionResult Header()
        {
            var today = new DateTime();
            var cart = ShoppingCart.Cart;
            try
            {
                var cats = db.ProductCategories.OrderBy(p => p.DisplayOrder).ToList();
                ViewBag.ProductCategories = cats;
            }
            catch
            {

            }

            try
            {
                // Lấy cookie cũ tên views
                var wishlist = Request.Cookies["wishlist"];
                // Nếu chưa có cookie cũ -> tạo mới
                if (wishlist == null)
                {
                    wishlist = new HttpCookie("wishlist");
                }
                ViewBag.Count = wishlist.Values.Count;
            }
            catch
            {

            }

            ViewBag.Provinces = db.Provinces.OrderBy(p => p.Name).ToList();

            ViewBag.BannerTop = db.Banners.Where(b => b.BannerTypeId == 1)
                                        .Where(b => b.BannerPositionId == 1)
                                        .Where(b => b.Active == true)
                                        .Where(b => b.StartDate >= today)
                                        .Where(b => b.EndDate > today).FirstOrDefault();

            return PartialView("Partials/_Header", cart.Items);
        }

        public ActionResult _SliderShow()
        {
            var model = db.Sliders.OrderByDescending(s => s.Id).Take(8).ToList();
            return PartialView(model);
        }
        public ActionResult _comboproduct()
        {
            var model = db.Products.Where(p=>p.Actived == true).Where(p=>p.CatId == 88).Take(3).ToList();
            return PartialView(model);
        }
        public ActionResult _topnineproduct()
        {
            var model = db.Products.OrderByDescending(p=>p.Id).Where(p => p.Actived == true).Where(p => p.CatId == 89).Take(9).ToList();
            return PartialView(model);
        }
        public ActionResult _toptintuc()
        {
            var model = db.Posts.OrderByDescending(p => p.Id).Where(p => p.CatId == 1).Take(10).ToList();
            return PartialView(model);
        }
        public ActionResult _toptuvan()
        {
            var model = db.Posts.OrderByDescending(p => p.Id).Where(p => p.CatId == 4).Take(6).ToList();
            return PartialView(model);
        }
        public ActionResult Footer()
        {

            ViewBag.MenuGioiThieu = db.Menus.Where(m => m.LocationId == 1).ToList();
            ViewBag.MenuTroGiup = db.Menus.Where(m => m.LocationId == 2).ToList();
            return PartialView("Partials/_Footer");
        }





        public static DataSet ConverttYourXmlNodeToDataSet(XmlNode xmlnodeinput)
        {
            DataSet dataset = null;
            if (xmlnodeinput != null)
            {
                XmlTextReader xtr = new XmlTextReader(xmlnodeinput.OuterXml, XmlNodeType.Element, null);
                dataset = new DataSet();
                dataset.ReadXml(xtr);
            }
            return dataset;
        }
        private Dictionary<int, string> FetchProduct(string url, string SearchIndex, string searchKeyword, int catID)
        {
            Dictionary<int, string> lsReturn = new Dictionary<int, string>();
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());
                string[] ResponeGroup = { "Accessories", "AlternateVersions", "BrowseNodes", "EditorialReview", "Images", "ItemAttributes", "ItemIds", "Large", "Medium"
                        ," OfferFull", "OfferListings", "Offers","OfferSummary", "PromotionSummary", "Reviews", "SalesRank", "SearchBins", "Similarities", "Small"
                        , "Tracks", "Variations", "VariationMatrix", "VariationOffers", "VariationSummary" };

                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    String message = errorMessageNodes.Item(0).InnerText;
                    lsReturn.Add(0, "not-found");
                    return lsReturn;// "Error: " + message + " (but signature worked)";
                }
                string strGuid = Guid.NewGuid().ToString();

                XmlNodeList ItemsNode = doc.GetElementsByTagName("Item", NAMESPACE);
                int i = 0;
                DataSet ds = new DataSet();
                foreach (XmlNode node in ItemsNode)
                {
                    ds = ConverttYourXmlNodeToDataSet(node);
                    List<Product> lstItem = new List<Models.Entities.Product>();
                    List<ProductLinks> lstItemLinks = new List<Models.Entities.ProductLinks>();
                    string listImage = "", ASIN = "", ParentASIN = "", FeatureText = "", FeatureImage = "";
                    DataTable tblLink = ds.Tables["ITEMLINK"];
                    DataTable tblFeature = ds.Tables["FEATURE"];
                    DataTable tblImage = ds.Tables["LARGEIMAGE"];
                    if (tblImage != null)
                    {
                        foreach (DataRow row in tblImage.Rows)
                        {
                            if (listImage.Length > 1)
                                listImage = listImage + "," + row["URL"].ToString();
                            else
                            {
                                listImage = row["URL"].ToString();
                                FeatureImage = row["URL"].ToString();
                            }

                        }
                    }
                    else
                    {

                        listImage = "/uploads/files/noimage.gif";
                        FeatureImage = "/uploads/files/noimage.gif";
                    }

                    if (tblFeature != null)
                    {
                        foreach (DataRow row in tblFeature.Rows)
                        {
                            if (FeatureText.Length > 1)
                                FeatureText = FeatureText + "<br />" + row["Feature_Text"].ToString();
                            else
                            {
                                FeatureText = row["Feature_Text"].ToString();
                            }
                        }
                    }

                    DataRow rowIT = ds.Tables["ITEM"].Rows[0];
                    DataRow rowAT = ds.Tables["ITEMATTRIBUTES"].Rows[0];

                    DataRow rowPrice = null, rowSaved = null, rowNew = null; bool isNew = false;
                    string Price = "0", PercentageSaved = "0", Condition = "";
                    if (ds.Tables.Contains("Price"))
                    {
                        rowPrice = ds.Tables["Price"].Rows[0];
                        Price = rowPrice["FormattedPrice"].ToString();
                    }

                    if (ds.Tables.Contains("OfferListing"))
                    {
                        rowSaved = ds.Tables["OfferListing"].Rows[0];
                        if (ds.Tables["OfferListing"].Columns.Contains("PercentageSaved"))
                        {
                            PercentageSaved = rowSaved["PercentageSaved"].ToString();
                        }
                    }
                    if (ds.Tables.Contains("OfferAttributes"))
                    {
                        rowNew = ds.Tables["OfferAttributes"].Rows[0];
                        isNew = rowNew["Condition"].ToString().ToLower() == "new" ? true : false;
                        Condition = rowNew["Condition"].ToString();
                    }
                    Price = Price.Replace('$', ' ');
                    if (ds.Tables["ITEM"].Columns.Contains("ASIN"))
                    {
                        ASIN = rowIT["ASIN"].ToString();
                    }
                    if (ds.Tables["ITEM"].Columns.Contains("ParentASIN"))
                    {
                        ParentASIN = rowIT["ParentASIN"].ToString();
                    }

                    var resL = db.ProductLinks.Where(p => p.AsinCode == ASIN);
                    try
                    {
                        db.ProductLinks.RemoveRange(resL);
                        db.SaveChanges();
                    }
                    catch (Exception e) { System.Console.WriteLine("Caught Exception: " + e.Message); }

                    var res = db.Products.Where(p => p.ASIN == ASIN);
                    try
                    {
                        db.Products.RemoveRange(res);
                        db.SaveChanges();
                    }
                    catch (Exception e) { System.Console.WriteLine("Caught Exception: " + e.Message); }
                    string Manufacturer = "";
                    if (ds.Tables["ITEMATTRIBUTES"].Columns.Contains("Manufacturer"))
                    {
                        Manufacturer = rowAT["Manufacturer"].ToString();
                    }
                    if (ds.Tables["ITEMATTRIBUTES"].Columns.Contains("Studio") && Manufacturer.Length <= 2)
                    {
                        Manufacturer = rowAT["Studio"].ToString();
                    }
                    int ManufactId = 0;
                    var resFac = db.Manufacts.Where(p => p.Name == Manufacturer);
                    foreach (var item in resFac)
                    {
                        ManufactId = item.Id;
                    }
                    if (ManufactId <= 0)
                    {
                        Manufact fac = new Manufact();
                        fac.Name = Manufacturer;
                        fac.Description = Manufacturer;
                        db.Manufacts.Add(fac);
                        db.SaveChanges();
                        resFac = db.Manufacts.Where(p => p.Name == Manufacturer);
                        foreach (var item in resFac)
                        {
                            ManufactId = item.Id;
                        }
                    }
                    string name = rowAT["Title"].ToString();
                    name = name.Replace('|', ' ');
                    name = name.Replace('/', ' ');
                    name = name.Replace(':', ' ');
                    Product pro = new Product();

                    pro.ProductZone = "US";
                    pro.ASIN = ASIN;
                    pro.Description = FeatureText;
                    pro.Images = listImage;
                    pro.ManufactId = ManufactId;
                    pro.FeaturedImage = FeatureImage;
                    pro.Name = name;
                    pro.DetailPageURL = rowIT["DetailPageURL"].ToString();
                    pro.MetaTitle = pro.Name;// sua 2 dong nay
                    pro.MetaDescription = pro.Name;
                    pro.Detail = FeatureText;
                    pro.MetaKeyword = searchKeyword + " ## " + pro.Name;
                    pro.CatId = catID;
                    pro.Slug = SearchIndex;
                    pro.SearchID = strGuid;
                    pro.Price = decimal.Parse(Price.Replace('.', ','));
                    pro.Amount = 0;
                    pro.Discount = decimal.Parse(PercentageSaved);
                    pro.ExtraDiscount = 0;
                    pro.CreateDate = DateTime.Now;
                    pro.IsNew = isNew;
                    pro.IsFeatured = false;
                    pro.IsSpecial = false;
                    pro.PriceAfter = pro.Price * Commons.DataHelper.CurrRank; // 22000
                    pro.Actived = true;
                    pro.Featured = FeatureText;
                    pro.Condition = Condition;
                    pro.ParentASIN = ParentASIN;
                    pro.LargeImageURL = FeatureImage;
                    pro.TypeId = 1;
                    lstItem.Add(pro);
                    db.Products.AddRange(lstItem);
                    db.SaveChanges();
                    foreach (DataRow row in tblLink.Rows)
                    {
                        ProductLinks item = new ProductLinks();
                        item.ProductID = db.Products.Where(p => p.ASIN == ASIN).FirstOrDefault().Id;
                        item.AsinCode = ASIN;
                        item.ItemUrl = row["URL"].ToString();
                        item.Description = row["Description"].ToString();
                        lstItemLinks.Add(item);
                    }
                    i++;
                    db.ProductLinks.AddRange(lstItemLinks);
                    db.SaveChanges();
                }

                lsReturn.Add(ItemsNode.Count, strGuid);
                return lsReturn;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
                lsReturn.Add(0, "not-found");
                return lsReturn;
            }
        }

        private AmazonAuthentication GetConfig()
        {
            var accessKey = ConfigurationManager.AppSettings["AmazonAccessKey"];
            var secretKey = ConfigurationManager.AppSettings["AmazonSecretKey"];
            var authentication = new AmazonAuthentication();
            authentication.AccessKey = accessKey;
            authentication.SecretKey = secretKey;
            return authentication;
        }

        List<Product> lst = new List<Product>();
        public int Listdiv(HtmlDocument doc, string catname, string keywords)
        {
            OnChotto.Models.Dao.CaptureEvent ev = new OnChotto.Models.Dao.CaptureEvent();
            try
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id='search-main-wrapper']//div[@id='main']//div[@id='searchTemplate']//div[@id='rightContainerATF']//div[@id='rightResultsATF']//div[@id='resultsCol']//div[@id='centerMinus']//div[@id='atfResults']//ul//li");
                //doc.DocumentNode.SelectNodes("//div/ul/li").ToList();
                foreach (HtmlNode item in nodes)
                {
                    if (item.Id.StartsWith("result_"))
                    {
                        string ASINCode = item.GetAttributeValue("data-asin", "");

                        //string TitleName = item.GetAttributeValue("h2", "");
                        string ImageLink = item.GetAttributeValue("src", "");
                        //lay title///////////
                        string titlealt = item.GetAttributeValue("alt", "");
                        var result = item.InnerHtml.Where(o => item.InnerHtml.StartsWith("<h2"));
                        var indexStartTitle = item.InnerHtml.IndexOf("<h2");
                        var indexEndTitle = item.InnerHtml.IndexOf("</h2>");
                        var title = item.InnerHtml.Substring(indexStartTitle, indexEndTitle - indexStartTitle);
                        var startSpan = title.IndexOf("</span>");
                        var titleName = title.Substring(startSpan, title.Length - startSpan).Substring(7);
                        /////////////
                        ///lay image//////////
                        ///var result = item.InnerHtml.Where(o => item.InnerHtml.StartsWith("<h2"));
                        var indexStartimage = item.InnerHtml.IndexOf("<img src=");
                        var indexEndimage = item.InnerHtml.IndexOf("srcset");
                        var im = item.InnerHtml.Substring(indexStartimage, indexEndimage - indexStartimage);
                        var ima = im.Split('"')[1];
                        //var startSpan = title.IndexOf("</span>");
                        //var titleName = title.Substring(startSpan, title.Length - startSpan).Substring(7);
                        /////////////
                        Product it = new Product();
                        it.ASIN = ASINCode;
                        it.Name = titleName;
                        it.Images = ima;
                        if (lst.Where(p => p.ASIN == ASINCode).ToList().Count <= 0)
                        {
                            lst.Add(it);
                            ev.SaveProductcapture(it, 0, catname, keywords);
                        }

                    }
                }
                return lst.Count;
            }
            catch (Exception ex)
            {
                Models.Dao.Log.Write(ex);
                return -1;
            }
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
        public string Detail(HtmlDocument document, string ID)
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
                Models.Dao.Log.Write(ex);
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
                Models.Dao.Log.Write(ex);
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
            //foreach (HtmlNode item in nodes)
            //{

            //    var notecon = item.InnerHtml;
            //    if (item.GetAttributeValue("class", "").StartsWith("a-spacing-small item imageThumbnail a-declarative"))
            //    {
            //        curimg = item.InnerHtml.Substring(item.InnerHtml.IndexOf("scr"), item.InnerHtml.IndexOf(">"));
            //        if (imageSrc.Length <= 1)
            //        {
            //            imageSrc = curimg.Replace(",50", "");
            //        }
            //        else
            //            imageSrc = curimg.Replace(",50", "") + "," + imageSrc;
            //    }
            //}

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
        private async Task<String> MakeRequestAsync(String url)
        {
            String responseText = await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream).ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                return null;
            });

            return responseText;
        }
        private void DownloadData(string strKeyID, string KeywordSeach, int categoryId, string CategoryName)
        {
            HtmlWeb web = new HtmlWeb();
            string strURL = "https://www.amazon.com/s/ref=nb_sb_noss?url=search-alias%3" + CategoryName + "&field-keywords=" + Url.Encode(KeywordSeach) + "";
            HtmlDocument document = web.Load(strURL);
            try
            {
                OnChotto.Models.Dao.CaptureEvent ev = new OnChotto.Models.Dao.CaptureEvent();
                int i = 0;
                int ichekcindex = Listdiv(document, CategoryName, KeywordSeach);
                while (i < 999999 && document.DocumentNode.SelectNodes("//div[@id='search-main-wrapper']//div[@id='main']//div[@id='searchTemplate']//div[@id='rightContainerATF']//div[@id='rightResultsATF']//div[@id='resultsCol']//div[@id='centerMinus']//div[@id='atfResults']//ul[@id='s-results-list-atf']") == null && ichekcindex <= 0)
                {
                    document = web.Load(strURL);
                    ichekcindex = Listdiv(document, CategoryName, KeywordSeach);
                    i++;
                }

            }
            catch (Exception ex)
            {
                OnChotto.Models.Dao.Log.Write(ex);
            }
        }

        [HttpPost]
        public ActionResult Search(FormCollection form, int? page)
        {
            var keyword = form["keyword"].ToString();
            var cat = form["cat"].ToString();

            ViewBag.tukhoa = keyword;
            ViewBag.cat = cat;

            if (cat == "All")
            {
                cat = null;
            }

            var results = (from s in db.Products.ToList()
                           where ((string.IsNullOrEmpty(keyword) ? true : s.Name.ToAscii().Contains(keyword.ToAscii(), StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(cat) ? true : s.ProductCategory.Name == cat))
                           select s).Where(p => p.Actived == true);
            int pageSize = 20;
            int pageNumber = (page ?? 1);


            if (results.Count() == 0)
            {
                ViewBag.notice = "Không tìm thấy sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
                return View(db.Products.Where(p => p.Actived == true).ToList().ToPagedList(pageNumber, pageSize));
            }

            ViewBag.notice = "Tìm thấy " + results.Count() + " sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
            return View(results.ToPagedList(pageNumber, pageSize));
        }
        //<!-- HttpPost Search Amazon end  -->
        //public ActionResult Search(FormCollection form, int? page)
        //{
        //    var keyword = form["keyword"].ToString(); var cat = form["cat"].ToString();
        //    ViewBag.tukhoa = keyword; ViewBag.cat = cat; string searchbyamazon = ""; string strKeyID = Guid.NewGuid().ToString();
        //    int catID = 0, itemCount = 0; string SearchIndex = "";
        //    searchbyamazon = DataHelper.CorrectValue(WebConfigurationManager.AppSettings["searchbyamazon"].ToString(), "false");
        //    if (cat == null && keyword == null && page == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    SearchIndex = cat;
        //    String[] Keywords = new String[] {
        //        keyword
        //    };
        //    if (SearchIndex != "All")
        //    {
        //        var catRes = db.ProductCategories.Where(p => p.SearchIndex == SearchIndex);
        //        foreach (var item in catRes)
        //        {
        //            catID = item.CatId;
        //            SearchIndex = item.SearchIndex;
        //        }
        //    }
        //    if (searchbyamazon == "false")
        //    {
        //        //Lưu Keyword Search;
        //        //if (keyword != null)
        //        //{
        //        //    CaptureEvent item = new CaptureEvent();
        //        //    item.TimeSeach = DateTime.Now;
        //        //    item.SearchID = strKeyID;
        //        //    item.KeywordSeach = keyword.ToString();
        //        //    item.CategoryId = catID;
        //        //    item.CategoryName = catID == 0 ? "All" : cat;
        //        //    item.IsCapture = false;
        //        //    item.CaptureCount = 0;
        //        //    db.CaptureEvents.Add(item);
        //        //    db.SaveChanges();
        //        //}
        //        DownloadData(strKeyID, keyword, catID, catID == 0 ? "All" : cat);
        //    }
        //    else
        //    {
        //        #region search by amazon
        //        try
        //        {
        //            foreach (string search in Keywords)
        //            {
        //                ViewBag.Search = search.Trim();
        //                var authentication = this.GetConfig();
        //                var wrapper = new AmazonWrapper(authentication, this.EndPointLocation, this.associateTag);
        //                var responseGroup = AmazonResponseGroup.ItemAttributes | AmazonResponseGroup.Images | AmazonResponseGroup.OfferSummary;
        //                AmazonSearchIndex searchIndex = DataHelper.getSearchIndex(SearchIndex);
        //                AmazonItemResponse checkRs = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search, 1, searchIndex, responseGroup);

        //                if (checkRs != null)
        //                {
        //                    int TotalPages = int.Parse(checkRs.Items.TotalPages.ToString());
        //                    if (TotalPages > 3)
        //                    {
        //                        for (int i = 1; i < 5; i++)
        //                        {
        //                            AmazonItemResponse result = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search, i, searchIndex, responseGroup);
        //                            itemCount += SaveProduct(result, strKeyID, SearchIndex, string.IsNullOrEmpty(search) ? SearchIndex : search, catID);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int i = 1; i <= TotalPages; i++)
        //                        {
        //                            AmazonItemResponse result = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search.Trim(), i, searchIndex, responseGroup);
        //                            itemCount += SaveProduct(result, strKeyID, SearchIndex, string.IsNullOrEmpty(search) ? SearchIndex : search, catID);
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }
        //        #endregion
        //    }
        //    var results = (from s in db.Products.ToList()
        //                   where ((string.IsNullOrEmpty(keyword) ? true : s.MetaKeyword.ToAscii().Contains(keyword.ToAscii(), StringComparison.OrdinalIgnoreCase)) &&
        //                          (string.IsNullOrEmpty(cat) ? true : s.ProductCategory.Name == cat))
        //                   select s).Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate);
        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);
        //    if (results.Count() == 0)
        //    {
        //        ViewBag.notice = "Không tìm thấy sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
        //        return View(db.Products.Where(p => p.Actived == true).ToList().ToPagedList(pageNumber, pageSize));
        //    }
        //    ViewBag.notice = "Tìm thấy " + results.Count() + " sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
        //    return View(results.ToPagedList(pageNumber, pageSize));
        //}
        //<!-- HttpPost Search Amazon  -->
        [HttpGet]
        public ActionResult Search(string cat, string keyword, int? page)
        {
            ViewBag.tukhoa = keyword;
            ViewBag.cat = cat;

            if (cat == "All")
            {
                cat = null;
            }

            var results = (from s in db.Products.ToList()
                           where ((string.IsNullOrEmpty(keyword) ? true : s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(cat) ? true : s.ProductCategory.Name == cat)

                                  || (string.IsNullOrEmpty(keyword) ? true : s.ASIN == keyword) 
                                  )
                           select s).Where(p => p.Actived == true);
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            if (results.Count() == 0)
            {
                ViewBag.notice = "Không tìm thấy sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
                return View(db.Products.Where(p => p.Actived == true).ToList().ToPagedList(pageNumber, pageSize));
            }

            ViewBag.notice = "Tìm thấy " + results.Count() + " sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
            return View(results.ToPagedList(pageNumber, pageSize));
        }
        //<!-- HttpGet Search Amazon  -->
        //public ActionResult Search(string cat, string keyword, int? page)
        //{
        //    string searchbyamazon = "";
        //    searchbyamazon = DataHelper.CorrectValue(WebConfigurationManager.AppSettings["searchbyamazon"].ToString(), "false");
        //    if (cat == null && keyword == null && page == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    ViewBag.tukhoa = keyword;
        //    int itemCount = 0;
        //    string strKeyID = Guid.NewGuid().ToString();
        //    string curCat = string.Empty;
        //    try
        //    {
        //        curCat = Request.QueryString["cat"].ToString();
        //    }
        //    catch
        //    {
        //        curCat = "All";
        //    }
        //    ViewBag.cat = curCat; cat = curCat;
        //    int catID = 0;
        //    string SearchIndex = "";
        //    if (curCat == "All")
        //    {
        //        cat = null;
        //        SearchIndex = "All";
        //    }
        //    else
        //        SearchIndex = curCat;

        //    String[] Keywords = new String[] {
        //        keyword
        //    };
        //    if (SearchIndex != "All")
        //    {
        //        var catRes = db.ProductCategories.Where(p => p.SearchIndex == SearchIndex);
        //        foreach (var item in catRes)
        //        {
        //            catID = item.CatId;
        //            SearchIndex = item.SearchIndex;
        //        }
        //    }

        //    if (searchbyamazon == "false")
        //    {
        //        //Lưu Keyword Search;
        //        if (keyword != null)
        //        {
        //            CaptureEvent item = new CaptureEvent();
        //            item.TimeSeach = DateTime.Now;
        //            item.SearchID = strKeyID;
        //            item.KeywordSeach = keyword.ToString();
        //            item.CategoryId = catID;
        //            item.CategoryName = catID == 0 ? "All" : cat;
        //            item.IsCapture = false;
        //            item.CaptureCount = 0;
        //            db.CaptureEvents.Add(item);
        //            db.SaveChanges();
        //        }
        //        DownloadData(strKeyID, keyword, catID, catID == 0 ? "All" : cat);

        //    }
        //    else
        //    {
        //        #region search by amazon
        //        try
        //        {
        //            foreach (string search in Keywords)
        //            {
        //                ViewBag.Search = search.Trim();
        //                var authentication = this.GetConfig();
        //                var wrapper = new AmazonWrapper(authentication, this.EndPointLocation, this.associateTag);
        //                var responseGroup = AmazonResponseGroup.ItemAttributes | AmazonResponseGroup.Images | AmazonResponseGroup.OfferSummary;
        //                AmazonSearchIndex searchIndex = DataHelper.getSearchIndex(SearchIndex);
        //                AmazonItemResponse checkRs = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search, 1, searchIndex, responseGroup);

        //                if (checkRs != null)
        //                {
        //                    int TotalPages = int.Parse(checkRs.Items.TotalPages.ToString());
        //                    if (TotalPages > 3)
        //                    {
        //                        for (int i = 1; i < 5; i++)
        //                        {
        //                            AmazonItemResponse result = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search, i, searchIndex, responseGroup);
        //                            itemCount += SaveProduct(result, strKeyID, SearchIndex, string.IsNullOrEmpty(search) ? SearchIndex : search, catID);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int i = 1; i <= TotalPages; i++)
        //                        {
        //                            AmazonItemResponse result = wrapper.Search(string.IsNullOrEmpty(search) ? SearchIndex : search.Trim(), i, searchIndex, responseGroup);
        //                            itemCount += SaveProduct(result, strKeyID, SearchIndex, string.IsNullOrEmpty(search) ? SearchIndex : search, catID);
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }
        //        #endregion
        //    }
        //    var results = (from s in db.Products.ToList()
        //                   where (s.MetaKeyword.Equals(keyword))
        //                   select s).Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate);
        //    if (results == null)
        //    {
        //        if (catID == 0)
        //        {
        //            results = (from s in db.Products.ToList()
        //                       where ((string.IsNullOrEmpty(keyword) ? true : s.SearchID.Equals(strKeyID)))
        //                       select s).Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate);
        //        }
        //        else
        //        {
        //            results = (from s in db.Products.ToList()
        //                       where ((string.IsNullOrEmpty(keyword) ? true : s.SearchID.Equals(strKeyID))
        //                       && (string.IsNullOrEmpty(cat) ? true : s.ProductCategory.SearchIndex == cat))
        //                       select s
        //                     ).Where(p => p.Actived == true).OrderByDescending(p => p.CreateDate);

        //        }
        //    }
        //    itemCount = results.Count();
        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);
        //    if (itemCount <= 0)
        //    {
        //        ViewBag.notice = "Không tìm thấy sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
        //        return View(db.Products.Where(p => p.Actived == true).ToList().ToPagedList(pageNumber, pageSize));
        //    }
        //    else
        //    {
        //        ViewBag.notice = "Tìm thấy " + itemCount + " sản phẩm phù hợp với từ khóa: \"" + keyword + "\"";
        //        return View(results.ToPagedList(pageNumber, pageSize));
        //    }
        //}

        //<!-- HttpGet Search Amazon end  -->

        private int SaveProduct(AmazonItemResponse result, string keyID, string SearchIndex, string searchKeyword, int catID)
        {
            string ASIN = "", Department = "";
            int i = 0;
            try
            {
                if (result.Items == null || result.Items.Item == null)
                {
                    return 0;
                }

                foreach (var item in result.Items.Item)
                {
                    List<Product> lstItem = new List<Models.Entities.Product>();
                    List<ProductLinks> lstItemLinks = new List<Models.Entities.ProductLinks>();
                    string listImage = "", ParentASIN = "", FeatureText = "", FeatureImage = "", Size = "", WeightUnits = "", DetailText = "";
                    decimal WeightLbs = 0, WeightKG = 0;/*, FederalTax = 0*//*, ShippingInLand = 0, TaxExport = 0;*/
                    if (item.LargeImage != null)
                    {
                        foreach (var image in item.ImageSets)
                        {
                            if (listImage.Length > 1)
                                listImage = listImage + "," + image.LargeImage.URL.ToString();
                            else
                            {
                                listImage = image.LargeImage.URL.ToString();
                                FeatureImage = image.LargeImage.URL.ToString();
                            }
                        }
                    }
                    else
                    {

                        listImage = "/uploads/files/noimage.gif";
                        FeatureImage = "/uploads/files/noimage.gif";
                    }

                    if (item.ItemAttributes.Feature != null)
                    {
                        foreach (var feature in item.ItemAttributes.Feature)
                        {
                            if (FeatureText.Length > 1)
                                FeatureText = FeatureText + "<br />" + feature.ToString();
                            else
                            {
                                FeatureText = feature.ToString();
                            }
                        }
                    }
                    bool isNew = false;
                    string Price = "0", PercentageSaved = "0", Condition = "";
                    if (item.OfferSummary != null)
                    {
                        Price = item.OfferSummary?.LowestNewPrice?.FormattedPrice.ToString();
                    }

                    if (item.OfferListing != null)
                    {
                        PercentageSaved = (item.OfferListing.PercentageSaved).ToString();

                    }
                    if (item.ItemAttributes != null)
                    {
                        isNew = item.ItemAttributes.IssuesPerYear == DateTime.Now.Year.ToString() ? true : false;
                        if (catID == 0)
                        {
                            Department = "";
                            if (item.ItemAttributes.Department != null)
                            {
                                Department = item.ItemAttributes.Department;
                            }
                            else if (item.ItemAttributes.ProductGroup != null)
                            {
                                Department = item.ItemAttributes.ProductGroup;
                            }
                            else if (item.ItemAttributes.Binding != null)
                            {
                                Department = item.ItemAttributes.Binding;
                            }
                            var resCat = db.ProductCategories.Where(p => p.Name.Contains(Department));
                            if (resCat == null || resCat.Count() <= 0)
                            {
                                ProductCategory cat = new Models.Entities.ProductCategory();
                                string ACIItex = Department.ToAscii();
                                cat.Description = Department;
                                cat.Slug = ACIItex;
                                cat.SearchIndex = Department;
                                cat.DisplayOrder = 1;
                                cat.Name = Department;
                                db.ProductCategories.Add(cat);
                                db.SaveChanges();
                            }
                            resCat = db.ProductCategories.Where(p => p.Name.Contains(Department));
                            foreach (var itemCat in resCat)
                            {
                                catID = itemCat.CatId;
                            }
                        }
                        Size = item.ItemAttributes.Size;
                        if (item.ItemAttributes.PackageDimensions != null)
                        {
                            WeightUnits = item.ItemAttributes.PackageDimensions.Weight.Units;
                            WeightLbs = item.ItemAttributes.PackageDimensions.Weight.Value;
                            if (WeightUnits.ToLower() == "hundredths pounds" && WeightLbs > 0)
                            {
                                WeightLbs = WeightLbs / 100;
                                WeightUnits = "Lbs";
                                WeightKG = DataHelper.ConvertLbsToKgs(WeightLbs);// WeightLbs * (decimal)0.45359237;
                                //FederalTax = (decimal)0.08 * Commons.Common.CurrRank; //Tỷ giá USD Vietcombank
                                //ShippingInLand = (decimal)2.00 * Commons.Common.CurrRank;*///Điều chỉnh ShippingInLand tính trên đơn hàng/HAWB
                                //TaxExport = (decimal)0.00; //Thuế nhập khẩu HSCODE
                                //FederalTax, ShippingInLand, TaxExport Fix cứng nhân với tỷ giá ngân hàng, sau khách đặt hàng theo bảng HS Code điều chỉnh lại.
                            }

                        }
                        if (item.ItemAttributes.Brand != null)
                            Condition = item.ItemAttributes.Brand.ToString();
                    }
                    if (item.EditorialReviews != null)
                    {
                        List<EditorialReview> lst = item.EditorialReviews.ToList();
                        foreach (EditorialReview itemView in lst)
                        {
                            DetailText = itemView.Source;
                            string a = "";
                            a = itemView.Content + itemView.IsLinkSuppressed;

                        }

                    }
                    Price = Price.Replace('$', ' ');
                    ASIN = item.ASIN;
                    ParentASIN = item.ParentASIN;
                    var resL = db.ProductLinks.Where(p => p.AsinCode == ASIN);
                    try
                    {
                        foreach (var itemL in resL)
                        {
                            db.ProductLinks.Remove(itemL);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception e) { System.Console.WriteLine("Caught Exception: " + e.Message); }


                    string Manufacturer = "";

                    if (item.ItemAttributes.Manufacturer != null)
                        Manufacturer = item.ItemAttributes.Manufacturer;
                    else
                    {
                        if (item.ItemAttributes.Studio != null)
                            Manufacturer = item.ItemAttributes.Studio;
                    }
                    int ManufactId = 0;
                    var resFac = db.Manufacts.Where(p => p.Name == Manufacturer);
                    foreach (var itemFac in resFac)
                    {
                        ManufactId = itemFac.Id;
                    }
                    if (ManufactId <= 0)
                    {
                        Manufact fac = new Manufact();
                        fac.Name = Manufacturer;
                        fac.Description = Manufacturer;
                        db.Manufacts.Add(fac);
                        db.SaveChanges();
                        resFac = db.Manufacts.Where(p => p.Name == Manufacturer);
                        foreach (var itemFac in resFac)
                        {
                            ManufactId = itemFac.Id;
                        }
                    }
                    string name = item.ItemAttributes.Title;
                    name = name.Replace('|', ' ');
                    name = name.Replace('/', ' ');
                    name = name.Replace(':', ' ');
                    Product pro = new Product();
                    pro.ProductZone = "US";
                    pro.ASIN = ASIN;
                    pro.Description = FeatureText;
                    pro.Images = listImage;
                    pro.ManufactId = ManufactId;
                    pro.FeaturedImage = FeatureImage;
                    pro.Name = name;
                    pro.DetailPageURL = item.DetailPageURL;
                    pro.MetaTitle = pro.Name;// sua 2 dong nay
                    pro.MetaDescription = pro.Name;
                    pro.Detail = FeatureText;
                    pro.WeightLbs = WeightLbs;
                    pro.WeightUnit = WeightUnits;
                    pro.WeightKG = WeightKG;
                    pro.Size = Size;
                    pro.IsNew = true;
                    pro.MetaKeyword = searchKeyword + " ## " + pro.Name;
                    pro.CatId = catID;
                    pro.Slug = Department;
                    pro.UserId = "72b39d66-279b-4c6c-a696-1a1c69659ada";
                    //pro.DisplayOrder = 1;
                    pro.SearchID = keyID;
                    pro.Price = decimal.Parse(Price.Replace('.', ',')) * Commons.DataHelper.CurrRank;
                    pro.Discount = decimal.Parse(PercentageSaved);
                    pro.HsCodeId = 1;
                    //pro.FederalTax = (pro.Price * Commons.DataHelper.CurrRank) * 0.05M; // Thuế liên bang 5%/product
                    //pro.TaxExport = (pro.Price * Commons.DataHelper.CurrRank) + ((pro.Price * Commons.DataHelper.CurrRank) * 0.2M) + (((pro.Price * Commons.DataHelper.CurrRank) + ((pro.Price * Commons.DataHelper.CurrRank) * 0.2M)) * 0.1M);
                    if (pro.Discount > 0)
                        pro.PriceAfter = pro.Price * (pro.Discount / 100);
                    else
                        pro.PriceAfter = pro.Price;

                    pro.Amount = 0;
                    pro.ExtraDiscount = 0;
                    pro.CreateDate = DateTime.Now;
                    pro.IsNew = isNew;
                    pro.IsFeatured = false;
                    pro.IsSpecial = false;
                    pro.Actived = true;
                    pro.Featured = FeatureText;
                    pro.Condition = Condition;
                    pro.ParentASIN = ParentASIN;
                    pro.LargeImageURL = FeatureImage;
                    pro.TypeId = 1;
                    var resPro = db.Products.Where(p => p.ASIN == ASIN);
                    int countExits = 0;
                    lstItem.Add(pro);
                    try
                    {
                        if (resPro != null && resPro.Count() > 0)
                        {

                            countExits = 1;
                        }
                    }
                    catch (Exception e) { countExits = 0; System.Console.WriteLine("Caught Exception: " + e.Message); }

                    try
                    {
                        if (countExits <= 0)
                        {
                            db.Products.Add(pro);

                        }
                        else
                        {
                            foreach (var itRes in resPro)
                            {
                                itRes.Detail = FeatureText;
                                itRes.WeightLbs = WeightLbs;
                                itRes.WeightUnit = WeightUnits;
                                itRes.WeightKG = WeightKG;
                                itRes.SearchID = keyID;
                                //itRes.FederalTax = FederalTax;
                                //itRes.ShippingInLand = ShippingInLand;
                                //itRes.TaxExport = TaxExport;
                            }
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        UserLog log = new Models.Entities.UserLog();
                        log.Message = ASIN;
                        log.Title = ASIN;
                        db.UserLog.Add(log);
                        db.SaveChanges();
                        Console.WriteLine(ex.ToString());
                    }
                    foreach (var itemlink in item.ItemLinks)
                    {
                        ProductLinks itLnk = new ProductLinks();
                        itLnk.ProductID = db.Products.Where(p => p.ASIN == ASIN).FirstOrDefault().Id;
                        itLnk.AsinCode = ASIN;
                        itLnk.ItemUrl = itemlink.URL;
                        itLnk.Description = itemlink.Description;
                        lstItemLinks.Add(itLnk);
                        db.ProductLinks.Add(itLnk);
                    }
                    db.SaveChanges();
                    i++;
                }
                db.SaveChangesAsync();
                return i;
            }
            catch (Exception e)
            {
                UserLog log = new Models.Entities.UserLog();
                log.Message = ASIN;
                log.Title = ASIN;
                db.UserLog.Add(log);
                db.SaveChanges();
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
                return i;
            }
        }


        public ActionResult ProductTags()
        {
            var model = db.ProductCategories.ToList();
            return PartialView("Partials/_ProductTags", model);
        }

        public ActionResult Culture(string id)
        {
            HttpCookie cookie = Request.Cookies["Culture"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Culture", id);
            }
            cookie.Value = id;
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.SetCookie(cookie);

            return Redirect("/");
        }

        public ActionResult PopoupBanner()
        {
            //var today = new DateTime();
            int banerId = 2;
            HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["khaitruong"];
            if (cookie == null)
            {
                cookie = new HttpCookie("khaitruong", DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"));
                cookie.Expires = DateTime.Now.AddDays(1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                Banner popup = db.Banners.Where(b => b.BannerTypeId == banerId)
                           .Where(b => b.Active == true).FirstOrDefault();
                return PartialView("Partials/_PopupBaner", popup);
            }
            else
            {
                try
                {
                    if ((DateTime.Parse(cookie.Value) - DateTime.Now).TotalDays <= 0)
                    {
                        cookie.Value = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Remove("KHAITRUONG");
                        HttpContext.Response.Cookies.Add(cookie);
                        Banner popup = db.Banners.Where(b => b.BannerTypeId == banerId)
                                   .Where(b => b.Active == true).FirstOrDefault();
                        return PartialView("Partials/_PopupBaner", popup);
                    }
                }
                catch { return null; }
                return null;
            }
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxGetDistrictByProvice(string provinceId)
        {
            //var m = db.Provinces.Find(provinceId);
            return PartialView(db.Districts.Where(d => d.ProvinceId == provinceId).ToList());
        }

        [HttpPost]
        public ActionResult AjaxGetWardsByDistrict(string districtId)
        {
            //var m = db.Provinces.Find(provinceId);
            return PartialView(db.Wards.Where(d => d.districtid == districtId).ToList());
        }

    }
}