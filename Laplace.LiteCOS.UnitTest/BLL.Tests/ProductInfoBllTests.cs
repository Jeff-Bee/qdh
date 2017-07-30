using Microsoft.VisualStudio.TestTools.UnitTesting;
using Laplace.LiteCOS.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll.Tests
{
    [TestClass()]
    public class ProductInfoBllTests
    {
        private string _errMsg;
        [TestMethod()]
        public void GetProductListTest()
        {
            var psg = ProductInfoBll.GetProductList(1, 10, "", "", 1, 1, out _errMsg);
            Assert.Fail();
        }
    }
}