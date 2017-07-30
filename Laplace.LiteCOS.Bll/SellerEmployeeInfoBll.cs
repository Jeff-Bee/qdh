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
    /// [卖家员工信息]操作接口
    /// </summary>
    public class SellerEmployeeInfoBll : BaseBll<SellerEmployeeInfo>
    {
        protected static readonly SellerEmployeeInfoDal<SellerEmployeeInfo> Dal = new SellerEmployeeInfoDal<SellerEmployeeInfo>();
        /// <summary>
        /// 添加新员工
        /// </summary>
        /// <param name="model"></param>
        /// <param name="employeeId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(SellerEmployeeInfo model, out int employeeId, out string errMsg)
        {
            employeeId = 0;
            if (CheckCode(model.SellerId, model.EmployeeCode, out errMsg))
            {
                errMsg = string.Format("员工代码:{0}已经存在!", model.EmployeeCode);
                return false;
            }
            employeeId = InsertWithReturnId(model, GetConnectionString(model.SellerId), out errMsg);
            return employeeId > 0;
        }
        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(SellerEmployeeInfo model, out string errMsg)
        {
            return Update(model,GetConnectionString(model.SellerId), out errMsg);
        }
        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="employeeId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int employeeId, out string errMsg)
        {
            errMsg = string.Empty;
            bool res = true;
            SellerEmployeeInfo model =GetList().FirstOrDefault(c=>c.SellerId==sellerId&&c.EmployeeId==employeeId);
            if (model != null)
            {
                model.IsUsed = false;
                model.Status = 100;
                res = res && Update(model);
            }
            return res;
        }
        /// <summary>
        /// 返回指定卖家所有员工列表
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerEmployeeInfo> GetList(int sellerId, out string errMsg)
        {
            return Dal.GetList(sellerId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 检查员工代码是否存在
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CheckCode(int sellerId,string code, out string errMsg)
        {
            return Dal.CheckCode(sellerId, code, GetConnectionString(sellerId), out errMsg);
        }

        /// <summary>
        /// 获取新增code 
        /// </summary>
        /// <param name="SupperId"></param>
        /// <returns></returns>
        public static string GetEmployeeCode(int SellerId)
        {
            string sql = string.Format("select count(EmployeeCode) from SellerEmployeeInfo where SellerId=" + SellerId);
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
