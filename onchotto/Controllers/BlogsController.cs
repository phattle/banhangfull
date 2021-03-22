using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Controllers
{
    public class BlogsController : BaseController
    {
        // GET: Blogs
        public ActionResult Index()
        {
            return View(db.Posts.Where(p=>p.Active == true).OrderByDescending(p=>p.createDate).ToList());
        }
        //
        //get siderbar
        public ActionResult SidebarBlog()
        {
            ViewBag.SpecialProducts = db.Products.OrderByDescending(p => p.CreateDate).Take(6).ToList();
            ViewBag.Cats = db.Categories.ToList();
            var PopularPosts = db.Posts.OrderByDescending(p => p.createDate).Where(p=>p.Active == true).Take(5);
            return PartialView("Partials/_SideBarBlog", PopularPosts);
        }

        // GET: Category
        public ActionResult Category(string Slug)
        {
            var Cat = db.Categories.SingleOrDefault(c => c.Slug == Slug);
            if (Cat == null)
            {
                return RedirectToRoute("404");
            }

            ViewBag.Cat = Cat;
            return View(Cat.Posts.OrderByDescending(p => p.createDate).Where(p => p.Active == true).ToList());
        }

        public ActionResult ShowPost(string Slug)
        {
            var Post = db.Posts.SingleOrDefault(p => p.Slug == Slug);

            if (Post == null)
            {
                return RedirectToRoute("404");
            }

            ViewBag.RelatedPosts = db.Posts.Where(p => p.CatId == Post.CatId).Where(p => p.Id != Post.Id).Take(4).ToList();
            ViewBag.Cat = Post.Category;

            return View(Post);
        }
    }
}