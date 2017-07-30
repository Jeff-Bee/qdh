using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerProductOnlineBll : BaseBll<SellerProductOnline>
    {
        static SellerProductOnlineDal<SellerProductOnline> Dal = new SellerProductOnlineDal<SellerProductOnline>();
        public static bool Save(List<SellerProductOnline> models)
        {
            string errMsg = string.Empty;
            bool res = true;
            foreach (var model in models)
            {
                if (Dal.CheckModel(model, GetConnectionString(model.SellerId), out errMsg))
                {
                    res = res && Update(model);
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
