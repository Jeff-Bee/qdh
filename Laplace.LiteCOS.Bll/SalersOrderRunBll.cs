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
    public class SalersOrderRunBll : BaseBll<SalersOrderRun>
    {
        protected static readonly SalersOrderRunDal<SalersOrderRun> Dal = new SalersOrderRunDal<SalersOrderRun>();
        /// <summary>
        /// 获取进货单号
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetSalersOrderRunCode(int SellerId)
        {
            string sql = "select count(OrderId) from SalersOrderRun where SellerId=" + SellerId + " and OrderDate>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and OrderDate<='" + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "' ";
            object res = Dal.ExecuteScalar(sql, GetConnectionString(SellerId));
            string id = string.Empty;
            if (res != null)
            {
                id = (Convert.ToInt32(res) + 1).ToString().PadLeft(3, '0');
            }
            else
            {
                id = "001";
            }
            //string id= Dal.ExecuteScalar(sql,GetConnectionString(SellerId)).ToString().PadLeft(3,'0');
            return "XS-" + DateTime.Now.ToString("yyyyMMdd") + "-" + id;
        }

        public static bool CheckCode(int sellerId, string code, out string errMsg)
        {
            return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
        }

        public static bool Insert(List<SalersOrderRun> models, int SellerId)
        {
            string errMsg = string.Empty;
            return Dal.Insert(models, GetConnectionString(SellerId), out errMsg);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int OrderId, int SellerId, out string errMsg)
        {
            return Dal.Delete(OrderId, GetConnectionString(SellerId), out errMsg);
        }

        /// <summary>
        /// 分页显示订单记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetOrderRunList(int page, int pagesize, string sort, int SellerId, DateTime startTime, DateTime endTime, out string errMsg, int customerId = 0, string type = null)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SalersOrderRun s left join SellerCustomerInfo b on s.BuyerId=b.BuyerId ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "OrderId";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            if (customerId != 0)
            {
                criteria.Condition += " and s.BuyerId=" + customerId;
            }

            if (string.IsNullOrEmpty(type))
            {
                if (startTime != DateTime.MinValue)
                {
                    criteria.Condition += " and s.OrderDate>='" + startTime + "' ";
                }
                if (endTime != DateTime.MinValue)
                {
                    criteria.Condition += " and s.OrderDate<='" + endTime + "' ";
                }
            }
            else
            {
                criteria.Condition += " and s.OrderState=0 ";
            }
            criteria.Condition += " and s.SellerId=" + SellerId + " ";
            criteria.Fields = string.Format(@" s.*,b.FullName ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(SellerId), out errMsg);
        }
    }
}
