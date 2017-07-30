using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家供应商信息
    /// </summary>
    public class SellerSupplierInfo : BaseModel
    {

        /// <summary>
        /// 供应商编号：自增，10000--2147483 647	0--1000000为系统保留*/
        /// </summary>
        public int SupplierId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 供应商代码（确保同卖家唯一）
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
        /// 换货天数
        /// </summary>
        public short ExchangeDay { get; set; } = 0;
        /// <summary>
        /// 换货比例
        /// </summary>
        public short ExchangePercent { get; set; } = 100;
        
    }
    public class SellerSupplierInfoMapper : ClassMapper<SellerSupplierInfo>
    {
        public SellerSupplierInfoMapper()
        {
            Table("SellerSupplierInfo");
            Map(x => x.SupplierId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
