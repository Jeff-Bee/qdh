using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 商品分类表
    /// </summary>
    public class ProductClassInfo : BaseModel
    {
        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int ClassId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;
        /// <summary>
        /// 上级分类编号
        /// </summary>
        public int ParentId { get; set; } = 0;

  
        
    }
    public class ProductClassInfoMapper : ClassMapper<ProductClassInfo>
    {
        public ProductClassInfoMapper()
        {
            Table("ProductClassInfo");
            Map(x => x.ClassId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
