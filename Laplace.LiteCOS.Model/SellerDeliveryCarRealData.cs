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
    public class SellerDeliveryCarRealData 
    {
        /// <summary>
        /// 车辆编号(SellerDeliveryCarInfo:CarId)
        /// </summary>
        public int CarId { get; set; } = 0;


        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiptTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
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
        /// 固件版本
        /// </summary>
        public int Version { get; set; } = 0;
        /// <summary>
        /// 在线状态:0:离线；1：在线
        /// </summary>
        public byte Online { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; } = string.Empty;
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; } = 0;


    }
    public class SellerDeliveryCarRealDataMapper : ClassMapper<SellerDeliveryCarRealData>
    {
        public SellerDeliveryCarRealDataMapper()
        {
            Table("SellerDeliveryCarRealData");
            Map(x => x.CarId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
