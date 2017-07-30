using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家库存
    /// </summary>
    public class SellerStockPile : BaseModel
    {

	

        /// <summary>
        /// 库存编号：自增，10001--2147483 647	0--10000为系统保留*/
        /// </summary>
        public int StockId { get; set; } = 0;

        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 商品编号(ProductInfo:ProductId)
        /// </summary>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// 所在仓库编号(SellerStoreHouseInfo:StoreHouseId)
        /// </summary>
        public int StoreHouseId { get; set; } = 0;

        /// <summary>
        /// 首次入库时间
        /// </summary>
        public DateTime FirstEnterDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后一次出库时间,为NULL 时,此种商品从来没有卖过
        /// </summary>
        public DateTime? LastLeaveDate { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; } = 0;
        /// <summary>
        /// 库存上限
        /// </summary>
        public int UpperLimit { get; set; } = 0;

        /// <summary>
        /// 库存下限
        /// </summary>
        public int LowerLitmit { get; set; } = 0;

        /// <summary>
        /// 库存成本(保留设计)
        /// </summary>
        public float Cost { get; set; } = 0;

        /// <summary>
        /// 加权价(保留设计)
        /// </summary>
        public float Price { get; set; } = 0;
        
    }
    public class SellerStockPileMapper : ClassMapper<SellerStockPile>
    {
        public SellerStockPileMapper()
        {
            Table("SellerStockPile");
            Map(x => x.StockId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
