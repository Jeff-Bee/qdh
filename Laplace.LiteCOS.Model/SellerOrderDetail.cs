using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家订单明细
    /// </summary>
    public class SellerOrderDetail : BaseModel
    {
        /// <summary>
        /// 订单编号：(SellerOrderRun:OrderId)
        /// </summary>
        public int OrderId { get; set; } = 0;
        /// <summary>
        /// 订单明细序号
        /// </summary>
        public int Index { get; set; } = 0;

    
        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 买家编号(BuyerInfo:BuyerId)
        /// </summary>
        public int BuyerId { get; set; } = 0;
        /// <summary>
        /// 商品编号(ProductInfo:ProductId)
        /// </summary>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// 商品名称(ProductInfo:ProductFullName)
        /// </summary>
        public string ProductName { get; set; } = String.Empty;

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductQuantity { get; set; } = 0;

        /// <summary>
        /// 商品单位(ProductInfo:ProductUnit)
        /// </summary>
        public string ProductUnit { get; set; } = String.Empty;

        /// <summary>
        /// 商品单价
        /// </summary>
        public float ProductPrice { get; set; } = 0;

        /// <summary>
        /// 商品总价
        /// </summary>
        public float TotalPrice { get; set; } = 0;

       
    }
    public class SellerOrderDetailMapper : ClassMapper<SellerOrderDetail>
    {
        public SellerOrderDetailMapper()
        {
            Table("SellerOrderDetail");
            Map(x => x.OrderId).Key(KeyType.Assigned);
            Map(x => x.Index).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
