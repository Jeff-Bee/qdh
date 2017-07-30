using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.Framework.Log;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System.Data;

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 【卖家图片信息】操作接口
    /// </summary>
    public class PictureInfoBll : BaseBll<PictureInfo>
    {
        protected static readonly PictureInfoDal<PictureInfo> Dal = new PictureInfoDal<PictureInfo>();
        /// <summary>
        /// 添加新图片
        /// </summary>
        /// <param name="model"></param>
        /// <param name="picId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(PictureInfo model, out int picId, out string errMsg)
        {
            picId = InsertWithReturnId(model, GetConnectionString(model.SellerId), out errMsg);
            return picId > 0;
        }
        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(PictureInfo model, out string errMsg)
        {
            return Update(model, GetConnectionString(model.SellerId), out errMsg);
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="picId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int picId, out string errMsg)
        {
            return Dal.Delete(picId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定卖家所有图片列表
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<PictureInfo> GetList(int sellerId, out string errMsg)
        {
            return Dal.GetList(sellerId, GetConnectionString(sellerId), out errMsg);
        }

        /// <summary>
        /// 根据图片id获取图片信息
        /// </summary>
        /// <param name="PicId">图片id</param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PictureInfo GetModel(int PicId, int SellerId, out string errMsg)
        {
            return Dal.GetModel(PicId,GetConnectionString(SellerId),out errMsg);
        }


        /// <summary>
        /// 根据图片id获取图片信息
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="errMsg"></param>
        /// <param name="productid"></param>
        /// <returns></returns>
        public static PictureInfo GetPic(int productid,int sellerId,  out string errMsg)
        {
            return Dal.GetPic(productid, GetConnectionString(sellerId), out errMsg);
        }
    }
}
