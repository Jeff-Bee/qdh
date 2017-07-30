using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;
using Laplace.LiteCOS.Common.Enum;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 用户手机短信接收日志
    /// </summary>
    public class SmsLog
    {
        //public int Id { get; set; } = 0;

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; } = string.Empty;

        /// <summary>
        /// 发送短信时间
        /// </summary>
        public DateTime SmsTime { get; set; } = DateTime.Now;


        /// <summary>
        /// 客户编号
        /// </summary>
        public int UserId { get; set; } = 0;

        /// <summary>
        /// 日志类型
        /// </summary>
        public ESmsLogType LogType { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string SmsContent { get; set; } = string.Empty;

        public string Config { get; set; } = string.Empty;

        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; } = 0;
    }

    public class SmsLogMapper : ClassMapper<SmsLog>
    {
        public SmsLogMapper()
        {
            Table("SmsLog");
            Map(o => o.MobilePhone).Key(KeyType.Assigned);
            Map(o => o.SmsTime).Key(KeyType.Assigned);
            AutoMap();
        }
    }

}
