using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    public class SellerInfo:BaseModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;
        /// <summary>
        /// 所属行业编号
        /// </summary>
        public int IndustryId { get; set; } = 0;
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

     

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }




        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EMail { get; set; }
  
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
        /// <summary>
        /// 关联微信号
        /// </summary>
        //public string Weixin { get; set; }
    }
    public class SellerInfoMapper : ClassMapper<SellerInfo>
    {
        public SellerInfoMapper()
        {
            Table("SellerInfo");
            Map(x => x.SellerId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
