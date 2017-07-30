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
    public class SellerProductOnlineDal<T> : BaseDal<SellerProductOnline> where T : class
    {
        public bool CheckModel(SellerProductOnline model,string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerProductOnline>(o => o.ProductId, Operator.Eq, model.ProductId));
                pdg.Predicates.Add(Predicates.Field<SellerProductOnline>(o => o.SellerId, Operator.Eq, model.SellerId));
                return base.GetModel(pdg, connectionString, out errMsg)!=null;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
    }
}
