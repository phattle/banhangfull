using Commons.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnChotto.Controllers
{
    public class MailController : BaseController
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send(string name, string email, string message)

        {
            try
            {
                var from = name + "<" + email + ">";
                XMail.Send(from, "cskh@onchotto.com", "test", message);
            }
            catch
            {

            }
            return View();
            
        }
    }
}