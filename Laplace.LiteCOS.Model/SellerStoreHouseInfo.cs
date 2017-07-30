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
    public class SellerStoreHouseInfo : BaseModel
    {
        /// <summary>
        /// 仓库编号：自增，10000--2147483 647	0--1000000为系统保留*/
        /// </summary>
        public int StoreHouseId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 仓库代码
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; } = string.Empty;

    }
    public class SellerStoreHouseInfoMapper : ClassMapper<SellerStoreHouseInfo>
    {
        public SellerStoreHouseInfoMapper()
        {
            Table("SellerStoreHouseInfo");
            Map(x => x.StoreHouseId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
