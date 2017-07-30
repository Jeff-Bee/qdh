using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 图片资源
    /// </summary>
    public class PictureInfo : BaseModel
    {
        /// <summary>
        /// 图片编号
        /// </summary>
        public int PicId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 图片名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 图片资源
        /// </summary>
        public byte[] Resource { get; set; } = null;
        /// <summary>
        /// 图片大小(字节)
        /// </summary>
        public int Size { get; set; } = 0;
        /// <summary>
        /// 图片格式
        /// </summary>
        public string Format { get; set; } = string.Empty;
        /// <summary>
        /// 图片宽度
        /// </summary>
        public short Width { get; set; } = 0;
        /// <summary>
        /// 图片高度
        /// </summary>
        public short Height { get; set; } = 0;

    }
    public class PictureInfoMapper : ClassMapper<PictureInfo>
    {
        public PictureInfoMapper()
        {
            Table("PictureInfo");
            Map(x => x.PicId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
