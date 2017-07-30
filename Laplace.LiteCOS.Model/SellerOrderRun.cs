using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家订单记录
    /// </summary>
    public class SellerOrderRun : BaseModel
    {
        /// <summary>
        /// 订单编号：自增，10001--2147483 647	0--10000为系统保留
        /// </summary>
        public int OrderId { get; set; } = 0;
        /// <summary>
        /// 订单代码：格式：DH-0-20161107-XXXXX
        /// </summary>
        public string Code { get; set; } = String.Empty;

    
        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 买家编号(BuyerInfo:BuyerId)
        /// </summary>
        public int BuyerId { get; set; } = 0;
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 商品类型
        /// </summary>
        public short ProductType { get; set; } = 0;

        /// <summary>
        /// 商品总数
        /// </summary>
        public int ProductQuantity { get; set; } = 0;
        /// <summary>
        /// 订单总金额
        /// </summary>
        public float Amount { get; set; } = 0;

        /// <summary>
        /// 订单状态
        /// </summary>
        public short OrderState { get; set; } = 0;

        /// <summary>
        /// 支付状态
        /// </summary>
        public short PayState { get; set; } = 0;

        /// <summary>
        /// 付款金额
        /// </summary>
        public float PayAmount { get; set; } = 0;
        /// <summary>
        /// 物流状态（出库发货状态）
        /// </summary>
        public short LogisticsState { get; set; } = 0;
       
    }
    public class SellerOrderRunMapper : ClassMapper<SellerOrderRun>
    {
        public SellerOrderRunMapper()
        {
            Table("SellerOrderRun");
            Map(x => x.OrderId).Key(KeyType.Identity);
            AutoMap();
        }
    }

    /// <summary>
    /// 卖家订单状态
    /// </summary>
    public enum ESellerOrderState
    {
        /// <summary>
        /// 买家下单
        /// </summary>
        [Description("已下单")]
        BuyerOrder = 0,
        /// <summary>
        /// 买家取消订单（卖家未确认时方可取消）
        /// </summary>
        [Description("订单取消")]
        BuyerCancel = 1,
        /// <summary>
        /// 买家确认收货
        /// </summary>
        [Description("已确认收货")]
        BuyerTakeDelivery = 2,
        /// <summary>
        /// 卖家确认订单
        /// </summary>
        [Description("卖家确认订单")]
        SellerConfirm = 10,
        /// <summary>
        /// 卖家取消订单
        /// </summary>
        [Description("卖家取消订单")]
        SellerCancel = 11,

        /// <summary>
        /// 卖家已发货
        /// </summary>
        [Description("卖家已发货")]
        SellerDelivery = 12,

        /// <summary>
        /// 订单完结（已收货，已收款）
        /// </summary>
        [Description("订单完成")]
        Finish = 100,

    }
}
