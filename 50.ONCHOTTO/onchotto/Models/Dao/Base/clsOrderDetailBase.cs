//-- ------------------------------------------------------------
//-- Company name  :  DTGSOFT 
//-- Class name    :  clsOrderDetailBase
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
	public abstract class clsOrderDetailBase : OnChotto.Models.Dao.BaseObject
	{
		#region  Constants
		public override String EntityTableName { get { return "OrderDetail"; } set { base.EntityTableName = value; } }
		public override String EntityProcedureName { get { return "sp_OrderDetail"; } set { base.EntityProcedureName = value; } }
		public override String EntityProcedureNameUser { get { return "usp_OrderDetail"; } set { base.EntityProcedureNameUser = value; } }

		#endregion  

		#region  Variables

		#endregion  

		#region  Constructors
		public clsOrderDetailBase()
			: base(DataAccess.AppConnectionString)
		{
		}

		public clsOrderDetailBase(string connectionString)
			: base(connectionString)
		{
		}

		public clsOrderDetailBase(DataAccess dal)
			: base(dal)
		{
		}

		protected override void Initialize()
		{
			//Entity Object Name
			base.EntityTableName = EntityTableName;
			base.EntityProcedureName = EntityProcedureName;

			//Table Columns Name
			FieldNames.Add("OrderId");
			FieldNames.Add("ProductId");
			FieldNames.Add("Amount");
			FieldNames.Add("Note");
			FieldNames.Add("PriceAfter");
			FieldNames.Add("Discount");
			FieldNames.Add("FederalTax");
			FieldNames.Add("ShippingInLand");
			FieldNames.Add("TaxExport");

			//Primary Key Columns Name
			FieldKeys.Add("OrderId");
			FieldKeys.Add("ProductId");
		}
		#endregion  

		#region  Properties
		public System.Int32 OrderId
		{
			get;
			set;
		}
		public System.Int32 ProductId
		{
			get;
			set;
		}
		public System.Int32 Amount
		{
			get;
			set;
		}
		public System.String Note
		{
			get;
			set;
		}
		public System.Decimal PriceAfter
		{
			get;
			set;
		}
		public System.Decimal Discount
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
		public System.Decimal TaxExport
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
				DataAccessObject.InputParams("OrderId", OrderId);
				DataAccessObject.InputParams("ProductId", ProductId);
				DataAccessObject.InputParams("Amount", Amount);
				DataAccessObject.InputParams("Note", Note);
				DataAccessObject.InputParams("PriceAfter", PriceAfter);
				DataAccessObject.InputParams("Discount", Discount);
				DataAccessObject.InputParams("FederalTax", FederalTax);
				DataAccessObject.InputParams("ShippingInLand", ShippingInLand);
				DataAccessObject.InputParams("TaxExport", TaxExport);
				int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues, ref OutputObject);

				if (OutputObject != null && OutputObject.Length > 0)
				{
					OrderId = DataAccess.CorrectValue(OutputObject.GetValue("OrderId").ToString(), System.Int32.MinValue);
					ProductId = DataAccess.CorrectValue(OutputObject.GetValue("ProductId").ToString(), System.Int32.MinValue);
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
				DataAccessObject.InputParams("OrderId", OrderId);
				DataAccessObject.InputParams("ProductId", ProductId);
				DataAccessObject.InputParams("Amount", Amount);
				DataAccessObject.InputParams("Note", Note);
				DataAccessObject.InputParams("PriceAfter", PriceAfter);
				DataAccessObject.InputParams("Discount", Discount);
				DataAccessObject.InputParams("FederalTax", FederalTax);
				DataAccessObject.InputParams("ShippingInLand", ShippingInLand);
				DataAccessObject.InputParams("TaxExport", TaxExport);
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
				DataAccessObject.InputParams("OrderId", OrderId);
				DataAccessObject.InputParams("ProductId", ProductId);
				return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override int Delete(params object[] keyValue)
		{
			OutputObject = null;
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "Delete");
				DataAccessObject.InputParams("OrderId", keyValue[0]);
				DataAccessObject.InputParams("ProductId", keyValue[1]);
				return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}


		public override bool GetByKey(params object[] keyValue)
		{
			OutputObject = null;
			DataTable dt = new DataTable(); 
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "GetByKey");
				DataAccessObject.InputParams("OrderId", keyValue[0]);
				DataAccessObject.InputParams("ProductId", keyValue[1]);
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


		public override bool IsExists(params object[] keyValue)
		{
			OutputObject = null;
			DataTable dt = new DataTable(); 
			try
			{
				DataAccessObject.ClearParam();
				DataAccessObject.InputDefaultParams();
				DataAccessObject.InputParams("Action", "IsExists");
				DataAccessObject.InputParams("OrderId", keyValue[0]);
				DataAccessObject.InputParams("ProductId", keyValue[1]);
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
			this.OrderId = Int32.MinValue;
			this.ProductId = Int32.MinValue;
			this.Amount = Int32.MinValue;
			this.Note = String.Empty;
			this.PriceAfter = Decimal.MinValue;
			this.Discount = Decimal.MinValue;
			this.FederalTax = Decimal.MinValue;
			this.ShippingInLand = Decimal.MinValue;
			this.TaxExport = Decimal.MinValue;
		}


		public override void Fill(DataRow row)
		{
			Reset();
			if(row.Table.Columns.Contains("OrderId"))
				this.OrderId = DataAccess.CorrectValue(row["OrderId"], OrderId);
			if(row.Table.Columns.Contains("ProductId"))
				this.ProductId = DataAccess.CorrectValue(row["ProductId"], ProductId);
			if(row.Table.Columns.Contains("Amount"))
				this.Amount = DataAccess.CorrectValue(row["Amount"], Amount);
			if(row.Table.Columns.Contains("Note"))
				this.Note = DataAccess.CorrectValue(row["Note"], Note);
			if(row.Table.Columns.Contains("PriceAfter"))
				this.PriceAfter = DataAccess.CorrectValue(row["PriceAfter"], PriceAfter);
			if(row.Table.Columns.Contains("Discount"))
				this.Discount = DataAccess.CorrectValue(row["Discount"], Discount);
			if(row.Table.Columns.Contains("FederalTax"))
				this.FederalTax = DataAccess.CorrectValue(row["FederalTax"], FederalTax);
			if(row.Table.Columns.Contains("ShippingInLand"))
				this.ShippingInLand = DataAccess.CorrectValue(row["ShippingInLand"], ShippingInLand);
			if(row.Table.Columns.Contains("TaxExport"))
				this.TaxExport = DataAccess.CorrectValue(row["TaxExport"], TaxExport);
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
