using System;
using System.Data;

namespace OnChotto.Models.Dao
{
    public interface IBaseObject : IDisposable
    {

        #region Methods
        int Insert();
        int Update();

        
        int Delete();
        int Delete(string keyValue);
        int Delete(byte keyValue);
        int Delete(Int16 keyValue);
        int Delete(Int32 keyValue);
        int Delete(Int64 keyValue);
        //int Delete(object[] keyValue);

        int Delete(string action, string paramName, string keyValue);
        int Delete(string action, string paramName, byte keyValue);
        int Delete(string action, string paramName, Int16 keyValue);
        int Delete(string action, string paramName, Int32 keyValue);
        int Delete(string action, string paramName, Int64 keyValue);
        int Delete(string action, string paramName, object[] keyValue);


        //bool GetByKey();
        bool GetByKey(string keyValue);
        bool GetByKey(byte keyValue);
        bool GetByKey(Int16 keyValue);
        bool GetByKey(Int32 keyValue);
        bool GetByKey(Int64 keyValue);
        //bool GetByKey(object[] keyValue);

        //bool GetByKey(string action);
        bool GetByKey(string action, string paramName, string keyValue);
        bool GetByKey(string action, string paramName, byte keyValue);
        bool GetByKey(string action, string paramName, Int16 keyValue);
        bool GetByKey(string action, string paramName, Int32 keyValue);
        bool GetByKey(string action, string paramName, Int64 keyValue);
        bool GetByKey(string action, string paramName, object[] keyValue);


        //bool IsExists();
        bool IsExists(string keyValue);
        bool IsExists(byte keyValue);
        bool IsExists(Int16 keyValue);
        bool IsExists(Int32 keyValue);
        bool IsExists(Int64 keyValue);
        //bool IsExists(object[] keyValue);

        //bool IsExists(string action);
        bool IsExists(string action, string paramName, string keyValue);
        bool IsExists(string action, string paramName, byte keyValue);
        bool IsExists(string action, string paramName, Int16 keyValue);
        bool IsExists(string action, string paramName, Int32 keyValue);
        bool IsExists(string action, string paramName, Int64 keyValue);
        bool IsExists(string action, string paramName, object[] keyValue);

        
        DataTable GetAll();
        DataTable GetAll(string action);
        DataTable GetAll(string paramName, string paramValue);
        DataTable GetAll(string paramName, byte paramValue);
        DataTable GetAll(string paramName, Int16 paramValue);
        DataTable GetAll(string paramName, Int32 paramValue);
        DataTable GetAll(string paramName, Int64 paramValue);
        DataTable GetAll(string paramName, object[] paramValue);

        DataTable GetAll(string action, string paramName, string paramValue);
        DataTable GetAll(string action, string paramName, byte paramValue);
        DataTable GetAll(string action, string paramName, Int16 paramValue);
        DataTable GetAll(string action, string paramName, Int32 paramValue);
        DataTable GetAll(string action, string paramName, Int64 paramValue);
        DataTable GetAll(string action, string paramName, object[] paramValue);


        void Reset();
        void Fill(DataRow row);

        //Bổ sung thêm ngày 21-Mar-2012
        //bool Validate(ref string errorMessage);
        //bool Validate(string action, ref string errorMessage); 
        //bool Validate(string action, string paramName, object[] paramValue, ref string errorMessage);

        //End of Bổ sung thêm ngày 21-Mar-2012

        int UpdateTable(DataTable dataSource);
        int UpdateTable(DataTable dataSource, string commandText, string[] paramNames);
        int UpdateTable(DataTable dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames);
        int UpdateTable(DataTable dataSource, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames);

        int UpdateTable(DataSet dataSource);
        int UpdateTable(DataSet dataSource, string commandText, string[] paramNames);
        int UpdateTable(DataSet dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames);
        int UpdateTable(DataSet dataSource, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames);


        //int UpdateTable(DataTable dataSource, string insertCommandText, string updateCommandText, string deleteCommandText);
        //int UpdateTable(DataTable dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, params object[] paramValues);
        //int UpdateTable(DataTable dataSource, string commandText, string insertAction, string updateAction, string deleteAction, params object[] paramValues);
        //int UpdateTable(DataSet dataSource, string insertCommandText, string updateCommandText, string deleteCommandText);
        //int UpdateTable(DataSet dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, params object[] paramValues);
        //int UpdateTable(DataSet dataSource, string commandText, string insertAction, string updateAction, string deleteAction, params object[] paramValues);
        #endregion


        #region Property
        
        DataAccess DataAccessObject { get; set; }
        OutputResult KeyObject { get; }
        DateTime SystemDate { get; }

        #endregion

    }

    //private interface IBaseObjectEntity : IDisposable
    //{
    //    string EntityTableName { get; set; }

    //    string EntityProcedureName { get; set; }

    //    List<string> FieldNames { get; }

    //    List<string> FieldKeys { get; }

    //    List<string> AlternateFieldKeys { get; }
    // }
}
