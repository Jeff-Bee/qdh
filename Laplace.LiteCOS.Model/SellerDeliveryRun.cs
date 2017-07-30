using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家订单送货物流记录
    /// </summary>
    public class SellerDeliveryRun : BaseModel
    {

        /// <summary>
        /// 物流编号：自增，10001--2147483 647	0--10000为系统保留
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// 物流代码：格式：WL-0-20161107-XXXXX
        /// </summary>
        public string Code { get; set; } = String.Empty;
        /// <summary>
        /// 车辆编号(SellerDeliveryCarInfo:CarId)
        /// </summary>
        public int CarId { get; set; } = 0;

        /// <summary>
        /// 订单编号(SellerOrderRun:OrderId)
        /// </summary>
        public int OrderId { get; set; } = 0;
        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 运送司机
        /// </summary>
        public string Driver { get; set; } = String.Empty;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string MobilePhone { get; set; } = String.Empty;
        /// <summary>
        /// 出发地
        /// </summary>
        public string Source { get; set; } = String.Empty;
        /// <summary>
        /// 目的地
        /// </summary>
        public string Destination { get; set; } = String.Empty;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
    public class SellerDeliveryRunMapper : ClassMapper<SellerDeliveryRun>
    {
        public SellerDeliveryRunMapper()
        {
            Table("SellerDeliveryRun");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
