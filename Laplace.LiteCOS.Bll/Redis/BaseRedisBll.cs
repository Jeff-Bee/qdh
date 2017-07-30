using Laplace.Framework.Log;
using System;
using System.Collections.Generic;
using System.Text;
using Laplace.Framework.Redis;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    public  class BaseRedisBll
    {
        protected static readonly Lazy<BaseRedisDal<SmsLog>> _smsLogDal;         //用户短信操作日志
        static BaseRedisBll()
        {
            
            _smsLogDal = new Lazy<BaseRedisDal<SmsLog>>();
        }
        public static void Config(string server,int port,string password)
        {
            RedisHost.Config(server, port, password);
        }

        

    }

    

}
