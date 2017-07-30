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
    public class SellerAccountInfo : BaseModel
    {

        /// <summary>
        /// 账户编号：自增，10000--2147483 647	0--1000000为系统保留*/
        /// </summary>
        public int AccountId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 账户代码
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 账户全名
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// 账户简名
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
       
        
    }
    public class SellerAccountInfoMapper : ClassMapper<SellerAccountInfo>
    {
        public SellerAccountInfoMapper()
        {
            Table("SellerAccountInfo");
            Map(x => x.AccountId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
