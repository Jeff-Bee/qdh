using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class ProductInfo : BaseModel
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int ClassId { get; set; } = 0;
        /// <summary>
        /// 商品代码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;
        /// <summary>
        /// 商品全名
        /// </summary>
        public string ProductFullName { get; set; } = string.Empty;
        /// <summary>
        /// 商品简名
        /// </summary>
        public string ProductShortName { get; set; } = string.Empty;

        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;
        /// <summary>
        /// 封装
        /// </summary>
        public string Package { get; set; } = string.Empty;
        /// <summary>
        /// 商品规格
        /// </summary>
        public string ProductSpec { get; set; } = string.Empty;
        /// <summary>
        /// 商品型号
        /// </summary>
        public string ProductModel { get; set; } = string.Empty;
        /// <summary>
        /// 计量单位
        /// </summary>
        public string ProductUnit { get; set; } = string.Empty;
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; } = 0;
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; } = string.Empty;
        /// <summary>
        /// 商品产地
        /// </summary>
        public string Place { get; set; } = string.Empty;
        /// <summary>
        /// 状态
        /// </summary>
        public short ProductState { get; set; } = 0;

        /// <summary>
        /// 售价1
        /// </summary>
        public float Price1 { get; set; } = 0;
        /// <summary>
        /// 售价2
        /// </summary>
        public float Price2 { get; set; } = 0;
        /// <summary>
        /// 售价3
        /// </summary>
        public float Price3 { get; set; } = 0;
        /// <summary>
        /// 售价4
        /// </summary>
        public float Price4 { get; set; } = 0;
        /// <summary>
        /// 售价5
        /// </summary>
        public float Price5 { get; set; } = 0;
        /// <summary>
        /// 售价6
        /// </summary>
        public float Price6 { get; set; } = 0;
        /// <summary>
        /// 售价7
        /// </summary>
        public float Price7 { get; set; } = 0;
        /// <summary>
        /// 售价8
        /// </summary>
        public float Price8 { get; set; } = 0;

        /// <summary>
        /// 商品图片1
        /// </summary>
        public int Picture1 { get; set; } = 0;
        /// <summary>
        /// 商品图片2
        /// </summary>
        public int Picture2 { get; set; } = 0;
        /// <summary>
        /// 商品图片3
        /// </summary>
        public int Picture3 { get; set; } = 0;
        /// <summary>
        /// 商品图片4
        /// </summary>
        public int Picture4 { get; set; } = 0;
        /// <summary>
        /// 商品图片5
        /// </summary>
        public int Picture5 { get; set; } = 0;
        /// <summary>
        /// 商品图片6
        /// </summary>
        public int Picture6 { get; set; } = 0;


        /// <summary>
        /// 商品简介
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// 供货信息
        /// </summary>
        public string Supply { get; set; } = string.Empty;


        /// <summary>
        /// 创建方式（0:手工录入;1:导入）
        /// </summary>
        public short CreateType { get; set; } = 0;

        
    }
    public class ProductInfoMapper : ClassMapper<ProductInfo>
    {
        public ProductInfoMapper()
        {
            Table("ProductInfo");
            Map(x => x.ProductId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
