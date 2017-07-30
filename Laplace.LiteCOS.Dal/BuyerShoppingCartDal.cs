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
using DapperExtensions = DapperExtensions.DapperExtensions;

namespace Laplace.LiteCOS.Dal
{

    public class BuyerShoppingCartDal<T> : BaseDal<BuyerShoppingCart> where T : class
    {
        /// <summary>
        /// 保存买家购物车信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Save(BuyerShoppingCart model, string connectionString, out string errMsg)
        {
            var old = GetModel(model.BuyerId, model.SellerId, model.ProductId, connectionString, out errMsg);
            if (old == null)
            {
                return Insert(model, connectionString, out errMsg);
            }
            else
            {
                model.RDate = old.RDate;
                model.RMan = old.RMan;
                return Update(model, connectionString, out errMsg);
            }
        }
        public BuyerShoppingCart GetModel(int buyerId, int sellerId, int productId, string connectionString, out string errMsg)
        {
            BuyerShoppingCart model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.BuyerId, Operator.Eq, buyerId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.ProductId, Operator.Eq, productId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.SellerId, Operator.Eq, sellerId));

                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }



        /// <summary>
        /// 删除购车中的信息
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool Delete(int buyerId, int productId, string connectionString, out string errMsg)
        {
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.BuyerId, Operator.Eq, buyerId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.ProductId, Operator.Eq, productId));
                return Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                errMsg = "删除失败";
                return false;
            }

        }


        /// <summary>
        /// 根据买家,卖家id 删除购车中的信息 (xie 用于清空当前选中卖家的购物车)
        /// </summary>
        /// <param name="buyerId"></param>   
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool DeleteBySellerId(int buyerId, int sellerId, string connectionString, out string errMsg)
        {
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.BuyerId, Operator.Eq, buyerId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.SellerId, Operator.Eq, sellerId));
                return Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                errMsg = "删除失败";
                return false;
            }

        }


        /// <summary>
        /// 根据买家,卖家id 删除购车中的信息 (xie 用于清空当前选中卖家的购物车)
        /// </summary>
        /// <param name="buyerId"></param>   
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool DeleteCertLine(int buyerId, int sellerId, int productId, string connectionString, out string errMsg)
        {
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.BuyerId, Operator.Eq, buyerId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.ProductId, Operator.Eq, productId));
                return Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                errMsg = "删除失败";
                return false;
            }

        }

        public  bool GetCertCount(int buyerId, int sellerId, string connectionString, out int count, out string msg)
        {
            PredicateGroup pdg = new PredicateGroup();
            pdg.Predicates = new List<IPredicate>();
            pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.BuyerId, Operator.Eq, buyerId));
            //pdg.Predicates.Add(Predicates.Field<BuyerShoppingCart>(o => o.SellerId, Operator.Eq, sellerId));
            try
            {
                count = GetList(pdg, connectionString).Sum(c => c.ProductQuantity);
            }
            catch
            {

                count = 0;
            }
            msg = "";
            return true;
        }


    }
}
