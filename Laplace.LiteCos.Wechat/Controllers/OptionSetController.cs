using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCos.Wechat.Controllers
{
    /// <summary>
    /// 个人配置
    /// </summary>
    public class OptionSetController : BaseBuyerController
    {
        // GET: OptionSet
        public ActionResult SetList()
        {
            return View(GetUser());
        }

        [HttpPost]
        public ActionResult SetName(string name)
        {

            return View(GetUser());
        }

        public ActionResult SetName()
        {
            return View(GetUser());
        }


        public ActionResult SetPhone()
        {
            return View(GetUser());
        }


        [HttpPost]
        public ActionResult SetPhone(string phone,string vcode)
        {
            return View();
        }



        public ActionResult SetPassword()
        {
            return View(GetUser());
        }

        [HttpPost]
        public ActionResult SetPassword(string oldpwd,string newpwd,string rnewpwd)
        {
            return View(GetUser());
        }


    }
}