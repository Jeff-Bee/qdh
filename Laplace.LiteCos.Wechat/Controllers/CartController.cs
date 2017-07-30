using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Newtonsoft.Json;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class CartController : BaseBuyerController
    {
        // GET: Cart
        public ActionResult Index()
        {

            var user = GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            string msg;

            ViewBag.Addr = user.Address;
            List<BuyerShoppingCartViewModel> list = BuyerInfoBll.GetBuyerShoppingCartViewModels(user.BuyerId, out msg);

            if (list.Count > 0)
                return RedirectToAction("ShoppingCert", "Product");

            


            return View();
        }



        /// <summary>
        /// 更新购物车一条信息
        /// </summary>
        /// <param name="cartLineInfo"></param>
        /// <returns></returns>
        public JsonResult UpdateCartLine(string cartLineInfo)
        {




            if (Session["IsGuest"] != null)
            {
                bool isGuest = (bool) Session["IsGuest"];
                if (isGuest)
                {
                    return new JsonResult() { Data = new JsonResponseData(){IsSuccess = false,Msg = "该功能暂未开通"} };
                }
            }

            string msg;
            JsonResponseData result = new JsonResponseData() { IsSuccess = true };
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }
            BuyerShoppingCart line = JsonConvert.DeserializeObject<BuyerShoppingCart>(cartLineInfo);


            int count;


            try
            {
                BuyerShoppingCartBll.DeleteCertLine(user.BuyerId, line.SellerId, line.ProductId, out msg);


                if (line.ProductQuantity > 0)
                {
                    line.LMan = user.BuyerId;
                    line.RMan = user.BuyerId;
                    line.BuyerId = user.BuyerId;

                    if (BuyerShoppingCartBll.Insert(line))
                    {
                        result.IsSuccess = result.IsSuccess && true;
                        result.Msg = "修改成功";

                     

                        if (BuyerShoppingCartBll.GetCertCount(user.BuyerId, line.SellerId, out count, out msg))
                        {
                            result.IsSuccess = result.IsSuccess && true;
                            result.Msg = count + "";
                        }
                        else
                        {
                         
                            result.Msg = count + "";
                        }

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Msg = "修改失败";
                    }
                }
                else
                {
                    if (BuyerShoppingCartBll.GetCertCount(user.BuyerId, line.SellerId, out count, out msg))
                    {
                        result.IsSuccess = result.IsSuccess && true;
                        result.Msg = count + "";
                    }
                    else
                    {

                        result.Msg = count + "";
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("更新购物车失败" + ex.Message);
            }

            return new JsonResult() { Data = result };
        }



        /// <summary>
        /// 更新购物车一条信息
        /// </summary>     
        /// <returns></returns>
        public JsonResult GetCartCount(int sellerId=0)
        {

            string msg;
            JsonResponseData result = new JsonResponseData() { IsSuccess = true };
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }


         

            try
            {

                int count;
                if (BuyerShoppingCartBll.GetCertCount(user.BuyerId, sellerId, out count, out msg))
                {
                    result.IsSuccess = result.IsSuccess && true;
                    result.Msg = count+"";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Msg = count+"";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("更新购物车失败" + ex.Message);
            }

            return new JsonResult() { Data = result };
        }

    }
}