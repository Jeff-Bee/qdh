using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.Framework.Log;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System.Data;

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 商品分类操作
    /// </summary>
    public class ProductClassInfoBll : BaseBll<ProductClassInfo>
    {
        protected static readonly ProductClassInfoDal<ProductClassInfo> Dal = new ProductClassInfoDal<ProductClassInfo>();
        /// <summary>
        /// 添加新商品分类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="classId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(ProductClassInfo model, out int classId, out string errMsg)
        {
            classId = InsertWithReturnId(model, GetConnectionString(model.SellerId), out errMsg);
            return classId > 0;
        }
        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(ProductClassInfo model, out string errMsg)
        {
            return Update(model,GetConnectionString(model.SellerId), out errMsg);
        }
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="classId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int classId, out string errMsg)
        {
            //TODO:检查是否有子分类

            //TODO:检查是否有商品关联

            return Dal.Delete(classId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定商家商品分类列表（所有分类）
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<ProductClassInfo> GetList(int sellerId, out string errMsg)
        {
            return Dal.GetList(sellerId, GetConnectionString(sellerId), out errMsg);
        }

     

        /// <summary>
        /// 返回指定商家,指定父类型的商品分类列表
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="parentId">父类型商品分类编号，如果为0，返回根目录分类</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<ProductClassInfo> GetListByParentId(int sellerId, int parentId, out string errMsg)
        {
            return Dal.GetListByParentId(sellerId, parentId, GetConnectionString(sellerId), out errMsg);
        }

        public static ProductClassInfo GetModel(int ClassId, int SellerId,out string errMsg)
        {
            return Dal.GetModel(ClassId,SellerId,GetConnectionString(SellerId),out errMsg);
        }


        /// <summary>
        /// 执行查询sql 返回dt
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public static DataTable ExecuteDt(string sql,int sellerId)
        {
            return ExecuteDataTable(sql,GetConnectionString(sellerId));
        }

        /// <summary>
        /// 根据classid获取父类商品类别
        /// </summary>
        /// <param name="ClassId"></param>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static int GetParentClassId(int ClassId, int SellerId)
        {
            string sql = string.Format(@"select ParentId from ProductClassInfo where ClassId={0}", ClassId);
            return (int)Dal.ExecuteScalar2Int(sql,GetConnectionString(SellerId));
        }

        /// <summary>
        /// 根据卖家Id，买家Id返回该买家在指定卖家上架商品类别列表
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="buyerId">买家Id</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<ProductClassInfo> GetListByBuyerId(int sellerId,int buyerId, out string errMsg)
        {
            var sql = "Select* From ProductClassInfo Where ClassId In("
                      + "Select ClassId From ProductInfo Where ProductId In("
                      + "Select ProductId From SellerProductOnlineCustomerStrategy"
                      + string.Format(" Where SellerId ={0}  And BuyerId = {1}))",
                          sellerId, buyerId);
            var list = dal.GetListBySqlString(sql, out errMsg, GetConnectionString(sellerId));
            if (list.Any())
            {
                return list;
            }
            else
            {
                //卖家商品上线销售用户策略记录中没有记录，就查询全部上架信息
                sql = "Select* From ProductClassInfo Where ClassId In("
                      + "Select ClassId From ProductInfo Where ProductId In("
                      + "Select ProductId From SellerProductOnline"
                      + string.Format(" Where SellerId ={0}  And SaleState=1))",
                          sellerId, buyerId);
                return dal.GetListBySqlString(sql, out errMsg, GetConnectionString(sellerId));
            }
            
        }
    }
}
