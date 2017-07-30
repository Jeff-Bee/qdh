using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.LiteCOS.Global
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public static class ApplicationConstants
    {
        #region--云通讯账号信息--
        /// <summary>
        /// 主帐号
        /// </summary>
        public const string SmsAccountSid = "8a48b551523a5c1201523dc97bfc0689";
        /// <summary>
        /// 主帐号令牌
        /// </summary>
        public const string SmsAuthToken = "927a3ca0f4be466fbaa943c80f7d2c6e";

        /// <summary>
        ///  应用Id
        /// </summary>
        public const string SmsAppId = "8a216da857f4d3ec0157fba8eb9905ac";//趣订货
        /// <summary>
        /// REST服务器IP
        /// </summary>
        //public const string SmsRestServerIp = "https://sandboxapp.cloopen.com";     //测试环境
        public const string SmsRestServerIp = "https://app.cloopen.com";       //生产环境

        /// <summary>
        /// REST服务器端口
        /// </summary>
        public const string SmsRestServerPort = "8883";
        #endregion-云通讯账号信息-
    }
}
