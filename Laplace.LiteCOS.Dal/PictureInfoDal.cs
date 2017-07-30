using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Dal
{
    public class PictureInfoDal<T> : BaseDal<PictureInfo> where T : class
    {
        /// <summary>
        /// 删除指定图片信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int picId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<PictureInfo>(o => o.PicId, Operator.Eq, picId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 返回指定卖家所有图片列表
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<PictureInfo> GetList(int sellerId, string connectionString, out string errMsg)
        {
            List<PictureInfo> list = new List<PictureInfo>();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<PictureInfo>(o => o.SellerId, Operator.Eq, sellerId));
                list = GetList(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        /// <summary>
        /// 根据图片id获取图片信息
        /// </summary>
        /// <param name="PicId">图片id</param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public PictureInfo GetModel(int PicId, string connectionString, out string errMsg)
        {
            PictureInfo list = new PictureInfo();
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<PictureInfo>(o => o.PicId, Operator.Eq, PicId));
                list = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return list;
        }

        public PictureInfo GetPic(int productid, string getConnectionString, out string errMsg)
        {
            string sql = "select a.* from PictureInfo a,productinfo b where a.PicId=b.Picture1 and b.productId=" +
                         productid;

            return GetModel(sql, getConnectionString, out errMsg);
        }
    }
}
