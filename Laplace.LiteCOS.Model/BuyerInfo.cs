using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    public class BuyerInfo : BaseModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int BuyerId { get; set; } = 0;

        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; } = String.Empty;

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; } = String.Empty;



        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; } = String.Empty;




        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; } = String.Empty;
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; } = String.Empty;
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; } = String.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EMail { get; set; } = String.Empty;

        /// <summary>
        /// 软件授权有效期
        /// </summary>
        public DateTime ValidityPeriod { get; set; } = DateTime.MaxValue;

        ///// <summary>
        ///// 数据库地址
        ///// </summary>
        //public string DbServer { get; set; }
        ///// <summary>
        ///// 数据库名称
        ///// </summary>
        //public string DbName { get; set; }
        ///// <summary>
        ///// 数据库用户名
        ///// </summary>
        //public string DbUserName { get; set; }
        ///// <summary>
        ///// 数据库密码
        ///// </summary>
        //public string DbPassword { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        //public bool IsUsed { get; set; }
        // <summary>
        // 关联微信号
        // </summary>
        public string WechatId { get; set; } = string.Empty;
        /// <summary>
        /// 买家类型 0：线上买家；1：线下买家；2：线上线下买家
        /// </summary>
        public byte BuyerType { get; set; } = 0;

    }
    public class BuyerInfoMapper : ClassMapper<BuyerInfo>
    {
        public BuyerInfoMapper()
        {
            Table("BuyerInfo");
            Map(x => x.BuyerId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
