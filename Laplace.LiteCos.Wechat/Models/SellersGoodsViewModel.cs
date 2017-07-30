using System.Collections.Generic;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Models
{
    public class SellersGoodsViewModel
    {

        public SellerInfo SellerInfo
        {
            get { return sellerInfo; }
            //set { sellerInfo = value; }
        }

        public BuyerInfo BuyerInfo
        {
            get { return buyerInfo; }
        }



        private SellerInfo sellerInfo;
        private BuyerInfo buyerInfo;

        private List<ProductClassInfo> productClassInfos;

        public List<ProductClassInfo> ProductClassInfos
        {
            get { return productClassInfos; }
        }

        private string msg;

        public SellersGoodsViewModel(int sellerId,int buyerId)
        {
            sellerInfo = SellerInfoBll.GetModel(sellerId, out msg);
            buyerInfo = BuyerInfoBll.GetModel(buyerId, out msg);
            productClassInfos= BuyerInfoBll.GetProductClassInfo(buyerInfo.BuyerId, sellerId, out msg);
        }
    }
}