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
    public class SellerProductOnlineCustomerStrategyDal<T> : BaseDal<SellerProductOnlineCustomerStrategy> where T : class
    {
        public bool CheckModel(SellerProductOnlineCustomerStrategy model, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerProductOnlineCustomerStrategy>(o => o.ProductId, Operator.Eq, model.ProductId));
                pdg.Predicates.Add(Predicates.Field<SellerProductOnlineCustomerStrategy>(o => o.SellerId, Operator.Eq, model.SellerId));
                pdg.Predicates.Add(Predicates.Field<SellerProductOnlineCustomerStrategy>(o => o.BuyerId, Operator.Eq, model.BuyerId));
                return base.GetModel(pdg, connectionString, out errMsg) != null;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
    }
}
