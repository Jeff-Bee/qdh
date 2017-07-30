using System.Collections.Generic;
using Laplace.Framework.Extensions;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Models
{
    public class SellerOrderRunViewModel: SellerOrderRun
    {
        public SellerOrderRunViewModel(SellerOrderRun run)
        {
            Amount = run.Amount;
            BuyerId = run.BuyerId;
            OrderDate = run.OrderDate;
            OrderState = run.OrderState;
            Code = run.Code;
            Config = run.Config;
            LDate = run.LDate;
            RDate = run.RDate;
            LMan = run.LMan;
            RMan = run.RMan;
            ProductQuantity = run.ProductQuantity;
            OrderId = run.OrderId;
            string msg = "";
            details = SellerOrderDetailBll.GetList(run.SellerId, run.OrderId, out msg);
            Message = msg;
            SellerId = run.SellerId;
            OrderStateString = ((ESellerOrderState) run.OrderState).ToDescription();
        }

        private List<SellerOrderDetail> details;

        public List<SellerOrderDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        public string Message { get; set; }


        public string OrderStateString { get; set; }
    }
}