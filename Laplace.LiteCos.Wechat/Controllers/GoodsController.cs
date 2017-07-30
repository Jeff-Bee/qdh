using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class GoodsController : BaseBuyerController
    {



        // GET: Goods
        public ActionResult List(int sellerId = -1, int classId = 0)
        {


            if (sellerId < 0)
            {
                try
                {
                    sellerId = Convert.ToInt32(Session["SellerId"]);
                    classId = Convert.ToInt32(Session["ClassId"]);

                }
                catch (Exception ex)
                {
                    ViewBag.SellerName = "获取商家信息失败";
                }
            }

            Session["SellerId"] = sellerId;
            Session["ClassId"] = classId;

            string msg;
            ViewBag.SellerId = sellerId;
            ViewBag.SelectType = "";

            var seller = SellerInfoBll.GetModel(sellerId, out msg);
            if (seller == null)
            {
                ViewBag.SellerName = "获取商家信息失败";

            }
            else
            {
                ViewBag.SellerName = seller.CompanyName;
            }


            var user = GetUser();

            if (user == null)
                return RedirectToAction("Login", "Login");



            List<BuyerProductView> list = new List<BuyerProductView>();
            if (classId == 0)
            {
                list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId);
            }
            else
            {
                ViewBag.SelectType = "class";
                list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId, classId);
            }

            return View(list);
        }



        public ActionResult Search()
        {
            return View();
        }


        public ActionResult Info(int sellerId, int productId)
        {
            string msg;
            ProductInfo productInfo = null;
            BuyerInfo user = Session["User"] as BuyerInfo;
            if (user == null)
            {
                return View(productInfo);
            }

            productInfo = ProductInfoBll.GetModel(sellerId, productId, out msg);


            var list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId);


            BuyerProductView productInfo1 = list.FirstOrDefault(p => p.ProductId == productId);

            if (productInfo == null)
                productInfo = new ProductInfo()
                {
                    ProductFullName = "商品信息有误"
                };


            return View(productInfo);
        }





        public PartialViewResult GetCategorysProduct(int sellerId = 0, int classId = 0)
        {
            List<BuyerProductView> productViews = new List<BuyerProductView>();


            BuyerInfo user = Session["User"] as BuyerInfo;
            if (user == null)
            {
                return PartialView(productViews);
            }

            List<BuyerProductView> list = new List<BuyerProductView>();
            if (classId == 0)
            {
                list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId);
            }
            else
            {
                ViewBag.SelectType = "class";
                list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId, classId);
            }



            return PartialView(list);
        }


    }
}