using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    public class BuyerShoppingCart : BaseModel
    {
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
        /// 商品数量
        /// </summary>
        public int ProductQuantity { get; set; } = 0;
    }
    public class BuyerShoppingCartMapper : ClassMapper<BuyerShoppingCart>
    {
        public BuyerShoppingCartMapper()
        {
            Table("BuyerShoppingCart");
            Map(x => x.BuyerId).Key(KeyType.Assigned);
            Map(x => x.ProductId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
