using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class AccountController : BaseBuyerController
    {
        // GET: Account
        public ActionResult Index()
        {
            var user = GetUser();

            return View(user);
        }
    }
}