using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class SellerController : BaseBuyerController
    {
        public PartialViewResult ListView()
        {
            var user = GetUser();
            if (user == null)
            {
                return PartialView("NeedLogin");
            }

            string msg = "";

            var list = BuyerInfoBll.GetSellerList(user.BuyerId, out msg);

            ViewBag.Msg = msg;
            return PartialView(list);
        }



        public PartialViewResult NeedLogin()
        {
            return PartialView();
        }


        public ActionResult NoSeller()
        {
            var user = GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            ViewBag.Title = user.ContactName;
            return View(user);
        }



        public PartialViewResult GetSellerClass(int sellerId)
        {
            var buyerInfo = GetUser();
            string msg;
            if (buyerInfo == null)
            {
                return PartialView("NeedLogin");
            }

          

            List<ProductClassInfo> classInfos = BuyerInfoBll.GetProductClassInfo(buyerInfo.BuyerId, sellerId, out msg);
            return PartialView(classInfos);
        }
    }
}