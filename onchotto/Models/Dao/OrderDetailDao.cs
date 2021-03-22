using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnChotto.Models.Entities;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace OnChotto.Models.Dao
{

    public class OrderDetailDao : OnChotto.Models.Dao.clsOrderDetailBase
    {
        #region  Constants

        #endregion

        #region  Variables
        SqlConnection samcnn;
        string strSam = WebConfigurationManager.ConnectionStrings["SAMConnection"].ToString();
        #endregion

        #region  Constructors
        public OrderDetailDao()
            : base(DataAccess.AppConnectionString)
        {
            samcnn = new SqlConnection(strSam);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDetailDao(string connectionString)
            : base(connectionString)
        {
            samcnn = new SqlConnection(connectionString);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDetailDao(DataAccess dal)
            : base(dal)
        {
        }

        #endregion

        #region  Developers

        public int OrderDetailToBilling()
        {
            OutputObject = null;
            try
            {
                DataAccessObject.ClearParam();
                DataAccessObject.InputDefaultParams();
                DataAccessObject.InputParams("Action", "Insert");
                DataAccessObject.InputParams("OrderId", OrderId);
                DataAccessObject.InputParams("ProductId", ProductId);
                DataAccessObject.InputParams("Amount", Amount);
                DataAccessObject.InputParams("Note", Note);
                DataAccessObject.InputParams("PriceAfter", PriceAfter);
                DataAccessObject.InputParams("Discount", Discount);
                DataAccessObject.InputParams("FederalTax", FederalTax);
                DataAccessObject.InputParams("ShippingInLand", ShippingInLand);
                DataAccessObject.InputParams("TaxExport", TaxExport);
                int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureNameUser, DataAccessObject.ParamNames, DataAccessObject.ParamValues, ref OutputObject);

                if (OutputObject != null && OutputObject.Length > 0)
                {
                    OrderId = DataAccess.CorrectValue(OutputObject.GetValue("OrderId").ToString(), System.Int32.MinValue);
                    ProductId = DataAccess.CorrectValue(OutputObject.GetValue("ProductId").ToString(), System.Int32.MinValue);
                }

               
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return OrderId;
        }


        #endregion

    }

}