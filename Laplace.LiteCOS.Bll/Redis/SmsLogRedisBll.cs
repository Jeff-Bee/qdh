using Laplace.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laplace.Framework.Redis;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    public class SmsLogRedisBll : BaseRedisBll
    {
        private static int databaseIndex = 1;
        private static string tableName = "SmsLog";
        private static BaseRedisDal<SmsLog> Dal
        {
            get { return _smsLogDal.Value; }
        }
        private static string GetKeyName(SmsLog model)
        {
            return string.Format("{0}_{1}_{2}", tableName, model.MobilePhone,model.SmsTime.ToString());
        }
        /// <summary>
        /// 添加短信日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Insert(SmsLog model)
        {
            return Dal.Insert(GetKeyName(model), model, databaseIndex,3600 * 24/*记录保存1天*/);
        }
        public static int GetSmsCount(string mobilePhone, ESmsLogType logType)
        {
            return GetList(mobilePhone, logType).Count();
        }
        /// <summary>
        /// 返回指定手机号最新的短信日志
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        public static SmsLog GetLastModel(string mobilePhone, ESmsLogType logType)
        {
            return GetList(mobilePhone, logType).OrderByDescending(o => o.SmsTime).FirstOrDefault();
        }
        /// <summary>
        /// 返回Redis中全部计划任务数据记录
        /// </summary>
        /// <returns></returns>
        public static List<SmsLog> GetList()
        {
            return Dal.GetList(tableName, databaseIndex);
        }

        /// <summary>
        /// 返回Redis中指定手机号的短信记录
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public static List<SmsLog> GetList(string mobilePhone, ESmsLogType logType)
        {
            return GetList().Where(o => o.MobilePhone == mobilePhone && o.LogType == logType).ToList();
        }

      
     
    }
}
