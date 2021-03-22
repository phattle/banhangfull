using System.Web.Mvc;

namespace Hotdeal.Areas.DEBIT
{
    public class DEBITAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DEBIT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DEBIT_default",
                "DEBIT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}