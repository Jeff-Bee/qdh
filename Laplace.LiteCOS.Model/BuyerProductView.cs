using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    public class BuyerProductView 
    {
        /// <summary>
        /// 卖家编号(SellerInfo:SellerId)
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 买家编号(BuyerInfo:BuyerId)
        /// </summary>
        public int BuyerId { get; set; } = 0;
        /// <summary>
        /// 商品编号(ProductInfo:ProductId)
        /// </summary>
        public int ProductId { get; set; } = 0;

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductQuantity { get; set; } = 0;

        
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
        /// 计量单位
        /// </summary>
        public string ProductUnit { get; set; } = string.Empty;

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
        /// 售价1
        /// </summary>
        public float Price1 { get; set; } = 0;

        /// <summary>
        /// 最小订购量
        /// </summary>
        public short MinOrder1 { get; set; } = 1;



        /// <summary>
        /// 最大订购量
        /// </summary>
        public int MaxOrder1 { get; set; } = 99999;


        /// <summary>
        /// 标记是否是新品
        /// </summary>
        public bool IsNew { get; set; } = false;
        /// <summary>
        /// 标记是否是促销商品
        /// </summary>
        public bool IsPromotion { get; set; } = false;

    }

}
