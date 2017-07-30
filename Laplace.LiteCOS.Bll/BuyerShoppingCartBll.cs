using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.Framework.Log;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 买家购物车信息
    /// </summary>
    public class BuyerShoppingCartBll : BaseBll<BuyerShoppingCart>
    {
        protected static readonly BuyerShoppingCartDal<BuyerShoppingCart> Dal = new BuyerShoppingCartDal<BuyerShoppingCart>();
        /// <summary>
        /// 保存/更新买家购物车信息
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="productQuantity"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Save(int buyerId, int sellerId, int productId, int productQuantity, out string errMsg)
        {
            var model = new BuyerShoppingCart();
            model.BuyerId = buyerId;
            model.SellerId = sellerId;
            model.ProductId = productId;
            model.ProductQuantity = productQuantity;
            return Dal.Save(model, GetConnectionString(sellerId), out errMsg);
        }


        /// <summary>
        /// 删除购物车中某条信息
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int buyerId, int productId, out string msg)
        {
            return Dal.Delete(buyerId, productId, GetConnectionString(sellerId), out msg);
        }



        /// <summary>
        /// 删除某一卖家购物车中某条信息
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>     
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool DeleteBySellerId(int buyerId, int sellerId, out string msg)
        {
            return Dal.DeleteBySellerId(buyerId, sellerId, GetConnectionString(sellerId), out msg);
        }

        /// <summary>
        /// 删除购物车中的某一行信息
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool DeleteCertLine(int buyerId, int sellerId,int productId, out string msg)
        {
            return Dal.DeleteCertLine(buyerId, sellerId, productId, GetConnectionString(sellerId), out msg);
        }


        public static bool GetCertCount(int buyerId, int sellerId,out int count, out string msg)
        {
            return Dal.GetCertCount(buyerId, sellerId,  GetConnectionString(sellerId),out count, out msg);
        }

    }
}
