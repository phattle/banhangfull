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
    
    public class OrderDao : OnChotto.Models.Dao.clsOrdersBase
    {
        #region  Constants

        #endregion

        #region  Variables
        SqlConnection samcnn;
        string strSam = WebConfigurationManager.ConnectionStrings["SAMConnection"].ToString();
        #endregion

        #region  Constructors
        public OrderDao()
            : base(DataAccess.AppConnectionString)
        {
            samcnn = new SqlConnection(strSam);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDao(string connectionString)
            : base(connectionString)
        {
            samcnn = new SqlConnection(connectionString);
            if (samcnn.State == ConnectionState.Closed || samcnn.State == ConnectionState.Broken)
                samcnn.Open();
        }

        public OrderDao(DataAccess dal)
            : base(dal)
        {
        }

        #endregion

        #region  Developers
        public List<PaymentMethod> GetAllPaymentMethods()
        {
            var db = new ApplicationDbContext();
            return db.PaymentMethods.Where(x => x.Actived.HasValue && x.Actived.Value == true).ToList();
        }

        public int OrderToBilling()
        {
            OutputObject = null;
            try
            {
                DataAccessObject.ClearParam();
                DataAccessObject.InputDefaultParams();
                DataAccessObject.InputParams("Action", "Insert");
                DataAccessObject.InputParams("Id", Id);
                DataAccessObject.InputParams("StatusId", StatusId);
                DataAccessObject.InputParams("Discount", Discount);
                DataAccessObject.InputParams("ExtraDiscount", ExtraDiscount);
                DataAccessObject.InputParams("TransportId", TransportId);
                DataAccessObject.InputParams("ReceiveAddress", ReceiveAddress);
                DataAccessObject.InputParams("ReceiveName", ReceiveName);
                DataAccessObject.InputParams("ReceivePhone", ReceivePhone);
                DataAccessObject.InputParams("Note", Note);
                DataAccessObject.InputParams("UserId", UserId);
                DataAccessObject.InputParams("TotalAmount", TotalAmount);
                DataAccessObject.InputParams("OrderDate", OrderDate);
                DataAccessObject.InputParams("Coupon", Coupon);
                DataAccessObject.InputParams("PaymentMethodId", PaymentMethodId);
                DataAccessObject.InputParams("TotalOrder", TotalOrder);
                DataAccessObject.InputParams("ReceiveEmail", ReceiveEmail);
                DataAccessObject.InputParams("Deposit", Deposit);
                DataAccessObject.InputParams("IsDeposit", IsDeposit);
                DataAccessObject.InputParams("CouponDescription", CouponDescription);
                DataAccessObject.InputParams("FederalTax", FederalTax);
                DataAccessObject.InputParams("ShippingInLand", ShippingInLand);
                DataAccessObject.InputParams("HandlingFee", HandlingFee);
                DataAccessObject.InputParams("AFFee", AFFee);
                DataAccessObject.InputParams("ClearanceFee", ClearanceFee);
                DataAccessObject.InputParams("ShippingFee", ShippingFee);
                DataAccessObject.InputParams("TECSServicesFee", TECSServicesFee);
                DataAccessObject.InputParams("TransactionFee", TransactionFee);
                DataAccessObject.InputParams("VATFee", VATFee);
                DataAccessObject.InputParams("ImportTax", ImportTax);
                DataAccessObject.InputParams("CustomFee", CustomFee);
                DataAccessObject.InputParams("PromotionCode", PromotionCode);
                DataAccessObject.InputParams("PromotionValue", PromotionValue);
                DataAccessObject.InputParams("TotalWeight", TotalWeight);
                DataAccessObject.InputParams("RequireDate", RequireDate);
                DataAccessObject.InputParams("Ispayenough", Ispayenough);
                DataAccessObject.InputParams("Noteshopper", Noteshopper);
                DataAccessObject.InputParams("NoteCustomerService", NoteCustomerService);
                DataAccessObject.InputParams("NoteWarehouseStaff", NoteWarehouseStaff);
                int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues, ref OutputObject);

                if (OutputObject != null && OutputObject.Length > 0)
                {
                    Id = DataAccess.CorrectValue(OutputObject.GetValue("Id").ToString(), System.Int32.MinValue);
                }

               
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return Id;
        }


        #endregion

    }

}