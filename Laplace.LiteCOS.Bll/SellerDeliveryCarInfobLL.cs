using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerDeliveryCarInfoBll : BaseBll<SellerDeliveryCarInfo>
    {
        static SellerDeliveryCarInfoDal<SellerDeliveryCarInfo> Dal = new SellerDeliveryCarInfoDal<SellerDeliveryCarInfo>();
        /// <summary>
        /// 返回指定设备信息
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="productId">设备编号</param>
        /// <param name="errMsg">如果执行失败，错误信息</param>
        /// <returns></returns>
        public static SellerDeliveryCarInfo GetModel(int sellerId, string Code, out string errMsg,int carId=0)
        {
            return Dal.GetModel(Code,sellerId, GetConnectionString(sellerId), out errMsg, carId);
        }

        /// <summary>
        /// 判断设备信息code是否重复
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code, out string errMsg, int customerId = 0)
        {
            if (customerId > 0)
            {
                if (Dal.CheckCode(sellerId, code, customerId, GetConnectionString(sellerId), out errMsg))
                {
                    return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
                }
                else
                {
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
        public static bool Delete(int sellerId, int carId)
        {
            string errMsg = string.Empty;
            return Dal.Delete(carId, GetConnectionString(sellerId), out errMsg);
        }

        /// <summary>
        /// 获取新增code
        /// </summary>
        /// <param name="SellerId"></param>
        /// <returns></returns>
        public static string GetDeliveryCarCode(int SellerId)
        {
            string sql = string.Format("select count(Code) from SellerDeliveryCarInfo where SellerId=" + SellerId);
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

        /// <summary>
        /// 检查校验码是否正确
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static bool CheckCodeChecked(string checkCode)
        {
            //处理逻辑后边在添加
            return true;
        }
    }
}
