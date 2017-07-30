using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家送货车辆信息
    /// </summary>
    public class SellerDeliveryCarInfo : BaseModel
    {
        /// <summary>
        /// 车辆编号：自增，10001--2147483 647	0--10000为系统保留
        /// </summary>
        public int CarId { get; set; } = 0;
        /// <summary>
        /// 车辆代码：6位
        /// </summary>
        public string Code { get; set; } = String.Empty;
        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; } = String.Empty;
        /// <summary>
        /// 车辆名称
        /// </summary>
        public string Name { get; set; } = String.Empty;
        /// <summary>
        /// 司机
        /// </summary>
        public string Driver { get; set; } = String.Empty;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string MobilePhone { get; set; } = String.Empty;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; } = true;
       
    }
    public class SellerDeliveryCarInfoMapper : ClassMapper<SellerDeliveryCarInfo>
    {
        public SellerDeliveryCarInfoMapper()
        {
            Table("SellerDeliveryCarInfo");
            Map(x => x.CarId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
