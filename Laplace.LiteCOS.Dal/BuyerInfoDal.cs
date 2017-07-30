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
    public class BuyerInfoDal<T> : BaseDal<BuyerInfo> where T : class
    {
        /// <summary>
        /// 通过手机号注册添加新买家(终端)用户
        /// </summary>
        /// <param name="mobilePhone">注册手机号</param>
        /// <param name="password">密码</param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns>如果成功返回用户编号>0</returns>
        public int Insert(string mobilePhone,string password, string connectionString, out string errMsg)
        {
            var ret = -1;
            errMsg = string.Empty;
            try
            {
                var p = new DynamicParameters();
                p.Add("@MobilePhone", mobilePhone, DbType.String);               /*注册手机号*/
                p.Add("@Password", password, DbType.String);               /*注册手机号*/
                p.Add("@BuyerId", 0, DbType.Int32, ParameterDirection.Output);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute("proc_Insert_BuyerInfo", p, null, null, CommandType.StoredProcedure);
                    var id = p.Get<int>("BuyerId");
                    if (id > 0)
                    {
                        //创建用户成功
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
        public BuyerInfo GetModel(int buyerId, string connectionString, out string errMsg)
        {
            BuyerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerInfo>(o => o.BuyerId, Operator.Eq, buyerId));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        public BuyerInfo GetModelByMobilePhone(string mobilePhone, string connectionString, out string errMsg)
        {
            BuyerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerInfo>(o => o.MobilePhone, Operator.Eq, mobilePhone));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        public BuyerInfo GetModelByLoginName(string loginName, string connectionString, out string errMsg)
        {
            BuyerInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<BuyerInfo>(o => o.LoginName, Operator.Eq, loginName));
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
        #endregion-GetModel-

    }
}
