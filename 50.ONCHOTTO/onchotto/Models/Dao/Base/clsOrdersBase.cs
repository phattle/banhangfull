//-- ------------------------------------------------------------
//-- Company name  :  DTGSOFT 
//-- Class name    :  clsOrdersBase
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
namespace OnChotto.Models.Dao
{
	public abstract class clsOrdersBase : OnChotto.Models.Dao.BaseObject
    {
		#region  Constants
		public override String EntityTableName { get { return "Orders"; } set { base.EntityTableName = value; } }
		public override String EntityProcedureName { get { return "sp_Orders"; } set { base.EntityProcedureName = value; } }
		public override String EntityProcedureNameUser { get { return "usp_Orders"; } set { base.EntityProcedureNameUser = value; } }

		#endregion  

		#region  Variables

		#endregion  

		#region  Constructors
		public clsOrdersBase()
			: base(DataAccess.AppConnectionString)
		{
		}

		public clsOrdersBase(string connectionString)
			: base(connectionString)
		{
		}

		public clsOrdersBase(DataAccess dal)
			: base(dal)
		{
		}

		protected override void Initialize()
		{
			//Entity Object Name
			base.EntityTableName = EntityTableName;
			base.EntityProcedureName = EntityProcedureName;

			//Table Columns Name
			FieldNames.Add("Id");
			FieldNames.Add("StatusId");
			FieldNames.Add("Discount");
			FieldNames.Add("ExtraDiscount");
			FieldNames.Add("TransportId");
			FieldNames.Add("ReceiveAddress");
			FieldNames.Add("ReceiveName");
			FieldNames.Add("ReceivePhone");
			FieldNames.Add("Note");
			FieldNames.Add("UserId");
			FieldNames.Add("TotalAmount");
			FieldNames.Add("OrderDate");
			FieldNames.Add("Coupon");
			FieldNames.Add("PaymentMethodId");
			FieldNames.Add("TotalOrder");
			FieldNames.Add("ReceiveEmail");
			FieldNames.Add("Deposit");
			FieldNames.Add("IsDeposit");
			FieldNames.Add("CouponDescription");
			FieldNames.Add("FederalTax");
			FieldNames.Add("ShippingInLand");
			FieldNames.Add("HandlingFee");
			FieldNames.Add("AFFee");
			FieldNames.Add("ClearanceFee");
			FieldNames.Add("ShippingFee");
			FieldNames.Add("TECSServicesFee");
			FieldNames.Add("TransactionFee");
			FieldNames.Add("VATFee");
			FieldNames.Add("ImportTax");
			FieldNames.Add("CustomFee");
			FieldNames.Add("PromotionCode");
			FieldNames.Add("PromotionValue");
			FieldNames.Add("TotalWeight");
			FieldNames.Add("RequireDate");
			FieldNames.Add("Ispayenough");

			//Primary Key Columns Name
			FieldKeys.Add("Id");
		}
		#endregion  

		#region  Properties
		public System.Int32 Id
		{
			get;
			set;
		}
		public System.Int32 StatusId
		{
			get;
			set;
		}
		public System.Decimal Discount
		{
			get;
			set;
		}
		public System.Decimal ExtraDiscount
		{
			get;
			set;
		}
		public System.Int32 TransportId
		{
			get;
			set;
		}
		public System.String ReceiveAddress
		{
			get;
			set;
		}
		public System.String ReceiveName
		{
			get;
			set;
		}
		public System.String ReceivePhone
		{
			get;
			set;
		}
		public System.String Note
		{
			get;
			set;
		}
		public System.String UserId
		{
			get;
			set;
		}
		public System.Decimal TotalAmount
		{
			get;
			set;
		}
		public System.DateTime OrderDate
		{
			get;
			set;
		}
		public System.String Coupon
		{
			get;
			set;
		}
		public System.Int32 PaymentMethodId
		{
			get;
			set;
		}
		public System.Decimal TotalOrder
		{
			get;
			set;
		}
		public System.String ReceiveEmail
		{
			get;
			set;
		}
		public System.Int32 Deposit
		{
			get;
			set;
		}
		public System.Boolean IsDeposit
		{
			get;
			set;
		}
		public System.String CouponDescription
		{
			get;
			set;
		}
		public System.Decimal FederalTax
		{
			get;
			set;
		}
		public System.Decimal ShippingInLand
		{
			get;
			set;
		}
		public System.Decimal HandlingFee
		{
			get;
			set;
		}
		public System.Decimal AFFee
		{
			get;
			set;
		}
		public System.Decimal ClearanceFee
		{
			get;
			set;
		}
		public System.Decimal ShippingFee
		{
			get;
			set;
		}
		public System.Decimal TECSServicesFee
		{
			get;
			set;
		}
		public System.Decimal TransactionFee
		{
			get;
			set;
		}
		public System.Decimal VATFee
		{
			get;
			set;
		}
		public System.Decimal ImportTax
		{
			get;
			set;
		}
		public System.Decimal CustomFee
		{
			get;
			set;
		}
		public System.String PromotionCode
		{
			get;
			set;
		}
		public System.String PromotionValue
		{
			get;
			set;
		}
		public System.Decimal TotalWeight
		{
			get;
			set;
		}
		public System.DateTime RequireDate
		{
			get;
			set;
		}
		public System.Boolean Ispayenough
		{
			get;
			set;
		}
        public System.String Noteshopper
        {
            get;
            set;
        }
        public System.String NoteCustomerService
        {
            get;
            set;
        }
        public System.String NoteWarehouseStaff
        {
            get;
            set;
        }
        #endregion

        #region  Methods


        public override int Insert()
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
				int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues, ref OutputObject);

				if (OutputObject != null && OutputObject.Length > 0)
				{
					Id = DataAccess.CorrectValue(OutputObject.GetValue("Id").ToString(), System.Int32.MinValue);
				}

				return intResult;
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int Update()
		{
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "Update");
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
				return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int Delete()
		{
			OutputObject = null;
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "Delete");
				DataAccessObject.InputParams("Id", Id);
				return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int Delete(System.Int32 id)
		{
			OutputObject = null;
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "Delete");
				DataAccessObject.InputParams("Id", id);
				return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override bool GetByKey(System.Int32 id)
		{
			OutputObject = null;
			DataTable dt = new DataTable(); 
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "GetByKey");
				DataAccessObject.InputParams("Id", id);
				dt = DataAccessObject.ExecuteDataTable(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
				Reset();
				if(dt == null || dt.Rows.Count == 0) 
					return false;

				Fill(dt.Rows[0]);
				return true;
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override bool IsExists(System.Int32 id)
		{
			OutputObject = null;
			DataTable dt = new DataTable(); 
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "IsExists");
				DataAccessObject.InputParams("Id", id);
				dt = DataAccessObject.ExecuteDataTable(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
				if(dt == null || dt.Rows.Count == 0) 
					return false;
				else
					return true;
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override DataTable GetAll()
		{
			return DataAccessObject.ExecuteDataTable(EntityProcedureName, "Action", new object[] {"GetAll"});
		}


		public override void Reset()
		{
			this.Id = Int32.MinValue;
			this.StatusId = Int32.MinValue;
			this.Discount = Decimal.MinValue;
			this.ExtraDiscount = Decimal.MinValue;
			this.TransportId = Int32.MinValue;
			this.ReceiveAddress = String.Empty;
			this.ReceiveName = String.Empty;
			this.ReceivePhone = String.Empty;
			this.Note = String.Empty;
			this.UserId = String.Empty;
			this.TotalAmount = Decimal.MinValue;
			this.OrderDate = DateTime.MinValue;
			this.Coupon = String.Empty;
			this.PaymentMethodId = Int32.MinValue;
			this.TotalOrder = Decimal.MinValue;
			this.ReceiveEmail = String.Empty;
			this.Deposit = Int32.MinValue;
			this.IsDeposit = false;
			this.CouponDescription = String.Empty;
			this.FederalTax = Decimal.MinValue;
			this.ShippingInLand = Decimal.MinValue;
			this.HandlingFee = Decimal.MinValue;
			this.AFFee = Decimal.MinValue;
			this.ClearanceFee = Decimal.MinValue;
			this.ShippingFee = Decimal.MinValue;
			this.TECSServicesFee = Decimal.MinValue;
			this.TransactionFee = Decimal.MinValue;
			this.VATFee = Decimal.MinValue;
			this.ImportTax = Decimal.MinValue;
			this.CustomFee = Decimal.MinValue;
			this.PromotionCode = String.Empty;
			this.PromotionValue = String.Empty;
			this.TotalWeight = Decimal.MinValue;
			this.RequireDate = DateTime.MinValue;
			this.Ispayenough = false;
		}


		public override void Fill(DataRow row)
		{
			Reset();
			if(row.Table.Columns.Contains("Id"))
				this.Id = DataAccess.CorrectValue(row["Id"], Id);
			if(row.Table.Columns.Contains("StatusId"))
				this.StatusId = DataAccess.CorrectValue(row["StatusId"], StatusId);
			if(row.Table.Columns.Contains("Discount"))
				this.Discount = DataAccess.CorrectValue(row["Discount"], Discount);
			if(row.Table.Columns.Contains("ExtraDiscount"))
				this.ExtraDiscount = DataAccess.CorrectValue(row["ExtraDiscount"], ExtraDiscount);
			if(row.Table.Columns.Contains("TransportId"))
				this.TransportId = DataAccess.CorrectValue(row["TransportId"], TransportId);
			if(row.Table.Columns.Contains("ReceiveAddress"))
				this.ReceiveAddress = DataAccess.CorrectValue(row["ReceiveAddress"], ReceiveAddress);
			if(row.Table.Columns.Contains("ReceiveName"))
				this.ReceiveName = DataAccess.CorrectValue(row["ReceiveName"], ReceiveName);
			if(row.Table.Columns.Contains("ReceivePhone"))
				this.ReceivePhone = DataAccess.CorrectValue(row["ReceivePhone"], ReceivePhone);
			if(row.Table.Columns.Contains("Note"))
				this.Note = DataAccess.CorrectValue(row["Note"], Note);
			if(row.Table.Columns.Contains("UserId"))
				this.UserId = DataAccess.CorrectValue(row["UserId"], UserId);
			if(row.Table.Columns.Contains("TotalAmount"))
				this.TotalAmount = DataAccess.CorrectValue(row["TotalAmount"], TotalAmount);
			if(row.Table.Columns.Contains("OrderDate"))
				this.OrderDate = DataAccess.CorrectValue(row["OrderDate"], OrderDate);
			if(row.Table.Columns.Contains("Coupon"))
				this.Coupon = DataAccess.CorrectValue(row["Coupon"], Coupon);
			if(row.Table.Columns.Contains("PaymentMethodId"))
				this.PaymentMethodId = DataAccess.CorrectValue(row["PaymentMethodId"], PaymentMethodId);
			if(row.Table.Columns.Contains("TotalOrder"))
				this.TotalOrder = DataAccess.CorrectValue(row["TotalOrder"], TotalOrder);
			if(row.Table.Columns.Contains("ReceiveEmail"))
				this.ReceiveEmail = DataAccess.CorrectValue(row["ReceiveEmail"], ReceiveEmail);
			if(row.Table.Columns.Contains("Deposit"))
				this.Deposit = DataAccess.CorrectValue(row["Deposit"], Deposit);
			if(row.Table.Columns.Contains("IsDeposit"))
				this.IsDeposit = DataAccess.CorrectValue(row["IsDeposit"], IsDeposit);
			if(row.Table.Columns.Contains("CouponDescription"))
				this.CouponDescription = DataAccess.CorrectValue(row["CouponDescription"], CouponDescription);
			if(row.Table.Columns.Contains("FederalTax"))
				this.FederalTax = DataAccess.CorrectValue(row["FederalTax"], FederalTax);
			if(row.Table.Columns.Contains("ShippingInLand"))
				this.ShippingInLand = DataAccess.CorrectValue(row["ShippingInLand"], ShippingInLand);
			if(row.Table.Columns.Contains("HandlingFee"))
				this.HandlingFee = DataAccess.CorrectValue(row["HandlingFee"], HandlingFee);
			if(row.Table.Columns.Contains("AFFee"))
				this.AFFee = DataAccess.CorrectValue(row["AFFee"], AFFee);
			if(row.Table.Columns.Contains("ClearanceFee"))
				this.ClearanceFee = DataAccess.CorrectValue(row["ClearanceFee"], ClearanceFee);
			if(row.Table.Columns.Contains("ShippingFee"))
				this.ShippingFee = DataAccess.CorrectValue(row["ShippingFee"], ShippingFee);
			if(row.Table.Columns.Contains("TECSServicesFee"))
				this.TECSServicesFee = DataAccess.CorrectValue(row["TECSServicesFee"], TECSServicesFee);
			if(row.Table.Columns.Contains("TransactionFee"))
				this.TransactionFee = DataAccess.CorrectValue(row["TransactionFee"], TransactionFee);
			if(row.Table.Columns.Contains("VATFee"))
				this.VATFee = DataAccess.CorrectValue(row["VATFee"], VATFee);
			if(row.Table.Columns.Contains("ImportTax"))
				this.ImportTax = DataAccess.CorrectValue(row["ImportTax"], ImportTax);
			if(row.Table.Columns.Contains("CustomFee"))
				this.CustomFee = DataAccess.CorrectValue(row["CustomFee"], CustomFee);
			if(row.Table.Columns.Contains("PromotionCode"))
				this.PromotionCode = DataAccess.CorrectValue(row["PromotionCode"], PromotionCode);
			if(row.Table.Columns.Contains("PromotionValue"))
				this.PromotionValue = DataAccess.CorrectValue(row["PromotionValue"], PromotionValue);
			if(row.Table.Columns.Contains("TotalWeight"))
				this.TotalWeight = DataAccess.CorrectValue(row["TotalWeight"], TotalWeight);
			if(row.Table.Columns.Contains("RequireDate"))
				this.RequireDate = DataAccess.CorrectValue(row["RequireDate"], RequireDate);
			if(row.Table.Columns.Contains("Ispayenough"))
				this.Ispayenough = DataAccess.CorrectValue(row["Ispayenough"], Ispayenough);
		}


		public override int UpdateTable(DataSet dataSource)
		{
			try
			{
				return DataAccessObject.UpdateTable(dataSource, EntityProcedureName, "Insert", "Update", "Delete");
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int UpdateTable(DataTable dataSource)
		{
			try
			{
				return DataAccessObject.UpdateTable(dataSource, EntityProcedureName, "Insert", "Update", "Delete");
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int UpdateTable(DataSet dataSource, string commandText, string[] paramNames)
		{
			try
			{
				return DataAccessObject.UpdateTable(dataSource, commandText, "Insert", "Update", "Delete", paramNames);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int UpdateTable(DataTable dataSource, string commandText, string[] paramNames)
		{
			try
			{
				return DataAccessObject.UpdateTable(dataSource, commandText, "Insert", "Update", "Delete", paramNames);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}

		#endregion  

	}
}