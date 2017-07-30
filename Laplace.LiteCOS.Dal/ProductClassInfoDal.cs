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

namespace Laplace.LiteCOS.Dal
{
    public class ProductClassInfoDal<T> : BaseDal<ProductClassInfo> where T : class
    {
        /// <summary>
        /// 返回指定商家商品分类列表（根目录）
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<ProductClassInfo> GetList(int sellerId, string connectionString, out string errMsg)
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.SellerId, Operator.Eq, sellerId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }
       

        /// <summary>
        /// 返回指定商家商品分类列表（根目录）
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="parentId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<ProductClassInfo> GetListByParentId(int sellerId,int parentId, string connectionString, out string errMsg)
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.ParentId, Operator.Eq, parentId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int classId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.ClassId, Operator.Eq, classId));
                return base.Delete(pdg,connectionString,out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        public ProductClassInfo GetModel(int classId, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.ClassId, Operator.Eq, classId));
                pdg.Predicates.Add(Predicates.Field<ProductClassInfo>(o => o.SellerId, Operator.Eq, SellerId));
                return GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return null;
        }
    }
}
