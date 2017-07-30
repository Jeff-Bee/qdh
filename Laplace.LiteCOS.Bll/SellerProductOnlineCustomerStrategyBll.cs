using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerProductOnlineCustomerStrategyBll : BaseBll<SellerProductOnlineCustomerStrategy>
    {
        static SellerProductOnlineCustomerStrategyDal<SellerProductOnlineCustomerStrategy> Dal = new SellerProductOnlineCustomerStrategyDal<SellerProductOnlineCustomerStrategy>();
        public static bool Save(List<SellerProductOnlineCustomerStrategy> models)
        {
            string errMsg = string.Empty;
            bool res = true;
            foreach (var model in models)
            {
                if (Dal.CheckModel(model, GetConnectionString(model.SellerId), out errMsg))
                {
                    res= res&& Update(model);
                }
                else
                {
                    res = res && Insert(model);
                }
            }
            return res;
        }
    }
}
