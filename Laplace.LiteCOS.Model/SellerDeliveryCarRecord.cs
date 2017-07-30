using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家送货车辆行车实时状态
    /// </summary>
    public class SellerDeliveryCarRecord 
    {
        /// <summary>
        /// 车辆编号(SellerDeliveryCarInfo:CarId)
        /// </summary>
        public int CarId { get; set; } = 0;


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; } = 0;
        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; } = 0;
        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location { get; set; } = String.Empty;
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; } = string.Empty;
        /// <summary>
        /// 状态值
        /// </summary>
        public byte Status { get; set; } = 0;


    }
    public class SellerDeliveryCarRecordMapper : ClassMapper<SellerDeliveryCarRecord>
    {
        public SellerDeliveryCarRecordMapper()
        {
            Table("SellerDeliveryCarRecord");
            Map(x => x.CarId).Key(KeyType.Assigned);
            Map(x => x.StartTime).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
