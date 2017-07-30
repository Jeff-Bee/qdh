using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerCustomerInfoBll : BaseBll<SellerCustomerInfo>
    {
        static SellerCustomerInfoDal<SellerCustomerInfo> Dal = new SellerCustomerInfoDal<SellerCustomerInfo>();

        /// <summary>
        /// 判断客户信息code是否重复
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code, out string errMsg, int customerId=0)
        {
            if (customerId > 0)
            {
                if (Dal.CheckCode(sellerId, code, customerId, GetConnectionString(sellerId), out errMsg))
                {
                    return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
                }
                else {
                    return false;
                }

            }
            else
            {
                return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId,int CarId)
        {
            string errMsg = string.Empty;
            return Dal.Delete(CarId, GetConnectionString(sellerId),out errMsg);
        }
        /// <summary>
        /// 获取code
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetCustomerCode(int SellerId)
        {
            string sql = string.Format("select count(Code) from SellerCustomerInfo where SellerId=" + SellerId);
            object res = Dal.ExecuteScalar(sql, GetConnectionString(SellerId));
            if (res != null)
            {
                return (Convert.ToInt32(res) + 1).ToString().PadLeft(4,'0');
            }
            else {
                return "0001";
            }
        }

        
    }
}
