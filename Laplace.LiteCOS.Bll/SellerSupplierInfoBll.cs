using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerSupplierInfoBll : BaseBll<SellerSupplierInfo>
    {
        static SellerSupplierInfoDal<SellerSupplierInfo> Dal = new SellerSupplierInfoDal<SellerSupplierInfo>();

        /// <summary>
        /// 检查供应商代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code, out string errMsg,int suppId=0)
        {
            if (suppId > 0)
            {
                if (!Dal.CheckCode(sellerId, suppId, code, GetConnectionString(sellerId), out errMsg))
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
        /// <param name="ids"></param>
        /// <param name="sellerId"></param>
        /// <param name="ErrMeg"></param>
        /// <returns></returns>
        public static bool Delete(List<int> ids, int sellerId, out string ErrMeg)
        {
            ErrMeg = string.Empty;
            bool res = true;
            foreach (var id in ids)
            {
                res = res && Dal.Delete(id,sellerId,GetConnectionString(sellerId),out ErrMeg);
            }
            return res;
        }

        /// <summary>
        /// 获取新增code 
        /// </summary>
        /// <param name="SupperId"></param>
        /// <returns></returns>
        public static string GetSupperCode(int SellerId)
        {
            string sql = string.Format("select count(Code) from SellerSupplierInfo where SellerId="+ SellerId);
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
