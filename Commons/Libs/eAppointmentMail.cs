using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Libs
{
    public class eAppointmentMail
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Location { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Subject { set; get; }
        public string Body { set; get; }
    }
}
