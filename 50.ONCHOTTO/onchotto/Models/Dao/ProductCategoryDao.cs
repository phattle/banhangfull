using OnChotto.Models;
using OnChotto.Models.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Models.Dao
{
    public class ProductCategoryDao
    {

        public ApplicationDbContext db = new ApplicationDbContext();

        public SelectList listCategory(int? selected = null, int exclude_id = -1)
        {
            //return new SelectList(new[] { new SelectListItem() { Text = "No Category", Value = "0" } });
            
            return new SelectList(db.ProductCategories.Where(p => p.CatId != exclude_id), "CatId", "Name", selected);
        }

       

        public List<int> ChildCategoryIds(int _parentId)
        {
            List<int> ListCats = new List<int>();
            ListCats.Add(_parentId);

            foreach (var cat in db.ProductCategories.Where(p=>p.ParentId == _parentId).ToList())
            {
                int _catid = Convert.ToInt32(cat.CatId);
                ListCats.Add(_catid);
            }

            return ListCats;
        }

        public List<Product> AllProducts(ProductCategory category, int Limit = 100)
        {
            var catids = ChildCategoryIds(category.CatId);
            var products = db.Products
                                .Where(p => p.Actived == true)
                                .Where(p => catids.Contains(p.CatId.Value))
                                .OrderByDescending(p => p.Id)
                                .Take(Limit)
                                .ToList();
            return products;
        }



        public SelectList ListSentGift()
        {
            return new SelectList(db.ProductCategories.Where(p => p.ParentId == 76), "CatId", "Name");

        }

        public List<int> ChildCategoryGiftIds(int _parentId)
        {
            List<int> ListGift = new List<int>();
            ListGift.Add(_parentId);

            foreach (var cat in db.ProductCategories.Where(p => p.ParentId == _parentId).ToList())
            {
                int _catid = Convert.ToInt32(cat.CatId);
                ListGift.Add(_catid);
            }

            return ListGift;
        }

        public List<Product> AllProductsGift(ProductCategory category, int Limit = 100)
        {
            var catids = ChildCategoryGiftIds(category.CatId);
            var products = db.Products
                                .Where(p => p.Actived == true)
                                .Where(p => catids.Contains(p.CatId.Value))
                                .OrderByDescending(p => p.Id)
                                .Take(Limit)
                                .ToList();
            return products;
        }
    }

}