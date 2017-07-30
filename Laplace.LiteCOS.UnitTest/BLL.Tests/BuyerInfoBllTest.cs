using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laplace.LiteCOS.Bll;

namespace Laplace.LiteCOS.UnitTests
{
    [TestClass()]
    public class BuyerInfoBllTest
    {
        private string _errMsg;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
           // BaseRedisBll.Config("www.litecms.cn", 9000, "qdh168");
            //AppConfig.RedisHost = "www.litecms.cn";
        }

        [TestMethod()]
        public void HandleInsertTest()
        {
            int id = 0;
            //Assert.IsTrue(BuyerInfoBll.RequestRegisterVerificationCode("18903838100",out _errMsg));
            Assert.IsTrue(BuyerInfoBll.RequestRegisterNewUser("18903838100","123456", "99999999", out id,out _errMsg));
        }
        [TestMethod()]
        public void GetBuyerProductViewTest()
        {
            var list =BuyerInfoBll.GetBuyerProductView(1000016, 10005);
        }

        
    }
}