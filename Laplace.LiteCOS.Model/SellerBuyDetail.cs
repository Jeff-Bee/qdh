using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家进货单明细
    /// </summary>
    public class SellerBuyDetail : BaseModel
    {
        /// <summary>
        /// 进货单编号（SellerBuyInfo:BuyId）
        /// </summary>
        public int BuyId { get; set; } = 0;

        /// <summary>
        /// 卖家用户编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 序号(从0开始)
        /// </summary>
        public short Index { get; set; } = 0;
        /// <summary>
        /// 商品编号(ProductInfo:ProductId)
        /// </summary>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; } = 0;
        
    }
    public class SellerBuyDetailMapper : ClassMapper<SellerBuyDetail>
    {
        public SellerBuyDetailMapper()
        {
            Table("SellerBuyDetail");
            Map(x => x.BuyId).Key(KeyType.Assigned);
            Map(x => x.Index).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
