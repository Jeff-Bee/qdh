using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家客户信息
    /// </summary>
    public class SellerCustomerInfo : BaseModel
    {

        /// <summary>
        /// 买家（客户）编号：BuyerInfo:BuyerId
        /// </summary>
        public int BuyerId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 客户代码（确保同卖家唯一）
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 单位全名
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// 单位简名
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
       
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;
        /// <summary>
        /// 所属地区编号（SellerAreaInfo主键）
        /// </summary>
        public int AreaId { get; set; } = 0;

        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNumber { get; set; } = string.Empty;
        /// <summary>
        /// 联系人
        /// </summary>
        public string ConstactPerson { get; set; } = string.Empty;
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; } = string.Empty;
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bank { get; set; } = string.Empty;
        /// <summary>
        /// 银行账户
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;
        /// <summary>
        /// 微信账号
        /// </summary>
        public string Wechat { get; set; } = string.Empty;

        /// <summary>
        /// 价格等级
        /// </summary>
        public byte PriceLevel { get; set; } = 0;

        /// <summary>
        /// 买家类型 0：线上买家；1：线下买家；2：线上线下买家
        /// </summary>
        public byte BuyerType { get; set; } = 0;

    }
    public class SellerCustomerInfoMapper : ClassMapper<SellerCustomerInfo>
    {
        public SellerCustomerInfoMapper()
        {
            Table("SellerCustomerInfo");
            Map(x => x.BuyerId).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
