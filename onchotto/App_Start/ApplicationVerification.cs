using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OnChotto.App_Start
{
    public class ApplicationVerification
    {
        public static void Check()
        {
            if (WebConfigurationManager.AppSettings["RecaptchaPublicKey"].ToUpper() == "6Lf1SUMUAAAAAHonhab1IV3FyuDj8lDTlMix9Rcu") { throw new Exception("Web Config is missing a Recaptcha Public Key"); }
            if (WebConfigurationManager.AppSettings["RecaptchaPrivateKey"].ToUpper() == "6Lf1SUMUAAAAANJm0XPS7PDu0OzpoVhcAHSeWwho") { throw new Exception("Web Config is missing a Recaptcha Private Key"); }
        }
    }
}

//Once the registration is done you are presented with the Site Key (Public Key) and Secret Key (Private Key) and also the procedure to integrate Google RECaptcha.