using System.Linq;
using System.Web.Mvc;

namespace OnChotto.Controllers
{
    public class BrandController : BaseController
    {
        // GET: Brand
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListBrand()
        {
            var model = db.Manufacts.ToList();
            return PartialView("Partials/_Brand", model);
        }
    }
}