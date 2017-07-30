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
    public class SellerDeliveryCarInfoDal<T> : BaseDal<SellerDeliveryCarInfo> where T : class
    {
        /// <summary>
        /// 返回指定设备信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public SellerDeliveryCarInfo GetModel(string Code, int SellerId, string connectionString, out string errMsg,int CarId=0)
        {
            SellerDeliveryCarInfo model = null;
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                if (Code != "")
                {
                    pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.Code, Operator.Eq, Code));
                }
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.SellerId, Operator.Eq, SellerId));
                if (CarId != 0)
                {
                    pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.CarId, Operator.Eq, CarId));

                }
                model = GetModel(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return model;
        }

        /// <summary>
        /// 判断客户code是否重复
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckCode(int sellerId, string code, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.Code, Operator.Eq, code));
                return base.GetCount(pdg, connectionString, out errMsg) > 0;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 检查编辑时是否修改code
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="code"></param>
        /// <param name="CustomerId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool CheckCode(int sellerId, string code, int CarId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.SellerId, Operator.Eq, sellerId));
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.Code, Operator.Eq, code));
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.CarId, Operator.Eq, CarId));
                return base.GetModel(pdg, connectionString, out errMsg) == null;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Delete(int CarId, string connectionString, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                PredicateGroup pdg = new PredicateGroup();
                pdg.Predicates = new List<IPredicate>();
                pdg.Predicates.Add(Predicates.Field<SellerDeliveryCarInfo>(o => o.CarId, Operator.Eq, CarId));
                return base.Delete(pdg, connectionString, out errMsg);
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
    }
}
