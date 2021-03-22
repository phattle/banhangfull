using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OnChotto.Models.Dao
{
    //public class BaseObject : IBaseObject, IDisposable
    public abstract class BaseObject : IBaseObject, IDisposable //, ICollection<T>
    {
        #region Variable

        public OutputResult OutputObject = null;
        private OutputResult m_KeyObject = null;
        private DataAccess m_DataAccessObject = null;
        List<BaseObject> m_Items = new List<BaseObject>();
        List<string> m_FieldNames = new List<string>();
        List<string> m_FieldKeys = new List<string>();
        List<string> m_FieldUniqueKeys = new List<string>();
        Dictionary<string, string> m_AlternateFieldKeys = new Dictionary<string, string>();


        public enum Action
        {
            Insert,
            Update,
            Delete,
            GetByKey,
            GetByCode,
            IsExists,
            GetAll
        }
        #endregion

        #region Constants
        const string ACTION_PARAM_NAME = "Action";
        const string FREE_PARAM_NAME = "FreeParameter";
        #endregion

        #region Contructor

        public BaseObject()
        {
            m_DataAccessObject = new DataAccess();
            this.Initialize();
            this.Reset();
        }

        public BaseObject(string connectionString)
            : this()
        {
            m_DataAccessObject = new DataAccess(connectionString);
        }

        public BaseObject(DataAccess dataAccess)
            : this()
        {
            m_DataAccessObject = dataAccess;
        }

        //public BaseObject(DataRow row)
        //    : this()
        //{
        //    this.Fill(row);
        //    //this.Items.Add(this);
        //}

        //public BaseObject(DataTable dataSource)
        //    : this()
        //{
        //    if (dataSource == null || dataSource.Rows.Count == 0) return;
        //    this.AddItems(dataSource);
        //}

        //public BaseObject(DataTable dataSource)
        //    : this()
        //{
        //    if (dataSource == null || dataSource.Rows.Count == 0) return;

        //    int index = 0;
        //    blnIsAddItems = false;
        //    foreach (DataRow row in dataSource.Rows)
        //    {
        //        //this.Items.Add(BaseObject());
        //        //this.Fill(row);
        //        BaseObject a = new BaseObject();
        //        this.Items.Add(this);
        //        this.Items[index++].Fill(row);
        //    }
        //    blnIsAddItems = true;
        //}

        protected virtual void Initialize()
        {

        }
        #endregion

        #region Property
        public List<BaseObject> Items
        {
            get { return m_Items; }
        }

        public virtual string EntityTableName { get; set; }

        public virtual string EntityProcedureName { get; set; }

        public virtual string EntityProcedureNameUser { get; set; } 

        public virtual List<string> FieldNames
        {
            get { return m_FieldNames; }
            protected set { m_FieldNames = value; }
        }

        public virtual List<string> FieldKeys
        {
            get { return m_FieldKeys; }
            protected set { m_FieldKeys = value; }
        }

        public virtual Dictionary<string, string> AlternateFieldKeys
        {
            get { return m_AlternateFieldKeys; }
            protected set { m_AlternateFieldKeys = value; }
        }

        public virtual List<string> FieldUniqueKeys
        {
            get { return m_FieldUniqueKeys; }
            protected set { m_FieldUniqueKeys = value; }
        }


        //public virtual List<string> AlternateFieldKeys
        //{
        //    get { return m_AlternateFieldKeys; }
        //    protected set { m_AlternateFieldKeys = value; }
        //}
        public DataAccess DataAccessObject
        {
            get
            {
                return m_DataAccessObject;
            }
            set
            {
                m_DataAccessObject = value;
            }
        }

        public OutputResult KeyObject
        {
            get
            {
                return m_KeyObject;
            }
        }

        public DateTime SystemDate
        {
            get
            {
                try
                {
                    using (DataAccess oda = new DataAccess())
                    {
                        if (OnChotto.Models.Dao.DataAccess.DataProvider == OnChotto.Models.Dao.DataProvider.Sql)
                            return Convert.ToDateTime(oda.ExecuteScalar("SELECT GETDATE()"));
                        else if (OnChotto.Models.Dao.DataAccess.DataProvider == OnChotto.Models.Dao.DataProvider.Oracle)
                            return Convert.ToDateTime(oda.ExecuteScalar("SELECT SYSDATE FROM DUAL"));
                        else
                            return Convert.ToDateTime(oda.ExecuteScalar("SELECT GETDATE()"));
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }

        //public static string LoginLanguageID
        //{
        //    get
        //    {
        //        return DataAccess.LanguageID;
        //    }
        //    //set
        //    //{
        //    //    m_LanguageID = value;
        //    //}
        //}

        //public static int LoginDomainID
        //{
        //    get
        //    {
        //        return DataAccess.DomainID;
        //    }
        //    //set
        //    //{
        //    //    m_DomainID = value;
        //    //}
        //}

        //public static string LoginDomainCode
        //{
        //    get
        //    {
        //        return DataAccess.DomainCode;
        //    }
        //    //set
        //    //{
        //    //    m_DomainID = value;
        //    //}
        //}

        //public static int LoginUserID
        //{
        //    get
        //    {
        //        return DataAccess.UserID;
        //    }
        //    //set
        //    //{
        //    //    m_DomainID = value;
        //    //}
        //}

        //public static string LoginUserCode
        //{
        //    get
        //    {
        //        return DataAccess.UserCode;
        //    }
        //    //set
        //    //{
        //    //    m_LanguageID = value;
        //    //}
        //}

        //public static int LoginSessionID
        //{
        //    get
        //    {
        //        return DataAccess.LoginSessionID;
        //    }
        //    //set
        //    //{
        //    //    m_DomainID = value;
        //    //}
        //}

        #endregion

        #region Override

        public override string ToString()
        {
            String sValue = String.Empty;
            try
            {
                if (FieldNames != null)
                {
                    if (FieldNames.Count > 0)
                    {
                        System.Reflection.PropertyInfo objInfo = null;
                        foreach (string FieldName in FieldNames)
                        {
                            objInfo = this.GetType().GetProperty(FieldName);
                            if (objInfo != null)
                            {
                                objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                                sValue += "<" + FieldName + ":" + objInfo.GetValue(this, null).ToString() + ">" + "; ";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return base.ToString();
            }
            return sValue;
        }

        public virtual String GetFieldKeysData()
        {
            String sValue = String.Empty;
            try
            {
                if (FieldKeys != null)
                {
                    if (FieldKeys.Count > 0)
                    {
                        System.Reflection.PropertyInfo objInfo = null;
                        foreach (string FieldName in FieldKeys)
                        {
                            objInfo = this.GetType().GetProperty(FieldName);
                            if (objInfo != null)
                                objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                            sValue += "<" + FieldName + ":" + objInfo.GetValue(this, null).ToString() + ">" + "; ";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return base.ToString();
            }
            return sValue;
        }

        public virtual String GetFieldUniqueKeysData()
        {
            String sValue = String.Empty;
            try
            {
                if (FieldUniqueKeys != null)
                {
                    if (FieldUniqueKeys.Count > 0)
                    {
                        System.Reflection.PropertyInfo objInfo = null;
                        foreach (string FieldName in FieldUniqueKeys)
                        {
                            objInfo = this.GetType().GetProperty(FieldName);
                            if (objInfo != null)
                                objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                            sValue += "<" + FieldName + ":" + objInfo.GetValue(this, null).ToString() + ">" + "; ";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return base.ToString();
            }
            return sValue;

        }

        public virtual Boolean IsBeforUpdateDictionary()
        {
            return true;
        }

        public virtual Boolean IsBeforDeleteDictionary()
        {
            return true;
        }


        #endregion

        #region Method


        #region IBaseObject Members

        public virtual BaseObject Clone()
        {

            return Clone(this);
        }

        public static BaseObject Clone(BaseObject source)
        {
            return source.MemberwiseClone() as BaseObject;
        }


        public virtual int AddItem(DataTable dataSource)
        {
            int index = 0;
            if (dataSource == null || dataSource.Rows.Count == 0) return index;

            foreach (DataRow row in dataSource.Rows)
            {
                object objType = this.GetType().InvokeMember(string.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                if (objType != null)
                {
                    (objType as BaseObject).Fill(row);
                    this.Items.Add(objType as BaseObject);
                    index++;
                }
            }
            return index;
        }

        public virtual int AddItem(DataRow[] rows)
        {
            int index = 0;
            if (rows == null || rows.Length == 0) return index;

            foreach (DataRow row in rows)
            {
                object objType = this.GetType().InvokeMember(string.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                if (objType != null)
                {
                    (objType as BaseObject).Fill(row);
                    this.Items.Add(objType as BaseObject);
                    index++;
                }
            }
            return index;
        }

        public virtual int AddItem(DataRow row)
        {
            int index = 0;
            if (row == null || row.ItemArray.Length == 0) return index;

            object objType = this.GetType().InvokeMember(string.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
            if (objType != null)
            {
                (objType as BaseObject).Fill(row);
                this.Items.Add(objType as BaseObject);
                index++;
            }
            return index;
        }

        public virtual int Insert()
        {
            this.OutputObject = null;

            DataAccessObject.ClearParam();
            DataAccessObject.InputDefaultParams(Action.Insert.ToString());

            System.Reflection.PropertyInfo objInfo = null;
            foreach (string FieldName in FieldNames)
            {
                objInfo = this.GetType().GetProperty(FieldName);
                if (objInfo == null)
                    objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                if (objInfo != null && objInfo.CanWrite)
                    DataAccessObject.InputParams(FieldName, objInfo.GetValue(this, null));
            }

            int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues, ref this.OutputObject);

            if (this.OutputObject != null && this.OutputObject.Length > 0 && FieldKeys != null && FieldKeys.Count > 0)
            {
                objInfo = null;
                foreach (string FieldName in FieldKeys)
                {
                    if (string.IsNullOrEmpty(FieldName)) continue;
                    objInfo = this.GetType().GetProperty(FieldName);
                    if (objInfo == null)
                        objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                    if (objInfo != null)
                    {
                        DataAccess.CorrectValue(this, objInfo, this.OutputObject.GetValue(FieldName));
                    }
                }
            }

            return intResult;
        }

        public virtual int Update()
        {
            this.OutputObject = null;

            DataAccessObject.ClearParam();
            DataAccessObject.InputDefaultParams(Action.Update.ToString());

            System.Reflection.PropertyInfo objInfo = null;
            foreach (string FieldName in FieldNames)
            {
                if (string.IsNullOrEmpty(FieldName)) continue;
                objInfo = this.GetType().GetProperty(FieldName);
                if (objInfo == null)
                    objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                if (objInfo != null && objInfo.CanWrite)
                    DataAccessObject.InputParams(FieldName, objInfo.GetValue(this, null));
            }

            int intResult = DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);

            return intResult;
        }

        #region Delete
        public virtual int Delete()
        {
            if (FieldKeys == null || FieldKeys.Count == 0)
                return -1;

            DataTable dt = new DataTable();

            DataAccessObject.ClearParam();
            DataAccessObject.InputDefaultParams(Action.Delete.ToString()); 

            System.Reflection.PropertyInfo objInfo = null;
            foreach (string FieldName in FieldKeys)
            {
                if (string.IsNullOrEmpty(FieldName)) continue;
                objInfo = this.GetType().GetProperty(FieldName);
                if (objInfo == null)
                    objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                if (objInfo != null)
                    DataAccessObject.InputParams(FieldName, objInfo.GetValue(this, null));
            }

            return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
        }

        public virtual int Delete(string keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(byte keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(Int16 keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(Int32 keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(Int64 keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(Guid keyValue)
        {
            return Delete(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual int Delete(object[] keyValue)
        {
            return Delete(Action.Delete.ToString(), string.Empty, keyValue);
        }

        public virtual int Delete(string action, string paramName, string keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, byte keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, Int16 keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, Int32 keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, Int64 keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, Guid keyValue)
        {
            return Delete(action, paramName, new object[] { keyValue });
        }

        public virtual int Delete(string action, string paramName, object[] keyValue)
        {
            if (keyValue == null || keyValue.Length == 0)
                return -1;

            DataTable dt = new DataTable();

            DataAccessObject.ClearParam();
            if (string.IsNullOrEmpty(action))
                DataAccessObject.InputDefaultParams(Action.Delete.ToString());
            else
                DataAccessObject.InputDefaultParams(action);

            if (paramName == null || paramName.Trim() == string.Empty)
                this.InputParams(paramName, keyValue);
            else
                DataAccessObject.InputParams(paramName, keyValue);

            return DataAccessObject.ExecuteNonQuery(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
        }

        #endregion

        #region GetByKey

        public virtual bool GetByKey(string keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(byte keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(Int16 keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(Int32 keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(Int64 keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(Guid keyValue)
        {
            return GetByKey(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool GetByKey(object[] keyValue)
        {
            return GetByKey(Action.GetByKey.ToString(), string.Empty, keyValue);
        }

        public virtual bool GetByKey(string action, string paramName, string keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }

        public virtual bool GetByKey(string action, string paramName, byte keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }

        public virtual bool GetByKey(string action, string paramName, Int16 keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }

        public virtual bool GetByKey(string action, string paramName, Int32 keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }

        public virtual bool GetByKey(string action, string paramName, Int64 keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }


        public virtual bool GetByKey(string action, string paramName, Guid keyValue)
        {
            return GetByKey(action, paramName, new object[] { keyValue });
        }

        public virtual bool GetByKey(string action, string paramName, object[] keyValue)
        {
            if (keyValue == null || keyValue.Length == 0)
                return false;

            DataTable dt = new DataTable();

            DataAccessObject.ClearParam();
            if (string.IsNullOrEmpty(action))
                DataAccessObject.InputDefaultParams(Action.GetByKey.ToString());
            else
                DataAccessObject.InputDefaultParams(action);

            if (paramName == null || paramName.Trim() == string.Empty)
                this.InputParams(paramName, keyValue);
            else
                DataAccessObject.InputParams(paramName, keyValue);

            dt = DataAccessObject.ExecuteDataTable(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);

            if (dt == null || dt.Rows.Count == 0 || dt.Columns.Count == 0)
            {
                Reset();
                return false;
            }
            Fill(dt.Rows[0]);
            return true;
        }
        #endregion

        #region IsExists
        public virtual bool IsExists(string keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(byte keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(Int16 keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(Int32 keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(Int64 keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(Guid keyValue)
        {
            return IsExists(string.Empty, string.Empty, new object[] { keyValue });
        }

        public virtual bool IsExists(object[] keyValue)
        {
            return IsExists(Action.IsExists.ToString(), string.Empty, keyValue);
        }

        public virtual bool IsExists(string action, string paramName, string keyValue)
        {
            return IsExists(action, paramName, new object[] { keyValue });
        }

        public virtual bool IsExists(string action, string paramName, byte keyValue)
        {
            return IsExists(action, paramName, new object[] { keyValue });
        }

        public virtual bool IsExists(string action, string paramName, Int16 keyValue)
        {
            return IsExists(action, paramName, new object[] { keyValue });
        }

        public virtual bool IsExists(string action, string paramName, Int32 keyValue)
        {
            return IsExists(action, paramName, new object[] { keyValue });
        }

        public virtual bool IsExists(string action, string paramName, Int64 keyValue)
        {
            return IsExists(action, paramName, new object[] { keyValue });
        }

        public virtual bool IsExists(string action, string paramName, object[] keyValue)
        {
            if (keyValue == null || keyValue.Length == 0)
                return false;

            DataTable dt = new DataTable();

            DataAccessObject.ClearParam();
            if (string.IsNullOrEmpty(action))
                DataAccessObject.InputDefaultParams(Action.IsExists.ToString());
            else
                DataAccessObject.InputDefaultParams(action);

            if (paramName == null || paramName.Trim() == string.Empty)
                this.InputParams(paramName, keyValue);
            else
                DataAccessObject.InputParams(paramName, keyValue);

            var Value = DataAccessObject.ExecuteScalar(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);

            if (DataAccess.IsNullOrEmpty(Value))
                return false;
            else if (DataAccess.CorrectValue(Value, int.MinValue) <= 0)
                return false;
            else
                return true;
        }
        #endregion

        #region GetAll
        public virtual DataTable GetAll()
        {
            return GetAll(string.Empty, string.Empty, new object[] { });

        }

        public virtual DataTable GetAll(string action)
        {

            return GetAll(action, string.Empty, new object[] { });

        }

        public virtual DataTable GetAll(string paramName, string paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, byte paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, Int16 paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, Int32 paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, Int64 paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, Guid paramValue)
        {
            return GetAll(paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string paramName, object[] paramValue)
        {
            return GetAll(Action.GetAll.ToString(), paramName, paramValue);

        }

        public virtual DataTable GetAll(string action, string paramName, string paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string action, string paramName, byte paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string action, string paramName, Int16 paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string action, string paramName, Int32 paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }

        public virtual DataTable GetAll(string action, string paramName, Int64 paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }
        public virtual DataTable GetAll(string action, string paramName, Guid paramValue)
        {
            return GetAll(action, paramName, new object[] { paramValue });
        }
        public virtual DataTable GetAll(string action, string paramName, object[] paramValue)
        {
            DataTable dt = new DataTable();

            DataAccessObject.ClearParam();
            if (string.IsNullOrEmpty(action))
                DataAccessObject.InputDefaultParams(Action.GetAll.ToString());
            else
                DataAccessObject.InputDefaultParams(action);


            if (paramValue != null && paramValue.Length > 0)
            {
                char[] SplitChar1 = new char[] { ';' };
                char[] SplitChar2 = new char[] { ',' };
                string Char1 = ";";
                string Char2 = ",";

                if (string.IsNullOrEmpty(paramName))
                {
                    //Nơi gọi không đưa vào tên tham số cho Procedure ==> Set cho tham số dùng chung là [FreeParameter]
                    string strParamValue = string.Empty;
                    for (int i = 0; i < paramValue.Length; i++)
                    {
                        if (i > 0)
                            strParamValue += ";";
                        strParamValue += paramValue[i].ToString();
                    }
                    DataAccessObject.InputParams(FREE_PARAM_NAME, strParamValue);
                }
                else if (paramName.IndexOf(Char1) > -1)
                {
                    //Đưa vào nhiều tên tham số, phân cách bằng dấu ";"
                    paramName = paramName.Trim();
                    string[] ParamArary = paramName.Split(SplitChar1);
                    for (int i = 0; i < ParamArary.Length; i++)
                        if (!string.IsNullOrEmpty(ParamArary[i].Trim()) && i < paramValue.Length )
                            DataAccessObject.InputParams(ParamArary[i].Trim(), paramValue[i]);
                }
                else if (paramName.IndexOf(Char2) > -1)
                {
                    //Đưa vào nhiều tên tham số, phân cách bằng dấu ","
                    paramName = paramName.Trim();
                    string[] ParamArary = paramName.Split(SplitChar2);
                    for (int i = 0; i < ParamArary.Length; i++)
                        if (!string.IsNullOrEmpty(ParamArary[i].Trim()) && i < paramValue.Length)
                            DataAccessObject.InputParams(ParamArary[i].Trim(), paramValue[i]);
                }
                else
                {
                    //Chỉ đưa vào một tên tham số duy nhất
                    string strParamValue = string.Empty;
                    for (int i = 0; i < paramValue.Length; i++)
                    {
                        if (i > 0)
                            strParamValue += ";";
                        strParamValue += paramValue[i].ToString();
                    }
                    DataAccessObject.InputParams(paramName, strParamValue);
                }
            }

            return DataAccessObject.ExecuteDataTable(EntityProcedureName, DataAccessObject.ParamNames, DataAccessObject.ParamValues);
        }

        #endregion

        public virtual void Reset()
        {
            this.dtBackupData = null;
            if (FieldNames == null || FieldNames.Count == 0) 
                return;

            System.Reflection.PropertyInfo objInfo = null;
            foreach (string FieldName in FieldNames)
            {
                if (string.IsNullOrEmpty(FieldName))
                    continue;
                objInfo = this.GetType().GetProperty(FieldName);
                if (objInfo == null)
                    objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                if (objInfo != null && objInfo.CanWrite)
                    DataAccess.CorrectValue(this, objInfo, null);
            }
        }

        public virtual void Fill(DataRow row)
        {
            Reset();
            if (row == null || row.ItemArray.Length == 0)
                return;

            string FieldName = string.Empty;
            System.Reflection.PropertyInfo objInfo = null;
            try
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    FieldName = row.Table.Columns[i].ColumnName;
                    if (string.IsNullOrEmpty(FieldName))
                        continue;
                    objInfo = this.GetType().GetProperty(FieldName);
                    if (objInfo == null)
                        objInfo = this.GetType().GetProperty(FieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);
                    if (objInfo != null && objInfo.CanWrite && row.Table.Columns.Contains(FieldName))
                    {
                        DataAccess.CorrectValue(this, objInfo, row[FieldName]);
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("an error occurred ", ex);
                string Mess = "Lỗi sai kiểu dữ liệu khi gán giá trị cho thuộc tính [" + FieldName + "]"
                        + " có kiểu dữ liệu [" + objInfo.PropertyType.ToString() + "] với kiểu dữ liệu [" + row.Table.Columns[FieldName].DataType.ToString() + "]";
                throw new Exception(Mess, ex);
            }
        }

        #region UpdateTable
        public virtual int UpdateTable(DataTable dataSource)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, Action.Insert.ToString(), Action.Update.ToString(), Action.Delete.ToString());
        }

        public virtual int UpdateTable(DataTable dataSource, string commandText, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            if (string.IsNullOrEmpty(commandText))
                commandText = EntityProcedureName;
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, Action.Insert.ToString(), Action.Update.ToString(), Action.Delete.ToString(), paramNames);
        }

        public virtual int UpdateTable(DataTable dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, insertCommandText, updateCommandText, deleteCommandText, paramNames);
        }

        public virtual int UpdateTable(DataTable dataSource, string insertAction, string updateAction, string deleteAction)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, insertAction, updateAction, deleteAction);
        }

        public virtual int UpdateTable(DataTable dataSource, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            if (string.IsNullOrEmpty(commandText))
                commandText = EntityProcedureName;
            return this.DataAccessObject.UpdateTable(dataSource, commandText, insertAction, updateAction, deleteAction, paramNames);
        }

        public virtual int UpdateTable(DataSet dataSource)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, Action.Insert.ToString(), Action.Update.ToString(), Action.Delete.ToString());
        }

        public virtual int UpdateTable(DataSet dataSource, string commandText, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            if (string.IsNullOrEmpty(commandText))
                commandText = EntityProcedureName;
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, Action.Insert.ToString(), Action.Update.ToString(), Action.Delete.ToString(), paramNames);
        }

        public virtual int UpdateTable(DataSet dataSource, string insertCommandText, string updateCommandText, string deleteCommandText, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, insertCommandText, updateCommandText, deleteCommandText, paramNames);
        }

        public virtual int UpdateTable(DataSet dataSource, string insertAction, string updateAction, string deleteAction)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            return this.DataAccessObject.UpdateTable(dataSource, EntityProcedureName, insertAction, updateAction, deleteAction);
        }

        public virtual int UpdateTable(DataSet dataSource, string commandText, string insertAction, string updateAction, string deleteAction, string[] paramNames)
        {
            if (AutoBackupDataSource)
                BackupDataSource(dataSource);
            if (string.IsNullOrEmpty(commandText))
                commandText = EntityProcedureName;
            return this.DataAccessObject.UpdateTable(dataSource, commandText, insertAction, updateAction, deleteAction, paramNames);
        }
        #endregion

        #endregion


        #region Input Params
        private void InputParams(string paramNames, object[] keyValue)
        {
            if (keyValue == null || keyValue.Length == 0)
                return;

            bool IsValidKey = false;
            if (!string.IsNullOrEmpty(paramNames))
            {
                //Input value cho các Parameters đưa vào từ nơi gọi
                string[] ParamList = null;
                if (paramNames.IndexOf(",") > -1)
                    ParamList = paramNames.Split(new char[] { ',' });
                else
                    ParamList = paramNames.Split(new char[] { ';' });

                List<string> ListFields = ParamList.ToList<string>();  //new List<string>();
                //ListFields = ParamList.ToList<string>();
                //for (int i = 0; i < ParamList.Length; i++)
                //    ListFields.Add(ParamList[i].Trim());

                this.FillParamValue(ListFields, keyValue);
            }
            else
            {
                if (FieldKeys != null && keyValue.Length == FieldKeys.Count)
                {
                    //Input value cho các FieldKeys or AlternateFieldKeys Parameters
                    IsValidKey = this.FillParamValue(FieldKeys, keyValue);
                }

                if (!IsValidKey && AlternateFieldKeys != null && AlternateFieldKeys.Count > 0)
                {
                    string[] Fields = null;
                    List<string> ListField = new List<string>();
                    Dictionary<string, string>.ValueCollection valueColl = AlternateFieldKeys.Values;
                    foreach (string value in valueColl)
                    {
                        Fields = value.Split(new char[] { ',' });
                        if (Fields.Length >= keyValue.Length)
                        {
                            ListField = Fields.ToList<string>();
                            IsValidKey = this.FillParamValue(ListField, keyValue);
                            if (IsValidKey)
                                break;
                        }
                    }
                }
            }


            //if (string.IsNullOrEmpty(paramNames) && keyValue.Length == FieldKeys.Count)
            //{
            //    //Input value cho các FieldKeys or AlternateFieldKeys Parameters
            //    if (!this.FillParamValue(FieldKeys, keyValue))     //Get by Primary Key
            //        this.FillParamValue(AlternateFieldKeys, keyValue); //Get by Alternate/Unique Key
            //}
            //else if (string.IsNullOrEmpty(paramNames) && keyValue.Length == AlternateFieldKeys.Count)
            //{
            //    this.FillParamValue(AlternateFieldKeys, keyValue); //Get by Alternate/Unique Key
            //}
            //else
            //{
            //    //Input value cho các Parameters đưa vào từ nơi gọi
            //    string[] ParamList = null;
            //    if (paramNames.IndexOf(",") > -1)
            //        ParamList = paramNames.Split(new char[] { ',' });
            //    else
            //        ParamList = paramNames.Split(new char[] { ';' });

            //    List<string> ListFields = new List<string>();
            //    for (int i = 0; i < ParamList.Length; i++)
            //        ListFields.Add(ParamList[i].Trim());

            //    this.FillParamValue(ListFields, keyValue);
            //}

        }

        private bool FillParamValue(List<string> paramNames, object[] keyValue)
        {
            if (keyValue == null || keyValue.Length == 0) return true;
            if (paramNames == null || paramNames.Count == 0) return true;

            System.Reflection.PropertyInfo objInfo = null;
            int Index = 0;
            double dblValue = 0;

            List<object> ListValue = keyValue.ToList<object>();
            foreach (string FieldName in paramNames)
            {
                #region Input to FieldKeys
                if (string.IsNullOrEmpty(FieldName)) continue;
                objInfo = this.GetType().GetProperty(FieldName.Trim());
                if (objInfo == null)
                    objInfo = this.GetType().GetProperty(FieldName.Trim(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase);

                if (objInfo != null)
                {
                    //Kiểm tra kiểu dữ liệu của Class Property và giá trị đưa vào có cùng kiểu hay không?
                    //- Nếu cùng kiểu dữ liệu thì Set cho thuộc tính
                    //- Nếu không kiểu dữ liệu thì tìm thuộc tính khác năm trong danh sách 
                    //FieldKeys or AlternateFieldKeys. Nếu tìm thấy thì Set giá trị
                    //- Nếu vẫn không tìm thấy thuộc tính có kiểu dữ liệu tương ứng thì sẽ so sánh kiểu
                    //dữ liệu nhỏ hơn để gán vào (đối với kiểu number)

                    Index = 0;
                    for (; Index < ListValue.Count; Index++)
                    {
                        #region Fill Value
                        if (objInfo.PropertyType.Name.Equals(ListValue[Index].GetType().Name))
                        {
                            DataAccessObject.InputParams(FieldName.Trim(), ListValue[Index]);
                            ListValue.RemoveAt(Index);
                            break;
                        }
                        else if (!ListValue[Index].GetType().Equals(typeof(System.String)))
                        {
                            dblValue = double.Parse(ListValue[Index].ToString());

                            switch (objInfo.PropertyType.ToString())
                            {
                                case "System.Byte":
                                    if (dblValue >= byte.MinValue & dblValue <= byte.MaxValue)
                                    {
                                        DataAccessObject.InputParams(FieldName.Trim(), dblValue);
                                        ListValue.RemoveAt(Index);
                                    }

                                    break;
                                case "System.Int16":
                                    if (dblValue >= Int16.MinValue & dblValue <= Int16.MaxValue)
                                    {
                                        DataAccessObject.InputParams(FieldName.Trim(), dblValue);
                                        ListValue.RemoveAt(Index);
                                    }

                                    break;
                                case "System.Int32":
                                    if (dblValue >= Int32.MinValue & dblValue <= Int32.MaxValue)
                                    {
                                        DataAccessObject.InputParams(FieldName.Trim(), dblValue);
                                        ListValue.RemoveAt(Index);
                                    }

                                    break;
                                case "System.Int64":
                                    if (dblValue >= Int64.MinValue & dblValue <= Int64.MaxValue)
                                    {
                                        DataAccessObject.InputParams(FieldName.Trim(), dblValue);
                                        ListValue.RemoveAt(Index);
                                        
                                    }
                                    break;                               
                                    
                            }
                        }
                        #endregion Fill Value
                    }
                }
                #endregion Input to FieldKeys
            }

            return (ListValue.Count == 0);

        }

        ///// <summary>
        ///// Add Default Param: LanguageID/LoginLanguageID, DomainID/LoginDomainID, UserID/LoginUserID, UserCode/LoginUserCode, LoginSessionID
        ///// Call this method of after add [Action] param if needed
        ///// </summary>
        //public void InputDefaultParams()
        //{
        //    if (!DataAccessObject.ContainsParam(ACTION_PARAM_NAME))
        //        DataAccessObject.InputParams(ACTION_PARAM_NAME, string.Empty);

        //    DataAccessObject.InputParams("LoginLanguageID", DataAccess.LanguageID);
        //    DataAccessObject.InputParams("LoginApplicationID", DataAccess.ApplicationID);
        //    DataAccessObject.InputParams("LoginDomainID", DataAccess.DomainID);
        //    DataAccessObject.InputParams("LoginDomainCode", DataAccess.DomainCode);
        //    DataAccessObject.InputParams("LoginUserID", DataAccess.UserID);
        //    DataAccessObject.InputParams("LoginUserCode", DataAccess.UserCode);
        //    DataAccessObject.InputParams("LoginSessionID", DataAccess.LoginSessionID);
        //    DataAccessObject.InputParams("FreeParameter", string.Empty);
        //}

        ///// <summary>
        ///// Add Default Param: Action, LanguageID/LoginLanguageID, DomainID/LoginDomainID, UserID/LoginUserID, UserCode/LoginUserCode, LoginSessionID
        ///// Call this method of after add [Action] param if needed
        ///// </summary>
        //public void InputDefaultParams(string actionParamValue)
        //{
        //    //if (DataAccess.ParamCollections != null && DataAccess.ParamCollections.Count > 0)
        //    //    DataAccess.ParamCollections.Clear();

        //    if (!string.IsNullOrEmpty(actionParamValue) && !DataAccessObject.ContainsParam(ACTION_PARAM_NAME))
        //        DataAccessObject.InputParams(ACTION_PARAM_NAME, actionParamValue);

        //    DataAccessObject.InputParams("LoginLanguageID", DataAccess.LanguageID);
        //    DataAccessObject.InputParams("LoginApplicationID", DataAccess.ApplicationID);
        //    DataAccessObject.InputParams("LoginDomainID", DataAccess.DomainID);
        //    DataAccessObject.InputParams("LoginDomainCode", DataAccess.DomainCode);
        //    DataAccessObject.InputParams("LoginUserID", DataAccess.UserID);
        //    DataAccessObject.InputParams("LoginUserCode", DataAccess.UserCode);
        //    DataAccessObject.InputParams("LoginSessionID", DataAccess.LoginSessionID);
        //    DataAccessObject.InputParams("FreeParameter", string.Empty);
        //}
        #endregion


        #region Backup and Restore Data
        public bool AutoBackupDataSource = false;
        private DataTable dtBackupData = null;

        public virtual void BackupDataSource(DataTable dataSource)
        {
            dtBackupData = null;
            if (dataSource == null) return;
            dtBackupData = dataSource.Clone();
            dtBackupData.Merge(dataSource, true);
        }

        public virtual void BackupDataSource(DataSet dataSource)
        {
            dtBackupData = null;
            if (dataSource == null || dataSource.Tables.Count == 0) return;
            dtBackupData = dataSource.Tables[0].Clone();
            dtBackupData.Merge(dataSource.Tables[0], true);
        }

        public virtual void BackupDataSource(DataSet dataSource, string tableName)
        {
            dtBackupData = null;
            if (dataSource == null || dataSource.Tables.Count == 0) return;
            if (dataSource.Tables.Contains(tableName))
            {
                dtBackupData = dataSource.Tables[tableName].Clone();
                dtBackupData.Merge(dataSource.Tables[tableName], true);
            }
        }

        public virtual void RestoreDataSource(ref DataTable dataSource)
        {
            if (dataSource == null) return;
            dataSource.Rows.Clear();
            if (dtBackupData == null) return;
            dataSource.Merge(dtBackupData, true);
        }

        public virtual void RestoreDataSource(ref DataSet dataSource)
        {
            if (dataSource == null || dataSource.Tables.Count == 0) return;
            dataSource.Tables[0].Rows.Clear();
            if (dtBackupData == null) return;
            dataSource.Tables[0].Merge(dtBackupData, true);
        }

        public virtual void RestoreDataSource(ref DataSet dataSource, string tableName)
        {
            if (dataSource == null || dataSource.Tables.Count == 0) return;
            if (dataSource.Tables.Contains(tableName))
            {
                dataSource.Tables[tableName].Rows.Clear();
                if (dtBackupData == null) return;
                dataSource.Tables[tableName].Merge(dtBackupData, true);
            }
        }
        #endregion


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (m_DataAccessObject != null)
                    m_DataAccessObject.Dispose();
                m_DataAccessObject = null;

                if (m_Items != null)
                    m_Items.Clear();

                if (m_FieldNames != null)
                    m_FieldNames.Clear();

                if (m_FieldKeys != null)
                    m_FieldKeys.Clear();

                if (m_AlternateFieldKeys != null)
                    m_AlternateFieldKeys.Clear();

                //((IDisposable)this).Dispose();
            }
            catch { }
        }

        #endregion

    }
}
