using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Global
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class ApplicationParms
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        ///  一天内接收短信验证码最大条数，默认5条
        /// </summary>
        public static int SmsMaxCount { get; private set; }
        /// <summary>
        /// 短信验证码有效时长，单位分钟
        /// </summary>
        public static int SmsVerificationCodeValidityPeriod { get; private set; }
        static ApplicationParms()
        {
            ConnectionString = "Data Source=www.litecms.cn,18433;Initial Catalog=趣订货;Persist Security Info=True;User ID=qdg;Password=qdg168";
            SmsMaxCount = 5;
            SmsVerificationCodeValidityPeriod = 15;
        }


    }
}
