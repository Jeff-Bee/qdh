using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.Bll;

namespace Laplace.LiteCOS.UnitTests
{
    [TestClass()]
    public class BaseBllTest
    {
        private string _errMsg;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
           // BaseRedisBll.Config("www.litecms.cn", 9000, "qdh168");
            //AppConfig.RedisHost = "www.litecms.cn";
        }

        [TestMethod()]
        public void GetPageDataDataTableTest()
        {
            PageCriteria page= new PageCriteria();
            page.TableName = "ProductInfo p left join  ProductClassInfo c on p.ClassId=c.ClassId"
                + " left join  SellerStockPile s on s.ProductId = p.ProductId"
                + " left join  PictureInfo i on i.PicId = p.Picture1";
            page.Fields = "p.ProductId,p.ProductCode,p.ProductFullName,c.Name as ClassName,i.Resource as picName,s.Quantity,p.ProductSpec,p.Price1,i.Format,p.SellerId";
            page.Condition = "";
            page.PrimaryKey = "p.ProductId";
            page.PageSize = 10;
            page.CurrentPage = 1;
            var ret = BaseBll<object>.GetPageDataDataTable(page, 0, out _errMsg);
            Assert.IsTrue(ret!=null);

        }
        [TestMethod()]
        public void HandleDeleteTest()
        {
           
        }
    }
}