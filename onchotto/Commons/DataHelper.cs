using OnChotto.Models;
using OnChotto.Models.Amazon;
using OnChotto.Models.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace OnChotto.Commons
{
    public static class DataHelper
    {

        #region CorrectValue
        public static bool IsNullOrEmpty(object value)
        {
            try
            {
                if (value == null || value is DBNull)
                    return true;

                if (value.GetType().Equals(Type.GetType("System.String")))
                    return string.IsNullOrEmpty(value.ToString());

                if (value.GetType().Equals(Type.GetType("System.Boolean")))
                    return (bool.Parse(value.ToString()) == false);

                if (value.GetType().Equals(Type.GetType("System.Byte")))
                    return (byte.Parse(value.ToString()) == byte.MinValue);

                if (value.GetType().Equals(Type.GetType("System.SByte")))
                    return (SByte.Parse(value.ToString()) == SByte.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Int16")))
                    return (Int16.Parse(value.ToString()) == Int16.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Int32")))
                    return (Int32.Parse(value.ToString()) == Int32.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Int64")))
                    return (Int64.Parse(value.ToString()) == Int64.MinValue);

                if (value.GetType().Equals(Type.GetType("System.UInt16")))
                    return (UInt16.Parse(value.ToString()) == UInt16.MinValue);

                if (value.GetType().Equals(Type.GetType("System.UInt32")))
                    return (UInt32.Parse(value.ToString()) == UInt32.MinValue);

                if (value.GetType().Equals(Type.GetType("System.UInt64")))
                    return (UInt64.Parse(value.ToString()) == UInt64.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Single")))
                    return (Single.Parse(value.ToString()) == Single.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Double")))
                    return (Double.Parse(value.ToString()) == Double.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Decimal")))
                    return (Decimal.Parse(value.ToString()) == Decimal.MinValue);

                if (value.GetType().Equals(Type.GetType("System.DateTime")))
                    return (DateTime.Parse(value.ToString()) == DateTime.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Char")))
                    return (Char.Parse(value.ToString()) == Char.MinValue);

                if (value.GetType().Equals(Type.GetType("System.Guid")))
                    return ((Guid)value == Guid.Empty);

            }
            catch //(Exception ex)
            {
                return false;
            }
            return false;
        }

        //private void CorrectSQL(IDbDataParameter DataParameter, object value)
        private static void CorrectSQL(DbParameter DataParameter, object value)
        {
            //if (value is Byte)
            //{
            //    Byte valueCorrect = (Byte)value;
            //    if (valueCorrect == Byte.MinValue)
            //        DataParameter.Value = DBNull.Value;
            //    else
            //        DataParameter.Value = value;
            //}
            //else

            if (value is Byte[])
            {
                Byte[] valueCorrect = (Byte[])value;
                if (valueCorrect == null || valueCorrect.Length == 0)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else
                if (value is UInt16)
            {
                UInt16 valueCorrect = (UInt16)value;
                if (valueCorrect == UInt16.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is UInt32)
            {
                UInt32 valueCorrect = (UInt32)value;
                if (valueCorrect == UInt32.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is UInt64)
            {
                UInt64 valueCorrect = (UInt64)value;
                if (valueCorrect == UInt64.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Int16)
            {
                Int16 valueCorrect = (Int16)value;
                if (valueCorrect == Int16.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Int32)
            {
                Int32 valueCorrect = (Int32)value;
                if (valueCorrect == Int32.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Int64)
            {
                Int64 valueCorrect = (Int64)value;
                if (valueCorrect == Int64.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Single)
            {
                float valueCorrect = (Single)value;
                if (valueCorrect == Single.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Double)
            {
                Double valueCorrect = (Double)value;
                if (valueCorrect == Double.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Decimal)
            {
                Decimal valueCorrect = (Decimal)value;
                if (valueCorrect == Decimal.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is Guid)
            {
                Guid valueCorrect = (Guid)value;
                if (valueCorrect == Guid.Empty)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is DateTime)
            {
                DateTime valueCorrect = (DateTime)value;
                if (valueCorrect == DateTime.MinValue)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value is String)
            {
                String valueCorrect = (String)value;
                if (valueCorrect == String.Empty)
                    DataParameter.Value = DBNull.Value;
                else
                    DataParameter.Value = value;
            }
            else if (value == null)
                DataParameter.Value = DBNull.Value;
            else
                DataParameter.Value = value;

        }

        public static void CorrectValue(object obj, System.Reflection.PropertyInfo propertyInfo, object value)
        {
            switch (propertyInfo.PropertyType.Name.ToLower())
            {
                case "boolean":
                case "bool":
                    propertyInfo.SetValue(obj, CorrectValue(value, false), null);
                    break;
                case "string":
                    if (value == null || value == DBNull.Value)
                        propertyInfo.SetValue(obj, string.Empty, null);
                    else
                        propertyInfo.SetValue(obj, value, null);
                    break;
                case "date":
                case "datetime":
                    propertyInfo.SetValue(obj, CorrectValue(value, DateTime.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, DateTime.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToDateTime(value), null);
                    break;
                case "byte[]":
                    propertyInfo.SetValue(obj, CorrectValue(value, new byte[] { }), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, new byte[] { }, null);
                    //else
                    //    propertyInfo.SetValue(obj, value, null);
                    break;
                case "byte":
                    propertyInfo.SetValue(obj, CorrectValue(value, Byte.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, byte.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToByte(value), null);
                    break;
                case "uint16":
                    propertyInfo.SetValue(obj, CorrectValue(value, UInt16.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, UInt16.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToUInt16(value), null);
                    break;
                case "uint32":
                    propertyInfo.SetValue(obj, CorrectValue(value, UInt32.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, UInt32.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToUInt32(value), null);
                    break;
                case "uint64":
                    propertyInfo.SetValue(obj, CorrectValue(value, UInt32.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, UInt64.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToUInt64(value), null);
                    break;
                case "int16":
                    propertyInfo.SetValue(obj, CorrectValue(value, Int16.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Int16.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToInt16(value), null);
                    break;
                case "int32":
                    propertyInfo.SetValue(obj, CorrectValue(value, Int32.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Int32.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToInt32(value), null);
                    break;
                case "int64":
                    propertyInfo.SetValue(obj, CorrectValue(value, Int64.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Int64.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToInt64(value), null);
                    break;
                case "single":
                    propertyInfo.SetValue(obj, CorrectValue(value, Single.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Single.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToSingle(value), null);
                    break;
                case "double":
                    propertyInfo.SetValue(obj, CorrectValue(value, Double.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Double.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToDouble(value), null);
                    break;
                case "decimal":
                    propertyInfo.SetValue(obj, CorrectValue(value, Decimal.MinValue), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Decimal.MinValue, null);
                    //else
                    //    propertyInfo.SetValue(obj, Convert.ToDecimal(value), null);
                    break;
                case "Guid":
                    propertyInfo.SetValue(obj, CorrectValue(value, Guid.Empty), null);
                    //if (value == null || value == DBNull.Value)
                    //    propertyInfo.SetValue(obj, Guid.Empty, null);
                    //else
                    //    propertyInfo.SetValue(obj, value, null);
                    break;
                default:
                    //propertyInfo.SetValue(obj, value, null);
                    break;
            }
        }

        public static object CorrectValue(object objSource, object returnValue)
        {
            object value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = objSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static String CorrectValue(object objSource, String returnValue)
        {
            String value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = objSource.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static bool CorrectValue(object objSource, bool returnValue)
        {
            bool value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else if (objSource.ToString() == "0")
                    value = false;
                else if (objSource.ToString() == "1")
                    value = true;
                else
                    value = Convert.ToBoolean(objSource);
                //value = bool.Parse(objSource.ToString());
                //else if (!(bool.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Byte CorrectValue(object objSource, Byte returnValue)
        {
            Byte value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToByte(objSource);

                //value = Byte.Parse(objSource.ToString());
                //else if (!(Byte.TryParse(objSource.ToString(), out value)))
                //        value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Byte[] CorrectValue(object objSource, Byte[] returnValue)
        {

            Byte[] value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = (objSource as byte[]);

                //value = Convert.ToByte(objSource);
                //value = Byte.Parse(objSource.ToString());
                //else if (!(Byte.TryParse(objSource.ToString(), out value)))
                //        value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static SByte CorrectValue(object objSource, SByte returnValue)
        {
            SByte value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToSByte(objSource);
                //value = SByte.Parse(objSource.ToString());
                //else if (!(SByte.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Int16 CorrectValue(object objSource, Int16 returnValue)
        {
            Int16 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToInt16(objSource);
                //value = Int16.Parse(objSource.ToString());
                //else if (!(Int16.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Int32 CorrectValue(object objSource, Int32 returnValue)
        {
            Int32 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToInt32(objSource);
                //value = Int32.Parse(objSource.ToString());
                //else if (!(Int32.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Int64 CorrectValue(object objSource, Int64 returnValue)
        {
            Int64 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToInt64(objSource);
                //value = Convert.ToInt64(objSource);
                //value = Int64.Parse(objSource.ToString());
                //else if (!(Int64.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static UInt16 CorrectValue(object objSource, UInt16 returnValue)
        {
            UInt16 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToUInt16(objSource);
                //value = UInt16.Parse(objSource.ToString());
                //else if (!(UInt16.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static UInt32 CorrectValue(object objSource, UInt32 returnValue)
        {
            UInt32 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToUInt32(objSource);
                //value = UInt32.Parse(objSource.ToString());
                //else if (!(UInt32.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static UInt64 CorrectValue(object objSource, UInt64 returnValue)
        {
            UInt64 value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToUInt64(objSource);
                //value = UInt64.Parse(objSource.ToString());
                //else if (!(UInt64.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Single CorrectValue(object objSource, Single returnValue)
        {
            Single value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToSingle(objSource);
                //value = Single.Parse(objSource.ToString());
                //else if (!(Single.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Double CorrectValue(object objSource, Double returnValue)
        {
            Double value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToDouble(objSource);
                //value = Double.Parse(objSource.ToString());
                //else if (!(Double.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static Decimal CorrectValue(object objSource, Decimal returnValue)
        {
            Decimal value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToDecimal(objSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static DateTime CorrectValue(object objSource, DateTime returnValue)
        {
            DateTime value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToDateTime(objSource);
                //value = DateTime.Parse(objSource.ToString());
                //else if (!(DateTime.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;

            //try
            //{
            //    if (objSource == null || objSource is DBNull)
            //        return DateTime.Parse("1/1/1900");
            //    if (String.IsNullOrEmpty(objsource.ToString()))
            //        returnvalue = DateTime.Parse("1/1/1900");
            //    else
            //        returnvalue = DateTime.Parse(objsource.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return returnvalue;
        }

        public static Char CorrectValue(object objSource, Char returnValue)
        {
            Char value = returnValue;
            try
            {
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = Convert.ToChar(objSource);
                //value = Char.Parse(objSource.ToString());
                //else if (!(Char.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public static decimal ConvertLbsToKgs(decimal Lbs)
        {
            var num = ((Lbs * (decimal)454.00) / (decimal)1000.00);
            return Math.Round(num, 2);
        }
        public static decimal ConvertKgsToLbs(decimal Lbs)
        {
            var num = ((Lbs * (decimal)1000.00) / (decimal)454.00);
            return Math.Round(num, 3);
        }

        public static Guid CorrectValue(object objSource, Guid returnValue)
        {
            Guid value = returnValue;
            try
            {
                //returnValue.ToString();
                if (objSource == null || objSource is DBNull)
                    value = returnValue;
                else if (String.IsNullOrEmpty(objSource.ToString()))
                    value = returnValue;
                else
                    value = (Guid)objSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        #endregion CorrectValue

        public static decimal CurrRank { get { return getRate(); } }
        public static XmlDocument GetXML(string link)
        {
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(link);
                request.Accept = "*/*";
                request.Method = "GET";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                request.Referer = "http://www.vietcombank.com.vn/ExchangeRates/?lang=en";
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                XmlDocument doc = new XmlDocument();
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    doc.Load(stream);
                }
                response.Close();
                return doc;
            }
            catch (Exception ex) { Models.Dao.Log.Write(ex); }
            return null;
        }
        public static decimal getRate(string bankName = "VCB")
        {
            decimal USD = 23350;
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var DateRate = DateTime.Now.Date;
                var result = db.ExchangeRates.Where(p => p.DateTime == DateRate);
                if (result == null || result.Count() <= 0)
                {
                    string link = @"http://www.vietcombank.com.vn/ExchangeRates/ExrateXML.aspx";
                    XmlDocument doc = GetXML(link);
                    foreach (XmlNode node in doc.GetElementsByTagName("Exrate"))
                    {
                        string ma = node.Attributes[0].Value;
                        if (ma == "USD" || ma == "JPY" || ma == "AUD")
                        {
                            string Exchang = node.Attributes[3].Value;
                            if (ma == "USD")
                                USD = CorrectValue(Exchang, decimal.One);
                            ExchangeRates rate = new ExchangeRates();
                            rate.Code = ma;
                            rate.ExchangeRate = CorrectValue(Exchang, decimal.MinValue);
                            rate.DateTime = CorrectValue(DateRate, DateTime.Now.Date);
                            rate.FiscalMonth = rate.DateTime.Month;
                            rate.FiscalYear = rate.DateTime.Year;
                            db.ExchangeRates.Add(rate);
                        }
                    }
                    db.SaveChanges();
                }
                else
                {
                    foreach (var item in result)
                    {
                        USD = item.ExchangeRate;
                    }
                }
            }
            catch (Exception ex)
            {
                Models.Dao.Log.Write(ex);
                return USD;
            }
            return USD;
        }

        static string GetCurrentPageUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }
        public static string GetVisitorDetails()
        {
            string varIPAddress = string.Empty;
            string varVisitorCountry = string.Empty;
            string varIpAddress = string.Empty;
            varIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(varIpAddress))
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    varIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
            }
            if (varIPAddress == "" || varIPAddress == null)
            {
                if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    varIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            return varIpAddress;
        }

        public static string GetCountryCode()
        {
            try
            {
                string IP = GetVisitorDetails();
                var url = "http://freegeoip.net/json/" + IP;
                var request = System.Net.WebRequest.Create(url);
                using (WebResponse wrs = request.GetResponse())
                using (Stream stream = wrs.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var obj = JObject.Parse(json);
                    //var City = (string)obj["city"];
                    // - For Country = (string)obj["region_name"];                    
                    var CountryCode = (string)obj["country_code"];
                    return (CountryCode);
                }
            }
            catch
            {
                return "en-US";
            }
        }
        public static AmazonSearchIndex getSearchIndex(string SearchIndex)
        {
            switch (SearchIndex)
            {
                case "All":
                    return AmazonSearchIndex.All;

                case "Apparel":
                    return AmazonSearchIndex.Apparel;

                case "Automotive":
                    return AmazonSearchIndex.Automotive;

                case "Baby":
                    return AmazonSearchIndex.Baby;

                case "Beauty":

                case "Blended":
                    return AmazonSearchIndex.Blended;

                case "Books":
                    return AmazonSearchIndex.Books;

                case "Classical":
                    return AmazonSearchIndex.Classical;

                case "DigitalMusic":
                    return AmazonSearchIndex.DigitalMusic;

                case "DVD":
                    return AmazonSearchIndex.DVD;

                case "Electronics":
                    return AmazonSearchIndex.Electronics;

                case "ForeignBooks":
                    return AmazonSearchIndex.ForeignBooks;

                case "GourmetFood":
                    return AmazonSearchIndex.GourmetFood;

                case "Grocery":
                    return AmazonSearchIndex.Grocery;

                case "HealthPersonalCare":
                    return AmazonSearchIndex.HealthPersonalCare;

                case "Hobbies":
                    return AmazonSearchIndex.Hobbies;

                case "HomeGarden":
                    return AmazonSearchIndex.HomeGarden;

                case "Industrial":
                    return AmazonSearchIndex.Industrial;

                case "Jewelry":
                    return AmazonSearchIndex.Jewelry;

                case "KindleStore":
                    return AmazonSearchIndex.KindleStore;

                case "Kitchen":
                    return AmazonSearchIndex.Kitchen;

                case "Magazines":
                    return AmazonSearchIndex.Magazines;

                case "Merchants":
                    return AmazonSearchIndex.Merchants;

                case "Miscellaneous":
                    return AmazonSearchIndex.Miscellaneous;

                case "MP3Downloads":
                    return AmazonSearchIndex.MP3Downloads;

                case "Music":
                    return AmazonSearchIndex.Music;

                case "MusicalInstruments":
                    return AmazonSearchIndex.MusicalInstruments;

                case "MusicTracks":
                    return AmazonSearchIndex.MusicTracks;

                case "OfficeProducts":
                    return AmazonSearchIndex.OfficeProducts;

                case "OutdoorLiving":
                    return AmazonSearchIndex.OutdoorLiving;

                case "PCHardware":
                    return AmazonSearchIndex.PCHardware;

                case "PetSupplies":
                    return AmazonSearchIndex.PetSupplies;

                case "Photo":
                    return AmazonSearchIndex.Photo;

                case "Software":
                    return AmazonSearchIndex.Software;

                case "SoftwareVideoGames":
                    return AmazonSearchIndex.SoftwareVideoGames;

                case "SportingGoods":
                    return AmazonSearchIndex.SportingGoods;

                case "Tools":
                    return AmazonSearchIndex.Tools;

                case "Toys":
                    return AmazonSearchIndex.Toys;

                case "VHS":
                    return AmazonSearchIndex.VHS;

                case "Video":
                    return AmazonSearchIndex.Video;


                case "VideoGames":
                    return AmazonSearchIndex.VideoGames;

                case "Watches":
                    return AmazonSearchIndex.Watches;

                case "Wireless":
                    return AmazonSearchIndex.Wireless;

                case "WirelessAccessories":
                    return AmazonSearchIndex.WirelessAccessories;

            }
            return AmazonSearchIndex.All;
        }

    }
}