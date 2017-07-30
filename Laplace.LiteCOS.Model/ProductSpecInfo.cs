using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 商品规格表
    /// </summary>
    public class ProductSpecInfo : BaseModel
    {
        /// <summary>
        /// 规格编号
        /// </summary>
        public int SpecId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 规格名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
    }
    public class ProductSpecInfoMapper : ClassMapper<ProductSpecInfo>
    {
        public ProductSpecInfoMapper()
        {
            Table("ProductSpecInfo");
            Map(x => x.SpecId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
