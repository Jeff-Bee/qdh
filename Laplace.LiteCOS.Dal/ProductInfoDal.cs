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
    public class ProductInfoDal<T> : BaseDal<ProductInfo> where T : class
    {
        public bool Delete(int productId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.ProductId, Operator.Eq, productId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 返回指定商品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public ProductInfo GetModel(int productId, string connectionString, out string errMsg)
        {
            ProductInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.ProductId, Operator.Eq, productId));
                
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        /// <summary>
        /// 返回指定商品信息,根据code获取model
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public ProductInfo GetModelByCode(string productCode,int SellerId, string connectionString, out string errMsg)
        {
            ProductInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.ProductCode, Operator.Eq, productCode));
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.SellerId, Operator.Eq, SellerId));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        /// <summary>
        /// 根据商品名称获取model
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public ProductInfo GetModel(string productName,int SellerId, string connectionString, out string errMsg)
        {
            ProductInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.ProductFullName, Operator.Eq, productName));
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.SellerId, Operator.Eq, SellerId));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }


        public List<ProductInfo> GetList(int sellerId, int productClassId, string connectionString, out string errMsg)
        {
            List<ProductInfo> list = new List<ProductInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.SellerId, Operator.Eq, sellerId));
                if (productClassId >0)
                {
                    pdg.Predicates.Add(Predicates.Field<ProductInfo>(o => o.ClassId, Operator.Eq, productClassId));
                }
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }
        public PageDataView<ProductInfo> GetList(int sellerId, int productClassId, int page, int pageSize
            , string connectionString, out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.Condition = string.Format("SellerId={0}", sellerId);
            if (productClassId > 0)
            {
                criteria.Condition += string.Format(" And ClassId ={0}", productClassId);
            }
            criteria.CurrentPage = page;
            criteria.Fields = "*";
            criteria.PageSize = pageSize;
            criteria.TableName = "ProductInfo";
            criteria.PrimaryKey = "ProductId";
            var r = GetPageData<ProductInfo>(criteria,connectionString,out errMsg);
            return r;
        }
    }
}
