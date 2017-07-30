using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.Framework.Log;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 卖家表格列展示配置信息
    /// </summary>
    public class SellerColumnConfigBll : BaseBll<SellerColumnConfig>
    {
        protected static readonly SellerColumnConfigDal<SellerColumnConfig> Dal = new SellerColumnConfigDal<SellerColumnConfig>();

        /// <summary>
        /// 插入指定卖家表配置记录
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="list"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(int sellerId, List<SellerColumnConfig> list, out string errMsg)
        {
            if (sellerId == 0)
            {
                errMsg = "不能添加系统配置";
                return false;
            }
            return Dal.Insert(list, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 删除指定卖家，指定表的列配置记录
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, string tableName, out string errMsg)
        {
            if (sellerId == 0)
            {
                errMsg = "不能删除系统配置";
                return false;
            }
            return Dal.Delete(sellerId, tableName, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定卖家，指定表的列配置记录（如果没有记录，插入并返回系统默认记录）
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerColumnConfig> GetList(int sellerId, string tableName, out string errMsg)
        {
            var ret = Dal.GetList(sellerId, tableName, GetConnectionString(sellerId), out errMsg);
            if (string.IsNullOrEmpty(errMsg) && !ret.Any())
            {
                //当前用户没有配置，读取默认配置
                ret = Dal.GetList(0, tableName, GetConnectionString(0), out errMsg);
                //拷贝配置默认配置
                foreach (var config in ret)
                {
                    config.SellerId = sellerId;
                    config.LDate = config.RDate = DateTime.Now;
                }
                if (!Insert(sellerId, ret, out errMsg))
                {
                    Logger.LogError(string.Format("拷贝系统默认列配置失败:sellerId={0},tableName={1},Err={2}"
                        , sellerId, tableName,errMsg),"AppLogger");
                }
            }
            return ret;
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="index"></param>
        /// <param name="Type">0:向下,1:向上</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool UpdateIndex(int sellerId, string tableName, int index, int Type, out string errMsg)
        {
            SellerColumnConfig currentModel = Dal.GetModel(sellerId, tableName, index, GetConnectionString(sellerId), out errMsg);
            SellerColumnConfig swapModel = new SellerColumnConfig();
            if (Type == 1)//向上更新index
            {
                swapModel = Dal.GetModel(sellerId, tableName, index - 1, GetConnectionString(sellerId), out errMsg);
                currentModel.Index = index - 1;
                swapModel.Index = index;
            }
            else
            {//向下更新index
                swapModel = Dal.GetModel(sellerId, tableName, index + 1, GetConnectionString(sellerId), out errMsg);
                currentModel.Index = index + 1;
                swapModel.Index = index;
            }
            if (Update(currentModel) && Update(swapModel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 返回指定卖家，指定表的列配置记录
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        public static SellerColumnConfig GetModel(int sellerId, string tableName, int index, out string errMsg)
        {
            return Dal.GetModel(sellerId,tableName,index,GetConnectionString(sellerId),out errMsg);
        }
    }
}
