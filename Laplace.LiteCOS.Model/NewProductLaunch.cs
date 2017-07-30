using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{


    /// <summary>
    /// 卖家新品发布
    /// </summary>
    public class NewProductLaunch : BaseModel
    {

        /// <summary>
        /// 新品发布编号：自增，10000--2147483 647	0--10000为系统保留*/
        /// </summary>
        public int Id { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 新品发布标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 新品发布内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 新品发布图片，PictureInfo:PicId
        /// </summary>
        public int? Picture { get; set; } = 0;
        /// <summary>
        /// 商品编号，ProductInfo:ProductId
        /// </summary>
        public int ProductId { get; set; } = 0;
        /// <summary>
        /// 新品发布接收终端类型:
        /// 0：所有终端
        /// 1：微信终端
        /// 2：手机App终端
        /// </summary>
        public int Receiver { get; set; } = 0;

        /// <summary>
        /// 新品发布链接
        /// </summary>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// 新品发布开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 新品发布结束时间
        /// </summary>
        public DateTime EndTime { get; set; } = new DateTime(2100,1,1);
        /// <summary>
        /// 新品发布是否有效
        /// </summary>
        public bool IsUsed { get; set; } = true;
        /// <summary>
        /// 自动关闭（停留）时间
        /// </summary>
        public byte RemainTime { get; set; } = 0;

    }
    public class NewProductLaunchMapper : ClassMapper<NewProductLaunch>
    {
        public NewProductLaunchMapper()
        {
            Table("NewProductLaunch");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
