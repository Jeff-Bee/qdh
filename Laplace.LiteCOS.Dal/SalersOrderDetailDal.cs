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
    public class SalersOrderDetailDal<T> : BaseDal<SalersOrderDetail> where T : class
    {
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int OrderId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SalersOrderDetail>(o => o.OrderId, Operator.Eq, OrderId));
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
