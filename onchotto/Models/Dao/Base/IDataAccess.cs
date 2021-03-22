using System.Data;
using System.Data.Common;

namespace OnChotto.Models.Dao
{
    public interface IDataAccess
    {
        DbDataReader DataReader
        {
            get;
        }

        DbConnection Connection
        {
            get;
        }

        DbTransaction Transaction
        {
            get;
        }

        DbCommand Command
        {
            get;
        }

        DbDataAdapter Adapter
        {
            get;
        }

        DbParameter[] Parameters
        {
            get;
        }

        //DbParameter[] Param
        //{
        //    get;
        //}

        void Open();
        void Close();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void CreateParameters(int paramsCount);
        void AddParameters(int index, string paramName, object objValue);
        object ExecuteScalar(string commandText);
        object ExecuteScalar(string commandText, object[] p_arrValue);
        int ExecuteNonQuery(string commandText);
        int ExecuteNonQuery(string commandText, object[] p_arrValue);
        void ExecuteDataSet(DataSet ds, string commandText);
        void ExecuteDataSet(DataSet ds, string commandText, object[] p_arrValue);
    }
}
