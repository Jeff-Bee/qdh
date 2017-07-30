using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家商品上线销售
    /// </summary>
    public class SellerProductOnline : BaseModel
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
        public DateTime SaleEndDate { get; set; } = new DateTime(2100,1,1);
        /// <summary>
        /// 销售库存量
        /// </summary>
        public int SaleStoreQuantity { get; set; } = 0;



        /// <summary>
        /// 销售价格1
        /// </summary>
        public float Price1 { get; set; } = 0;
        /// <summary>
        /// 销售价格2
        /// </summary>
        public float Price2 { get; set; } = 0;
        /// <summary>
        /// 销售价格3
        /// </summary>
        public float Price3 { get; set; } = 0;
        /// <summary>
        /// 销售价格4
        /// </summary>
        public float Price4 { get; set; } = 0;
        /// <summary>
        /// 销售价格5
        /// </summary>
        public float Price5 { get; set; } = 0;
        /// <summary>
        /// 销售价格6
        /// </summary>
        public float Price6 { get; set; } = 0;
        /// <summary>
        /// 销售价格7
        /// </summary>
        public float Price7 { get; set; } = 0;
        /// <summary>
        /// 销售价格8
        /// </summary>
        public float Price8 { get; set; } = 0;
        /// <summary>
        /// 最小订购量1
        /// </summary>
        public short MinOrder1 { get; set; } = 1;
        /// <summary>
        /// 最小订购量2
        /// </summary>
        public short MinOrder2 { get; set; } = 1;
        /// <summary>
        /// 最小订购量3
        /// </summary>
        public short MinOrder3{ get; set; } = 1;
        /// <summary>
        /// 最小订购量4
        /// </summary>
        public short MinOrder4 { get; set; } = 1;
        /// <summary>
        /// 最小订购量5
        /// </summary>
        public short MinOrder5 { get; set; } = 1;
        /// <summary>
        /// 最小订购量6
        /// </summary>
        public short MinOrder6 { get; set; } = 1;
        /// <summary>
        /// 最小订购量7
        /// </summary>
        public short MinOrder7 { get; set; } = 1;
        /// <summary>
        /// 最小订购量8
        /// </summary>
        public short MinOrder8 { get; set; } = 1;


        /// <summary>
        /// 最大订购量1
        /// </summary>
        public int MaxOrder1 { get; set; } = 99999;
        /// <summary>
        /// 最大订购量2
        /// </summary>
        public int MaxOrder2 { get; set; } = 99999;

        /// <summary>
        /// 最大订购量3
        /// </summary>
        public int MaxOrder3 { get; set; } = 99999;
        /// <summary>
        /// 最大订购量4
        /// </summary>
        public int MaxOrder4 { get; set; } = 99999;
        /// <summary>
        /// 最大订购量6
        /// </summary>
        public int MaxOrder6 { get; set; } = 99999;
        /// <summary>
        /// 最大订购量7
        /// </summary>
        public int MaxOrder7 { get; set; } = 99999;
        /// <summary>
        /// 最大订购量8
        /// </summary>
        public int MaxOrder8 { get; set; } = 99999;
       

        /// <summary>
        /// 销售策略
        /// </summary>
        public int Strategy { get; set; } = 0;
        /// <summary>
        /// 标记是否是新品
        /// </summary>
        public bool IsNew { get; set; } = false;
        /// <summary>
        /// 标记是否是促销商品
        /// </summary>
        public bool IsPromotion { get; set; } = false;

    }
    public class SellerProductOnlineMapper : ClassMapper<SellerProductOnline>
    {
        public SellerProductOnlineMapper()
        {
            Table("SellerProductOnline");
            Map(x => x.ProductId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
