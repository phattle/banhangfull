using System;
using System.Data;
using System.Data.Common;

namespace OnChotto.Models.Dao
{
    public class Common
    {
        #region CorrectValue
        //private void CorrectSQL(IDbDataParameter DataParameter, object value)
        private void CorrectSQL(DbParameter DataParameter, object value)
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
                else if (!(bool.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Byte.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(SByte.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Int16.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Int32.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Int64.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(UInt16.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(UInt32.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(UInt64.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Single.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Double.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Decimal.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(DateTime.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
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
                else if (!(Char.TryParse(objSource.ToString(), out value)))
                    value = returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
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

        #region GetDataNullValue
        public static object GetValueNull(string dataTypeName)
        {

            return GetValueNull(dataTypeName, false);

        }

        public static object GetValueNull(string dataTypeName, bool returnNullValue)
        {
            dataTypeName = dataTypeName.Trim().ToLower();

            //if (value == null || value is DBNull) return true;

            if (dataTypeName.Equals("System.String".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return string.Empty;
            //return (returnNullValue == true ? null : string.Empty);

            if (dataTypeName.Equals("System.Boolean".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return false;
            //return false;

            if (dataTypeName.Equals("System.Byte".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return byte.MinValue;
            //return byte.MinValue;

            if (dataTypeName.Equals("System.SByte".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return SByte.MinValue;
            //return SByte.MinValue;

            if (dataTypeName.Equals("System.Int16".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Int16.MinValue;
            //return Int16.MinValue;

            if (dataTypeName.Equals("System.Int32".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Int32.MinValue;
            //return null;
            //return Int32.MinValue;

            if (dataTypeName.Equals("System.Int64".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Int64.MinValue;
            //return Int64.MinValue;

            if (dataTypeName.Equals("System.UInt16".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return UInt16.MinValue;
            //return UInt16.MinValue;

            if (dataTypeName.Equals("System.UInt32".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return UInt32.MinValue;
            //return UInt32.MinValue;

            if (dataTypeName.Equals("System.UInt64".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return UInt64.MinValue;
            //return UInt64.MinValue;

            if (dataTypeName.Equals("System.Single".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Single.MinValue;
            //return Single.MinValue;

            if (dataTypeName.Equals("System.Double".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Double.MinValue;
            //return Double.MinValue;

            if (dataTypeName.Equals("System.Decimal".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Decimal.MinValue;
            //return Decimal.MinValue;

            if (dataTypeName.Equals("System.DateTime".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return DateTime.MinValue;
            //return DateTime.MinValue;

            if (dataTypeName.Equals("System.Char".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Char.MinValue;
            //return Char.MinValue;

            if (dataTypeName.Equals("System.Guid".ToLower()))
                if (returnNullValue)
                    return null;
                else
                    return Guid.Empty;
            //return Guid.Empty;

            if (returnNullValue)
                return null;
            else
                return null;

        }

        public static object GetValueDBNull(string dataTypeName)
        {

            return GetValueDBNull(dataTypeName, false);

        }

        public static object GetValueDBNull(string dataTypeName, bool returnDBNull)
        {
            dataTypeName = dataTypeName.Trim().ToLower();

            //if (value == null || value is DBNull) return true;

            if (dataTypeName.Equals("System.String".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return string.Empty;
            //return (returnDBNull == true ? DBNull.Value : string.Empty);

            if (dataTypeName.Equals("System.Boolean".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return false;
            //return false;

            if (dataTypeName.Equals("System.Byte".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return byte.MinValue;
            //return byte.MinValue;

            if (dataTypeName.Equals("System.SByte".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return SByte.MinValue;
            //return SByte.MinValue;

            if (dataTypeName.Equals("System.Int16".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Int16.MinValue;
            //return Int16.MinValue;

            if (dataTypeName.Equals("System.Int32".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Int32.MinValue;
            //return null;
            //return Int32.MinValue;

            if (dataTypeName.Equals("System.Int64".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Int64.MinValue;
            //return Int64.MinValue;

            if (dataTypeName.Equals("System.UInt16".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return UInt16.MinValue;
            //return UInt16.MinValue;

            if (dataTypeName.Equals("System.UInt32".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return UInt32.MinValue;
            //return UInt32.MinValue;

            if (dataTypeName.Equals("System.UInt64".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return UInt64.MinValue;
            //return UInt64.MinValue;

            if (dataTypeName.Equals("System.Single".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Single.MinValue;
            //return Single.MinValue;

            if (dataTypeName.Equals("System.Double".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Double.MinValue;
            //return Double.MinValue;

            if (dataTypeName.Equals("System.Decimal".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Decimal.MinValue;
            //return Decimal.MinValue;

            if (dataTypeName.Equals("System.DateTime".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return DateTime.MinValue;
            //return DateTime.MinValue;

            if (dataTypeName.Equals("System.Char".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Char.MinValue;
            //return Char.MinValue;

            if (dataTypeName.Equals("System.Guid".ToLower()))
                if (returnDBNull)
                    return DBNull.Value;
                else
                    return Guid.Empty;
            //return Guid.Empty;

            if (returnDBNull)
                return DBNull.Value;
            else
                return null;

        }
        #endregion GetDataNullValue

        #region Utility Method
        public static DataTable CloneDataTable(DataTable tblSource, Boolean _AddBlankRow)
        {
            try
            {
                if (tblSource == null) return null;
                DataTable tbl = tblSource.Copy();
                if (_AddBlankRow)
                {
                    DataRow r = tbl.NewRow();
                    tbl.Rows.InsertAt(r, 0);
                }
                return tbl;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

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

        public static bool HasChangedDataTable(DataTable table)
        {
            if (table == null) return false;

            return (table.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Detached | DataRowState.Modified) != null);

        }
        public static void AddNewBlankRow(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Columns.Count == 0)
                return;

            try
            {
                DataRow NewRow = dataTable.NewRow();

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    NewRow[i] = GetValueDBNull(dataTable.Columns[i].DataType.FullName, true);
                }

                dataTable.Rows.InsertAt(NewRow, 0);
                dataTable.AcceptChanges();
            }
            catch { }

        }

        public static void AddNewBlankRow(DataTable dataTable, int position)
        {
            if (dataTable == null || dataTable.Columns.Count == 0)
                return;

            try
            {
                DataRow NewRow = dataTable.NewRow();

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    NewRow[i] = GetValueDBNull(dataTable.Columns[i].DataType.FullName, true);
                }

                if (position < 0)
                    position = 0;
                if (position > dataTable.Rows.Count)
                    position = dataTable.Rows.Count;

                dataTable.Rows.InsertAt(NewRow, position);
                dataTable.AcceptChanges();
            }
            catch { }

        }

        public static void SetAddedRowState(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            try
            {
                dataTable.AcceptChanges();

                foreach (DataRow row in dataTable.Rows)
                    row.SetAdded();
            }
            catch { }
        }

        public static void SetAddedRowState(DataTable dataTable, bool setAllRow)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            try
            {
                if (setAllRow)
                    dataTable.AcceptChanges();

                foreach (DataRow row in dataTable.Rows)
                    if (setAllRow)
                        row.SetAdded();
                    else if (row.RowState == DataRowState.Unchanged)
                        row.SetAdded();
            }
            catch { }
        }

        public static void SetDeletedRowState(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            try
            {
                dataTable.AcceptChanges();

                foreach (DataRow row in dataTable.Rows)
                    row.Delete();
            }
            catch { }
        }

        public static void SetModifiedRowState(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            try
            {
                dataTable.AcceptChanges();

                foreach (DataRow row in dataTable.Rows)
                    row.SetModified();
            }
            catch { }
        }

        public static void SetModifiedRowState(DataTable dataTable, bool setAllRow)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            try
            {
                if (setAllRow)
                    dataTable.AcceptChanges();

                foreach (DataRow row in dataTable.Rows)
                    if (setAllRow)
                        row.SetModified();
                    else if (row.RowState == DataRowState.Unchanged)
                        row.SetModified();
            }
            catch { }
        }
        #endregion

    }
}
