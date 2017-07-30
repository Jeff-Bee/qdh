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
    public class SellerInfoBll:BaseBll<SellerInfo>
    {
        protected static readonly SellerInfoDal<SellerInfo> Dal = new SellerInfoDal<SellerInfo>();


        /// <summary>
        /// 添加新卖家用户
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="password"></param>
        /// <param name="contactName"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="industry"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static int Insert(string companyName,string password,string contactName, string mobilePhone,int industry, out string errMsg)
        {
            var connectionString = Global.ApplicationParms.ConnectionString;
            //检查企业名称
            if (Dal.CheckModelExist("CompanyName", companyName, connectionString))
            {
                errMsg = string.Format("企业名称:{0} 已经存在!", companyName);
                return -1;
            }

            if (Dal.CheckModelExist("MobilePhone", mobilePhone, connectionString))
            {
                errMsg = string.Format("手机号:{0}已经被注册过!", mobilePhone);
                return -1;
            }
           
            return Dal.Insert(companyName, password, contactName, mobilePhone, industry,Global.ApplicationParms.ConnectionString, out errMsg);
        }
        /// <summary>
        /// 返回指定用户信息
        /// </summary>
        /// <param name="sellerId">用户Id</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static SellerInfo GetModel(int sellerId, out string errMsg)
        {
            return Dal.GetModel(sellerId, Global.ApplicationParms.ConnectionString, out errMsg);
        }

        /// <summary>
        /// 根据手机号返回用户信息
        /// </summary>
        /// <param name="mobilePhone">手机号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static SellerInfo GetModelByMobilePhone(string mobilePhone, out string errMsg)
        {
            return Dal.GetModelByMobilePhone(mobilePhone, Global.ApplicationParms.ConnectionString, out errMsg);
        }

        public static SellerInfo GetModelByLoginName(string loginName, out string errMsg)
        {
            return Dal.GetModelByLoginName(loginName, Global.ApplicationParms.ConnectionString, out errMsg);
        }



        #region--服务（业务）接口--
        /// <summary>
        /// 手机注册：申请验证码
        /// </summary>
        /// <param name="mobilePhone">接收验证码的手机号，同时也是注册号</param>
        /// <param name="errMsg">请求失败时的错误提示</param>
        /// <returns></returns>
        public static bool RequestRegisterVerificationCode(string mobilePhone, out string errMsg)
        {
            errMsg = String.Empty;

            try
            {
                //1.检查手机是否已经注册
                if (SellerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg) != null)
                {
                    errMsg = string.Format("手机号:{0}已经被注册过，请使用其它手机号注册！", mobilePhone);
                    return false;
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    //访问出错
                    return false;
                }
                //2.检查该手机今日接收短信业务条数
                var count = SmsLogBll.GetSmsCount(mobilePhone, ESmsLogType.SellerRegister);
                if (count >= Laplace.LiteCOS.Global.ApplicationParms.SmsMaxCount)
                {
                    errMsg = string.Format("手机号:{0}今日接收验证码过多，请明天再试！", mobilePhone);
                    return false;
                }
                //3.生成手机验证码
                var code = SmsLogBll.CreateSmsVerificationCode();
                //code = "123456";
                string smsContent;
                //4.发送短信,调用短信接口
                //#if !DEBUG
                if (!SmsLogBll.SendSms4RegisterVerificationCode(mobilePhone, code, out smsContent, out errMsg))
                {
                    errMsg = string.Format("发送验证码短信失败，请稍后重试！\r\n错误描述:{0}", errMsg);
                    return false;
                }
                //#endif
                //5.保存日志
                var log = new SmsLog()
                {
                    MobilePhone = mobilePhone,
                    SmsTime = DateTime.Now,
                    SmsContent = smsContent,
                    UserId = 0,
                    Config = code,           //注册码
                    LogType = ESmsLogType.SellerRegister
                };
                SmsLogBll.Insert(log);
            }
            catch (Exception ex)
            {
                errMsg = string.Format("异常:{0}", ex.Message);
                Logger.LogError4Exception(ex, "AppLogger");
            }

            return true;
        }
        /// <summary>
        /// 检查注册验证码是否合法
        /// </summary>
        /// <param name="mobilePhone">注册手机号</param>
        /// <param name="code">手机接收验证码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>true:合法</returns>
        public static bool CheckRegisterVerificationCode(string mobilePhone, string code, out string errMsg)
        {
            errMsg = String.Empty;
            try
            {
                //1.校验注册码是否合法
                return SmsLogBll.CheckSmsVerificationCode(mobilePhone, code, ESmsLogType.SellerRegister, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = "异常:" + ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

        /// <summary>
        /// 请求注册新用户
        /// </summary>
        /// <param name="companyName">企业名称</param>
        /// <param name="password">登录密码（不能为空，）</param>
        /// <param name="contactName">联系人</param>
        /// <param name="mobilePhone">注册手机号</param>
        /// <param name="industry">所属行业编号</param>
        /// <param name="code">注册验证码</param>
        /// <param name="sellerId">如果用户添加成功，返回用户编号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>true:添加新用户成功</returns>
        public static bool RequestRegisterNewUser(string companyName,string password,string contactName, string mobilePhone
            ,int industry, string code, out int sellerId, out string errMsg)
        {
            errMsg = String.Empty;
            sellerId = -1;

            try
            {
                //1.校验注册码是否合法
                if (!CheckRegisterVerificationCode(mobilePhone, code, out errMsg))
                {
                    return false;
                }

                

                //添加新用户
                sellerId = SellerInfoBll.Insert(companyName, password, contactName, mobilePhone, industry, out errMsg);
                return sellerId >0;

            }
            catch (Exception ex)
            {
                errMsg = "执行异常:" + ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }

    
        /// <summary>
        /// 找回密码：验证通过后将密码发送到手机上
        /// </summary>
        /// <param name="mobilePhone">接收密码通知的手机号，同时也是注册号</param>
        /// <param name="errMsg">请求失败时的错误提示</param>
        /// <returns></returns>
        public static bool RequestGetPassword(string mobilePhone, out string errMsg)
        {
            errMsg = String.Empty;

            try
            {
                //1.检查手机是否已经注册
                var customer = SellerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg);
                if (customer == null)
                {
                    errMsg = string.Format("手机号:{0}不存在！", mobilePhone);
                    return false;
                }
                //2.检查该手机今日接收短信业务条数
                var count = SmsLogBll.GetSmsCount(mobilePhone, ESmsLogType.SellerGetPassword);
                if (count >= Laplace.LiteCOS.Global.ApplicationParms.SmsMaxCount)
                {
                    errMsg = string.Format("手机号:{0}今日接收验证码过多，请明天再试！", mobilePhone);
                    return false;
                }
                //4.发送短信
                string smsContent = string.Empty;
                //调用短信接口
                if (!SmsLogBll.SendSms4GetPassword(mobilePhone, customer.Password, out smsContent, out errMsg))
                {
                    errMsg = string.Format("发送密码通知短信失败，请稍后重试！\r\n错误描述:{0}", errMsg);
                    return false;
                }
                //5.保存日志
                var log = new SmsLog()
                {
                    MobilePhone = mobilePhone,
                    SmsTime = DateTime.Now,
                    SmsContent = smsContent,
                    UserId = 0,
                    LogType = ESmsLogType.SellerGetPassword
                };
                SmsLogBll.Insert(log);
            }
            catch (Exception ex)
            {
                errMsg = string.Format("异常:{0}", ex.Message);
                Logger.LogError4Exception(ex, "AppLogger");
            }

            return true;
        }
        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="userName">用户名或者手机号</param>
        /// <param name="password">密码</param>
        /// <param name="userInfo">用户信息</param>
        /// <param name="errMsg">错误代码</param>
        /// <returns></returns>
        public static bool RequestLogin(string userName, string password, out SellerInfo userInfo, out string errMsg)
        {
            errMsg = String.Empty;
            userInfo = null;
            try
            {
                //TODO:判断是否是手机号

                userInfo = GetModelByMobilePhone(userName, out errMsg);
                if (userInfo == null)
                {
                    userInfo = GetModelByLoginName(userName, out errMsg);
                }
                if (userInfo == null)
                {
                    int userId;
                    if (int.TryParse(userName, out userId))
                    {
                        //用户可能直接用Id号登录
                        userInfo = GetModel(userId, out errMsg);
                    }
                }

                if (userInfo == null)
                {
                    errMsg = string.Format("用户名/手机号:{0}不存在！", userName);
                    return false;
                }

                //进行用户在线数量判断
                //if (!CheckUserOnlineCount(userInfo.Id, out errMsg))
                //{
                //    errMsg = "当前账号在线人数超过系统许可，请退出其他账号后再登录！";
                //    return false;
                //}
                if (userInfo.Password == password)
                {
                    return true;
                }
                else
                {
                    errMsg = "密码错误！";
                    return false;
                }



            }
            catch (Exception ex)
            {
                errMsg = "执行异常:" + ex.Message;
                Logger.LogError4Exception(ex);
            }
            return false;
        }
        #endregion-服务（业务）接口-

       

    }
}
