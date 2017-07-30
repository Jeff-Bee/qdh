using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家在线支付信息信息
    /// </summary>
    public class SellerOnlinePaymentInfo : BaseModel
    {

        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 支付类型:1：微信;2：支付宝
        /// </summary>
        public byte PayType { get; set; } = 0;

        /// <summary>
        /// (微信)应用Id
        /// </summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// (微信)应用密钥
        /// </summary>
        public string AppSecret { get; set; } = string.Empty;


        /// <summary>
        /// (微信)商户号/支付宝企业账号
        /// </summary>
        public string MchId { get; set; } = string.Empty;


        /// <summary>
        /// (支付宝)合作者身份
        /// </summary>
        public string PartnerId { get; set; } = string.Empty;

        /// <summary>
        /// (微信)API密钥/支付宝安全校验码
        /// </summary>
        public string AppKey { get; set; } = string.Empty;

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsUsed { get; set; } = false;

       
    }
    public class SellerOnlinePaymentInfoMapper : ClassMapper<SellerOnlinePaymentInfo>
    {
        public SellerOnlinePaymentInfoMapper()
        {
            Table("SellerOnlinePaymentInfo");
            Map(x => x.SellerId).Key(KeyType.Assigned);
            Map(x => x.PayType).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
