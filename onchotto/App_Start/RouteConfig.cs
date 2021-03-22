using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnChotto
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product Category",
                url: "danh-muc/{metatitle}-{Id}",
                defaults: new { controller = "Product", action = "Cat", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Product Detail",
            //    url: "san-pham/{metatitle}-{Id}",
            //    defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
            //    namespaces: new[] { "OnChotto.Controllers" }
            //);

            routes.MapRoute(
               name: "Product Detail",
               url: "san-pham/{metatitle}-{asin}",
               defaults: new { controller = "Product", action = "Detail", ASIN = UrlParameter.Optional },
               namespaces: new[] { "OnChotto.Controllers" }
           );

            routes.MapRoute(
               name: "Product Search Detail",
               url: "chi-tiet-search/{metatitle}-{asin}",
               defaults: new { controller = "Product", action = "DetailSearch", ASIN = UrlParameter.Optional },
               namespaces: new[] { "OnChotto.Controllers" }
           );

            routes.MapRoute(
               name: "Search Product",
               url: "tim-kiem-san-pham",
               defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional, cat = "All" },
               namespaces: new[] { "OnChotto.Controllers" }
           );

            routes.MapRoute(
              name: "Quotations",
              url: "bao-gia-ngay",
              defaults: new { controller = "Quotations", action = "Index", id = UrlParameter.Optional, cat = "All" },
              namespaces: new[] { "OnChotto.Controllers" }
          );
            routes.MapRoute(
              name: "StoreDiff",
              url: "mua-ho",
              defaults: new { controller = "StoreDiff", action = "Index", id = UrlParameter.Optional, cat = "All" },
              namespaces: new[] { "OnChotto.Controllers" }
          );

            //Fix lỗi backlink về OnChotto: Đưa về trang search
            routes.MapRoute(
               name: "Fix link deal",
               url: "chi-tiet/{Id}",
               defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional, cat = "All" },
               namespaces: new[] { "OnChotto.Controllers" }
           );

            routes.MapRoute(
               name: "Fix link chi tiet",
               url: "chi-tiet/{keyword}-{Id}.html",
               defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional, cat = "All" },
               namespaces: new[] { "OnChotto.Controllers" }
           );
            //End Fix

            routes.MapRoute(
                name: "Product Favoris",
                url: "danh-dau-yeu-thich/{Id}",
                defaults: new { controller = "Product", action = "AddToWishList", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "Product Favoris List",
                url: "deal-yeu-thich",
                defaults: new { controller = "Product", action = "MyWishList", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "LastestProduct",
                url: "deal-moi-nhat",
                defaults: new { controller = "Product", action = "LastestProducts", Id = "Latest" },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "BestSaler product",
                url: "deal-ban-chay",
                defaults: new { controller = "Product", action = "BestSaleProducts", Id = "Best" },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
               name: "Get Product Sites",
               url: "Goi-qua-tang",
               defaults: new { controller = "Product", action = "GetProductSites", Id = "Get" },
               namespaces: new[] { "OnChotto.Controllers" }
           );

            routes.MapRoute(
                name: "Contact View",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "Blog Detail",
                url: "blog/{CatSlug}/{Slug}",
                defaults: new { controller = "Blogs", action = "ShowPost", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "BlogCategory",
                url: "blog/{Slug}",
                defaults: new { controller = "Blogs", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "Blogs All",
                url: "blog",
                defaults: new { controller = "Blogs", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            

            routes.MapRoute(
                name: "Page view",
                url: "{Slug}",
                defaults: new { controller = "Pages", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "Account Manage",
                url: "Manage/{action}/{id}",
                defaults: new { controller = "Manage", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnChotto.Controllers" }
            );

            //routes.MapRoute(
            //    name: "404",
            //    url: "{*url}",
            //    defaults: new { controller = "Home", action = "NotFound", id = UrlParameter.Optional },
            //    namespaces: new[] { "OnChotto.Controllers" }
            //);

            //Show a 404 error page for anything else.

           //routes.MapRoute("Error", "{*url}",
           //    new { controller = "Home", action = "NotFound", id = UrlParameter.Optional },
           //    new[] { "OnChotto.Controllers" }
           //);
        }
    }
}
