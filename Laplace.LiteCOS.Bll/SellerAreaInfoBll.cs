using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerAreaInfoBll : BaseBll<SellerAreaInfo>
    {
        static SellerAreaInfoDal<SellerAreaInfo> Dal = new SellerAreaInfoDal<SellerAreaInfo>();
        /// <summary>
        /// 检查部门代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code, out string errMsg, int DeptId = 0)
        {
            if (DeptId != 0)
            {
                if (Dal.CheckCode(sellerId, DeptId, code, GetConnectionString(sellerId), out errMsg))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
            }
        }

        /// <summary>
        /// 获取code
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetAreaCode(int SellerId)
        {
            string sql = string.Format("select count(Code) from SellerAreaInfo where SellerId=" + SellerId);
            object res = Dal.ExecuteScalar(sql, GetConnectionString(SellerId));
            if (res != null)
            {
                return (Convert.ToInt32(res) + 1).ToString().PadLeft(4,'0');
            }
            else
            {
                return "0001";
            }
        }


        public static bool Delete(int sellerId,List<int> ids,out string errMsg)
        {
            bool res = true;
            errMsg = string.Empty;
            foreach (var item in ids)
            {
                res= res&& Dal.Delete(sellerId,item,GetConnectionString(sellerId),out errMsg);
            }
            return res;
        }

    }
}
