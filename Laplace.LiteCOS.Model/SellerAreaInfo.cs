using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家地区信息
    /// </summary>
    public class SellerAreaInfo : BaseModel
    {

        /// <summary>
        /// 地区编号：自增，10000--2147483 647	0--1000000为系统保留*/
        /// </summary>
        public int AreaId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 地区代码
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 地区全名
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// 地区简名
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
       
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;
  
        
    }
    public class SellerAreaInfoMapper : ClassMapper<SellerAreaInfo>
    {
        public SellerAreaInfoMapper()
        {
            Table("SellerAreaInfo");
            Map(x => x.AreaId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
