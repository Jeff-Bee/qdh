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
    public class SellerOrderDetailDal<T> : BaseDal<SellerOrderDetail> where T : class
    {
        public List<SellerOrderDetail> GetList(int sellerId, int orderId, string connectionString, out string errMsg)
        {
            List<SellerOrderDetail> list = new List<SellerOrderDetail>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerOrderDetail>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerOrderDetail>(o => o.OrderId, Operator.Eq, orderId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        public bool Delete(SellerOrderDetail model, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerOrderDetail>(o => o.SellerId, Operator.Eq, model.SellerId));
                pdg.Predicates.Add(Predicates.Field<SellerOrderDetail>(o => o.OrderId, Operator.Eq, model.OrderId));
                pdg.Predicates.Add(Predicates.Field<SellerOrderDetail>(o => o.Index, Operator.Eq, model.Index));
                return Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                return false;
            }
        }

    }
}
