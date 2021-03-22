using System.Threading;
using System.Web;

namespace OnChotto
{
    public class SessionManager
    { 
        
        public static int CurrentCulture
        {
            get
            {
                string culture = "vi"; 
                var httpCookie = HttpContext.Current.Request.Cookies["language"];                 
                if (httpCookie != null)
                {
                    culture = httpCookie.Value;
                }
                if (culture == "en")
                    return 0;
                else if (culture == "vi")
                    return 1;
                else
                    return 0;
            }            
        }
    }
}