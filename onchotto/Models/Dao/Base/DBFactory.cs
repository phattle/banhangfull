using System;
using System.Data.Odbc;
using System.Data.OleDb; 
using System.Data.SqlClient;
using System.Data.Common;

namespace OnChotto.Models.Dao
{
    public class DBFactory : IDisposable
    {
        public DBFactory()
        {
        }

        //public static IDbConnection GetConnection(string connectionString, DataProvider dataProvider)
        public static DbConnection GetConnection(string connectionString, DataProvider dataProvider)
        {
            DbConnection conn = null;
            switch (dataProvider)
            {
                case DataProvider.Odbc:
                    conn = new OdbcConnection(connectionString); break;
                case DataProvider.Oledb:
                    conn = new OleDbConnection(connectionString); break;
                 
                case DataProvider.Sql:
                    conn = new SqlConnection(connectionString); break;
            }
            return conn;
        }

        //public static IDbCommand GetCommand(DataProvider dataType)
        public static DbCommand GetCommand(DataProvider dataProvider)
        {
            DbCommand cmd = null;
            switch (dataProvider)
            {
                case DataProvider.Odbc:
                    cmd = new OdbcCommand(); break;
                case DataProvider.Oledb:
                    cmd = new OleDbCommand(); break;
                
                case DataProvider.Sql:
                    cmd = new SqlCommand(); break;
            }
            return cmd;
        }

        //public static IDbTransaction GetTransaction(string connectionString, DataProvider dataType)
        public static DbTransaction GetTransaction(string connectionString, DataProvider dataProvider)
        {
            DbConnection conn = GetConnection(connectionString, dataProvider);
            DbTransaction trans = conn.BeginTransaction();
            return trans;
        }

        //public static IDbDataAdapter GetDataAdapter(DataProvider dataType)
        public static DbDataAdapter GetDataAdapter(DataProvider dataProvider)
        {
            DbDataAdapter adapter = null;
            switch (dataProvider)
            {
                case DataProvider.Odbc:
                    adapter = new OdbcDataAdapter();
                    //adapter.AcceptChangesDuringFill = false;
                    adapter.AcceptChangesDuringUpdate = false;
                    break;
                case DataProvider.Oledb:
                    adapter = new OleDbDataAdapter();
                    //adapter.AcceptChangesDuringFill = false;
                    adapter.AcceptChangesDuringUpdate = false;
                    break;
                
                case DataProvider.Sql:
                    adapter = new SqlDataAdapter();
                    //adapter.AcceptChangesDuringFill = false;
                    adapter.AcceptChangesDuringUpdate = false;
                    break;

            }
            return adapter;
        }

        //public static IDataParameter GetParameter(DataProvider dataType)
        public static DbParameter GetParameter(DataProvider dataProvider)
        {
            DbParameter param = null;
            switch (dataProvider)
            {
                case DataProvider.Odbc:
                    param = new OdbcParameter(); break;
                case DataProvider.Oledb:
                    param = new OleDbParameter(); break;
                
                case DataProvider.Sql:
                    param = new SqlParameter(); break;
            }
            return param;
        }

        //public static IDbDataParameter[] GetParameters(DataProvider dataType, int paramCount)
        public static DbParameter[] GetParameters(DataProvider dataProvider, int paramCount)
        {
            DbParameter[] param = new DbParameter[paramCount];
            int i = 0;
            switch (dataProvider)
            {
                case DataProvider.Odbc:
                    for (i = 0; i < paramCount; i++)
                        param[i] = new OdbcParameter(); break;
                case DataProvider.Oledb:
                    for (i = 0; i < paramCount; i++)
                        param[i] = new OleDbParameter(); break;
                 
                case DataProvider.Sql:
                    for (i = 0; i < paramCount; i++)
                        param[i] = new SqlParameter(); break;
            }
            return param;
        }

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
