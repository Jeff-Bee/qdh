using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    public class SmsLogBll
    {
        protected static readonly SmsLogDal<SmsLog> dal = new SmsLogDal<SmsLog>();


        public static bool Insert(SmsLog model)
        {
            string errMsg;
            if (SmsLogRedisBll.Insert(model))
            {
                return dal.Insert(model, Global.ApplicationParms.ConnectionString, out errMsg);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 返回指定手机号指定日志
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public static SmsLog GetLastModel(string mobilePhone, ESmsLogType logType)
        {
            return SmsLogRedisBll.GetLastModel(mobilePhone, logType);
        }
        /// <summary>
        /// 返回指定手机号发送短信条数(1天有效)
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        public static int GetSmsCount(string mobilePhone, ESmsLogType logType)
        {
            return SmsLogRedisBll.GetSmsCount(mobilePhone, logType);
        }


        /// <summary>
        /// 生成手机验证码
        /// </summary>
        /// <returns></returns>
        internal static string CreateSmsVerificationCode()
        {
            var code = new Random().Next(100000, 999999).ToString() + "000000";
            return code.Substring(0, 6);//验证码为6位
        }

        /// <summary>
        /// 校验手机验证码是否正确
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="code"></param>
        /// <param name="logType"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal static bool CheckSmsVerificationCode(string mobilePhone, string code, ESmsLogType logType, out string errMsg)
        {
            errMsg = string.Empty;
#if DEBUG
            if (code == "99999999")
            {
                return true;
            }
#endif
            errMsg = string.Empty;
            

            if (string.IsNullOrEmpty(code) || code.Length != 6)
            {
                errMsg = "验证码错误！";
                return false;
            }
            var log = SmsLogBll.GetLastModel(mobilePhone, logType);
            if (log != null)
            {
                if (log.Config == code.Trim())
                {
                    if ((DateTime.Now - log.SmsTime).TotalMinutes > Global.ApplicationParms.SmsVerificationCodeValidityPeriod)
                    {
                        errMsg = "验证码失效，请重新获取！";
                        return false;
                    }
                }
                else
                {
                    errMsg = "验证码错误！";
                    return false;
                }
            }
            else
            {
                errMsg = "请先获取手机验证码！";
                return false;
            }

            return true;
        }

        #region--短信发送--
        #region--短信发送--

        internal static bool SendSms4RegisterVerificationCode(string mobilePhone, string code, out string smsContent, out string errMsg)
        {
            errMsg = string.Empty;
            const string smsTemplateId = "127529"; //    //短信模板Id
            var timeout = Global.ApplicationParms.SmsVerificationCodeValidityPeriod.ToString();
            string[] smsData = new string[] { code, timeout };//短信内容
            smsContent = string.Format("【趣订货】您的注册验证码为：{0}，请于{1}分钟内提交注册，如非本人操作，请忽略此短信。", code, timeout);
            return SmsLogBll.SendSms(mobilePhone, smsTemplateId, smsData, out errMsg);
        }
        internal static bool SendSms4GetPassword(string mobilePhone, string superPassword, out string smsContent, out string errMsg)
        {
            errMsg = string.Empty;
            const string smsTemplateId = "127027";//     //短信模板Id
            string[] smsData = new string[] { superPassword };//短信内容
                                                              //您当前手机号的登录密码为：{1}，请妥善保存，如非本人操作，请忽略此短信。
            smsContent = string.Format("【趣订货】您当前手机号的登录密码为：{0}，请妥善保存，如非本人操作，请忽略此短信。", superPassword);
            
            return SmsLogBll.SendSms(mobilePhone, smsTemplateId, smsData, out errMsg);
        }
        /// <summary>
        /// 为修改密码发送验证码请求
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="code"></param>
        /// <param name="smsContent"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal static bool SendSms4ModifyPassword(string mobilePhone, string code, out string smsContent, out string errMsg)
        {
            errMsg = string.Empty;
            const string smsTemplateId = "152258";//     //短信模板Id
            string[] smsData = new string[] { code };//短信内容
            smsContent = string.Format("【趣订货】验证码为{0}，您正在修改登录密码，请确认是本人操作。", code);
            return SmsLogBll.SendSms(mobilePhone, smsTemplateId, smsData, out errMsg);
        }

        #endregion-短信发送-

        internal static bool SendSms(string mobilePhone, string smsTemplateId, string[] smsData, out string errMsg)
        {
            var ret = false;
            errMsg = string.Empty;
            //return true;
            CCPRestSDK api = new CCPRestSDK();
            //ip格式如下，不带https://
            bool isInit = api.Init(Global.ApplicationConstants.SmsRestServerIp, Global.ApplicationConstants.SmsRestServerPort);
            api.SetAccount(Laplace.LiteCOS.Global.ApplicationConstants.SmsAccountSid, Laplace.LiteCOS.Global.ApplicationConstants.SmsAuthToken);
            api.SetAppId(Laplace.LiteCOS.Global.ApplicationConstants.SmsAppId);

            try
            {
                if (isInit)
                {
                    Dictionary<string, object> retData = api.SendTemplateSMS(mobilePhone, smsTemplateId, smsData);

                    if (retData.ContainsKey("statusCode") && retData["statusCode"].ToString() == "000000"/*表示请求发送成功*/)
                    {
                        ret = true;
                    }
                    else
                    {
                        errMsg = GetStatusMsg(retData);
                    }
                }
                else
                {
                    errMsg = "初始化失败";
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            Debug.WriteLine(errMsg);
            return ret;
        }
        private static string GetDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += GetDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取状态值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetStatusMsg(Dictionary<string, object> data)
        {
            string ret = string.Empty;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Key == "statusMsg" && item.Value != null)
                {
                    ret = item.Value.ToString();
                    break;
                }
                
            }
            return ret;
        }
        #endregion-短信发送-


    }
}
