using System;
using System.Data.Odbc;
using System.Data.OleDb; 
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace OnChotto.Models.Dao
{
    #pragma warning disable 618
    //public sealed class clsCacheDB : IDisposable
    public sealed class DBCache : IDisposable
    {
        private DBCache() { }

        private static Hashtable ParameterCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable TableColumnCache = Hashtable.Synchronized(new Hashtable());
        private static Hashtable ConnectionStringCache = Hashtable.Synchronized(new Hashtable());

        //****************************************************************************************************
        //
        // resolve at run time the appropriate set of SqlParameters for a stored procedure
        // 
        // param name="connectionString" a valid connection string for a SqlConnection 
        // param name="spName" the name of the stored procedure 
        // param name="includeReturnValueParameter" whether or not to include their return value parameter 
        //
        //****************************************************************************************************

        //private static IDbDataParameter[] DiscoverSpParameterSet(DataProvider dataType, string connectionString, string spName, bool includeReturnValueParameter)
        private static DbParameter[] DiscoverSpParameterSet(DataProvider dataType, string connectionString, string procedureName, bool includeReturnValueParameter)
        {
            //SqlConnection cn = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand(spName,cn);
            //SqlParameter[] discoveredParameters;

            DbConnection cn = DBFactory.GetConnection(connectionString, dataType);
            DbCommand cmd = DBFactory.GetCommand(dataType);
            DbParameter[] discoveredParameters;

            try
            {
                cn.ConnectionString = connectionString;
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = cn;
                cmd.CommandText = procedureName;
                switch (dataType)
                {
                    case DataProvider.Odbc:
                        OdbcCommandBuilder.DeriveParameters((OdbcCommand)cmd); break;
                    case DataProvider.Oledb:
                        OleDbCommandBuilder.DeriveParameters((OleDbCommand)cmd); break;
                     
                    case DataProvider.Sql:
                        SqlCommandBuilder.DeriveParameters((SqlCommand)cmd); break;
                }

                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }

                discoveredParameters = new DbParameter[cmd.Parameters.Count]; ;
                cmd.Parameters.CopyTo(discoveredParameters, 0);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                cn.Close();
                cmd.Dispose();
            }

            return discoveredParameters;
        }

        //private static IDbDataParameter[] CloneParameters(IDbDataParameter[] originalParameters)
        private static DbParameter[] CloneParameters(DbParameter[] originalParameters)
        {
            //deep copy of cached SqlParameter array
            DbParameter[] clonedParameters = new DbParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (DbParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        //*********************************************************************
        //
        // add parameter array to the cache
        //
        // param name="connectionString" a valid connection string for a SqlConnection 
        // param name="commandText" the stored procedure name or T-SQL command 
        // param name="commandParameters" an array of SqlParamters to be cached 
        //
        //*********************************************************************

        public static void CacheParameterSet(string connectionString, string commandText, params IDbDataParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            ParameterCache[hashKey] = commandParameters;
        }

        //*********************************************************************
        //
        // Retrieve a parameter array from the cache
        // 
        // param name="connectionString" a valid connection string for a SqlConnection 
        // param name="commandText" the stored procedure name or T-SQL command 
        // returns an array of SqlParamters
        //
        //*********************************************************************

        //public static IDbDataParameter[] GetCachedParameterSet(string connectionString, string commandText)
        public static DbParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            DbParameter[] cachedParameters = (DbParameter[])ParameterCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        //*********************************************************************
        //
        // Retrieves the set of SqlParameters appropriate for the stored procedure
        // 
        // This method will query the database for this information, and then store it in a cache for future requests.
        // 
        // param name="connectionString" a valid connection string for a SqlConnection 
        // param name="spName" the name of the stored procedure 
        // returns an array of SqlParameters
        //
        //*********************************************************************

        //public static IDbDataParameter[] GetSpParameterSet(DataProvider dataType, string connectionString, string procedureName)
        public static DbParameter[] GetSpParameterSet(DataProvider dataType, string connectionString, string procedureName)
        {
            return GetSpParameterSet(dataType, connectionString, procedureName, false);
        }

        //*********************************************************************
        //
        // Retrieves the set of SqlParameters appropriate for the stored procedure
        // 
        // This method will query the database for this information, and then store it in a cache for future requests.
        // 
        // param name="connectionString" a valid connection string for a SqlConnection 
        // param name="spName" the name of the stored procedure 
        // param name="includeReturnValueParameter" a bool value indicating whether the return value parameter should be included in the results 
        // returns an array of SqlParameters
        //
        //*********************************************************************

        //public static IDbDataParameter[] GetSpParameterSet(DataProvider dataType, string connectionString, string procedureName, bool includeReturnValueParameter)
        public static DbParameter[] GetSpParameterSet(DataProvider dataType, string connectionString, string procedureName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + procedureName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            DbParameter[] cachedParameters;

            if (DatabseCacheEnabled)
            {
                cachedParameters = (DbParameter[])ParameterCache[hashKey];

                if (cachedParameters == null)
                {
                    cachedParameters = (DbParameter[])(ParameterCache[hashKey] = DiscoverSpParameterSet(dataType, connectionString, procedureName, includeReturnValueParameter));
                }
            }
            else
                cachedParameters = DiscoverSpParameterSet(dataType, connectionString, procedureName, includeReturnValueParameter);

            return CloneParameters(cachedParameters);
        }

        public static void ResetParameterCache()
        {
            if (ParameterCache != null)
                ParameterCache.Clear();

            if (TableColumnCache != null)
                TableColumnCache.Clear();
        }

        public static string GetConnectionString(string connectionStringItem)
        {
            string hashKey = connectionStringItem;
            string cachedConnectionString;
            try
            {
                cachedConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DefaultConnection"].ToString();                 
                return cachedConnectionString;
            }
            catch //(Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetConnectionString(DataTable dataConnectionString)
        {
            try
            {
                string ConnectionString = string.Empty;

                if (dataConnectionString == null
                    || dataConnectionString.Columns.Count == 0
                    || dataConnectionString.Rows.Count == 0)
                {
                    return ConnectionString;
                }

                int intTimeout;
                if (dataConnectionString.Columns.Contains("CommandTimeout") && dataConnectionString.Rows[0]["CommandTimeout"].ToString().Trim() != "")
                {
                    try
                    {
                        intTimeout = int.Parse(dataConnectionString.Rows[0]["CommandTimeout"].ToString().Trim());
                        if (intTimeout > OnChotto.Models.Dao.DataAccess.DefaultCommandTimeout)
                            OnChotto.Models.Dao.DataAccess.CommandTimeout = intTimeout;
                    }
                    catch { }
                }

                intTimeout = OnChotto.Models.Dao.DataAccess.DefaultConnectionTimeout;
                if (dataConnectionString.Columns.Contains("ConnectionTimeout") && dataConnectionString.Rows[0]["ConnectionTimeout"].ToString().Trim() != "")
                {
                    try
                    {
                        intTimeout = int.Parse(dataConnectionString.Rows[0]["ConnectionTimeout"].ToString().Trim());
                    }
                    catch { }
                }


                if (IsEncryptConnection(dataConnectionString))
                {
                    ConnectionString = "Password=" + Encryptor.Decrypt(dataConnectionString.Rows[0]["Password"].ToString())
                        + ";Persist Security Info=True;User ID=" + dataConnectionString.Rows[0]["UserId"].ToString()
                        + ";Initial Catalog=" + dataConnectionString.Rows[0]["InitialCatalog"].ToString()
                        + ";Data Source=" + dataConnectionString.Rows[0]["ServerName"].ToString() + ";";
                }
                else
                {
                    ConnectionString = "Password=" + dataConnectionString.Rows[0]["Password"].ToString()
                        + ";Persist Security Info=True;User ID=" + dataConnectionString.Rows[0]["UserId"].ToString()
                        + ";Initial Catalog=" + dataConnectionString.Rows[0]["InitialCatalog"].ToString()
                        + ";Data Source=" + dataConnectionString.Rows[0]["ServerName"].ToString() + ";";
                }

                if (intTimeout > OnChotto.Models.Dao.DataAccess.DefaultConnectionTimeout)
                    ConnectionString += "Connect Timeout=" + intTimeout.ToString() + ";";

                return ConnectionString;
            }
            catch //(Exception ex)
            {
                return string.Empty;
            }
        }

        private static bool IsEncryptConnection(DataTable dataConnectionString)
        {
            bool m_IsEncryption = false;
            try
            {
                if (dataConnectionString != null || dataConnectionString.Rows.Count > 0)
                    if (dataConnectionString.Columns.Contains("DatabaseEncryption") && dataConnectionString.Rows[0]["DatabaseEncryption"] != null)
                        m_IsEncryption = (dataConnectionString.Rows[0]["DatabaseEncryption"].ToString().Trim().ToLower() == "yes");
                    else if (dataConnectionString.Columns.Contains("DatabseEncryption") && dataConnectionString.Rows[0]["DatabseEncryption"] != null)
                        m_IsEncryption = (dataConnectionString.Rows[0]["DatabseEncryption"].ToString().Trim().ToLower() == "yes");
                    else if (dataConnectionString.Columns.Contains("Encryption") && dataConnectionString.Rows[0]["Encryption"] != null)
                        m_IsEncryption = (dataConnectionString.Rows[0]["Encryption"].ToString().Trim().ToLower() == "yes");

            }
            catch
            {
                m_IsEncryption = false;
            }

            return m_IsEncryption;
        }

        private static bool DatabseCacheEnabled
        {
            get
            {
                bool m_IsEncryption = false;
                DataTable dataConnectionString = null;
                try
                {
                    DataSet ds = AppDomain.CurrentDomain.GetData("dsConfig") as DataSet;
                    if (ds != null && ds.Tables.Contains("AppConfig"))
                        dataConnectionString = ds.Tables["AppConfig"];

                    if (dataConnectionString != null || dataConnectionString.Rows.Count > 0)
                        if (dataConnectionString.Columns.Contains("DatabaseCacheEnabled") && dataConnectionString.Rows[0]["DatabaseCacheEnabled"] != null)
                            m_IsEncryption = (dataConnectionString.Rows[0]["DatabaseCacheEnabled"].ToString().Trim().ToLower() == "yes");
                        else if (dataConnectionString.Columns.Contains("DatabseCacheEnabled") && dataConnectionString.Rows[0]["DatabseCacheEnabled"] != null)
                            m_IsEncryption = (dataConnectionString.Rows[0]["DatabseCacheEnabled"].ToString().Trim().ToLower() == "yes");
                        else if (dataConnectionString.Columns.Contains("IsDatabaseCacheEnable") && dataConnectionString.Rows[0]["IsDatabaseCacheEnable"] != null)
                            m_IsEncryption = (dataConnectionString.Rows[0]["IsDatabaseCacheEnable"].ToString().Trim().ToLower() == "yes");
                        else if (dataConnectionString.Columns.Contains("IsDatabseCacheEnable") && dataConnectionString.Rows[0]["IsDatabseCacheEnable"] != null)
                            m_IsEncryption = (dataConnectionString.Rows[0]["IsDatabseCacheEnable"].ToString().Trim().ToLower() == "yes");
                }
                catch
                {
                    m_IsEncryption = false;
                }

                return m_IsEncryption;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
