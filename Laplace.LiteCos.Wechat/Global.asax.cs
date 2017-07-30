using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Wechat;

namespace Laplace.LiteCos.Wechat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //初始化Redis
            BaseRedisBll.Config("www.litecms.cn", 9000, "qdh168");
        }
    }
}
