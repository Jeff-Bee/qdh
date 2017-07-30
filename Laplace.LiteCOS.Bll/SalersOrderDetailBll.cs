using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SalersOrderDetailBll : BaseBll<SalersOrderDetail>
    {
        protected static readonly SalersOrderDetailDal<SalersOrderDetail> Dal = new SalersOrderDetailDal<SalersOrderDetail>();
        public static bool Insert(List<SalersOrderDetail> models, int SellerId)
        {
            string errMsg = string.Empty;
            return Dal.Insert(models, GetConnectionString(SellerId), out errMsg);
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int OrderId, int SellerId, out string errMsg)
        {
            return Dal.Delete(OrderId, GetConnectionString(SellerId),out errMsg);
        }
    }
}
