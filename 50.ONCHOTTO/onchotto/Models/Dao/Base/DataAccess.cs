using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace OnChotto.Models.Dao
{

    public enum DataProvider
    {
        Sql = 1,
        Oracle = 2,
        Odbc = 3,
        Oledb = 4,

    }
   
    public enum CommonParamName
    {
        Action,
        LanguageID,
        ApplicationID,
        LoginDomainID,
        LoginDomainCode,
        LoginUserID,
        LoginUserCode,
        LoginSessionID,
        FreeParameter
    }

    internal class DataAccessParam
    {
        internal DataAccessParam()
        {
            Index = 0;
        }
        internal DataAccessParam(string name, object value)
            : this()
        {
            Name = name;
            Value = value;
        }
        internal DataAccessParam(string name, object value, string sourceColumn)
            : this()
        {
            Name = name;
            Value = value;
            SourceColumn = sourceColumn;
        }

        internal string Name = string.Empty;
        internal object Value = null;
        internal DbType DbDataType = DbType.AnsiString;
        internal Type DataType = null;
        internal int Size = 0;
        internal string SourceColumn = string.Empty;
        internal object DefaultValue = null;
        internal int Index = 0;
    }

    public class DataAccess : IDataAccess, IDisposable
    {

        public const string SQLConnectString = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};Min Pool Size=0;Max Pool Size=500;Pooling=true;";
        public const string OracleConnectString = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};Min Pool Size=0;Max Pool Size=500;Pooling=true;";

        public const int DefaultConnectionTimeout = 30;
        public const int DefaultCommandTimeout = 60;
        public static int CommandTimeout = DefaultCommandTimeout;

        public static string AppConnectionString = string.Empty;

        public static string SamConnectionString = string.Empty;

        public static string SysConnectionString = string.Empty;
         

        public static string ApplicationID = string.Empty;
        public static string LanguageID = string.Empty;
        public static int DomainID = int.MinValue;
        public static string DomainCode = string.Empty;
        public static int UserID = int.MinValue;
        public static string UserCode = string.Empty;
        public static int LoginSessionID = int.MinValue;

        private static DataProvider m_DataProvider = DataProvider.Sql;
        private string m_ConnectionString = string.Empty;
        private System.Data.Common.DbConnection m_Conn = null;
        private System.Data.Common.DbCommand m_Cmd = null;
        private System.Data.Common.DbDataAdapter m_Adapter = null;
        private System.Data.Common.DbTransaction m_Trans = null;
        private System.Data.Common.DbParameter[] m_Para = null;
        private System.Data.Common.DbDataReader m_Dr = null;
        private Dictionary<string, DataAccessParam> ParamItems = null;


        //private IDbConnection m_Conn = null;
        //private IDbCommand m_Cmd = null;
        //private IDbDataAdapter m_Adapter = null;
        //private IDbTransaction m_Trans = null;
        //private IDbDataParameter[] m_Para = null;
        //private IDataReader m_Dr = null;

        #region Constructor
        public DataAccess()
        {
            if (!string.IsNullOrEmpty(AppConnectionString) && AppConnectionString.IndexOf(";") > -1 && AppConnectionString.IndexOf("=") > -1)
                this.ConnectionString = AppConnectionString;
            else
                this.ConnectionString = DBCache.GetConnectionString(AppConnectionString);
        }

        public DataAccess(string _ConnectionString)
        {
            if (!string.IsNullOrEmpty(_ConnectionString) && _ConnectionString.IndexOf(";") > -1 && _ConnectionString.IndexOf("=") > -1)
                this.ConnectionString = _ConnectionString;
            else
                this.ConnectionString = DBCache.GetConnectionString(_ConnectionString);
        }

        //public DataAccess(IDbConnection _Connection)
        public DataAccess(DbConnection _Connection)
        {
            this.m_Conn = _Connection;
        }
        #endregion Constructor


        #region Property
        public static DataProvider DataProvider
        {
            set
            {
                m_DataProvider = value;
            }
            get
            {
                return m_DataProvider;
            }
        }

        public string ConnectionString
        {
            set
            {
                m_ConnectionString = value;
            }
            get
            {
                return m_ConnectionString;
            }
        }

        public bool IsInTransaction
        {
            get
            {
                if (m_Conn != null && m_Conn.State == ConnectionState.Open && m_Trans != null && m_Trans.Connection != null)
                    return true;
                else
                    return false;
            }
        }

        //public IDataReader DataReader
        public DbDataReader DataReader
        {
            set { m_Dr = value; }
            get { return m_Dr; }
        }

        public DbConnection Connection
        {
            get
            {
                if (m_Conn == null)
                    m_Conn = DBFactory.GetConnection(ConnectionString, DataProvider);
                return m_Conn;
            }
        }

        public DbTransaction Transaction
        {
            get { return m_Trans; }
        }

        public DbCommand Command
        {
            get
            {
                if (m_Cmd == null)
                    m_Cmd = DBFactory.GetCommand(DataProvider);
                return m_Cmd;
            }
        }

        public DbDataAdapter Adapter
        {
            get
            {
                if (m_Adapter == null)
                    m_Adapter = DBFactory.GetDataAdapter(DataProvider);
                return m_Adapter;
            }
        }

        public DbParameter[] Parameters
        {
            get { return m_Para; }
        }

        //public DbParameter[] Param
        //{
        //    get { return m_Para; }
        //}

        public string ParamNames
        {
            get { return GetParamList(); }
        }

        public object[] ParamValues
        {
            get { return GetValueList(); }
        }

        #endregion Property


        #region Common Methods
        public void Open()
        {
            if (m_Conn == null)
            {
                m_Conn = DBFactory.GetConnection(m_ConnectionString, DataProvider);
                try
                {
                    m_Conn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (m_Conn.State != ConnectionState.Open)
            {
                m_Conn.Open();
            }
        }

        public void Close()
        {
            if (m_Conn == null || IsInTransaction)
                return;

            if (m_Adapter != null)
                m_Adapter.Dispose();
            m_Adapter = null;

            if (m_Conn.State == ConnectionState.Open)
                m_Conn.Close();

            m_Conn.Dispose();
            m_Conn = null;
        }

        public void Dispose()
        {
            if (IsInTransaction)
                return;

            if (m_Cmd != null)
                m_Cmd.Dispose();

            if (m_Dr != null)
                m_Dr.Dispose();

            if (m_Adapter != null)
                m_Adapter.Dispose();
            m_Adapter = null;

            if (m_Conn != null)
            {
                if (m_Conn.State == ConnectionState.Open)
                    m_Conn.Close();

                m_Conn.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            if (!IsInTransaction)
            {
                if (m_Conn == null || m_Conn.State != ConnectionState.Open)
                    this.Open();

                m_Trans = m_Conn.BeginTransaction();
            }
            //BeginTransaction(IsolationLevel.ReadUncommitted);
            //BeginTransaction(IsolationLevel.Unspecified);
        }

        public void BeginTransaction(IsolationLevel transIsolationLevel)
        {
            if (!IsInTransaction)
            {
                if (m_Conn == null || m_Conn.State != ConnectionState.Open)
                    this.Open();

                m_Trans = m_Conn.BeginTransaction(transIsolationLevel);
            }

            //if (m_Conn != null && m_Conn.State == ConnectionState.Open && m_Trans == null)
            //    m_Trans = m_Conn.BeginTransaction();
            //else
            //{
            //    this.Open();
            //    m_Trans = m_Conn.BeginTransaction();
            //}
        }

        public void CommitTransaction()
        {
            if (!IsInTransaction)     //(m_Trans != null)
                return;

            m_Trans.Commit();
            m_Trans.Dispose();
            m_Trans = null;
            this.Close();
        }

        public void RollbackTransaction()
        {
            if (!IsInTransaction)    //(m_Trans != null)
                return;

            m_Trans.Rollback();
            m_Trans.Dispose();
            m_Trans = null;
            this.Close();
        }
        #endregion Common Methods


        #region Parameter Methods
        public void CreateParameters(int paramCount)
        {
            m_Para = DBFactory.GetParameters(DataProvider, paramCount);
        }

        public void AddParameters(int index, string paramName, object objValue)
        {
            if (m_Para != null && m_Para.Length >= 0 && index < m_Para.Length)
            {
                m_Para[index].ParameterName = paramName;
                m_Para[index].Value = objValue;
            }
        }

        private void AttachParameters(DbParameter[] parameters)
        {
            if (parameters != null && parameters.Length >= 0)
                foreach (DbParameter par in parameters)
                    m_Cmd.Parameters.Add(par);

        }

        private void PrepareCommand(DbCommand command, DbConnection m_Connection, DbTransaction m_Transaction
                , CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            command.Connection = m_Connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (CommandTimeout > 0)
                command.CommandTimeout = CommandTimeout;

            command.Parameters.Clear();

            //if (m_Transaction != null)
            if (IsInTransaction)
            {
                command.Transaction = m_Transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(commandParameters);
            }
        }
        #endregion Parameter Methods


        #region ExecuteNonQuery
        public int ExecuteNonQuery(string commandText)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, null);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            catch (Exception ex)
            {
                if (!IsInTransaction)
                    this.Close();
                throw ex;
            }
            return intResult;
        }

        //public int ExecuteNonQuery(string commandText, params object[] paramValues)
        public int ExecuteNonQuery(string commandText, object[] paramValues)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null)
                    AssignParameterValues(arrSQLParameter, paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            return intResult;
        }

        public int ExecuteNonQuery(string commandText, object[] paramValues, ref object[] refValue)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null && paramValues.Length > 0)
                    AssignCombineParameterValues(arrSQLParameter, ref refValue, paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

                if (refValue != null)
                    for (int i = 0; i < refValue.Length; i++)
                        refValue[i] = arrSQLParameter[i].Value;

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            return intResult;
        }

        public int ExecuteNonQuery(string commandText, string paramNames, object[] paramValues, ref OutputResult refValue)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {

                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                //string paramNames = paramInputs;
                string paramInputs = paramNames;
                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                    paramInputs = paramInputs.Trim();
                    paramInputs = paramInputs.Replace(" ", "");
                }
                int OutCount = 0;
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {

                            #region VERSION V1.0

                            //arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            //arrMainSQLParameter[index].Size = dbPara.Size;
                            //arrMainSQLParameter[index].Direction = dbPara.Direction;
                            //arrMainSQLParameter[index++].DbType = dbPara.DbType;

                            #endregion 

                            #region VERSION NEW 1 - FIX ISSUE <Failed to convert parameter value from a TimeSpan to a DateTime>

                            if (dbPara.DbType == DbType.Time)
                            {
                                arrMainSQLParameter[index] = new SqlParameter(dbPara.ParameterName, SqlDbType.Time, dbPara.Size);
                                //((SqlParameter)arrMainSQLParameter[index]).ParameterName = dbPara.ParameterName;
                                //((SqlParameter)arrMainSQLParameter[index]).Size = dbPara.Size;
                                ((SqlParameter)arrMainSQLParameter[index]).Direction = dbPara.Direction;
                                //((SqlParameter)arrMainSQLParameter[index]).DbType = dbPara.DbType;
                            }
                            else
                            {
                                arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                                arrMainSQLParameter[index].Size = dbPara.Size;
                                arrMainSQLParameter[index].Direction = dbPara.Direction;
                                arrMainSQLParameter[index].DbType = dbPara.DbType;
                            }
                            index++;

                            #endregion

                            if (dbPara.Direction == ParameterDirection.InputOutput || dbPara.Direction == ParameterDirection.Output)
                                OutCount++;

                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                AssignOKParameterValues(arrMainSQLParameter, paramInputs, paramValues);
                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

                if (OutCount > 0)
                {
                    refValue = new OutputResult();
                    refValue.Initialize(OutCount);
                    OutCount = 0;

                    for (int j = 0; j < arrMainSQLParameter.Length; j++)
                        if (arrMainSQLParameter[j].Direction == ParameterDirection.InputOutput | arrMainSQLParameter[j].Direction == ParameterDirection.Output)
                            if (paramNames.IndexOf("," + arrMainSQLParameter[j].ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + arrMainSQLParameter[j].ParameterName.Replace("@", "").ToLower() + ",") > -1)
                            {
                                OutputResult inf = new OutputResult();
                                inf.Name = arrMainSQLParameter[j].ParameterName.Replace("@", "");
                                inf.Value = arrMainSQLParameter[j].Value;
                                refValue.Add(inf);

                                OutCount++;
                                if (OutCount == refValue.Length)
                                    break;
                            }

                }

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();

                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            return intResult;
        }

        //public int ExecuteNonQueryPar(string commandText, string paramNames, params object[] paramValues)
        public int ExecuteNonQuery(string commandText, string paramNames, object[] paramValues)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                }
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;
                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                paramNames = strParamTemp;
                if (paramValues != null && paramValues.Length > 0)
                    AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                else
                    arrMainSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            return intResult;
        }

        //public int ExecuteNonQueryRef(string commandText, ref object[] paramValues)
        public int ExecuteNonQuery(string commandText, ref object[] paramValues)
        {
            int intResult = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null && paramValues.Length > 0)
                    AssignRefParameterValues(arrSQLParameter, ref paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

                if (paramValues != null)
                    for (int i = 0; i < paramValues.Length; i++)
                        paramValues[i] = arrSQLParameter[i].Value;

                m_Cmd.Parameters.Clear();
                return intResult;
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                if (!IsInTransaction)
                    this.Close();
            }
        }

        //public int ExecuteNonQueryOut(string commandText, string paramOutputs, string paramInputs, ref object[] refValue, params object[] paramValues)
        public int ExecuteNonQuery(string commandText, string paramOutputs, string paramInputs, object[] paramValues, ref object[] refValue)
        {
            int intResult = 0;
            int index = 0;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;


                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, refValue.Length + paramValues.Length < arrSQLParameter.Length ? refValue.Length + paramValues.Length : arrSQLParameter.Length);
                else
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, refValue.Length < arrSQLParameter.Length ? refValue.Length : arrSQLParameter.Length);

                if (!String.IsNullOrEmpty(paramOutputs))
                {
                    paramOutputs = paramOutputs.Trim().Replace(" ", "");
                }
                if (!String.IsNullOrEmpty(paramInputs))
                {
                    paramInputs = paramInputs.Trim().Replace(" ", "");
                }
                string strTempParamOuputs = paramOutputs;
                string strTempParamInputs = paramInputs;
                char[] chrArr = { ',' };
                string[] strArrOut = paramOutputs.Split(chrArr);
                string[] strArrIn = paramInputs.Split(chrArr);
                paramOutputs = "," + paramOutputs.ToLower() + ",";
                paramInputs = "," + paramInputs.ToLower() + ",";

                if (strTempParamOuputs != string.Empty && DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramOutputs.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramOutputs.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Direction = ParameterDirection.Output;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;

                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (strTempParamInputs != string.Empty && DataProvider == DataProvider.Sql && index < arrMainSQLParameter.Length)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramInputs.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramInputs.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;

                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                paramOutputs = strTempParamOuputs;
                paramInputs = strTempParamInputs;

                AssignCombineParameterValues(arrMainSQLParameter, paramOutputs, paramInputs, ref refValue, paramValues);

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                intResult = m_Cmd.ExecuteNonQuery();
                if (intResult < 0)  //Nếu không có lỗi mà kết quả trả về = -1 là do lệnh SET NOCOUNT ON;
                    intResult = 0;

                if (strArrOut != null)
                {
                    int count = 0;
                    foreach (string paraName in strArrOut)
                        for (int j = 0; j < arrMainSQLParameter.Length; j++)
                            if (paraName.Trim().ToLower() == arrMainSQLParameter[j].ParameterName.ToLower() | paraName.Trim().ToLower() == arrMainSQLParameter[j].ParameterName.Replace("@", "").ToLower())
                            {
                                refValue[count++] = arrMainSQLParameter[j].Value;
                                break;
                            }
                }
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            return intResult;
        }
        #endregion


        #region ExecuteScalar
        public object ExecuteScalar(string commandText)
        {
            object objResult = null;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();
                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, null);
                objResult = m_Cmd.ExecuteScalar();
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
                throw ex;
            }
            return objResult;
        }

        //public object ExecuteScalar(string commandText, params object[] paramValues)
        public object ExecuteScalar(string commandText, object[] paramValues)
        {
            object objResult = null;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null)
                    AssignParameterValues(arrSQLParameter, paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, null, CommandType.StoredProcedure, commandText, arrSQLParameter);
                objResult = m_Cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                m_Para = null;
                if (!IsInTransaction)
                    this.Close();
            }

            return objResult;
        }

        //public object ExecuteScalarPar(string commandText, string paramNames, params object[] paramValues)
        public object ExecuteScalar(string commandText, string paramNames, object[] paramValues)
        {
            object objResult = null;
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;     //= DBFactory.GetParameters(DataProvider, paramValues.Length);

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                }
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;
                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                paramNames = strParamTemp;
                if (paramValues != null)
                    AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                else
                    arrMainSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                objResult = m_Cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
                m_Para = null;
                if (!IsInTransaction)
                    this.Close();
            }
            return objResult;
        }
        #endregion


        #region ExecuteDataTable
        public DataTable ExecuteDataTable(string commandText)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);
            DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
            using (DataSet ds = new DataSet())
            {
                try
                {
                    this.Open();
                    PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, m_Para);
                    dataAdapter.SelectCommand = m_Cmd;
                    dataAdapter.Fill(ds);
                    m_Cmd.Parameters.Clear();
                    if (!IsInTransaction)
                        this.Close();
                }
                catch (Exception ex)
                {
                    if (!IsInTransaction)
                        this.Close();
                    throw ex;
                }
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);
            DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
            using (DataSet ds = new DataSet())
            {
                try
                {
                    this.Open();
                    PrepareCommand(m_Cmd, m_Conn, m_Trans, commandType, commandText, m_Para);
                    dataAdapter.SelectCommand = m_Cmd;
                    dataAdapter.Fill(ds);
                    m_Cmd.Parameters.Clear();
                    if (!IsInTransaction)
                        this.Close();
                }
                catch (Exception ex)
                {
                    if (!IsInTransaction)
                        this.Close();
                    throw ex;
                }
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
        }

        //public DataTable ExecuteDataTable(string commandText, string paramNames, params object[] paramValues)
        public DataTable ExecuteDataTable(string commandText, string paramNames, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();
                using (DataSet ds = new DataSet())
                {
                    DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                    DbParameter[] arrMainSQLParameter = null;
                    if (paramValues != null && arrSQLParameter != null)
                        arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);
                    if (!string.IsNullOrEmpty(paramNames))
                    {
                        paramNames = paramNames.Trim();
                        paramNames = paramNames.Replace(" ", "");
                    }
                    int index = 0;
                    char[] chrArr = { ',' };
                    string[] strArr = paramNames.Split(chrArr);
                    string strParamTemp = paramNames;
                    paramNames = "," + paramNames.ToLower() + ",";
                    if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                        foreach (DbParameter dbPara in arrSQLParameter)
                            if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                            {
                                arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                                arrMainSQLParameter[index].Size = dbPara.Size;
                                arrMainSQLParameter[index++].DbType = dbPara.DbType;
                                if (index >= arrMainSQLParameter.Length)
                                    break;
                            }
                    if (index < arrMainSQLParameter.Length)
                    {
                        //Delete no matching parameters
                        DbParameter[] arrTemp = arrMainSQLParameter;
                        arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                        for (int i = 0; i < index; i++)
                            arrMainSQLParameter[i] = arrTemp[i];
                    }
                    paramNames = strParamTemp;
                    if (paramValues != null && arrMainSQLParameter != null)
                        AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                    else
                        arrMainSQLParameter = null;
                    PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                    DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
                    dataAdapter.SelectCommand = m_Cmd;
                    dataAdapter.Fill(ds);
                    m_Cmd.Parameters.Clear();
                    if (ds != null && ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }

            }
            catch  
            {
                return null;
            }
            finally
            {
                if (m_Cmd != null)
                    m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
        }

        //public DataTable ExecuteDataTable(string commandText, string paramInputs, ref OutputResult refValue, params object[] paramValues)
        public DataTable ExecuteDataTable(string commandText, string paramNames, object[] paramValues, ref OutputResult refValue)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();
                using (DataSet ds = new DataSet())
                {
                    DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                    DbParameter[] arrMainSQLParameter = null;
                    if (paramValues != null && arrSQLParameter != null)
                        arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                    //string paramNames = paramInputs;
                    string paramInputs = paramNames;
                    if (!string.IsNullOrEmpty(paramNames))
                    {
                        paramNames = paramNames.Trim().Replace(" ", "");
                        paramInputs = paramInputs.Trim().Replace(" ", "");
                    }
                    int OutCount = 0;
                    int index = 0;
                    char[] chrArr = { ',' };
                    string[] strArr = paramNames.Split(chrArr);
                    string strParamTemp = paramNames;
                    paramNames = "," + paramNames.ToLower() + ",";

                    if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                        foreach (DbParameter dbPara in arrSQLParameter)
                            if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                            {
                                arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                                arrMainSQLParameter[index].Size = dbPara.Size;
                                arrMainSQLParameter[index].Direction = dbPara.Direction;
                                arrMainSQLParameter[index++].DbType = dbPara.DbType;
                                if (dbPara.Direction == ParameterDirection.InputOutput || dbPara.Direction == ParameterDirection.Output)
                                    OutCount++;
                                if (index >= arrMainSQLParameter.Length)
                                    break;
                            }
                    if (index < arrMainSQLParameter.Length)
                    {
                        //Delete no matching parameters
                        DbParameter[] arrTemp = arrMainSQLParameter;
                        arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                        for (int i = 0; i < index; i++)
                            arrMainSQLParameter[i] = arrTemp[i];
                    }
                    AssignOKParameterValues(arrMainSQLParameter, paramInputs, paramValues);
                    PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                    DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
                    dataAdapter.SelectCommand = m_Cmd;
                    dataAdapter.Fill(ds);
                    m_Cmd.Parameters.Clear();

                    //Get Return value from Store Procedure
                    if (OutCount > 0)
                    {
                        refValue = new OutputResult();
                        refValue.Initialize(OutCount);
                        OutCount = 0;
                        for (int j = 0; j < arrMainSQLParameter.Length; j++)
                            if (arrMainSQLParameter[j].Direction == ParameterDirection.InputOutput | arrMainSQLParameter[j].Direction == ParameterDirection.Output)
                                if (paramNames.IndexOf("," + arrMainSQLParameter[j].ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + arrMainSQLParameter[j].ParameterName.Replace("@", "").ToLower() + ",") > -1)
                                {
                                    OutputResult inf = new OutputResult();
                                    inf.Name = arrMainSQLParameter[j].ParameterName.Replace("@", "");
                                    inf.Value = arrMainSQLParameter[j].Value;
                                    refValue.Add(inf);
                                    OutCount++;
                                    if (OutCount == refValue.Length)
                                        break;
                                }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (m_Cmd != null)
                    m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
        }
        #endregion


        #region ExecuteDataSet
        public void ExecuteDataSet(DataSet ds, string commandText)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);
            DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, m_Para);
                dataAdapter.SelectCommand = m_Cmd;
                dataAdapter.Fill(ds);

                m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ExecuteDataSet(DataSet ds, string commandText, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null && paramValues.Length > 0)
                    AssignParameterValues(arrSQLParameter, paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

                DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);
                dataAdapter.SelectCommand = m_Cmd;
                dataAdapter.Fill(ds);
                m_Cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (m_Cmd != null)
                    m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }
        }

        public void ExecuteDataSet(DataSet ds, string commandText, string paramNames, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;

                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                }
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;
                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                paramNames = strParamTemp;
                if (paramValues != null && arrMainSQLParameter != null)
                    AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                else
                    arrMainSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);

                DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);

                dataAdapter.SelectCommand = m_Cmd;
                dataAdapter.Fill(ds);
                m_Cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (m_Cmd != null)
                    m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }

        }

        private void ExecuteDataSetPar_(DataSet ds, string commandText, string paramNames, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;

                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                }
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;
                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);
                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                paramNames = strParamTemp;
                if (paramValues != null && arrMainSQLParameter != null)
                    AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                else
                    arrMainSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);

                DbDataAdapter dataAdapter = DBFactory.GetDataAdapter(DataProvider);

                dataAdapter.SelectCommand = m_Cmd;
                dataAdapter.Fill(ds);
                m_Cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (m_Cmd != null)
                    m_Cmd.Parameters.Clear();
                if (!IsInTransaction)
                    this.Close();
            }

        }
        #endregion


        #region ExecuteReader
        public DbDataReader ExecuteReader(string commandText)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, null);
                m_Dr = m_Cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                m_Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            return m_Dr;
        }

        public DbDataReader ExecuteReader(string commandText, CommandBehavior commandBehavior)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.Text, commandText, null);
                m_Dr = m_Cmd.ExecuteReader(commandBehavior);
                m_Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            return m_Dr;
        }

        public DbDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, commandType, commandText, null);
                m_Dr = m_Cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                m_Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            return m_Dr;
        }

        public DbDataReader ExecuteReader(string commandText, CommandType commandType, CommandBehavior commandBehavior)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                PrepareCommand(m_Cmd, m_Conn, m_Trans, commandType, commandText, null);
                m_Dr = m_Cmd.ExecuteReader(commandBehavior);
                m_Cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            return m_Dr;
        }

        public DbDataReader ExecuteReader(string commandText, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

                if (paramValues != null)
                    AssignParameterValues(arrSQLParameter, paramValues);
                else
                    arrSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);
                m_Dr = m_Cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
            }
            return m_Dr;

        }

        public DbDataReader ExecuteReader(string commandText, string paramNames, object[] paramValues)
        {
            if (m_Cmd == null)
                m_Cmd = DBFactory.GetCommand(DataProvider);

            try
            {
                this.Open();

                DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);
                DbParameter[] arrMainSQLParameter = null;     //= DBFactory.GetParameters(DataProvider, paramValues.Length);

                if (paramValues != null && arrSQLParameter != null)
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, paramValues.Length < arrSQLParameter.Length ? paramValues.Length : arrSQLParameter.Length);

                if (!string.IsNullOrEmpty(paramNames))
                {
                    paramNames = paramNames.Trim();
                    paramNames = paramNames.Replace(" ", "");
                }
                int index = 0;
                char[] chrArr = { ',' };
                string[] strArr = paramNames.Split(chrArr);
                string strParamTemp = paramNames;
                paramNames = "," + paramNames.ToLower() + ",";

                if (DataProvider == DataProvider.Sql & arrMainSQLParameter.Length > 0)
                    foreach (DbParameter dbPara in arrSQLParameter)
                        if (paramNames.IndexOf("," + dbPara.ParameterName.ToLower() + ",") > -1 | paramNames.IndexOf("," + dbPara.ParameterName.Replace("@", "").ToLower() + ",") > -1)
                        {
                            arrMainSQLParameter[index].ParameterName = dbPara.ParameterName;
                            arrMainSQLParameter[index].Size = dbPara.Size;
                            arrMainSQLParameter[index++].DbType = dbPara.DbType;

                            if (index >= arrMainSQLParameter.Length)
                                break;
                        }

                if (index < arrMainSQLParameter.Length)
                {
                    //Delete no matching parameters
                    DbParameter[] arrTemp = arrMainSQLParameter;
                    arrMainSQLParameter = DBFactory.GetParameters(DataProvider, index);

                    for (int i = 0; i < index; i++)
                        arrMainSQLParameter[i] = arrTemp[i];
                }

                paramNames = strParamTemp;
                if (paramValues != null)
                    AssignParameterValues(arrMainSQLParameter, paramNames, paramValues);
                else
                    arrMainSQLParameter = null;

                PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrMainSQLParameter);
                m_Dr = m_Cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            }
            catch (Exception ex)
            {
                m_Cmd.Parameters.Clear();
                throw ex;
            }
            finally
            {
                m_Cmd.Parameters.Clear();
            }
            return m_Dr;

        }
        #endregion


        #region AssignParameterValues
        private void AssignParameterValues(DbParameter[] p_arrSQLParameter, string paramNames, object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0 || paramValues == null || paramValues.Length == 0)
            {
                return;
            }
            //if (p_arrSQLParameter.Length != p_arrValue.Length)
            //{
            //    throw new Exception("Parameter count does not match Parameter Value count.");
            //}
            if (paramNames.StartsWith(","))
                paramNames = paramNames.Substring(1, paramNames.Length - 1);
            if (paramNames.EndsWith(","))
                paramNames = paramNames.Substring(0, paramNames.Length - 1);

            char[] charArr = { ',' };
            string[] strArr = paramNames.Split(charArr);

            for (int i = 0; i < strArr.Length; i++)
                for (int j = 0; j < p_arrSQLParameter.Length; j++)
                    if (strArr[i].Trim().ToLower() == p_arrSQLParameter[j].ParameterName.Replace("@", "").ToLower())
                    {
                        CorrectSQL(p_arrSQLParameter[j], paramValues[i]);
                        break;
                    }


            //for (int i = 0; i < strArr.Length; i++)
            //    for (int j = 0; j < paramValues.Length; j++)
            //        if (strArr[i].Trim().ToLower() == p_arrSQLParameter[j].ParameterName.Replace("@", "").ToLower())
            //        {
            //            CorrectSQL(p_arrSQLParameter[j], paramValues[i]);
            //            break;
            //        }

        }

        private static void AssignCombineParameterValues(DbParameter[] p_arrSQLParameter, string paramOutputs, string paramInputs, ref object[] refValue, object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0) return;

            char[] chrArr = { ',' };
            string[] OutArr = paramOutputs.Split(chrArr);
            string[] InArr = paramInputs.Split(chrArr);
            int i = 0;

            if (refValue != null)
                for (i = 0; i < OutArr.Length; i++)
                    for (int j = 0; j < p_arrSQLParameter.Length; j++)
                        if (OutArr[i].Trim().ToLower() == p_arrSQLParameter[j].ParameterName.Replace("@", "").ToLower())
                        {
                            CorrectSQL(p_arrSQLParameter[j], refValue[i]);
                            break;
                        }


            if (paramValues != null)
                for (int j = 0; i < (paramValues.Length + refValue.Length) && j < paramValues.Length; i++, j++)
                    for (int k = 0; k < p_arrSQLParameter.Length; k++)
                        if (InArr[j].Trim().ToLower() == p_arrSQLParameter[k].ParameterName.Replace("@", "").ToLower())
                        {
                            CorrectSQL(p_arrSQLParameter[k], paramValues[j]);
                            break;
                        }
        }

        private void AssignOKParameterValues(DbParameter[] p_arrSQLParameter, string paramInputs, object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0 || paramValues == null || paramValues.Length == 0)
            {
                return;
            }

            char[] chrArr = { ',' };
            string[] InArr = paramInputs.Split(chrArr);
            int i = 0;
            for (i = 0; i < InArr.Length; i++)
                for (int j = 0; j < p_arrSQLParameter.Length; j++)
                    if (InArr[i].Trim().ToLower() == p_arrSQLParameter[j].ParameterName.Replace("@", "").ToLower())
                    {
                        CorrectSQL(p_arrSQLParameter[j], paramValues[i]);
                        break;
                    }
        }

        private static void AssignParameterValues(DbParameter[] p_arrSQLParameter, object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0 || paramValues == null || paramValues.Length == 0)
            {
                return;
            }

            int i = 0;

            for (i = 0; i < paramValues.Length; i++)
            {
                if (i < p_arrSQLParameter.Length)
                    CorrectSQL(p_arrSQLParameter[i], paramValues[i]);
            }

            if (i < p_arrSQLParameter.Length)
                for (; i < p_arrSQLParameter.Length; i++)
                    p_arrSQLParameter[i].Value = null;
        }

        private static void AssignRefParameterValues(DbParameter[] p_arrSQLParameter, ref object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0 || paramValues == null || paramValues.Length == 0)
            {
                return;
            }

            if (p_arrSQLParameter.Length != paramValues.Length)
            {
                throw new Exception("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = p_arrSQLParameter.Length; i < j; i++)
            {
                if (i < p_arrSQLParameter.Length)
                {
                    CorrectSQL(p_arrSQLParameter[i], paramValues[i]);
                    p_arrSQLParameter[i].Direction = ParameterDirection.Output;
                }
            }
        }

        private static void AssignCombineParameterValues(DbParameter[] p_arrSQLParameter, ref object[] refValue, object[] paramValues)
        {
            if (p_arrSQLParameter == null || p_arrSQLParameter.Length == 0 || paramValues == null || paramValues.Length == 0)
            {
                return;
            }

            if (p_arrSQLParameter.Length != (refValue.Length + paramValues.Length))
            {
                throw new Exception("Parameter count does not match Parameter Value count.");
            }

            int i = 0;
            for (i = 0; i < refValue.Length; i++)
            {
                if (i < p_arrSQLParameter.Length)
                {
                    CorrectSQL(p_arrSQLParameter[i], refValue[i]);
                    p_arrSQLParameter[i].Direction = ParameterDirection.Output;
                }
            }

            for (int j = 0; i < (paramValues.Length + refValue.Length) && j < paramValues.Length; i++, j++)
            {
                if (i < p_arrSQLParameter.Length)
                    CorrectSQL(p_arrSQLParameter[i], paramValues[j]);
            }
        }
        #endregion


        #region UpdateTable
        private static void AttachParametersByColumn(DbCommand command, DbParameter[] parameters)
        {
            if (parameters != null && parameters.Length >= 0)
                foreach (DbParameter par in parameters)
                    command.Parameters.Add(par);

        }

        private void PrepareCommandByColumn(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Parameters.Clear();

            if (IsInTransaction)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParametersByColumn(command, commandParameters);
            }
        }

        private static void AssignParameterByColumn(DbParameter[] sqlParameter, object[] paramValues)
        {
            if (sqlParameter == null || sqlParameter.Length == 0) return;
            if (paramValues == null || paramValues.Length == 0) return;

            AssignParameterByColumn(sqlParameter, string.Empty, paramValues);
        }

        private static void AssignParameterByColumn(DbParameter[] sqlParameter, string actionValue, object[] paramValues)
        {
            if (sqlParameter == null || sqlParameter.Length == 0) return;

            if (!string.IsNullOrEmpty(actionValue))
                sqlParameter[0].Value = actionValue;   //@Action parameter
            if (paramValues == null || paramValues.Length == 0) return;

            List<object> CopyParamValues = new List<object>();
            foreach (object obj in paramValues)
                CopyParamValues.Add(obj);

            int intCommonParam = 0;
            int i = 1;
            int index = 1;

            while (i < sqlParameter.Length && i < CopyParamValues.Count && (CopyParamValues[i] == null
                || (CopyParamValues[i] != null && sqlParameter[index].ParameterName.ToLower() != "@" + CopyParamValues[i].ToString().ToLower())))
            {
                sqlParameter[index].Value = CopyParamValues[i];
                CopyParamValues.RemoveAt(i);
                index++;
                //i++;
            }
            intCommonParam = index;
            i = index;
            int j = 0;    //intCommonParam;

            while (index < sqlParameter.Length)
            {
                j = 0;
                for (; j < CopyParamValues.Count; j++)
                {
                    if (CopyParamValues[j] != null && CopyParamValues[j].ToString() != string.Empty
                        && sqlParameter[index].ParameterName.ToLower() == "@" + CopyParamValues[j].ToString().ToLower())
                    {
                        sqlParameter[index].SourceColumn = CopyParamValues[j].ToString();
                        CopyParamValues.RemoveAt(j);
                        index++;
                        break;
                    }
                }
                if (j >= CopyParamValues.Count)
                    break;
            }
            string Value = string.Empty;

            for (i = 0; i < sqlParameter.Length; i++)
                if (sqlParameter[i].Value != null)
                    Value += sqlParameter[i].ParameterName + ":" + sqlParameter[i].Value.ToString() + ",";
                else
                    Value += sqlParameter[i].ParameterName + ":" + sqlParameter[i].SourceColumn.ToString() + ",";

        }

        private static void AssignParameterByColumn(DbParameter[] sqlParameter, string actionValue, Dictionary<string, DataAccessParam> paramValues)
        {
            if (sqlParameter == null || sqlParameter.Length == 0) return;
            if (paramValues == null || paramValues.Count == 0) return;

            if (!string.IsNullOrEmpty(actionValue))
                paramValues["action"].Value = actionValue;

            DataAccessParam paramItem;
            for (int i = 0; i < sqlParameter.Length; i++)
                if (paramValues.ContainsKey(sqlParameter[i].ParameterName.ToLower().Replace("@", "")))
                {
                    paramItem = paramValues[sqlParameter[i].ParameterName.ToLower().Replace("@", "")];
                    if (DataAccess.IsNullOrEmpty(paramItem.Value) && !DataAccess.IsNullOrEmpty(paramItem.SourceColumn))
                        sqlParameter[i].SourceColumn = paramItem.SourceColumn;
                    else
                        sqlParameter[i].Value = paramItem.Value;
                }
        }

        private static void AssignParameterByColumn1(DbParameter[] sqlParameter, string actionValue, object[] paramValues)
        {
            if (sqlParameter == null || sqlParameter.Length == 0) return;

            if (!string.IsNullOrEmpty(actionValue))
                sqlParameter[0].Value = actionValue;   //@Action parameter
            if (paramValues == null || paramValues.Length == 0) return;

            object[] CopyParamValues = new object[paramValues.Length];
            paramValues.CopyTo(CopyParamValues, 0);

            int intCommonParam = 0;
            int i = 1;

            while (i < sqlParameter.Length && i < CopyParamValues.Length && (CopyParamValues[i] == null
                || (CopyParamValues[i] != null && sqlParameter[i].ParameterName.ToLower() != "@" + CopyParamValues[i].ToString().ToLower())))
            {
                sqlParameter[i].Value = CopyParamValues[i];
                i++;
            }
            intCommonParam = i;
            int j = intCommonParam;

            while (i < sqlParameter.Length)
            {
                j = intCommonParam;
                for (; j < CopyParamValues.Length; j++)
                {
                    if (CopyParamValues[j] != null && CopyParamValues[j].ToString() != string.Empty
                        && sqlParameter[i].ParameterName.ToLower() == "@" + CopyParamValues[j].ToString().ToLower())
                    {
                        sqlParameter[i].SourceColumn = CopyParamValues[j].ToString();
                        CopyParamValues[j] = string.Empty;
                        i++;
                        break;
                    }
                }
                if (j >= CopyParamValues.Length)
                    break;
            }
            string Value = string.Empty;

            for (i = 0; i < sqlParameter.Length; i++)
                if (sqlParameter[i].Value != null)
                    Value += sqlParameter[i].ParameterName + ":" + sqlParameter[i].Value.ToString() + ",";
                else
                    Value += sqlParameter[i].ParameterName + ":" + sqlParameter[i].SourceColumn.ToString() + ",";

        }

        //private IDbCommand GetInsertCommand(string commandText, params object[] paramValues)
        private DbCommand GetInsertCommand(string commandText, object[] paramValues)
        {
            DbCommand cmdInsert = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, paramValues);

            PrepareCommandByColumn(cmdInsert, this.Connection, this.Transaction, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdInsert;
        }

        //private IDbCommand GetInsertCommand(string commandText, string insertAction, params object[] paramValues)
        private DbCommand GetInsertCommand(string commandText, string insertAction, object[] paramValues)
        {
            DbCommand cmdInsert = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, insertAction, paramValues);

            PrepareCommandByColumn(cmdInsert, this.Connection, this.Transaction, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdInsert;
        }
        private DbCommand GetInsertCommand(string commandText, string insertAction, Dictionary<string, DataAccessParam> paramValues)
        {
            DbCommand cmdInsert = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, insertAction, paramValues);

            PrepareCommandByColumn(cmdInsert, this.Connection, this.Transaction, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdInsert;
        }

        //private IDbCommand GetUpdateCommand(string commandText, params object[] paramValues)
        private DbCommand GetUpdateCommand(string commandText, object[] paramValues)
        {
            DbCommand cmdUpdate = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, paramValues);

            PrepareCommandByColumn(cmdUpdate, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdUpdate;
        }

        //private IDbCommand GetUpdateCommand(string commandText, string updateAction, params object[] paramValues)
        private DbCommand GetUpdateCommand(string commandText, string updateAction, object[] paramValues)
        {
            DbCommand cmdUpdate = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, updateAction, paramValues);

            PrepareCommandByColumn(cmdUpdate, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdUpdate;
        }

        private DbCommand GetUpdateCommand(string commandText, string updateAction, Dictionary<string, DataAccessParam> paramValues)
        {
            DbCommand cmdUpdate = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, updateAction, paramValues);

            PrepareCommandByColumn(cmdUpdate, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdUpdate;
        }

        //private IDbCommand GetDeleteCommand(string commandText, params  object[] paramValues)
        private DbCommand GetDeleteCommand(string commandText, object[] paramValues)
        {
            DbCommand cmdDelete = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, paramValues);

            PrepareCommandByColumn(cmdDelete, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdDelete;
        }

        //private IDbCommand GetDeleteCommand(string commandText, string deleteAction, params object[] paramValues)
        private DbCommand GetDeleteCommand(string commandText, string deleteAction, object[] paramValues)
        {
            DbCommand cmdDelete = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, deleteAction, paramValues);

            PrepareCommandByColumn(cmdDelete, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdDelete;
        }

        private DbCommand GetDeleteCommand(string commandText, string deleteAction, Dictionary<string, DataAccessParam> paramValues)
        {
            DbCommand cmdDelete = DBFactory.GetCommand(DataProvider);
            DbParameter[] arrSQLParameter = DBCache.GetSpParameterSet(DataProvider, ConnectionString, commandText);

            AssignParameterByColumn(arrSQLParameter, deleteAction, paramValues);

            PrepareCommandByColumn(cmdDelete, m_Conn, m_Trans, CommandType.StoredProcedure, commandText, arrSQLParameter);

            return cmdDelete;
        }

        /// <summary>
        /// Mục đích: Update những ForeignColumn có giá trị khác DBNull.Value và Không tồn tại giá trị trong PK Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataSource"></param>
        private void CorrectDataTableValue(string tableName, DataTable dataSource)
        {
            if (string.IsNullOrEmpty(tableName) || dataSource == null)
                return;

            try
            {
                //foreach (DataRow row in dataSource.Rows)
                //    if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified)
                //    {
                //        foreach (string ColumnName in dataSource.Columns)
                //            if (IsNullOrEmpty(row[ColumnName]))
                //                row[ColumnName] = DBNull.Value;
                //    }


                string strFK = this.GetForeignKeyColumn(tableName);
                if (string.IsNullOrEmpty(strFK))
                    return;

                char[] chrArr = { ',' };
                string[] arrFK = strFK.Split(chrArr);

                foreach (DataRow row in dataSource.Rows)
                    if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified)
                    {
                        foreach (string ColumnName in arrFK)
                            if (dataSource.Columns.Contains(ColumnName) && IsNullOrEmpty(row[ColumnName]))
                                row[ColumnName] = DBNull.Value;
                    }

            }
            catch { }
        }

        private void CorrectDataTableValue(string tableName, DataTable dataSource, DataTable dataSchemaTable)
        {
            if (string.IsNullOrEmpty(tableName) || dataSource == null)
                return;

            try
            {
                string strFK = this.GetForeignKeyColumn(tableName);
                if (string.IsNullOrEmpty(strFK))
                    return;

                char[] chrArr = { ',' };
                string[] arrFK = strFK.Split(chrArr);

                foreach (DataRow row in dataSource.Rows)
                    if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified)
                    {
                        foreach (string ColumnName in arrFK)
                            if (dataSource.Columns.Contains(ColumnName) && IsNullOrEmpty(row[ColumnName]))
                                row[ColumnName] = DBNull.Value;
                    }

            }
            catch { }
        }

        public int UpdateTable(DataTable ds, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames)
        {
            if (ds == null || ds.Columns.Count == 0)
                return -1;
            if (string.IsNullOrEmpty(insertCommandText) & string.IsNullOrEmpty(updateCommandText) & string.IsNullOrEmpty(deleteCommandText))
                return -1;
            if (paramNames == null || paramNames.Length == 0)
                return -1;
            if (ds.GetChanges() == null || ds.GetChanges().Rows.Count == 0)
                return 0;


            bool blnIsInTransaction = this.IsInTransaction;
            if (!blnIsInTransaction)
                BeginTransaction();

            m_Adapter = DBFactory.GetDataAdapter(DataProvider);

            m_Adapter.InsertCommand = GetInsertCommand(insertCommandText, paramNames);

            m_Adapter.UpdateCommand = GetUpdateCommand(updateCommandText, paramNames);

            m_Adapter.DeleteCommand = GetDeleteCommand(deleteCommandText, paramNames);

            try
            {
                if (ds != null && ds.TableName != "Table")
                {
                    //Table Name của DataTable phải là [Table] thì mới Update bằng DataAdapter thành công
                    ds.TableName = "Table";
                }

                int RowAff = m_Adapter.Update(ds);

                if (!blnIsInTransaction)
                    CommitTransaction();

                m_Adapter = null;

                return RowAff;
            }
            catch (Exception ex)
            {
                if (!blnIsInTransaction)
                    RollbackTransaction();

                m_Adapter = null;
                throw ex;
            }
        }

        public int UpdateTable(DataTable ds, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames)
        {
            if (ds == null || ds.Columns.Count == 0)
                return -1;
            if (string.IsNullOrEmpty(commandText))
                return -1;
            if (ds.GetChanges() == null || ds.GetChanges().Rows.Count == 0)
                return 0;

            if (ParamItems == null)
                ParamItems = new Dictionary<string, DataAccessParam>();
            else
                ParamItems.Clear();

            ParamItems.Add("Action".ToLower(), new DataAccessParam("Action", ""));
            ParamItems.Add("LoginLanguageID".ToLower(), new DataAccessParam("LoginLanguageID", LanguageID));
            ParamItems.Add("LoginApplicationID".ToLower(), new DataAccessParam("LoginApplicationID", ApplicationID));
            ParamItems.Add("LoginDomainID".ToLower(), new DataAccessParam("LoginDomainID", DomainID));
            ParamItems.Add("LoginDomainCode".ToLower(), new DataAccessParam("LoginDomainCode", DomainCode));
            ParamItems.Add("LoginUserID".ToLower(), new DataAccessParam("LoginUserID", UserID));
            ParamItems.Add("LoginUserCode".ToLower(), new DataAccessParam("LoginUserCode", UserCode));
            ParamItems.Add("LoginSessionID".ToLower(), new DataAccessParam("LoginSessionID", LoginSessionID));


            string TableName = string.Empty;
            if (paramNames == null || paramNames.Length == 0)
            {
                //Get TableName from CommandText Parameter
                if (commandText.ToLower().StartsWith("sp_"))
                    TableName = commandText.Substring(3, commandText.Length - 3);
                else if (commandText.ToLower().StartsWith("usp_"))
                    TableName = commandText.Substring(4, commandText.Length - 4);
                else if (commandText.ToLower().StartsWith("_sp_"))
                    TableName = commandText.Substring(4, commandText.Length - 4);

                if (TableName == string.Empty)
                    return -1;

                DataTable SchemaTable = GetSchemaTable(TableName);

                if (SchemaTable == null || SchemaTable.Rows.Count == 0)
                    return -1;

                for (int i = 0; i < SchemaTable.Rows.Count; i++)
                    ParamItems.Add(SchemaTable.Rows[i]["ColumnName"].ToString().ToLower(), new DataAccessParam(SchemaTable.Rows[i]["ColumnName"].ToString(), null, SchemaTable.Rows[i]["ColumnName"].ToString()));
            }
            else
            {
                for (int i = 0; i < paramNames.Length; i++)
                    ParamItems.Add(paramNames[i].ToLower(), new DataAccessParam(paramNames[i], null, paramNames[i]));

            }

            //Update những Record có giá trị FK Column không đúng
            CorrectDataTableValue(TableName, ds);

            bool blnIsInTransaction = this.IsInTransaction;
            if (!blnIsInTransaction)
                BeginTransaction();

            m_Adapter = DBFactory.GetDataAdapter(DataProvider);

            m_Adapter.InsertCommand = GetInsertCommand(commandText, insertAction, ParamItems);

            m_Adapter.UpdateCommand = GetUpdateCommand(commandText, updateAction, ParamItems);

            m_Adapter.DeleteCommand = GetDeleteCommand(commandText, deleteAction, ParamItems);

            try
            {
                if (ds != null && ds.TableName != "Table")
                {
                    //Table Name của DataTable phải là [Table] thì mới Update bằng DataAdapter thành công
                    ds.TableName = "Table";
                }

                int RowAff = m_Adapter.Update(ds);

                if (!blnIsInTransaction)
                    CommitTransaction();

                m_Adapter = null;

                return RowAff;
            }
            catch (Exception ex)
            {
                if (!blnIsInTransaction)
                    RollbackTransaction();

                m_Adapter = null;
                throw ex;
            }
        }
        private int UpdateTable1(DataTable ds, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames)
        {
            if (ds == null)
                return -1;
            if (string.IsNullOrEmpty(commandText))
                return -1;
            if (ds.GetChanges() == null || ds.GetChanges().Rows.Count == 0)
                return 0;

            object[] arrayValues = null;
            if (ParamItems == null)
                ParamItems = new Dictionary<string, DataAccessParam>();
            else
                ParamItems.Clear();

            string TableName = string.Empty;
            if (paramNames == null || paramNames.Length == 0)
            {
                //Get TableName from CommandText Parameter
                if (commandText.ToLower().StartsWith("sp_"))
                    TableName = commandText.Substring(3, commandText.Length - 3);
                else if (commandText.ToLower().StartsWith("usp_"))
                    TableName = commandText.Substring(4, commandText.Length - 4);
                else if (commandText.ToLower().StartsWith("_sp_"))
                    TableName = commandText.Substring(4, commandText.Length - 4);

                if (TableName == string.Empty)
                    return -1;

                DataTable SchemaTable = GetSchemaTable(TableName);

                if (SchemaTable == null || SchemaTable.Rows.Count == 0)
                    return -1;

                arrayValues = new object[SchemaTable.Rows.Count + 3];
                arrayValues.SetValue("Action", 0);
                arrayValues.SetValue(LanguageID, 1);
                arrayValues.SetValue(DomainID, 2);

                for (int i = 0; i < SchemaTable.Rows.Count; i++)
                    arrayValues.SetValue(SchemaTable.Rows[i]["ColumnName"].ToString(), i + 3);
            }
            else
            {
                arrayValues = paramNames;
                if (paramNames == null || paramNames.Length == 0)
                {
                    arrayValues = new object[3];
                    arrayValues.SetValue("Action", 0);
                    arrayValues.SetValue(LanguageID, 1);
                    arrayValues.SetValue(DomainID, 2);
                }
                else if (paramNames[0].ToString().ToLower() != "action")
                {
                    arrayValues = new object[paramNames.Length + 3];
                    arrayValues.SetValue("Action", 0);
                    arrayValues.SetValue(LanguageID, 1);
                    arrayValues.SetValue(DomainID, 2);

                    paramNames.CopyTo(arrayValues, 3);
                }
            }

            //Update những Record có giá trị FK Column không đúng
            CorrectDataTableValue(TableName, ds);

            bool blnIsInTransaction = this.IsInTransaction;
            if (!blnIsInTransaction)
                BeginTransaction();

            m_Adapter = DBFactory.GetDataAdapter(DataProvider);

            m_Adapter.InsertCommand = GetInsertCommand(commandText, insertAction, arrayValues);

            m_Adapter.UpdateCommand = GetUpdateCommand(commandText, updateAction, arrayValues);

            m_Adapter.DeleteCommand = GetDeleteCommand(commandText, deleteAction, arrayValues);

            try
            {
                if (ds != null && ds.TableName != "Table")
                {
                    //Table Name của DataTable phải là [Table]
                    ds.TableName = "Table";
                }

                int RowAff = m_Adapter.Update(ds);

                if (!blnIsInTransaction)
                    CommitTransaction();

                m_Adapter = null;

                return RowAff;
            }
            catch (Exception ex)
            {
                if (!blnIsInTransaction)
                    RollbackTransaction();

                m_Adapter = null;
                throw ex;
            }
        }

        public int UpdateTable(DataTable ds, string commandText, string insertAction, string updateAction, string deleteAction)
        {
            if (ds == null || ds.Columns.Count == 0)
                return -1;
            if (string.IsNullOrEmpty(commandText))
                return -1;

            return UpdateTable(ds, commandText, insertAction, updateAction, deleteAction, null);
        }

        public int UpdateTable(DataSet ds, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames)
        {
            if (ds == null)
                return -1;
            if (ds.Tables.Count == 0)
                return 0;
            if (string.IsNullOrEmpty(insertCommandText) & string.IsNullOrEmpty(updateCommandText) & string.IsNullOrEmpty(deleteCommandText))
                return -1;
            if (paramNames == null || paramNames.Length == 0)
                return -1;
            if (ds.Tables[0].GetChanges() == null || ds.Tables[0].GetChanges().Rows.Count == 0)
                return 0;

            return UpdateTable(ds.Tables[0], insertCommandText, updateCommandText, deleteCommandText, paramNames);
        }

        public int UpdateTable(DataSet ds, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames)
        {
            if (ds == null)
                return -1;
            if (ds.Tables.Count == 0)
                return 0;
            if (string.IsNullOrEmpty(commandText))
                return -1;
            if (ds.Tables[0].GetChanges() == null || ds.Tables[0].GetChanges().Rows.Count == 0)
                return 0;

            return UpdateTable(ds.Tables[0], commandText, insertAction, updateAction, deleteAction, paramNames);
        }

        public int UpdateTable(DataSet ds, string commandText, string insertAction, string updateAction, string deleteAction)
        {
            if (ds == null)
                return -1;
            if (ds.Tables.Count == 0)
                return 0;
            if (string.IsNullOrEmpty(commandText))
                return -1;

            return UpdateTable(ds.Tables[0], commandText, insertAction, updateAction, deleteAction);
        }

        #endregion UpdateTable


        #region Support Input Params Into Supply
        //private static Hashtable ParamCollections { get; set; }
        //internal static Hashtable ParamCollections { get; set; }
        internal Hashtable ParamCollections { get; set; }

        /// <summary>
        /// Add Default Param: LanguageID/LoginLanguageID, DomainID/LoginDomainID, UserID/LoginUserID, UserCode/LoginUserCode, LoginSessionID
        /// Call this method of after add [Action] param if needed
        /// </summary>
        public void InputDefaultParams()
        {
            //if (!ContainsParam(ACTION_PARAM_NAME))
            //    InputParams(ACTION_PARAM_NAME, string.Empty);

            if (!ContainsParam(CommonParamName.Action.ToString()))
                InputParams(CommonParamName.Action.ToString(), string.Empty);

            InputParams("LoginLanguageID", DataAccess.LanguageID);
            InputParams("LoginApplicationID", DataAccess.ApplicationID);
            InputParams("LoginDomainID", DataAccess.DomainID);
            InputParams("LoginDomainCode", DataAccess.DomainCode);
            InputParams("LoginUserID", DataAccess.UserID);
            InputParams("LoginUserCode", DataAccess.UserCode);
            InputParams("LoginSessionID", DataAccess.LoginSessionID);
            InputParams("FreeParameter", string.Empty);
        }

        /// <summary>
        /// Add Default Param: Action, LanguageID/LoginLanguageID, DomainID/LoginDomainID, UserID/LoginUserID, UserCode/LoginUserCode, LoginSessionID
        /// Call this method of after add [Action] param if needed
        /// </summary>
        public void InputDefaultParams(string actionParamValue)
        {
            //if (DataAccess.ParamCollections != null && DataAccess.ParamCollections.Count > 0)
            //    DataAccess.ParamCollections.Clear();

            //if (!ContainsParam(ACTION_PARAM_NAME))
            //    InputParams(ACTION_PARAM_NAME, string.Empty);

            if (!ContainsParam(CommonParamName.Action.ToString()))
                InputParams(CommonParamName.Action.ToString(), actionParamValue);
            else
                ParamCollections[CommonParamName.Action.ToString()] = actionParamValue;

            InputParams("LoginLanguageID", DataAccess.LanguageID);
            InputParams("LoginApplicationID", DataAccess.ApplicationID);
            InputParams("LoginDomainID", DataAccess.DomainID);
            InputParams("LoginDomainCode", DataAccess.DomainCode);
            InputParams("LoginUserID", DataAccess.UserID);
            InputParams("LoginUserCode", DataAccess.UserCode);
            InputParams("LoginSessionID", DataAccess.LoginSessionID);
            InputParams("FreeParameter", string.Empty);
        }

        public void InputParams(string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(paramName))
                return;
            paramName = paramName.Trim();
            if (string.IsNullOrEmpty(paramName))
                return;

            if (ParamCollections == null)
                ParamCollections = new Hashtable();


            if (ParamCollections.ContainsKey(paramName))
                ParamCollections[paramName] = paramValue;
            else
                ParamCollections.Add(paramName, paramValue);
        }

        public void InputParams(string paramName, params object[] paramValue)
        {
            if (string.IsNullOrEmpty(paramName) || paramValue == null || paramValue.Length == 0)
                return;
            if (ParamCollections == null)
                ParamCollections = new Hashtable();

            string[] ParamList = paramName.Split(new char[] { ',' });
            if (paramName.IndexOf(";") > -1)
                ParamList = paramName.Split(new char[] { ';' });
            int index = 0;
            foreach (string ParamName in ParamList)
            {
                InputParams(ParamName.Trim(), paramValue[index++]);
            }
        }

        public void DeleteParam(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return;
            if (ParamCollections != null && ParamCollections.Count > 0)
                ParamCollections.Remove(paramName);
        }

        public bool ContainsParam(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return false;
            if (ParamCollections == null || ParamCollections.Count == 0)
                return false;
            return ParamCollections.ContainsKey(paramName.Trim());
        }

        public void ClearParam()
        {
            if (ParamCollections != null)
                ParamCollections.Clear();
        }

        public void GetParams(ref string paramNames, ref object[] paramValues)
        {
            paramNames = GetParamList();
            paramValues = GetValueList();
        }

        private string GetParamList()
        {
            string Params = string.Empty;
            if (ParamCollections != null && ParamCollections.Count > 0)
            {
                IDictionaryEnumerator en = ParamCollections.GetEnumerator();

                while (en.MoveNext())
                {
                    Params += en.Key.ToString() + ",";
                }

                if (Params.Length > 0)
                    Params = Params.Substring(0, Params.Length - 1);
            }
            return Params;
        }

        private object[] GetValueList()
        {
            object[] obj = null;
            if (ParamCollections != null && ParamCollections.Count > 0)
            {
                IDictionaryEnumerator en = ParamCollections.GetEnumerator();
                int count = 0;
                obj = new object[ParamCollections.Count];

                while (en.MoveNext())
                {
                    obj[count++] = en.Value;
                }
            }
            return obj;
        }

        public int ParamLength()
        {
            if (ParamCollections != null)
                return ParamCollections.Count;
            else
                return 0;
        }

        public void DebugParams()
        {
            if (ParamCollections != null && ParamCollections.Count > 0)
            {
                IDictionaryEnumerator en = ParamCollections.GetEnumerator();

                while (en.MoveNext())
                {

                   Log.Write(en.Key.ToString() + ":" + en.Value.ToString());

                }
            }
        }
        #endregion


        #region Utility Method

        /// <summary>
        /// Returns String of Primary key column of Table, separate with comma character
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetPrimaryKey(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            //string strSQL = "SELECT * FROM " + tableName;
            //this.Open();

            //if (m_Cmd == null)
            //    m_Cmd = DBFactory.GetCommand(DataProvider);
            //PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.TableDirect, tableName, null);

            //IDataReader dr = m_Cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //DataTable dt = dr.GetSchemaTable();

            IDataReader dr = this.ExecuteReader("SELECT * FROM " + tableName, CommandBehavior.KeyInfo);
            DataTable dt = dr.GetSchemaTable();
            string strColumn = string.Empty;
            foreach (DataRow row in dt.Rows)
                if (bool.Parse(row["IsKey"].ToString()))
                {
                    if (strColumn != string.Empty)
                        strColumn += ",";
                    strColumn += row["ColumnName"].ToString();
                }

            dr.Close();

            if (!IsInTransaction)
                this.Close();

            return strColumn;
        }

        /// <summary>
        /// Returns Array of Primary key column of Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string[] GetPrimaryKeys(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return null;

            //string strSQL = "SELECT * FROM " + tableName;
            //this.Open();

            //if (m_Cmd == null)
            //    m_Cmd = DBFactory.GetCommand(DataProvider);
            //PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.TableDirect, tableName, null);

            //IDataReader dr = m_Cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //DataTable dt = dr.GetSchemaTable();

            IDataReader dr = this.ExecuteReader("SELECT * FROM " + tableName, CommandBehavior.KeyInfo);
            DataTable dt = dr.GetSchemaTable();
            string[] strPrimaryKey = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] KeyRows = dt.Select("IsKey = 1");
                strPrimaryKey = new string[KeyRows.Length];
                int index = 0;
                foreach (DataRow row in KeyRows)
                    strPrimaryKey[index++] = row["ColumnName"].ToString();
            }
            dr.Close();

            if (!IsInTransaction)
                this.Close();

            return strPrimaryKey;
        }

        /// <summary>
        /// Returns String of Primary key column of Table, separate with comma character
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetTableColumn(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            //string strSQL = "SELECT * FROM " + tableName;
            //this.Open();

            //if (m_Cmd == null)
            //    m_Cmd = DBFactory.GetCommand(DataProvider);
            //PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.TableDirect, tableName, null);

            //IDataReader dr = m_Cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //DataTable dt = dr.GetSchemaTable();

            IDataReader dr = this.ExecuteReader("SELECT * FROM " + tableName, CommandBehavior.KeyInfo);
            DataTable dt = dr.GetSchemaTable();
            string strColumn = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if (strColumn != string.Empty)
                    strColumn += ",";
                strColumn += row["ColumnName"].ToString();
            }

            dr.Close();

            if (!IsInTransaction)
                this.Close();

            return strColumn;
        }

        /// <summary>
        /// Returns Array of Primary key column of Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string[] GetTableColumns(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return null;

            //string strSQL = "SELECT * FROM " + tableName;
            //this.Open();

            //if (m_Cmd == null)
            //    m_Cmd = DBFactory.GetCommand(DataProvider);
            //PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.TableDirect, tableName, null);

            //IDataReader dr = m_Cmd.ExecuteReader(CommandBehavior.KeyInfo);
            //DataTable dt = dr.GetSchemaTable();

            IDataReader dr = this.ExecuteReader("SELECT * FROM " + tableName, CommandBehavior.KeyInfo);
            DataTable dt = dr.GetSchemaTable();
            string[] strColumn = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                strColumn = new string[dt.Rows.Count];
                int index = 0;
                foreach (DataRow row in dt.Rows)
                    strColumn[index++] = row["ColumnName"].ToString();
            }
            dr.Close();

            if (!IsInTransaction)
                this.Close();

            return strColumn;
        }

        /// <summary>
        /// Return DataTable Schema of Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetSchemaTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return null;

            try
            {
                //string strSQL = "SELECT * FROM " + tableName;
                //this.Open();

                //if (m_Cmd == null)
                //    m_Cmd = DBFactory.GetCommand(DataProvider);
                //PrepareCommand(m_Cmd, m_Conn, m_Trans, CommandType.TableDirect, tableName, null);

                //IDataReader dr = m_Cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //DataTable dt = dr.GetSchemaTable();

                IDataReader dr = this.ExecuteReader("SELECT * FROM " + tableName, CommandBehavior.KeyInfo);
                DataTable dt = dr.GetSchemaTable();
                dr.Close();

                if (!IsInTransaction)
                    this.Close();

                return dt;
            }
            catch //(Exception ex)
            {
                if (!IsInTransaction)
                    this.Close();

                return null;
                //throw ex;
            }
        }


        /// <summary>
        /// Return all Foreign Key Column of Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetForeignKeyColumn(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            string strSQL = "SELECT * FROM dbo.ufn_SysGetForeignKey('" + tableName + "')";
            strSQL = "SELECT " +
            "FK_Column = CU.COLUMN_NAME " +
            "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C " +
            "INNER JOIN (SELECT CONSTRAINT_NAME, TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = '" + tableName + "') FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME " +
            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME " +
            "INNER JOIN ( " +
            "SELECT i1.TABLE_NAME, i2.COLUMN_NAME " +
            "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 " +
            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME " +
            "WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
            ") PT ON PT.TABLE_NAME = PK.TABLE_NAME";   //AND FK.TABLE_NAME = @TableName " +

            DataTable dtData = this.ExecuteDataTable(strSQL);

            if (dtData == null || dtData.Rows.Count == 0)
                return string.Empty;

            string strPrimaryKey = "";
            foreach (DataRow row in dtData.Rows)
                strPrimaryKey += row["FK_Column"].ToString() + ",";

            if (strPrimaryKey.LastIndexOf(",") > -1)
                strPrimaryKey = strPrimaryKey.Substring(0, strPrimaryKey.Length - 1);

            return strPrimaryKey;
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

        #endregion


        #region Set DataRow State
        /// <summary>
        /// Set RowState = Added cho tất cả DataRow thuộc DataTable
        /// </summary>
        /// <param name="dataTable"></param>
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

        /// <summary>
        /// Set RowState = Added cho tất cả DataRow hoặc những DataRow có RowState = UnChanged
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="setAllRow"></param>
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

        /// <summary>
        /// Set RowState = Deleted cho tất cả DataRow thuộc DataTable
        /// </summary>
        /// <param name="dataTable"></param>
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

        /// <summary>
        /// Set RowState = Modified cho tất cả DataRow
        /// </summary>
        /// <param name="dataTable"></param>
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

        /// <summary>
        /// Set RowState = Modified cho tất cả DataRow hoặc những DataRow có RowState = UnChanged
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="setAllRow"></param>
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
                case "guid":
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
            }
            catch {
                return (Single)0.00;
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
                //value = Decimal.Parse(objSource.ToString());
                //else if (!(Decimal.TryParse(objSource.ToString(), out value)))
                //    value = returnValue;
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
                else if (objSource is Guid)
                    value = new Guid(objSource.ToString());
                else
                    value = new Guid(objSource.ToString());
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

            return GetValueNull(dataTypeName, true);

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

            return GetValueDBNull(dataTypeName, true);

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

    }

}


