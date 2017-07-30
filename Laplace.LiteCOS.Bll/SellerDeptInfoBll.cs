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

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 卖家部门信息操作接口
    /// </summary>
    public class SellerDeptInfoBll : BaseBll<SellerDeptInfo>
    {
        protected static readonly SellerDeptInfoDal<SellerDeptInfo> Dal = new SellerDeptInfoDal<SellerDeptInfo>();
        /// <summary>
        /// 添加新部门
        /// </summary>
        /// <param name="model"></param>
        /// <param name="deptId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(SellerDeptInfo model, out int deptId, out string errMsg)
        {
            deptId = 0;
            if (CheckCode(model.SellerId,model.DeptCode,out errMsg))
            {
                errMsg = string.Format("部门代码:{0}已经存在!", model.DeptCode);
                return false;
            }
            deptId = InsertWithReturnId(model, GetConnectionString(model.SellerId), out errMsg);
            return deptId > 0;
        }
        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(SellerDeptInfo model, out string errMsg)
        {
            return Update(model,GetConnectionString(model.SellerId), out errMsg);
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="deptId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int deptId, out string errMsg)
        {
            return Dal.Delete(deptId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定卖家所有部门列表
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerDeptInfo> GetList(int sellerId, out string errMsg)
        {
            return Dal.GetList(sellerId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 检查部门代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId, string code, out string errMsg, int DeptId=0)
        {
            if (DeptId != 0)
            {
                if (Dal.CheckCode(sellerId, DeptId, code, GetConnectionString(sellerId), out errMsg))
                {
                    return false;
                }
                else {
                    return true;
                }
            }
            else
            {
                return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
            }
        }

        public static string GetDepartmentCode(int SellerId)
        {
            string sql = string.Format("select count(DeptCode) from SellerDeptInfo where SellerId=" + SellerId);
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


    }
}
