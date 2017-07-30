using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class NewProductLaunchBll : BaseBll<NewProductLaunch>
    {
        protected static readonly NewProductLaunchDal<NewProductLaunch> Dal = new NewProductLaunchDal<NewProductLaunch>();

        /// <summary>
        /// 根据主键id删除
        /// </summary>
        /// <param name="AdsId"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int Id, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return Dal.Delete(Id, SellerId, GetConnectionString(SellerId), out errMsg);
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
            NewProductLaunch model = Dal.CheckName(name, SellerId, GetConnectionString(SellerId), out errMsg);
            if (model != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static NewProductLaunch GetModel(int Id, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return Dal.GetModel(Id, SellerId, GetConnectionString(SellerId), out errMsg);
        }
        public static NewProductLaunch GetModel(bool IsUsed, int SellerId, out string errMsg)
        {
            errMsg = string.Empty;
            return Dal.GetModel(IsUsed, SellerId, GetConnectionString(SellerId), out errMsg);
        }
    }
}
