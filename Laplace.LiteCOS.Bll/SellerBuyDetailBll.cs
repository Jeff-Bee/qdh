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
    public class SellerBuyDetailBll : BaseBll<SellerBuyDetail>
    {
        protected static readonly SellerBuyDetailDal<SellerBuyDetail> Dal = new SellerBuyDetailDal<SellerBuyDetail>();
        public static bool Insert(List<SellerBuyDetail> models,int SellerId)
        {
            string errMsg = string.Empty;
            return Dal.Insert(models, GetConnectionString(SellerId),out errMsg);
        }

        public static bool Delete(int BuyId, int SellerId) {
            string errMsg = string.Empty;
            return Dal.Delete(BuyId,GetConnectionString(SellerId),out errMsg);
        }

        /// <summary>
        /// 进货报表查询订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="SellerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetPORProductList(int page, int pagesize, string sort, int SellerId, int BuyId, out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerBuyDetail s left join ProductInfo p on p.ProductId = s.ProductId ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "s.ProductId";

            criteria.Condition += " and s.SellerId=" + SellerId + " ";
            criteria.Condition += " and s.BuyId=" + BuyId + " ";

            criteria.Fields = string.Format(@" s.Quantity,s.Price,s.ProductId,p.ProductFullName,p.ProductCode  ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(SellerId), out errMsg);
        }
    }
}
