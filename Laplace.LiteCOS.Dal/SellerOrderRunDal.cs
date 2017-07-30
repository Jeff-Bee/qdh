using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Laplace.LiteCOS.Dal
{
    public class SellerOrderRunDal<T> : BaseDal<SellerOrderRun> where T : class
    {
        public bool Insert(SellerOrderRun model,string connectionString, out int orderId,out string orderCode, out string errMsg)
        {
            var ret = false;
            errMsg = string.Empty;
            orderId = 0;
            orderCode=String.Empty;
            try
            {
                var p = new DynamicParameters();
                p.Add("@SellerId", model.SellerId, DbType.Int32);           
                p.Add("@BuyerId", model.BuyerId, DbType.Int32);
                p.Add("@OrderDate", model.OrderDate, DbType.DateTime);
                p.Add("@ProductType", model.ProductType, DbType.Int16);
                p.Add("@ProductQuantity", model.ProductQuantity, DbType.Int32);
                p.Add("@Amount", model.Amount, DbType.Decimal);
                p.Add("@OrderState", model.OrderState, DbType.Int16);
                p.Add("@PayState", model.PayState, DbType.Int16);
                p.Add("@PayAmount", model.PayAmount, DbType.Decimal);
                p.Add("@LogisticsState", model.LogisticsState, DbType.Int16);
                p.Add("@Config", model.Config, DbType.String);
                p.Add("@Notes", model.Notes, DbType.String);
                p.Add("@OrderId", 0, DbType.Int32, ParameterDirection.Output); /*返回订单号*/
                p.Add("@OrderCode", "", DbType.String, ParameterDirection.Output); /*返回订单代码*/

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute("proc_Insert_SellerOrderRun", p, null, null, CommandType.StoredProcedure);
                    orderId = p.Get<int>("OrderId");
                    orderCode = p.Get<string>("OrderCode");
                    return orderId > 0;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return ret;
        }
        public SellerOrderRun GetModel(int sellerId,int OrderId, string connectionString, out string errMsg)
        {
            SellerOrderRun model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerOrderRun>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerOrderRun>(o => o.OrderId, Operator.Eq, OrderId));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }


        public List<SellerOrderRun> GetList(int buyerId, string connectionString, out string errMsg)
        {
            List<SellerOrderRun> models = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerOrderRun>(o => o.BuyerId, Operator.Eq, buyerId));
             
                models = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return models;
        }



    }
}
