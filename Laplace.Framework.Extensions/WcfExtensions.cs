using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;


namespace Laplace.Framework.Extensions
{
    /// <summary>
    /// WCF 扩展类
    /// </summary>
    public static class WcfExtensions
    {
        /// <summary>
        /// 关闭wcf连接
        /// </summary>
        /// <param name="myServiceClient"></param>
        public static void CloseConnection(this ICommunicationObject myServiceClient)
        {
            if (myServiceClient.State != CommunicationState.Opened)
            {
                return;
            }
            try
            {
                myServiceClient.Close();
            }
            catch (CommunicationException ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
            }
            catch (TimeoutException ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
                throw;
            }
        }
    }
}
