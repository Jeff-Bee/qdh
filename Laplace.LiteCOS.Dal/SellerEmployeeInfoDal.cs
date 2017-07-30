using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Dal
{
    public class SellerEmployeeInfoDal<T> : BaseDal<SellerEmployeeInfo> where T : class
    {
        /// <summary>
        /// 删除指定员工信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int employeeId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerEmployeeInfo>(o => o.EmployeeId, Operator.Eq, employeeId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 返回指定卖家所有员工列表
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<SellerEmployeeInfo> GetList(int sellerId, string connectionString, out string errMsg)
        {
            List<SellerEmployeeInfo> list = new List<SellerEmployeeInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerEmployeeInfo>(o => o.SellerId, Operator.Eq, sellerId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        /// <summary>
        /// 检查员工代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckCode(int sellerId, string code, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerEmployeeInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerEmployeeInfo>(o => o.EmployeeCode, Operator.Eq, code));
                return base.GetCount(pdg, connectionString, out errMsg)>0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

    }
}
