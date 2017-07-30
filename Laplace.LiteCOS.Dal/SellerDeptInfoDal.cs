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
    public class SellerDeptInfoDal<T> : BaseDal<SellerDeptInfo> where T : class
    {
        /// <summary>
        /// 删除指定部门信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int deptId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.DeptId, Operator.Eq, deptId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 返回指定卖家所有部门列表
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<SellerDeptInfo> GetList(int sellerId, string connectionString, out string errMsg)
        {
            List<SellerDeptInfo> list = new List<SellerDeptInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.SellerId, Operator.Eq, sellerId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        /// <summary>
        /// 检查部门代码是否存在
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
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.DeptCode, Operator.Eq, code));
                return base.GetCount(pdg, connectionString, out errMsg)>0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 检查部门代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckCode(int sellerId, int DeptId, string code, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.DeptCode, Operator.Eq, code));
                pdg.Predicates.Add(Predicates.Field<SellerDeptInfo>(o => o.DeptId, Operator.Eq, DeptId));
                return base.GetCount(pdg, connectionString, out errMsg) > 0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

    }
}
