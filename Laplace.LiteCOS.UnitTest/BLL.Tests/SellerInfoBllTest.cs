using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laplace.LiteCOS.Bll;

namespace Laplace.LiteCOS.UnitTests
{
    [TestClass()]
    public class SellerInfoBllTest
    {
        private string _errMsg;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            BaseRedisBll.Config("www.litecms.cn", 9000, "qdh168");
            //AppConfig.RedisHost = "www.litecms.cn";
        }
        /// <summary>
        /// 请求新用户注册验证码
        /// </summary>

        [TestMethod()]
        public void RequestRegisterVerificationCodeTest()
        {
            var phone = "18903838100";
            Assert.IsTrue(SellerInfoBll.RequestRegisterVerificationCode(phone, out _errMsg));
        }
        /// <summary>
        /// 注册新用户
        /// </summary>
        [TestMethod()]
        public void RequestRegisterNewUserTest()
        {
            var phone = "18903838100";
            var code = "99999999";
            code = "942684";
            int id = 0;
            Assert.IsTrue(SellerInfoBll.RequestRegisterNewUser("阿里巴巴", "123456", "龚俊", phone, 1001, code, out id, out _errMsg));
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        [TestMethod()]
        public void RequestGetPasswordTest()
        {
            var phone = "18903838100";
            Assert.IsTrue(SellerInfoBll.RequestGetPassword(phone, out _errMsg));
        }
        
        [TestMethod()]
        public void HandleMyTest()
        {
            Assert.IsTrue(SellerColumnConfigBll.GetList(1001, "ProductInfo", out _errMsg).Any());
        }

       
    }
}