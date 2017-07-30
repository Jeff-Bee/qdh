using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
  
	
    /// <summary>
    /// 卖家促销广告
    /// </summary>
    public class SaleAds : BaseModel
    {

        /// <summary>
        /// 广告编号：自增，10000--2147483 647	0--10000为系统保留*/
        /// </summary>
        public int AdsId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 广告标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 广告内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 广告图片，PictureInfo:PicId
        /// </summary>
        public int? Picture { get; set; } = 0;
        /// <summary>
        /// 商品编号，ProductInfo:ProductId
        /// </summary>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// 广告接收终端类型:
        /// 0：所有终端
        /// 1：微信终端
        /// 2：手机App终端
        /// </summary>
        public int Receiver { get; set; } = 0;

        /// <summary>
        /// 广告链接
        /// </summary>
        public string Link { get; set; } = string.Empty;
        /// <summary>
        /// 广告弹出时间，单位秒
        /// </summary>
        public byte PopTime { get; set; } = 0;
        /// <summary>
        /// 广告停留时间，单位秒
        /// </summary>
        public byte RemainTime { get; set; } = 0;
        /// <summary>
        /// 广告推送开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 广告结束开始时间
        /// </summary>
        public DateTime EndTime { get; set; } = new DateTime(2100,1,1);
        /// <summary>
        /// 广告是否有效
        /// </summary>
        public bool IsUsed { get; set; } = true;
  
        
    }
    public class SaleAdsMapper : ClassMapper<SaleAds>
    {
        public SaleAdsMapper()
        {
            Table("SaleAds");
            Map(x => x.AdsId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
