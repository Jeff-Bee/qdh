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
    public  class SellerBuyInfoBll : BaseBll<SellerBuyInfo>
    {
        static SellerBuyInfoDal<SellerBuyInfo> Dal = new SellerBuyInfoDal<SellerBuyInfo>();

        /// <summary>
        /// 检查code 是否重复
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code,out string errMsg)
        {
            return Dal.CheckCode(sellerId,code,GetConnectionString(sellerId),out errMsg);
        }

        public static bool Delete(int BuyId, int SellerId)
        {
            string errMsg = string.Empty;
            return Dal.Delete(BuyId, GetConnectionString(SellerId), out errMsg);
        }

        public static PageDataView GetOrderList(int page, int pagesize, string sort, int customerId, string ProductId,out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerBuyDetail d left join SellerBuyInfo i on i.BuyId = d.BuyId left join ProductInfo p on p.ProductId = d.ProductId ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "i.BuyId";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            criteria.Condition += " and d.ProductId="+ProductId;
            criteria.Condition += " and p.SellerId=" + customerId + " ";
            criteria.Fields = string.Format(@" i.BuyId,i.Code,i.ComeDate,i.Remark,i.Notes,d.Quantity,d.Price,p.ProductFullName,p.ProductShortName ");

            return Dal.GetPageDataDataTable(criteria, GetConnectionString(customerId), out errMsg);
        }

        public static PageDataView GetSellerBuyInfoList(int page, int pagesize, string sort, int customerId,DateTime startTime,DateTime endTime, string content,int sellerId, out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerBuyInfo s left join SellerSupplierInfo si on si.SupplierId=s.SupplierId ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "s.Code";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            if (startTime != DateTime.MinValue)
            {
                criteria.Condition += " and s.ComeDate>='" + startTime+"' ";
            }
            if (endTime != DateTime.MinValue)
            {
                criteria.Condition += " and s.ComeDate<='" + endTime+"' ";
            }
            if (!string.IsNullOrEmpty(content))
            {
                criteria.Condition += " and s.Code like '%" + content + "%' ";
            }
            if (sellerId != -1)
            {
                criteria.Condition += " and s.SellerId=" + sellerId + " ";
            }

            if (customerId != -1)
            {
                criteria.Condition += " and s.SupplierId=" + customerId + " ";
            }
            criteria.Fields = string.Format(@" s.Code,s.ComeDate,s.TotalAmount,s.Notes,s.BuyId,si.FullName,s.SupplierId ");

            return Dal.GetPageDataDataTable(criteria, GetConnectionString(sellerId), out errMsg);
        }

        /// <summary>
        /// 获取进货单号
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetBuyerInfoCode(int SellerId)
        {
            string sql = "select count(BuyId) from SellerBuyInfo where SellerId="+SellerId+ " and ComeDate>='"+DateTime.Now.ToString("yyyy-MM-dd 00:00:00")+ "' and ComeDate<='"+DateTime.Now.ToString("yyyy-MM-dd 23:59:59")+"' ";
            object res = Dal.ExecuteScalar(sql, GetConnectionString(SellerId));
            string id = string.Empty;
            if (res != null)
            {
                id = (Convert.ToInt32(res) + 1).ToString().PadLeft(3, '0');
            }
            else {
                id = "001";
            }
            //string id= Dal.ExecuteScalar(sql,GetConnectionString(SellerId)).ToString().PadLeft(3,'0');
            return "JHD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + id;
        }
    }
}
