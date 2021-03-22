//-- ------------------------------------------------------------
//-- Company name  :  DTGSOFT 
//-- Class name    :  clsOrderDiffsBase
//-- Creation date :  03/08/2018
//-- Created by    :  
//-- Description   :  
//-- Generated by  :  DTG Class Generator
//-- ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using OnChotto.Models.Entities;

namespace OnChotto.Models.Dao
{
	public class OrderDiffs : OnChotto.Models.Dao.OrderDiffsBase
    {

        #region  Constants

        #endregion

        #region  Variables
        SqlConnection samcnn;
        string strSam = WebConfigurationManager.ConnectionStrings["SAMConnection"].ToString();
        #endregion

        #region  Constructors
        public OrderDiffs()
            : base(DataAccess.AppConnectionString)
        {
            samcnn = new SqlConnection(strSam);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDiffs(string connectionString)
            : base(connectionString)
        {
            samcnn = new SqlConnection(connectionString);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDiffs(DataAccess dal)
            : base(dal)
        {
        }

        #endregion

        #region  Methods         
        public int OrderDiffToBilling(OrderDiff item)
        { 
            try
            { 
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_OrderDiffs";
                cmd.Connection = this.samcnn;
                if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                    samcnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Insert");       
                cmd.Parameters.AddWithValue("@UserId", item.UserId);
                cmd.Parameters.AddWithValue("@StatusId", item.StatusId);
                cmd.Parameters.AddWithValue("@TotalWeight", item.TotalWeight);
                cmd.Parameters.AddWithValue("@TotalAmount", item.TotalAmount);
                cmd.Parameters.AddWithValue("@IsDeposit", item.IsDeposit);
                cmd.Parameters.AddWithValue("@Ispayenough", item.Ispayenough);
                cmd.Parameters.AddWithValue("@PaymentMethodId", item.PaymentMethodId);
                cmd.Parameters.AddWithValue("@MAWB", item.MAWB);
                cmd.Parameters.AddWithValue("@ReceiveName", item.ReceiveName);

                cmd.Parameters.AddWithValue("@DistrictId", item.DistrictId);
                cmd.Parameters.AddWithValue("@ProvinceId", item.ProvinceId);
                cmd.Parameters.AddWithValue("@ReceiveEmail", item.ReceiveEmail);
                cmd.Parameters.AddWithValue("@ReceiveAddress", item.ReceiveAddress);
                cmd.Parameters.AddWithValue("@ReceivePhone", item.ReceivePhone);
                cmd.Parameters.AddWithValue("@Note", item.Note);
                cmd.Parameters.AddWithValue("@OrderDate", item.OrderDate);
                cmd.Parameters.AddWithValue("@RequireDate", item.RequireDate);
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Id = int.Parse(cmd.Parameters["@Id"].Value.ToString());
            }
            catch (Exception ex) {
                Id = 0;
                Console.Write(ex.Message);
            }
            return Id;
        }

        

        #endregion

    }
}
