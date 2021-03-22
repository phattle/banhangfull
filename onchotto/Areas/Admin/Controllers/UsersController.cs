using OnChotto.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private ApplicationUserManager _userManager;
        // GET: Admin/Users
        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> users = db.Users.AsEnumerable();
            return View(users);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FullName,Discriminator")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                //user.PasswordHash = "AD5GpO4nWO2cBLCc9SM4KNRkkySFhzcZ7gKK7Q4oPBSkfsx13TXpwIir3/oJMTpfOQ==";
                //db.Users.Add(user);
                user.UserName = user.Email;
                RegisterViewModel model = new RegisterViewModel();
                model.Password = "123456";
                var result = await UserManager.CreateAsync(user, model.Password);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser model = db.Users.Find(id);
            if (model == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin User");
                return RedirectToAction("Index");
            }

            if (model.District != null)//second
            {
                ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", model.District.ProvinceId);
                ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == model.District.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", model.DistrictId);
            }else
            {
                ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull");
                ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "-1").Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            try
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                if (model.District != null)//second
                {
                    ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull", model.District.ProvinceId);
                    ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == model.District.ProvinceId).Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull", model.DistrictId);
                }
                else
                {
                    ViewBag.ProvinceId = new SelectList(db.Provinces.Select(x => new { ProvinceId = x.ProvinceId, NameFull = x.Type + " " + x.Name }), "ProvinceId", "NameFull");
                    ViewBag.DistrictId = new SelectList(db.Districts.Where(d => d.ProvinceId == "-1").Select(x => new { DistrictId = x.DistrictId, NameFull = x.Type + " " + x.Name }), "DistrictId", "NameFull");
                }
                return View(model);
            }

        }

        public ActionResult EditRole(string Id)
        {
            ApplicationUser model = db.Users.Find(Id);
            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToRole(string UserId, string[] RoleId)
        {
            ApplicationUser user = db.Users.Find(UserId);
            if (RoleId != null && RoleId.Count() > 0)
            {
                foreach (string item in RoleId)
                {

                    IdentityRole role = db.Roles.Find(item);
                    //Check Role not exist
                    if ( role != null && !user.Roles.Any(r => r.RoleId == item))
                    {
                        user.Roles.Add(new IdentityUserRole() { UserId = UserId, RoleId = item });
                    }

                }

                db.SaveChanges();
            }

            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => user.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");

            return RedirectToAction("EditRole", new { Id = UserId });

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult DeleteRoleFromUser(string UserId, string RoleId)
        {
            ApplicationUser user = db.Users.Find(UserId);

            var uRole = user.Roles.Single(m => m.RoleId == RoleId);

            db.Entry(uRole).State = System.Data.Entity.EntityState.Deleted;

            //user.Roles.Remove(user.Roles.Single(m => m.RoleId == RoleId));

            db.SaveChanges();

            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => user.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");

            return RedirectToAction("EditRole", new { Id = UserId });

        }

        public ActionResult Delete(string Id)
        {
            var model = db.Users.Find(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string Id)
        {
            ApplicationUser model = null;
            try
            {
                model = db.Users.Find(Id);

                db.Users.Remove(model);

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Delete", model);
            }

        }

    }
}