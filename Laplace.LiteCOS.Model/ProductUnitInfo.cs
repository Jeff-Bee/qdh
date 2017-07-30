using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 商品计量单位表
    /// </summary>
    public class ProductUnitInfo : BaseModel
    {
        /// <summary>
        /// 商品计量单位编号
        /// </summary>
        public int UnitId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
   
  
        
    }
    public class ProductUnitInfoMapper : ClassMapper<ProductUnitInfo>
    {
        public ProductUnitInfoMapper()
        {
            Table("ProductUnitInfo");
            Map(x => x.UnitId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
