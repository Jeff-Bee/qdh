using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCOS.Web.Controllers
{
    public class CommonlyUsedFunctionController : Controller
    {
        // GET: CommonlyUsedFunction
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 业务草稿
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessDraft()
        {
            return View();
        }
    }
}