using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laplace.LiteCOS.Wechat.Models;

namespace Laplace.LiteCOS.Wechat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      
        public ActionResult Index()
        {
            UserInfo user = new UserInfo()
            {
                Name = "欢迎使用趣订货"
            };

            return View(user);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
    }
}