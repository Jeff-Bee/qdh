using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.DAL;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Dal
{
    public class SmsLogDal<T> : BaseDal<SmsLog> where T : class
    {
        /// <summary>
        /// 返回指定手机号，从指定时间内发送短信条数
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public int GetSmsCount(string mobilePhone, DateTime dtStartTime, DateTime dtEndTime, string connectionString)
        {
            var sql = new StringBuilder("Select Count(*) From SmsLog");
            sql.AppendFormat(" Where MobilePhone='{0}'", mobilePhone);
            sql.AppendFormat(" And SmsTime Between '{0}' And '{1}'", dtStartTime.ToString("G"), dtEndTime.ToString("G"));
            var ret = SqlHelper.ExecuteScalar2Int(sql.ToString(), connectionString);
            if (ret.HasValue)
            {
                return ret.Value;
            }
            else
            {
                return 0;
            }
        }
    }
}
