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
    public class SaleAdsDal<T> : BaseDal<SaleAds> where T : class
    {
        /// <summary>
        /// 根据主键id删除
        /// </summary>
        /// <param name="AdsId"></param>
        /// <param name="SellerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int AdsId,int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.AdsId, Operator.Eq, AdsId));
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.SellerId, Operator.Eq, SellerId));
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
        public SaleAds CheckName(string name,int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            SaleAds model = new SaleAds();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.Title, Operator.Eq, name));
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.SellerId, Operator.Eq, SellerId));
                model= base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        public SaleAds GetModel(int AdsId, int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            SaleAds model = new SaleAds();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.AdsId, Operator.Eq, AdsId));
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.SellerId, Operator.Eq, SellerId));
                model = base.GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        public SaleAds GetModel(bool IsUsed,int SellerId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            SaleAds model = new SaleAds();
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.IsUsed, Operator.Eq, IsUsed));
                pdg.Predicates.Add(Predicates.Field<SaleAds>(o => o.SellerId, Operator.Eq, SellerId));
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
