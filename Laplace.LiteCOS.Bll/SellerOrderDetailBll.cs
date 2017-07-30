using Laplace.Framework.Orm;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerOrderDetailBll : BaseBll<SellerOrderDetail>
    {
        static SellerOrderDetailDal<SellerOrderDetail> Dal = new SellerOrderDetailDal<SellerOrderDetail>();

        /// <summary>
        /// 商品上架商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="SellerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetOPProductList(int page, int pagesize, string sort, int SellerId, int orderId, out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerOrderDetail s left join ProductInfo p on p.ProductId=s.ProductId ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "s.ProductId";
  
            criteria.Condition += " and s.SellerId=" + SellerId + " ";
            criteria.Condition += " and s.OrderId=" + orderId + " ";

            criteria.Fields = string.Format(@" s.OrderId,s.[Index],s.ProductId,s.ProductName,s.ProductPrice,s.ProductQuantity,s.ProductUnit,s.TotalPrice,s.Notes,p.ProductCode,p.Package,p.ProductSpec  ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(SellerId), out errMsg);
        }

        /// <summary>
        /// 根据订单id获取商品信息
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="orderId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerOrderDetail> GetList(int sellerId, int orderId, out string errMsg)
        {
            return Dal.GetList(sellerId,orderId,GetConnectionString(sellerId),out errMsg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(List<SellerOrderDetail> models, out string errMsg)
        {
            bool res = true;
            errMsg = string.Empty;
            foreach (var model in models)
            {
                res= res&& Dal.Delete(model, GetConnectionString(model.SellerId), out errMsg);
            }
            return res;
        }
    }
}
