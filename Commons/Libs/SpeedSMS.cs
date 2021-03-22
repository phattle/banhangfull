using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Libs
{
    class SpeedSMS
    {
        const int SMS_TYPE_QC = 1; // loai tin nhan quang cao
        const int SMS_TYPE_CSKH = 2; // loai tin nhan cham soc khach hang
        const int SMS_TYPE_BRANDNAME = 3; // loai tin nhan brand name cskh

        string accessToken = "";
        string url = "http://api.speedsms.vn/index.php/sms/send";

        public SpeedSMS(string Token)
        {
            accessToken = Token;
        }

        public bool SendSMS(string phonenumber)
        {
            WebRequest myReq = WebRequest.Create(url);
            string credentials = accessToken + ":x";
            CredentialCache mycache = new CredentialCache();
            myReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            myReq.Headers["Content-Type"] = "application/json";

            WebResponse wr = myReq.GetResponse();

            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            Console.WriteLine(content);
            var json = "[" + content + "]"; // change this to array
            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject o in objects.Children<JObject>())
            {
                foreach (JProperty p in o.Properties())
                {
                    string name = p.Name;
                    string value = p.Value.ToString();
                    Console.Write(name + ": " + value);
                }
            }
            Console.ReadLine();
            return true;
        }
    }
}
