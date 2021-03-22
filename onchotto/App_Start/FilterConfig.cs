using System.Web;
using System.Web.Mvc;
using OnChotto.Filters;


namespace OnChotto
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RecaptchaFilter());//Filter reCAPTCHA
        }
    }
}
