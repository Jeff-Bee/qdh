using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Dal
{
    public class SellerStockPileDal<T> : BaseDal<SellerStockPile> where T : class
    {
        /// <summary>
        /// 根据卖家id,商品id和所在仓库id获取库存数据
        /// </summary>
        /// <param name="SellerId"></param>
        /// <param name="ProductId"></param>
        /// <param name="StoreHouseId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public SellerStockPile GetModel(int SellerId,int ProductId,int StoreHouseId,string connectionString,out string errMsg)
        {
            errMsg = string.Empty;
            var model = new SellerStockPile();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerStockPile>(o => o.ProductId, Operator.Eq, ProductId));
                pdg.Predicates.Add(Predicates.Field<SellerStockPile>(o => o.StoreHouseId, Operator.Eq, StoreHouseId));
                pdg.Predicates.Add(Predicates.Field<SellerStockPile>(o => o.SellerId, Operator.Eq, SellerId));
                model =base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

       
    }
}
