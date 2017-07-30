using System;
using System.Collections.Generic;
using System.Data;
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
    public class BuyerInfoBll:BaseBll<BuyerInfo>
    {
        protected static readonly BuyerInfoDal<BuyerInfo> Dal = new BuyerInfoDal<BuyerInfo>();

        /// <summary>
        ///  通过手机号注册添加新卖家用户
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        //public static BuyerInfo Insert(string mobilePhone,string password, out string errMsg)
        //{
        //    BuyerInfo model;
        //    Dal.Insert(mobilePhone, out model, Global.ApplicationParms.ConnectionString, out errMsg);
        //    if (model != null)
        //    {
        //        //ScheduleTaskBll.Insert4UserUpdate(model.Id, ETaskCmd.AddUser, out errMsg);
        //    }
        //    return model;
        //}

        /// <summary>
        /// 返回指定用户信息
        /// </summary>
        /// <param name="buyerId">用户Id</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static BuyerInfo GetModel(int buyerId, out string errMsg)
        {
            return Dal.GetModel(buyerId, Global.ApplicationParms.ConnectionString, out errMsg);
        }

        /// <summary>
        /// 根据手机号返回用户信息
        /// </summary>
        /// <param name="mobilePhone">手机号</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static BuyerInfo GetModelByMobilePhone(string mobilePhone, out string errMsg)
        {
            return Dal.GetModelByMobilePhone(mobilePhone, Global.ApplicationParms.ConnectionString, out errMsg);
        }

        public static BuyerInfo GetModelByLoginName(string loginName, out string errMsg)
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
                if (BuyerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg) != null)
                {
                    errMsg = string.Format("手机号:{0}已经被注册过，请使用其它手机号注册！", mobilePhone);
                    return false;
                }
                //2.检查该手机今日接收短信业务条数
                var count = SmsLogBll.GetSmsCount(mobilePhone, ESmsLogType.BuyerRegister);
                if (count >= Laplace.LiteCOS.Global.ApplicationParms.SmsMaxCount)
                {
                    errMsg = string.Format("手机号:{0}今日接收验证码过多，请明天再试！", mobilePhone);
                    return false;
                }
                //3.生成手机验证码
                var code = SmsLogBll.CreateSmsVerificationCode();
                //code = "123456";
                string smsContent = string.Empty;
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
                    LogType = ESmsLogType.BuyerRegister
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
        /// 请求注册新用户（买家用户）
        /// </summary>
        /// <param name="mobilePhone">注册手机号</param>
        /// <param name="password">登录密码（不能为空，）</param>
        /// <param name="code">注册验证码</param>
        /// <param name="buyerId">如果用户添加成功，返回用户编号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>true:添加新用户成功</returns>
        public static bool RequestRegisterNewUser(string mobilePhone,string password, string code, out int buyerId, out string errMsg)
        {
            errMsg = String.Empty;
            buyerId = -1;

            try
            {
                //1.校验注册码是否合法
                if (!SmsLogBll.CheckSmsVerificationCode(mobilePhone, code, ESmsLogType.BuyerRegister, out errMsg))
                {
                    return false;
                }

                //添加新用户
                buyerId=Dal.Insert(mobilePhone, password, Global.ApplicationParms.ConnectionString, out errMsg);
                return buyerId > 0;

            }
            catch (Exception ex)
            {
                errMsg = "执行异常:" + ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
        }
        /// <summary>
        /// 请求修改密码
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool RequestChangePassword(string mobilePhone, string password, string code, out string errMsg)
        {
            errMsg = String.Empty;
            try
            {
                if(string.IsNullOrEmpty(password))
                {
                    errMsg = "密码不能为空!";
                    return false;
                }
                //1.校验注册码是否合法
                if (!SmsLogBll.CheckSmsVerificationCode(mobilePhone, code, ESmsLogType.BuyerChangePassword, out errMsg))
                {
                    return false;
                }

                var buyer = BuyerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg);
                if (buyer == null)
                {
                    errMsg = string.Format("手机号:{0}不存在！", mobilePhone);
                    return false;
                }
                buyer.Password = password;
                buyer.LDate = DateTime.Now;
                buyer.LMan = buyer.BuyerId;
                return Dal.Update(buyer, Global.ApplicationParms.ConnectionString);

            }
            catch (Exception ex)
            {
                errMsg = "执行异常:" + ex.Message;
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
           
        }
        /// <summary>
        /// 找回密码：申请验证码
        /// </summary>
        /// <param name="mobilePhone">接收验证码的手机号，同时也是注册号</param>
        /// <param name="errMsg">请求失败时的错误提示</param>
        /// <returns></returns>
        public static bool RequestGetPasswordVerificationCode(string mobilePhone, out string errMsg)
        {
            errMsg = String.Empty;

            try
            {
                //1.检查手机是否已经注册
                var customer = BuyerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg);
                if (customer == null)
                {
                    errMsg = string.Format("手机号:{0}不存在！", mobilePhone);
                    return false;
                }
                //2.检查该手机今日接收短信业务条数
                var count = SmsLogBll.GetSmsCount(mobilePhone, ESmsLogType.BuyerGetPassword);
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
                    LogType = ESmsLogType.BuyerGetPassword
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
        /// 修改密码：申请验证码
        /// </summary>
        /// <param name="mobilePhone">接收验证码的手机号，同时也是注册号</param>
        /// <param name="errMsg">请求失败时的错误提示</param>
        /// <returns></returns>
        public static bool RequestChangePasswordVerificationCode(string mobilePhone, out string errMsg)
        {
            errMsg = String.Empty;

            try
            {
                //1.检查手机是否已经注册
                var customer = BuyerInfoBll.GetModelByMobilePhone(mobilePhone, out errMsg);
                if (customer == null)
                {
                    errMsg = string.Format("手机号:{0}不存在！", mobilePhone);
                    return false;
                }
                //2.检查该手机今日接收短信业务条数
                var count = SmsLogBll.GetSmsCount(mobilePhone, ESmsLogType.BuyerGetPassword);
                if (count >= Laplace.LiteCOS.Global.ApplicationParms.SmsMaxCount)
                {
                    errMsg = string.Format("手机号:{0}今日接收验证码过多，请明天再试！", mobilePhone);
                    return false;
                }
                //3.生成手机验证码
                var code = SmsLogBll.CreateSmsVerificationCode();
                //4.发送短信
                string smsContent = string.Empty;
                //调用短信接口
                if (!SmsLogBll.SendSms4ModifyPassword(mobilePhone, code, out smsContent, out errMsg))
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
                    Config = code,           //注册码
                    LogType = ESmsLogType.BuyerChangePassword
                };
                return SmsLogBll.Insert(log);
            }
            catch (Exception ex)
            {
                errMsg = string.Format("异常:{0}", ex.Message);
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return false;
            
        }
        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="userName">用户名或者手机号</param>
        /// <param name="password">密码</param>
        /// <param name="userInfo">用户信息</param>
        /// <param name="errMsg">错误代码</param>
        /// <returns></returns>
        public static bool RequestLogin(string userName, string password, out BuyerInfo userInfo, out string errMsg)
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
                //if (userInfo == null)
                //{
                //    int userId;
                //    if (int.TryParse(userName, out userId))
                //    {
                //        //用户可能直接用Id号登录
                //        userInfo = GetModel(userId, out errMsg);
                //    }
                //}

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

        /// <summary>
        /// 返回指定买家关联卖家的Id列表
        /// gongjun@2016-12-05
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerInfo> GetSellerIdList(int buyerId, out string errMsg)
        {
            var sql = string.Format("Select * From [SellerCustomerInfo] Where [BuyerId]={0}",buyerId);
            return BaseBll<SellerInfo>.GetList(sql, GetConnectionString(buyerId), out errMsg);
        }



        /// <summary>
        /// 返回指定买家关联卖家的Id列表
        /// xieguanheng@2016-12-29
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static List<SellerInfo> GetSellerList(int buyerId, out string errMsg)
        {
            var sql = string.Format("Select a.* From [SellerInfo] a,  [SellerCustomerInfo] b    Where a.SellerId=b.SellerId and [BuyerId]={0}", buyerId);
            return BaseBll<SellerInfo>.GetList(sql, GetConnectionString(buyerId), out errMsg);
        }






        /// <summary>
        /// 根据买家Id，返回指定卖家当前其上线商品类别集合
        /// gongjun@2016-12-05
        /// </summary>
        /// <param name="buyerId">买家Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static  List<ProductClassInfo> GetProductClassInfo(int buyerId,int sellerId,  out string errMsg)
        {
            return ProductClassInfoBll.GetListByBuyerId(sellerId, buyerId, out errMsg);
        }

        /// <summary>
        /// 根据买家Id，商品类别，返回指定卖家上线商品信息列表
        /// BuyerProductView不是表记录，而是业务封装视图。
        /// gongjun@2016-12-05
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static List<BuyerProductView> GetBuyerProductView(int buyerId, int sellerId, int classId)
        {
            //xieguanheng 增加字段 SellerId 的返回  20161231
            var list = new List<BuyerProductView>();
            var sql = "Select T1.[SellerId],T1.[ClassId], T1.ProductId,[ProductCode],[ProductFullName],[ProductShortName],ProductUnit"
                + ",[Picture1],[Picture2],[Picture3],[Picture4],[Picture5],[Picture6]"
                + ",T2.[Price1],T2.[MinOrder1],T2.[MaxOrder1],T2.IsNew,T2.IsPromotion,T3.[ProductQuantity]"
                + " From[dbo].[ProductInfo] T1"
                + " Left Join [SellerProductOnlineCustomerStrategy] T2"
                + " On T1.[ProductId]= T2.[ProductId]"
                 + " Inner Join [BuyerShoppingCart] T3"
                + " On T1.[ProductId]=T3.[ProductId]"
                + string.Format("Where T1.[SellerId]= {0} And T1.[ClassId]= {1} And T2.BuyerId = {2}"
                        ,sellerId,classId,buyerId)
                + " And [SaleStartDate]<GETDATE() And[SaleEndDate]>GETDATE()";
            var dt = BaseBll<object>.ExecuteDataTable(sql, GetConnectionString(sellerId));
            if (dt.Rows.Count == 0)
            {
                sql = "Select T1.[SellerId],T1.[ClassId], T1.ProductId,[ProductCode],[ProductFullName],[ProductShortName],ProductUnit"
                + ",[Picture1],[Picture2],[Picture3],[Picture4],[Picture5],[Picture6]"
                + ",T2.[Price1],T2.[MinOrder1],T2.[MaxOrder1],T2.IsNew,T2.IsPromotion,T3.[ProductQuantity]"
                + " From[dbo].[ProductInfo] T1"
                + " Left Join [SellerProductOnline] T2"
                + " On T1.[ProductId]= T2.[ProductId]"
                 + " Inner Join [BuyerShoppingCart] T3"
                + " On T1.[ProductId]=T3.[ProductId]"
                + string.Format("Where T1.[SellerId]= {0} And T1.[ClassId]= {1} And T2.SaleState=1"
                        , sellerId, classId)
                + " And [SaleStartDate]<GETDATE() And[SaleEndDate]>GETDATE()";
                dt = BaseBll<object>.ExecuteDataTable(sql, GetConnectionString(sellerId));
            }
            foreach (DataRow row in dt.Rows)
            {
                var view = new BuyerProductView();
                view.SellerId = Convert.ToInt32(row["SellerId"]);
                view.ClassId = Convert.ToInt32(row["ClassId"]);
                view.ProductId = Convert.ToInt32(row["ProductId"]);
                view.ProductCode = row["ProductCode"].ToString();
                view.ProductFullName = row["ProductFullName"].ToString();
                view.ProductShortName = row["ProductShortName"].ToString();
                view.ProductUnit = row["ProductUnit"].ToString();
                view.Picture1 = Convert.ToInt32(row["Picture1"]);
                view.Picture2 = Convert.ToInt32(row["Picture2"]);
                view.Picture3 = Convert.ToInt32(row["Picture3"]);
                view.Picture4 = Convert.ToInt32(row["Picture4"]);
                view.Picture5 = Convert.ToInt32(row["Picture5"]);
                view.Price1 = Convert.ToSingle(row["Price1"]);
                view.MinOrder1 = Convert.ToInt16(row["MinOrder1"]);
                view.MaxOrder1 = Convert.ToInt32(row["MaxOrder1"]);
                view.IsNew = Convert.ToBoolean(row["IsNew"]);
                view.IsPromotion = Convert.ToBoolean(row["IsPromotion"]);
                try
                {
                    view.ProductQuantity = Convert.ToInt32(row["ProductQuantity"]);
                }
                catch (Exception ex)
                {
                    view.ProductQuantity = 0;
                }
                list.Add(view);
            }
            return list;
        }



        /// <summary>
        /// 根据买家Id，商品类别，返回指定卖家上线商品信息列表
        /// BuyerProductView不是表记录，而是业务封装视图。
        /// gongjun@2016-12-05
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="sellerId"></param>        
        /// <returns></returns>
        public static List<BuyerProductView> GetBuyerProductView(int buyerId, int sellerId)
        {
            //xieguanheng 增加字段 SellerId 的返回  20161231
            var list = new List<BuyerProductView>();
            var sql = "Select T1.[SellerId],T1.[ClassId], T1.ProductId,[ProductCode],[ProductFullName],[ProductShortName],ProductUnit"
                + ",[Picture1],[Picture2],[Picture3],[Picture4],[Picture5],[Picture6]"
                + ",T2.[Price1],T2.[MinOrder1],T2.[MaxOrder1],T2.IsNew,T2.IsPromotion,T3.[ProductQuantity]"
                + " From [dbo].[ProductInfo] T1"
                + " Left Join [SellerProductOnlineCustomerStrategy] T2"
                + " On T1.[ProductId]= T2.[ProductId]"
                 + " Inner Join [BuyerShoppingCart] T3"
                + " On T1.[ProductId]=T3.[ProductId]"
                + string.Format("Where T1.[SellerId]= {0}  And T2.BuyerId = {1}"
                        , sellerId,  buyerId)
                + " And [SaleStartDate]<GETDATE() And[SaleEndDate]>GETDATE()";
            var dt = BaseBll<object>.ExecuteDataTable(sql, GetConnectionString(sellerId));
            if (dt.Rows.Count == 0)
            {
                sql = "Select T1.[SellerId],T1.[ClassId], T1.ProductId,[ProductCode],[ProductFullName],[ProductShortName],ProductUnit"
                + ",[Picture1],[Picture2],[Picture3],[Picture4],[Picture5],[Picture6]"
                + ",T2.[Price1],T2.[MinOrder1],T2.[MaxOrder1],T2.IsNew,T2.IsPromotion,T3.[ProductQuantity]"
                + " From [dbo].[ProductInfo] T1"
                + " Left Join [SellerProductOnline] T2"
                + " On T1.[ProductId]= T2.[ProductId]"
                 + " Inner Join [BuyerShoppingCart] T3"
                + " On T1.[ProductId]=T3.[ProductId]"
                + string.Format("Where T1.[SellerId]= {0} And T2.SaleState=1"
                        , sellerId)
                + " And [SaleStartDate]<GETDATE() And[SaleEndDate]>GETDATE()";
                dt = BaseBll<object>.ExecuteDataTable(sql, GetConnectionString(sellerId));
            }
            foreach (DataRow row in dt.Rows)
            {
                var view = new BuyerProductView();
                view.SellerId = Convert.ToInt32(row["SellerId"]);
                view.ClassId = Convert.ToInt32(row["ClassId"]);
                view.ProductId = Convert.ToInt32(row["ProductId"]);
                view.ProductCode = row["ProductCode"].ToString();
                view.ProductFullName = row["ProductFullName"].ToString();
                view.ProductShortName = row["ProductShortName"].ToString();
                view.ProductUnit = row["ProductUnit"].ToString();
                view.Picture1 = Convert.ToInt32(row["Picture1"]);
                view.Picture2 = Convert.ToInt32(row["Picture2"]);
                view.Picture3 = Convert.ToInt32(row["Picture3"]);
                view.Picture4 = Convert.ToInt32(row["Picture4"]);
                view.Picture5 = Convert.ToInt32(row["Picture5"]);
                view.Price1 = Convert.ToSingle(row["Price1"]);
                view.MinOrder1 = Convert.ToInt16(row["MinOrder1"]);
                view.MaxOrder1 = Convert.ToInt32(row["MaxOrder1"]);
                view.IsNew = Convert.ToBoolean(row["IsNew"]);
                view.IsPromotion = Convert.ToBoolean(row["IsPromotion"]);
                try
                {
                    view.ProductQuantity = Convert.ToInt32(row["ProductQuantity"]);
                }
                catch (Exception ex)
                {
                    view.ProductQuantity = 0;
                }
                list.Add(view);
            }
            return list;
        }



        public static List<BuyerShoppingCartViewModel> GetBuyerShoppingCartViewModels(int buyerId,out string msg)
        {
            string sql = "select c.*,s.CompanyName as SellerName,p.ProductFullName as ProductName,p.ProductUnit as ProductUnit,p.Price1 as Price from BuyerShoppingCart c ,SellerInfo s,ProductInfo p where c.SellerId=s.SellerId and c.ProductId =p.ProductId and c.BuyerId =" + buyerId;
            return BaseBll<BuyerShoppingCartViewModel>.GetList(sql,GetConnectionString(buyerId), out msg).ToList();
        }




        

    }


  
}
