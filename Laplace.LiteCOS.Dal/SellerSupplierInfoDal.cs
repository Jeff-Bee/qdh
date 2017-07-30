using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Dal
{
    public class SellerSupplierInfoDal<T> : BaseDal<SellerSupplierInfo> where T : class
    {
        /// <summary>
        /// 检查供应商代码是否存在
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
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.Code, Operator.Eq, code));
                return base.GetCount(pdg, connectionString, out errMsg) > 0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 编辑判断code
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="suppId"></param>
        /// <param name="code"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckCode(int sellerId,int suppId, string code, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.Code, Operator.Eq, code));
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.SupplierId, Operator.Eq, suppId));
                return base.GetCount(pdg, connectionString, out errMsg) > 0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int id, int sellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerSupplierInfo>(o => o.SupplierId, Operator.Eq, id));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
    }
}
