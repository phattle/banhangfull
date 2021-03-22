using OnChotto.Controllers;
using OnChotto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Areas.Admin.Controllers
{
    [Authorize]
    public abstract class AdminController : BaseController
    {
        ///protected ApplicationDbContext db = new ApplicationDbContext();

    }
}