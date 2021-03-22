using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hotdeal.Models
{
    public class ConnectionString
    {
        string strresult = ConfigurationManager.ConnectionStrings["DevConnectionSbsExpress"].ToString();
        string strResultDbSam = ConfigurationManager.ConnectionStrings["DevConnectionsamdb"].ToString();
        private SqlConnection cn = null;
        public ConnectionString(string str)
        {
            if (str == "sbsp-express")
            {
                cn = new SqlConnection(strresult);
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();
            }
            else if (str == "samdb")
            {
                cn = new SqlConnection(strResultDbSam);
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();
            }
        }

        public SqlConnection GetConnect()
        {
            return cn;
        }
    }
}