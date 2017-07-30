using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SaleAdsBll : BaseBll<SaleAds>
    {
        static SaleAdsDal<SaleAds> dal = new SaleAdsDal<SaleAds>();
        /// <summary>
        /// 根据主键id删除
        /// </summary>
        /// <param name="AdsId"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int AdsId, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return dal.Delete(AdsId,SellerId,GetConnectionString(SellerId),out errMsg);
        }

        /// <summary>
        /// 检查名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns>true 为存在名字</returns>
        public static bool CheckName(string name, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            SaleAds model= dal.CheckName(name,SellerId,GetConnectionString(SellerId),out errMsg);
            if (model != null)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static SaleAds GetModel(int AdsId, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return dal.GetModel(AdsId,SellerId,GetConnectionString(SellerId),out errMsg);
        }
        public static SaleAds GetModel(bool IsUsed, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return dal.GetModel(IsUsed, SellerId, GetConnectionString(SellerId), out errMsg);
        }
    }
}
