using DapperExtensions;
using Laplace.Framework.Log;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Dal
{
    public class NewProductLaunchDal<T> : BaseDal<NewProductLaunch> where T : class
    {
        /// <summary>
        /// 根据主键id删除
        /// </summary>
        /// <param name="AdsId"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int ID, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.Id, Operator.Eq, ID));
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.SellerId, Operator.Eq, SellerId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 检查名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public NewProductLaunch CheckName(string name, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            NewProductLaunch model = new NewProductLaunch();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.Title, Operator.Eq, name));
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.SellerId, Operator.Eq, SellerId));
                model = base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        public NewProductLaunch GetModel(int ID, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            NewProductLaunch model = new NewProductLaunch();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.Id, Operator.Eq, ID));
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.SellerId, Operator.Eq, SellerId));
                model = base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        public NewProductLaunch GetModel(bool IsUsed, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            NewProductLaunch model = new NewProductLaunch();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.IsUsed, Operator.Eq, IsUsed));
                pdg.Predicates.Add(Predicates.Field<NewProductLaunch>(o => o.SellerId, Operator.Eq, SellerId));
                model = base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }
    }
}
