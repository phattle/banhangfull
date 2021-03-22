using OnChotto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OnChotto.Models.Dao
{
    public partial class CaptureEvent
    {
        SqlConnection cn; 
        public CaptureEvent()
        {
            string cnn = WebConfigurationManager.ConnectionStrings["DevConnection"].ToString();
            cn = new SqlConnection(cnn);
            if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                cn.Open();
        }
        public float GetBySearchID(string SearchID)
        {
            float iResult = -1;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "usp_CaptureEvents";
            cmd.Connection = this.cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GetBySearchID");
            cmd.Parameters.AddWithValue("@SearchID", SearchID); 
            cmd.Parameters.Add("@SuccessFull", SqlDbType.Int);
            cmd.Parameters["@SuccessFull"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            iResult = float.Parse(cmd.Parameters["@SuccessFull"].Value.ToString());
            return iResult;
        }
        
        public DataTable GetFirst(int Captureid)
        { 
            DataTable tbl = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_CaptureEvents";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getcommand");
                cmd.Parameters.AddWithValue("@Captureid", Captureid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return tbl;
        }
        public DataTable GetCaptureByASIN(string ASIN)
        { 
            DataTable tbl = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_CaptureEvents";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetCaptureByASIN");
                cmd.Parameters.AddWithValue("@ASIN", ASIN);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return tbl;
        }
        public int GetExitsASin(string ASIN)
        {
            int iResult = -1;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "usp_CaptureEvents";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetExitsASin");
                cmd.Parameters.AddWithValue("@ASIN", ASIN);
                cmd.Parameters.Add("@CaptureId", SqlDbType.Int);
                cmd.Parameters["@CaptureId"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                iResult = int.Parse(cmd.Parameters["@CaptureId"].Value.ToString());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return iResult;
        }
        public int SaveOrderDetailDiff(Entities.CaptureEvent item)
        {
            int iResult = -1;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "usp_CaptureEvents";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "insert");
                cmd.Parameters.AddWithValue("@TimeSeach", item.TimeSeach);
                cmd.Parameters.AddWithValue("@KeywordSeach", item.KeywordSeach);
                cmd.Parameters.AddWithValue("@IsCapture", item.IsCapture);
                cmd.Parameters.Add("@CaptureId", SqlDbType.Int);
                cmd.Parameters["@CaptureId"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                iResult = int.Parse(cmd.Parameters["@CaptureId"].Value.ToString());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return iResult;
        }
        public int Update(int Captureid, int CaptureCount = 0)
        {
            int iResult = -1;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "usp_CaptureEvents";
                cmd.Connection = this.cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "updatestatus");
                cmd.Parameters.AddWithValue("@Captureid", Captureid);
                cmd.Parameters.AddWithValue("@CaptureCount", CaptureCount);
                iResult = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return iResult;
        }
        public int SaveProductcapture(Entities.Product item, int Captureid, string CategoryName, string KeywordSeach)
        { 
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "usp_ProductCapture";
            cmd.Connection = this.cn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@CatId", item.CatId);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Slug", item.Slug);
                cmd.Parameters.AddWithValue("@ManufactId", item.ManufactId);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@Detail", item.Detail);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@TypeId", item.TypeId);
                cmd.Parameters.AddWithValue("@Images", item.Images);
                cmd.Parameters.AddWithValue("@FeaturedImage", item.FeaturedImage);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Discount", item.Discount);
                cmd.Parameters.AddWithValue("@EndDate", item.EndDate);
                cmd.Parameters.AddWithValue("@UserId", item.UserId);
                cmd.Parameters.AddWithValue("@CreateDate", item.CreateDate);
                cmd.Parameters.AddWithValue("@ExtraDiscount", item.ExtraDiscount);
                cmd.Parameters.AddWithValue("@IsNew", item.IsNew);
                cmd.Parameters.AddWithValue("@IsFeatured", item.IsFeatured);
                cmd.Parameters.AddWithValue("@IsSpecial", item.IsSpecial);
                cmd.Parameters.AddWithValue("@Views", item.Views);
                cmd.Parameters.AddWithValue("@PriceAfter", item.PriceAfter);
                cmd.Parameters.AddWithValue("@Featured", item.Featured);
                cmd.Parameters.AddWithValue("@Condition", item.Condition);
                cmd.Parameters.AddWithValue("@Actived", item.Actived);
                cmd.Parameters.AddWithValue("@MetaTitle", item.MetaTitle);
                cmd.Parameters.AddWithValue("@MetaDescription", item.MetaDescription);
                cmd.Parameters.AddWithValue("@MetaKeyword", item.MetaKeyword);
                cmd.Parameters.AddWithValue("@ASIN", item.ASIN);
                cmd.Parameters.AddWithValue("@ParentASIN", item.ParentASIN);
                cmd.Parameters.AddWithValue("@DetailPageURL", item.DetailPageURL);
                cmd.Parameters.AddWithValue("@LargeImageURL", item.LargeImageURL);
                cmd.Parameters.AddWithValue("@ProductZone", item.ProductZone);
                cmd.Parameters.AddWithValue("@SearchID", item.SearchID);
                cmd.Parameters.AddWithValue("@ChargeWeight", item.ChargeWeight);
                cmd.Parameters.AddWithValue("@WeightUnit", item.WeightUnit);
                cmd.Parameters.AddWithValue("@Size", item.Size);
                cmd.Parameters.AddWithValue("@WeightKG", item.WeightKG);
                cmd.Parameters.AddWithValue("@WeightLbs", item.WeightLbs);
                cmd.Parameters.AddWithValue("@VLWeightKG", item.VLWeightKG);
                cmd.Parameters.AddWithValue("@VLWeightLbs", item.VLWeightLbs);
                cmd.Parameters.AddWithValue("@ChargeWeightKG", item.ChargeWeightKG);
                cmd.Parameters.AddWithValue("@ChargeWeightLbs", item.ChargeWeightLbs);
                cmd.Parameters.AddWithValue("@FederalTax", item.FederalTax);
                cmd.Parameters.AddWithValue("@ShippingInLand", item.ShippingInLand);
                cmd.Parameters.AddWithValue("@TaxExport", item.TaxExport);
                cmd.Parameters.AddWithValue("@HsCodeId", item.HsCodeId);
                cmd.Parameters.AddWithValue("@PriceTaxPercentage", item.PriceTaxPercentage);
                cmd.Parameters.AddWithValue("@PriceTaxVatPercentage", item.PriceTaxVatPercentage);
                cmd.Parameters.AddWithValue("@ProductSiteId", item.ProductSiteId);
                cmd.Parameters.AddWithValue("@Dimensions", item.Dimensions);
                cmd.Parameters.AddWithValue("@Captureid", Captureid);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                cmd.Parameters.AddWithValue("@KeywordSeach", KeywordSeach);
                int intResult = cmd.ExecuteNonQuery();

                return intResult;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

        }

        public int SaveProduct(Entities.Product item, int Captureid, string CategoryName, string KeywordSeach)
        { 
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "usp_ProductCapture";
            cmd.Connection = this.cn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Action", "InsertProduct");
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@CatId", item.CatId);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Slug", item.Slug);
                cmd.Parameters.AddWithValue("@ManufactId", item.ManufactId);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@Detail", item.Detail);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@TypeId", item.TypeId);
                cmd.Parameters.AddWithValue("@Images", item.Images);
                cmd.Parameters.AddWithValue("@FeaturedImage", item.FeaturedImage);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@Discount", item.Discount);
                cmd.Parameters.AddWithValue("@EndDate", item.EndDate);
                cmd.Parameters.AddWithValue("@UserId", item.UserId);
                cmd.Parameters.AddWithValue("@CreateDate", item.CreateDate);
                cmd.Parameters.AddWithValue("@ExtraDiscount", item.ExtraDiscount);
                cmd.Parameters.AddWithValue("@IsNew", item.IsNew);
                cmd.Parameters.AddWithValue("@IsFeatured", item.IsFeatured);
                cmd.Parameters.AddWithValue("@IsSpecial", item.IsSpecial);
                cmd.Parameters.AddWithValue("@Views", item.Views);
                cmd.Parameters.AddWithValue("@PriceAfter", item.PriceAfter);
                cmd.Parameters.AddWithValue("@Featured", item.Featured);
                cmd.Parameters.AddWithValue("@Condition", item.Condition);
                cmd.Parameters.AddWithValue("@Actived", item.Actived);
                cmd.Parameters.AddWithValue("@MetaTitle", item.MetaTitle);
                cmd.Parameters.AddWithValue("@MetaDescription", item.MetaDescription);
                cmd.Parameters.AddWithValue("@MetaKeyword", item.MetaKeyword);
                cmd.Parameters.AddWithValue("@ASIN", item.ASIN);
                cmd.Parameters.AddWithValue("@ParentASIN", item.ParentASIN);
                cmd.Parameters.AddWithValue("@DetailPageURL", item.DetailPageURL);
                cmd.Parameters.AddWithValue("@LargeImageURL", item.LargeImageURL);
                cmd.Parameters.AddWithValue("@ProductZone", item.ProductZone);
                cmd.Parameters.AddWithValue("@SearchID", item.SearchID);
                cmd.Parameters.AddWithValue("@ChargeWeight", item.ChargeWeight);
                cmd.Parameters.AddWithValue("@WeightUnit", item.WeightUnit);
                cmd.Parameters.AddWithValue("@Size", item.Size);
                cmd.Parameters.AddWithValue("@WeightKG", item.WeightKG);
                cmd.Parameters.AddWithValue("@WeightLbs", item.WeightLbs);
                cmd.Parameters.AddWithValue("@VLWeightKG", item.VLWeightKG);
                cmd.Parameters.AddWithValue("@VLWeightLbs", item.VLWeightLbs);
                cmd.Parameters.AddWithValue("@ChargeWeightKG", item.ChargeWeightKG);
                cmd.Parameters.AddWithValue("@ChargeWeightLbs", item.ChargeWeightLbs);
                cmd.Parameters.AddWithValue("@FederalTax", item.FederalTax);
                cmd.Parameters.AddWithValue("@ShippingInLand", item.ShippingInLand);
                cmd.Parameters.AddWithValue("@TaxExport", item.TaxExport);
                cmd.Parameters.AddWithValue("@HsCodeId", item.HsCodeId);
                cmd.Parameters.AddWithValue("@PriceTaxPercentage", item.PriceTaxPercentage);
                cmd.Parameters.AddWithValue("@PriceTaxVatPercentage", item.PriceTaxVatPercentage);
                cmd.Parameters.AddWithValue("@ProductSiteId", item.ProductSiteId);
                cmd.Parameters.AddWithValue("@Dimensions", item.Dimensions);
                cmd.Parameters.AddWithValue("@Captureid", Captureid);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                cmd.Parameters.AddWithValue("@KeywordSeach", KeywordSeach);


                int intResult = cmd.ExecuteNonQuery();

                return intResult;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

        }
    }
}