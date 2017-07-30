using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerDeliveryRunBll : BaseBll<SellerDeliveryRun>
    {
        static SellerDeliveryRunDal<SellerDeliveryRun> Dal = new SellerDeliveryRunDal<SellerDeliveryRun>();
        /// <summary>
        /// 删除指定配送车辆信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int Id, int sellerId, out string errMsg)
        {
            errMsg = "";
            return Dal.Delete(Id, GetConnectionString(sellerId),out errMsg);
        }
    }
}
