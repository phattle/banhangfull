using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OnChotto.Models.Dao
{
    public partial class OrderDetailDiff
    {
        SqlConnection cn;
        SqlConnection samcnn;
        public OrderDetailDiff()
        {            
            string strChuoi = WebConfigurationManager.ConnectionStrings["DevConnection"].ToString();
            cn = new SqlConnection(strChuoi);
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                cn.Open();

            string samconnect = WebConfigurationManager.ConnectionStrings["SAMConnection"].ToString();
            samcnn = new SqlConnection(samconnect);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }
        public int SaveOrderDetailDiff(List<Entities.OrderDetailDiff> strItem)
        {
            int iResult = -1;
           
            for (int i = 0; i < strItem.Count; i++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "OrderDetailDiff_INSERT";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductLink", strItem[i].ProductLink);
                cmd.Parameters.AddWithValue("@PriceAfter", strItem[i].PriceAfter);
                cmd.Parameters.AddWithValue("@Discount", strItem[i].Discount);
                cmd.Parameters.AddWithValue("@Amount", strItem[i].Amount);
                cmd.Parameters.AddWithValue("@Note", strItem[i].Note);
                cmd.Parameters.Add("@ReVal", SqlDbType.Int);
                cmd.Parameters["@ReVal"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                iResult = int.Parse(cmd.Parameters["@ReVal"].Value.ToString());
            }
           
            return iResult;
        }

        public int SendOrderDetailDiff(List<Entities.OrderDetailDiff> lst)
        {
            int Id = -1;
            for (int i = 0; i < lst.Count; i++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_OrderDetailDiff";
                cmd.Connection = this.samcnn;
                if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                    samcnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert"); 
                cmd.Parameters.AddWithValue("@OrderDiffId", lst[i].OrderDiffId);
                cmd.Parameters.AddWithValue("@OrderNo", lst[i].OrderNo);
                cmd.Parameters.AddWithValue("@OrderTrackingNo", lst[i].OrderTrackingNo);
                cmd.Parameters.AddWithValue("@StoreName", lst[i].StoreName);
                cmd.Parameters.AddWithValue("@ProductLink", lst[i].ProductLink);
                cmd.Parameters.AddWithValue("@ProductName", lst[i].ProductName);
                cmd.Parameters.AddWithValue("@Size", lst[i].Size);
                cmd.Parameters.AddWithValue("@Amount", lst[i].Amount);
                cmd.Parameters.AddWithValue("@Weight", lst[i].Weight);
                cmd.Parameters.AddWithValue("@Price", lst[i].Price);
                cmd.Parameters.AddWithValue("@Discount", lst[i].Discount);
                cmd.Parameters.AddWithValue("@PriceAfter", lst[i].PriceAfter);
                cmd.Parameters.AddWithValue("@Note", lst[i].Note);
                cmd.Parameters.AddWithValue("@ProductStatus", lst[i].ProductStatus);
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Id = int.Parse(cmd.Parameters["@Id"].Value.ToString());
            }
            return Id;

        }
    }
}