using Laplace.LiteCOS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Laplace.Framework.Orm;

namespace Laplace.LiteCOS.Bll
{
    public class BaseBll<T>  where T : class
    {
        protected static readonly BaseDal<T> dal = new BaseDal<T>();

        public static bool Insert(T model)
        {
            string errMsg;
            return  dal.Insert(model, Global.ApplicationParms.ConnectionString,out errMsg);
        }
        
        public static bool Insert(int customerId,T model)
        {
            string errMsg;
            return dal.Insert(model, GetConnectionString(customerId),out errMsg);
        }
        public static bool Insert(T model,string connectionString, out string errMsg)
        {
            return dal.Insert(model, connectionString, out errMsg);
        }
        public static int InsertWithReturnId(T model, out string errMsg)
        {
            return dal.InsertWithReturnId(model, GetConnectionString(0), out errMsg);
        }
        public static int InsertWithReturnId(T model,string connectionString,out string errMsg)
        {
            return dal.InsertWithReturnId(model, connectionString, out errMsg);
        }
        public static bool Update(T model)
        {
            return dal.Update(model, Global.ApplicationParms.ConnectionString);
        }
        public static bool Update(T model, string connectionString,out string errMsg)
        {
            return dal.Update(model, connectionString,out errMsg);
        }
        /// <summary>
        /// 更新Model
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(int customerId, T model,out string errMsg)
        {
            return dal.Update(model, GetConnectionString(customerId),out errMsg);
        }
        public static IList<T> GetList()
        {
            return dal.GetList(Global.ApplicationParms.ConnectionString);
        }
        public static IList<T> GetList(int customerId)
        {
            return dal.GetList(GetConnectionString(customerId));
        }
        public static IList<T> GetList(string connectionString,out string errMsg)
        {
            return dal.GetList(connectionString,out errMsg);
        }

        public static List<T> GetList(string sqlString, string connectionString, out string errMsg)
        {
            return dal.GetList(sqlString, connectionString, out errMsg);
        }

        public static List<int> GetList4Int32(string sqlString, string connectionString, out string errMsg)
        {
            return dal.GetList4Int32(sqlString, connectionString, out errMsg);
        }
        public static string GetConnectionString(int customerId)
        {
            return Global.ApplicationParms.ConnectionString;

            //return GlobalDataBll.GetUserConnectString(customerId);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView<T> GetPageData(PageCriteria criteria, int customerId, out string errMsg)
        {
            return dal.GetPageData<T>(criteria, GetConnectionString(customerId), out errMsg);
        }
        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetPageDataDataTable(PageCriteria criteria, int customerId, out string errMsg) 
        {
            return dal.GetPageDataDataTable(criteria, GetConnectionString(customerId), out errMsg);
        }
        /// <summary>
        /// 批量写记录到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool BulkCopy(DataTable dt,string connectionString)
        {
            return dal.BulkCopy(dt, connectionString);
        }

        /// <summary>
        /// 查询结果返回datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, string connectionString)
        {
            return dal.ExecuteDataTable(sql, connectionString);
        }

    }
}
