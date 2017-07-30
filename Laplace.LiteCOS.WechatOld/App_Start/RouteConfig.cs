using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Laplace.LiteCOS.Wechat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //  routes.MapRoute(
            //    name: "DefaultArea",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Author", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { "MVCUI.Areas.User.Controllers" }
            //).DataTokens.Add("Area", "User");






            routes.MapRoute(
              name: "DefaultArea",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Author", action = "Login", id = UrlParameter.Optional },
              namespaces: new string[] { "MVCUI.Areas.User.Controllers" }
          ).DataTokens.Add("Area", "User");


            //routes.MapRoute(
            //name: "DefaultArea",
            //url: "{controller}/{action}/{id}",
            //defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
            //namespaces: new string[] { "MVCUI.Areas.Product.Controllers" }
            //).DataTokens.Add("Area", "Product");





            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}

