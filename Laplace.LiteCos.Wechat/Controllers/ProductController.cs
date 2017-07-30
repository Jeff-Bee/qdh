using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Newtonsoft.Json;

namespace Laplace.LiteCos.Wechat.Controllers
{

    public class ProductController : BaseBuyerController
    {
      
        public ActionResult ShoppingCert()
        {
            var user = GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Login",new {area=""});
            }

            string msg;

            ViewBag.Addr = user.Address;
            List<BuyerShoppingCartViewModel> list = BuyerInfoBll.GetBuyerShoppingCartViewModels(user.BuyerId, out msg);
            return View(list);
        }

        public PartialViewResult GetCategorysProduct(int sellerId = 0, int classId = 0)
        {

            BuyerInfo user = Session["User"] as BuyerInfo;
            if (user == null)
            {
                return PartialView("NeedLogin");
            }

            var list = BuyerInfoBll.GetBuyerProductView(user.BuyerId, sellerId, classId);
            return PartialView(list);
        }



      



        public PartialViewResult SellerListView()
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

      
  
        public ActionResult MainView()
        {
            var user = GetUser();

            string msg;
            var list = BuyerInfoBll.GetSellerList(user.BuyerId, out msg);
            if (list != null)
            {
                if (list.Count == 0)
                {
                    return RedirectToAction("NoSeller");
                }
                if (list.Count == 1)
                {
                    SellerInfo sellerInfo = list.FirstOrDefault();
                    if (sellerInfo != null)
                        return RedirectToAction("ShowSellersGoods", new { sellerId = sellerInfo.SellerId });
                }
            }
            
            return View(user);
        }

        public ActionResult ShowSellersGoods(int sellerId)
        {
            var user = GetUser();
            if (user == null)
            {
                return PartialView("NeedLogin");
            }

            SellersGoodsViewModel viewModel = new SellersGoodsViewModel(sellerId, user.BuyerId);
            return View(viewModel);

        }




        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="cartLines"></param>
        /// <returns></returns>
        public JsonResult AddGoodsToCart(string cartLines)
        {


            if (Session["IsGuest"] != null)
            {
                bool isGuest = (bool)Session["IsGuest"];
                if (isGuest)
                {
                    return new JsonResult() { Data = new JsonResponseData() { IsSuccess = false, Msg = "该功能暂未开通" } };
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
            List<BuyerShoppingCart> lines = JsonConvert.DeserializeObject<List<BuyerShoppingCart>>(cartLines);

            int sellerId = 0;
            if (lines.Count > 0)
            {
                sellerId = lines.FirstOrDefault().SellerId;
            }


            try
            {
                BuyerShoppingCartBll.DeleteBySellerId(user.BuyerId, sellerId, out msg);


                lines = lines.Where(m => m.ProductQuantity > 0).ToList();

                foreach (BuyerShoppingCart line in lines)
                {
                    line.LMan = user.BuyerId;
                    line.RMan = user.BuyerId;
                    line.BuyerId = user.BuyerId;

                    if (BuyerShoppingCartBll.Insert(line))
                    {
                        result.IsSuccess = result.IsSuccess && true;
                        result.Msg = "添加成功";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Msg = "添加失败";
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
        /// 从购物车中删除商品
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>      
        /// <returns></returns>
        public JsonResult DeleteGoodsFromCart(int sellerId, int productId)
        {

            if (Session["IsGuest"] != null)
            {
                bool isGuest = (bool)Session["IsGuest"];
                if (isGuest)
                {
                    return new JsonResult() { Data = new JsonResponseData() { IsSuccess = false, Msg = "该功能暂未开通" } };
                }
            }

            JsonResponseData result = new JsonResponseData();
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }

            string msg;

            if (BuyerShoppingCartBll.Delete(sellerId, user.BuyerId, productId, out msg))
            {
                result.IsSuccess = true;
                result.Msg = "删除成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Msg = msg;
            }

            return new JsonResult() { Data = result };
        }

        public JsonResult AddSellerOrdersToDb(string orderdetail)
        {

            if (Session["IsGuest"] != null)
            {
                bool isGuest = (bool)Session["IsGuest"];
                if (isGuest)
                {
                    return new JsonResult() { Data = new JsonResponseData() { IsSuccess = false, Msg = "该功能暂未开通" } };
                }
            }


            JsonResponseData result = new JsonResponseData();
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }


            var models =
                JsonConvert.DeserializeObject<List<SellerOrderDetailTinyViewModel>>(orderdetail);


            if (models == null)
            {
                result.IsSuccess = false;
                result.Msg = "订单无效,无法提交";
                return new JsonResult() { Data = result };
            }

            models = models.Where(m => m.ProductQuantity > 0).ToList();


            if (models.Count == 0)
            {
                result.IsSuccess = false;
                result.Msg = "什么也没有买~";
                return new JsonResult() { Data = result };
            }




            SellerOrderRun run = new SellerOrderRun()
            {
                Amount = models.Sum(m => m.ProductQuantity * m.Price),
                BuyerId = user.BuyerId,
                SellerId = models.First().SellerId,
                OrderDate = DateTime.Now,
                Code = "",
                Config = "",
                LDate = DateTime.Now,
                RDate = DateTime.Now,
                LMan = user.BuyerId,
                RMan = user.BuyerId,
                ProductQuantity = models.Sum(m => m.ProductQuantity)
            };

            int orderId;
            string code;
            string msg;



            if (SellerOrderRunBll.Insert(run,out orderId,out code,out msg))
            {
                int index = 0;
                foreach (SellerOrderDetailTinyViewModel detailTinyViewModel in models)
                {
                    SellerOrderDetail detail = new SellerOrderDetail()
                    {
                        SellerId = detailTinyViewModel.SellerId,
                        BuyerId = user.BuyerId,
                        LMan = user.BuyerId,
                        RMan = user.BuyerId,
                        LDate = DateTime.Now,
                        RDate = DateTime.Now,
                        Index = index++,
                        OrderId = orderId,
                        ProductId = detailTinyViewModel.ProductId,
                        ProductQuantity = detailTinyViewModel.ProductQuantity,
                        ProductName = detailTinyViewModel.ProductName,
                        ProductPrice = detailTinyViewModel.Price,
                        TotalPrice = detailTinyViewModel.Price * detailTinyViewModel.ProductQuantity,
                        ProductUnit = detailTinyViewModel.ProductUnit
                    };

                    SellerOrderDetailBll.Insert(detail);
                    BuyerShoppingCartBll.Delete(detail.SellerId, detail.BuyerId, detail.ProductId, out msg);
                }

                result.Msg = "订单生成成功";
                result.IsSuccess = true;

            }
            else
            {
                result.Msg = "通讯异常,订单创建失败";
            }









            return new JsonResult() { Data = result };
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


        public PartialViewResult GetOrdersByType(int  ordertype)
        {

            var user = GetUser();
            if (user == null)
            {
                return PartialView("NeedLogin");
            }


            List<SellerOrderRunViewModel> viewModels= new List<SellerOrderRunViewModel>();

            string msg;
            var list = SellerOrderRunBll.GetList(user.BuyerId, out msg).OrderByDescending(o => o.RDate).ToList();


         

            int[] type1 = new[] {(int) ESellerOrderState.BuyerOrder,(int)ESellerOrderState.BuyerCancel };
            int[] type2 = new[] {(int) ESellerOrderState.SellerConfirm,(int)ESellerOrderState.SellerDelivery };
            int[] type3 = new[] {(int) ESellerOrderState.BuyerTakeDelivery,(int)ESellerOrderState.SellerCancel, (int)ESellerOrderState.Finish };


            switch (ordertype)
            {
                case 1:
                    list = list.Where(m => type1.Contains(m.OrderState)).ToList();
                    break;
                case 2:
                    list = list.Where(m => type2.Contains(m.OrderState)).ToList();
                    break;
                case 3:
                    list = list.Where(m => type3.Contains(m.OrderState)).ToList();
                    break;
            }

            foreach (SellerOrderRun orderRun in list)
            {
                var viewModel = new SellerOrderRunViewModel(orderRun);
                if (viewModel.Details.Count > 0)
                    viewModels.Add(viewModel);
            }

            ViewBag.Addr = user.Address;
            ViewBag.Phone = user.MobilePhone;

            return PartialView(viewModels);
        }

        public ActionResult PreviewOrder(int status=0)
        {
            var user = GetUser();
            if (user == null)
            {
                

                return RedirectToAction("Login", "Login");
            }

            List<SellerOrderRunViewModel> viewModels = new List<SellerOrderRunViewModel>();


            ViewBag.Status = status;

            //string msg;
            //var list = SellerOrderRunBll.GetList(user.BuyerId,out msg).OrderByDescending(o => o.RDate);
            //foreach (SellerOrderRun orderRun in list)
            //{
            //    var viewModel = new SellerOrderRunViewModel(orderRun);
            //    if (viewModel.Details.Count > 0)
            //        viewModels.Add(viewModel);
            //}

            return View(viewModels);
        }




    }
}