using Dapper;
using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Laplace.LiteCOS.DAL
{
    public class BaseDal<T>  where T : class
    {
        public virtual bool Insert(T model, string connectionString,out string errMsg)
        {
            return SqlHelper.Insert<T>(model, connectionString,out errMsg);
        }
        public bool Insert(List<T> list, string connectionString, out string errMsg)
        {
            errMsg = String.Empty;
            foreach (var model in list)
            {
                if(!SqlHelper.Insert<T>(model, connectionString, out errMsg))
                {
                    return false;
                }
            }
            return true;
        }
        public int InsertWithReturnId(T model, string connectionString)
        {
            return SqlHelper.InsertWithReturnId<T>(model, connectionString);
        }
        public int InsertWithReturnId(T model, string connectionString, out string errMsg)
        {
            return SqlHelper.InsertWithReturnId<T>(model, connectionString,out errMsg);
        }
        public bool Update(T model, string connectionString, out string errMsg)
        {
            return SqlHelper.Update<T>(model, connectionString,out errMsg);
        }
        public bool Update(T model,string connectionString)
        {
            string errMsg;
            return SqlHelper.Update<T>(model, connectionString,out errMsg);
        }
        public bool Delete(PredicateGroup predicate, string connectionString,out string errMsg)
        {
            return SqlHelper.Delete<T>(predicate, connectionString,out errMsg);
        }

        #region--GetModel--
        public T GetModel(PredicateGroup pdg, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                return SqlHelper.GetModel<T>(pdg, connectionString,out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError(string.Format("BaseDal::GetModel() Error:{0},connectionString:{1}"
                    , errMsg, connectionString), "AppLogger");
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return null;
        }
        /// <summary>
        /// 根据SQL语句返回Model
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public virtual T GetModel(string sqlString, string connectionString, out string errMsg)
        {
            T ret = null;
            errMsg = string.Empty;
            try
            {
                var reader = SqlHelper.GetReader(connectionString, sqlString);
                while (reader.Read())
                {
                    ret = GetInstance(ref reader);
                    break;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.Log(LoggingLevel.Error, ex.Message);
            }
            return ret;
        }
        #endregion-GetModel-
        /// <summary>
        /// 返回满足条件记录数（如果返回-1表示查询失败）
        /// </summary>
        /// <param name="pdg"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int GetCount(PredicateGroup pdg, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                return SqlHelper.GetCount<T>(pdg, connectionString);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError(string.Format("BaseDal::GetCount() Error:{0},connectionString:{1}"
                    , errMsg, connectionString), "AppLogger");
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return -1;
        }

        #region----

        public List<T> GetList(string connectionString)
        {
            string errMsg;
            return GetList(connectionString, out errMsg);
        }


        public List<T> GetList(string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                return SqlHelper.GetList<T>(connectionString).ToList();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError(string.Format("BaseDal::GetList(string connectionString, out string errMsg) Error:{0},connectionString:{1}"
                    , errMsg, connectionString), "AppLogger");
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return null;
        }
        public List<T> GetList(PredicateGroup pdg, string connectionString)
        {
            try
            {
                return SqlHelper.GetList<T>(pdg, connectionString).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("BaseDal::GetList(PredicateGroup pdg, string connectionString) Error:{0},connectionString:{1}"
                    , ex.Message, connectionString), "AppLogger");
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return null;
        }
        public virtual List<T> GetList(PredicateGroup pdg, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                return SqlHelper.GetList<T>(pdg, connectionString).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("BaseDal::GetList(PredicateGroup pdg, string connectionString, out string errMsg) Error:{0},connectionString:{1}"
                    , ex.Message, connectionString), "AppLogger");
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return null;
        }
        public virtual List<T> GetList(string sqlString, string connectionString, out string errMsg)
        {
            var listRet = new List<T>();
            errMsg = string.Empty;
            try
            {
                var reader = SqlHelper.GetReader(connectionString, sqlString);
                while (reader.Read())
                {
                    listRet.Add(GetInstance(ref reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                listRet = null;
                errMsg = ex.Message;
                Logger.LogError(string.Format("BaseDal::GetList(string sqlString, string connectionString, out string errMsg) Error:{0},connectionString:{1}"
                    , ex.Message, connectionString), "AppLogger");
                Logger.Log(LoggingLevel.Error, ex.Message);
            }
            return listRet;
        }
        /// <summary>
        /// 根据SQL语句返回Model列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="errMsg"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public virtual List<T> GetListBySqlString(string sqlString, out string errMsg, string connectionString /*= ""*/)
        {
            var listRet = new List<T>();
            errMsg = string.Empty;
            try
            {
                var reader = SqlHelper.GetReader(connectionString, sqlString);
                while (reader.Read())
                {
                    listRet.Add(GetInstance(ref reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                listRet = null;
                errMsg = ex.Message;
                Logger.LogError(string.Format("BaseDal::GetList(string sqlString, out string errMsg, string connectionString) Error:{0},connectionString:{1}"
                    , ex.Message, connectionString), "AppLogger");
                Logger.Log(LoggingLevel.Error, ex.Message);
            }
            return listRet;
        }
        public List<int> GetList4Int32(string sqlString, string connectionString, out string errMsg)
        {
            var listRet = new List<int>();
            errMsg = string.Empty;
            try
            {
                var reader = SqlHelper.GetReader(connectionString, sqlString);
                while (reader.Read())
                {
                    listRet.Add(reader.GetInt32(0));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                listRet = null;
                errMsg = ex.Message;
                Logger.Log(LoggingLevel.Error, ex.Message);
            }
            return listRet;
        }
        public List<Int64> GetList4Int64(string sqlString, string connectionString, out string errMsg)
        {
            var listRet = new List<Int64>();
            errMsg = string.Empty;
            try
            {
                var reader = SqlHelper.GetReader(connectionString, sqlString);
                while (reader.Read())
                {
                    listRet.Add(reader.GetInt64(0));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                listRet = null;
                errMsg = ex.Message;
                Logger.Log(LoggingLevel.Error, ex.Message);
            }
            return listRet;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public PageDataView<T> GetPageData<T>(PageCriteria criteria, string connectionString, out string errMsg) where T : class
        {
            return SqlHelper.GetPageData<T>(criteria, connectionString, out errMsg);
        }
        /// <summary>
        /// 分页查询,返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public PageDataView GetPageDataDataTable(PageCriteria criteria, string connectionString, out string errMsg) 
        {
            return SqlHelper.GetPageDataDataTable(criteria, connectionString, out errMsg);
        }
        #endregion--
        /// <summary>
        /// 检查满足参数条件的记录是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected bool CheckModelExist(string table,string paramName, string value, string connectionString)
        {
            var ret =ExecuteScalar2Int(string.Format("Select Count(*) From {0} Where {1}='{2}'"
                , table, paramName, value), connectionString);
            if (ret.HasValue && ret.Value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量写记录到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public bool BulkCopy(DataTable dt, string connectionString)
        {
            var ret = false;
            try
            {
                ret = SqlHelper.BulkCopy(dt, connectionString);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex,  new string[] { "AppLogger", "DbErrorDataLogger" });
            }
            return ret;
        }
        public bool Execute(DynamicParameters p, string procedureName, string connectionString)
        {
            var ret = false;
            try
            {
                SqlHelper.Execute(p, procedureName, connectionString);
                ret = true;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return ret;
        }
        public bool Execute(string sql, string connectionString)
        {
            string errMsg;
            return Execute(sql,connectionString,out errMsg);
        }
        public bool Execute(string sql, string connectionString,out string errMsg)
        {
            var ret = false;
            errMsg = string.Empty;
            try
            {
                SqlHelper.Execute(sql, connectionString);
                ret = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return ret;
        }


        public object ExecuteScalar(string sql, string connectionString)
        {
            return SqlHelper.ExecuteScalar(sql, connectionString);
        }
        public int? ExecuteScalar2Int(string sql, string connectionString)
        {
            return SqlHelper.ExecuteScalar2Int(sql, connectionString);
        }

        public DataTable ExecuteDataTable(string sql, string connectionString)
        {
            return SqlHelper.ExecuteDataTable(sql,connectionString);
        }
        protected virtual T GetInstance(ref SqlDataReader rdr)
        {
            T _t = Activator.CreateInstance<T>();

            PropertyInfo[] propertyInfos = _t.GetType().GetProperties();

            foreach (var property in propertyInfos)
            {
                string name = string.Empty;
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    if (property.Name.ToUpper() == rdr.GetName(i).ToUpper())
                    {
                        name = rdr.GetName(i);
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    object _value = rdr[name];
                    if (_value != null && _value != DBNull.Value)
                    {
                        if (property.PropertyType.Name == "Single")
                            property.SetValue(_t, Convert.ToSingle(_value), null);
                        else if (property.PropertyType.Name == "Date")
                        {
                            property.SetValue(_t, Convert.ToDateTime(_value), null);
                        }
                        else
                            property.SetValue(_t, _value, null);
                    }
                }
            }

            //RichModelLazyLoader(ref _t);

            return _t;
        }
    }
}
