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
using Laplace.Framework.Orm;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Dal
{
    public class SellerColumnConfigDal<T> : BaseDal<SellerColumnConfig> where T : class
    {
        /// <summary>
        /// 插入指定卖家表配置记录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        //public bool Insert(List<SellerColumnConfig> list, string connectionString, out string errMsg)
        //{
        //    return base.Insert(list, connectionString, out errMsg);
        //}

        /// <summary>
        /// 删除指定卖家，指定表的列配置记录
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        public bool Delete(int sellerId, string tableName, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.TableName, Operator.Eq, tableName));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 返回指定卖家，指定表的列配置记录
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        public List<SellerColumnConfig> GetList(int sellerId, string tableName, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.TableName, Operator.Eq, tableName));
                return base.GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return new List<SellerColumnConfig>();
        }


        public SellerColumnConfig GetModel(int sellerId, string tableName, int index, string connectionString, out string errMsg)
        {
            PredicateGroup pdg = new PredicateGroup();
            pdg.Predicates = new List<IPredicate>();
            pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.SellerId, Operator.Eq, sellerId));
            pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.TableName, Operator.Eq, tableName));
            pdg.Predicates.Add(Predicates.Field<SellerColumnConfig>(o => o.Index, Operator.Eq, index));
            return base.GetModel(pdg,connectionString,out errMsg);
        }

        
    }
}
