using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;

namespace Laplace.Framework.Helper
{
    /// <summary>  
    ///WCF Using使用封装
    /// </summary>  
    public static class WcfHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="action"></param>
        public static void Using<T>(T client, Action<T> action) where T : ICommunicationObject
        {
            try
            {
                action(client);
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }
        //使用时，可以将原本的客户端代码作为Action委托的Lambda表达式传递给Using方法中：
        //WcfHelper.Using(new MyClient(), client =>
        //    {
        //        client.SomeWCFOperation();
        //        //其他代码;
        //    });
    }
}
