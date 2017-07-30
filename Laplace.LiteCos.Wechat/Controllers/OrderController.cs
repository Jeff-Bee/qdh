using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class OrderController : Controller
    {
        BuyerInfo GetUser()
        {
            BuyerInfo user = Session["User"] as BuyerInfo;
            return user;
        }

        // GET: Product/Order
        public JsonResult CancelOrder(int sellerId, int orderId)
        {
            JsonResponseData result = new JsonResponseData();
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }

            string msg;
            var model = SellerOrderRunBll.GetModel(sellerId, orderId, out msg);
            model.OrderState = (short)ESellerOrderState.BuyerCancel;
            if (SellerOrderRunBll.Update(model))
            {
                result.IsSuccess = true;
                result.Msg = "订单取消成功";
            }
            else
            {
                result.IsSuccess = false;
                result.Msg = "订单取消失败";
            }

            return new JsonResult() { Data = result };

        }



        public JsonResult ConfirmReceive(int sellerId, int orderId)
        {

            JsonResponseData result = new JsonResponseData();
            var user = GetUser();
            if (user == null)
            {
                result.IsSuccess = false;
                result.Msg = "请重新登陆";
                return new JsonResult() { Data = result };
            }

            string msg;
            var model = SellerOrderRunBll.GetModel(sellerId, orderId, out msg);
            model.OrderState = (short)ESellerOrderState.BuyerTakeDelivery;
            if (SellerOrderRunBll.Update(model))
            {
                result.IsSuccess = true;
                result.Msg = "已收货";
            }
            else
            {
                result.IsSuccess = false;
                result.Msg = "确认收货失败";
            }

            return new JsonResult() { Data = result };

        }
    }
}