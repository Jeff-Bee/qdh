using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家商品上线销售用户策略
    /// </summary>
    public class SellerProductOnlineCustomerStrategy : BaseModel
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 卖家客户编号(SellerCustomerInfo:BuyerId)
        /// </summary>
        public int BuyerId { get; set; } = 0;


        /// <summary>
        /// 销售状态 0：停止销售；1：正常销售
        /// </summary>
        public int SaleState { get; set; } = 0;
        /// <summary>
        /// 销售开始日期
        /// </summary>
        public DateTime SaleStartDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 销售结束日期
        /// </summary>
        public DateTime SaleEndDate { get; set; } = new DateTime(2100, 1, 1);
        /// <summary>
        /// 销售库存量
        /// </summary>
        public int SaleStoreQuantity { get; set; } = 0;



        /// <summary>
        /// 销售价格1
        /// </summary>
        public float Price1 { get; set; } = 0;

  
       
        /// <summary>
        /// 最小订购量
        /// </summary>
        public short MinOrder1 { get; set; } = 1;
       


        /// <summary>
        /// 最大订购量
        /// </summary>
        public int MaxOrder1 { get; set; } = 99999;

        /// <summary>
        /// 标记是否是新品
        /// </summary>
        public bool IsNew { get; set; } = false;
        /// <summary>
        /// 标记是否是促销商品
        /// </summary>
        public bool IsPromotion { get; set; } = false;

    }
    public class SellerProductOnlineCustomerStrategyMapper : ClassMapper<SellerProductOnlineCustomerStrategy>
    {
        public SellerProductOnlineCustomerStrategyMapper()
        {
            Table("SellerProductOnlineCustomerStrategy");
            Map(x => x.ProductId).Key(KeyType.Assigned);
            Map(x => x.BuyerId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
