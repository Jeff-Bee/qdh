using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laplace.Framework.Log;
using Laplace.Framework.Win32;

namespace Laplace.Framework.Helper
{
    public static class NetworkHelper
    {
        /// <summary>
        /// 判断当前网络状态 
        /// </summary>
        /// <returns>true:网络访问正常</returns>
        public static bool CheckNetwork()
        {
            var ret = false;
            int dwFlag = 0;
            //string netstatus = string.Empty;
            if (NativeMethods.InternetGetConnectedState(ref dwFlag, 0))
            {
                //if ((dwFlag & NativeMethods.INTERNET_CONNECTION_MODEM) != 0)
                //    netstatus += " 采用调治解调器上网 \n";

                //if ((dwFlag & NativeMethods.INTERNET_CONNECTION_LAN) != 0)
                //    netstatus += " 采用网卡上网  \n";

                //if ((dwFlag & NativeMethods.INTERNET_CONNECTION_PROXY) != 0)
                //    netstatus += " 采用代理上网  \n";

                //if ((dwFlag & NativeMethods.INTERNET_CONNECTION_MODEM_BUSY) != 0)
                //    netstatus += " MODEM被其他非INTERNET连接占用  \n";

                //访问网站再次验证
                System.Net.NetworkInformation.Ping ping;
                System.Net.NetworkInformation.PingReply res;
                ping = new System.Net.NetworkInformation.Ping();
                try
                {
                    res = ping.Send("www.baidu.com");
                    if (res != null && res.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                }

            }
            else
            {
                //netstatus = "未联网";
            }

            return ret;
        }
      

    }
    
}
