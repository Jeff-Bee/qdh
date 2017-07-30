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
    public class SellerOrderRunBll : BaseBll<SellerOrderRun>
    {
        static SellerOrderRunDal<SellerOrderRun> Dal = new SellerOrderRunDal<SellerOrderRun>();
        /// <summary>
        /// 买家插入订单，返回订单编号和订单代码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderId"></param>
        /// <param name="orderCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(SellerOrderRun model,out int orderId,out string orderCode, out string errMsg)
        {
            return Dal.Insert(model, GetConnectionString(model.SellerId), out orderId,out orderCode, out errMsg);
        }
        /// <summary>
        /// 分页显示订单记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetOrderRunList(int page, int pagesize, string sort,int SellerId, DateTime startTime,DateTime endTime,string content, out string errMsg, int customerId=0,string type=null)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerOrderRun s left join SellerCustomerInfo b on s.BuyerId=b.BuyerId ";
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
           
            if (string.IsNullOrEmpty(type)|| type!="-2")
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "0")//未完成
                    {
                        criteria.Condition += " and s.OrderState!=100 ";
                    }
                    else if (type == "1")//已完成
                    {
                        criteria.Condition += " and s.OrderState=100 ";
                    }
                   
                }

                if (!string.IsNullOrEmpty(content))
                {
                    criteria.Condition += " and s.Code like '%" + content + "%' ";
                }
                else
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
            }
            else {
                criteria.Condition += " and s.OrderState=0 ";
            }
            criteria.Condition += " and s.SellerId=" + SellerId + " ";
            criteria.Fields = string.Format(@" s.*,b.FullName ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(SellerId), out errMsg);
        }

        /// <summary>
        /// 返回指定订单信息
        /// </summary>
        /// <param name="sellerId">用户Id</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static SellerOrderRun GetModel(int sellerId,int OrderId, out string errMsg)
        {
            return Dal.GetModel(sellerId, OrderId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 获取订单是否符合强制确认收货的条件
        /// </summary>
        /// <param name="SellerId"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public static bool GetForcedReceipt(int SellerId, int OrderId)
        {
            string errMsg = string.Empty;
            SellerOrderRun model = Dal.GetModel(SellerId, OrderId, GetConnectionString(SellerId), out errMsg);
            if (model != null)
            {
                if (model.PayState != 0)
                {
                    if (DateTime.Now.Date > model.OrderDate.AddDays(1).Date)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取新订单个数(未处理订单个数)
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetNewOrderCount(int SellerId)
        {
            string sql = "select count(OrderId) from SellerOrderRun where OrderState=0 and SellerId="+ SellerId;
            return Dal.ExecuteScalar(sql,GetConnectionString(SellerId)).ToString();
        }

        /// <summary>
        /// 返回指定订单信息（买家调用）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="buyerId">买家编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetOrderRunList(int page, int pagesize, string sort, int sellerId, int buyerId, DateTime startTime,
            DateTime endTime,int orderState , out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " SellerOrderRun ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "OrderId";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            criteria.Condition =
                string.Format("SellerId ={0} And BuyerId={1} And OrderState={2} And OrderDate Between('{3}' And '{4}'"
                    , sellerId, buyerId, orderState, startTime.ToString("G"), endTime.ToString("G"));
            criteria.Fields = string.Format(@" * ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(sellerId), out errMsg);
        }

        public static List<SellerOrderRun> GetList(int buyerId, out string msg)
        {
            return Dal.GetList(buyerId, GetConnectionString(buyerId), out msg);
        } 




    }
}
