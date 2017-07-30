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
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Dal
{
    public class SellerInfoDal<T> : BaseDal<SellerInfo> where T : class
    {
        /// <summary>
        /// 通过手机号注册添加新(终端)用户
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="password"></param>
        /// <param name="contactName"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="industry"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Insert(string companyName, string password, string contactName, string mobilePhone, int industry, string connectionString, out string errMsg)
        {
            var ret = -1;
            errMsg = string.Empty;
            try
            {
                var p = new DynamicParameters();
                p.Add("@CompanyName", companyName, DbType.String);              /*企业名称*/
                p.Add("@Password", password, DbType.String);                    /*密码*/
                p.Add("@ContactName", contactName, DbType.String);              /*联系人*/
                p.Add("@MobilePhone", mobilePhone, DbType.String);              /*注册手机号*/
                p.Add("@IndustryId", industry, DbType.Int32);                     /*所属行业*/
                p.Add("@SellerId", 0, DbType.Int32, ParameterDirection.Output); /*返回卖家编号*/
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute("proc_Insert_SellerInfo", p, null, null, CommandType.StoredProcedure);
                    var id = p.Get<int>("SellerId");
                    if (id > 0)
                    {
                        //创建用户失败
                        ret = id;
                    }
                    else
                    {
                        errMsg = "操作异常";
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return ret;
        }
        #region--GetModel--
        public SellerInfo GetModel(int sellerId, string connectionString, out string errMsg)
        {
            SellerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerInfo>(o => o.SellerId, Operator.Eq, sellerId));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        public SellerInfo GetModelByMobilePhone(string mobilePhone, string connectionString, out string errMsg)
        {
            SellerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerInfo>(o => o.MobilePhone, Operator.Eq, mobilePhone));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        public SellerInfo GetModelByLoginName(string loginName, string connectionString, out string errMsg)
        {
            SellerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerInfo>(o => o.LoginName, Operator.Eq, loginName));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        #endregion-GetModel-

        public bool CheckModelExist(string paramName, string value, string connectionString)
        {
            return base.CheckModelExist("SellerInfo", paramName, value, connectionString);
        }
    }
}
