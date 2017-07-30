using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laplace.Framework.Helper;
using Laplace.Framework.Log;
using Laplace.Framework.Orm;
using Laplace.LiteCOS.Common.Enum;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCOS.Bll
{
    /// <summary>
    /// 商品信息业务接口
    /// </summary>
    public class ProductInfoBll : BaseBll<ProductInfo>
    {
        protected static readonly ProductInfoDal<ProductInfo> Dal = new ProductInfoDal<ProductInfo>();
        /// <summary>
        /// 插入商品信息Model，返回新增商品编号
        /// </summary>
        /// <param name="model"></param>
        /// <param name="productId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert(ProductInfo model, out int productId, out string errMsg)
        {
            productId = BaseBll<ProductInfo>.InsertWithReturnId(model, out errMsg);
            return productId > 0;
        }

        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(ProductInfo model, out string errMsg)
        {
            return BaseBll<ProductInfo>.Update(model.SellerId, model, out errMsg);
        }
        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete(int sellerId, int productId, out string errMsg)
        {
            ProductInfo model = GetModel(sellerId, productId, out errMsg);
            if (model != null)
            {
                model.Status = 100;
                Update(model);
                return true;
            }
            else
            {
                errMsg = "获取不到商品";
                return false;
            }
        }
        /// <summary>
        /// 返回指定商品信息
        /// </summary>
        /// <param name="sellerId">卖家编号</param>
        /// <param name="productId">商品编号</param>
        /// <param name="errMsg">如果执行失败，错误信息</param>
        /// <returns></returns>
        public static ProductInfo GetModel(int sellerId, int productId, out string errMsg)
        {
            return Dal.GetModel(productId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 根据商品名称获取model
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="sellerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static ProductInfo GetModel(string productName, int sellerId, out string errMsg)
        {
            return Dal.GetModel(productName, sellerId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定商品信息,根据code获取model
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static ProductInfo GetModelByCode(string productCode, int SellerId, out string errMsg)
        {
            return Dal.GetModelByCode(productCode, SellerId, GetConnectionString(SellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定商品类型的商品列表
        /// </summary>
        /// <param name="sellerId">商家编号</param>
        /// <param name="productClassId">商品分类编号，如果为0：返回所有商品列表</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static IList<ProductInfo> GetList(int sellerId, int productClassId, out string errMsg)
        {
            return Dal.GetList(sellerId, productClassId, GetConnectionString(sellerId), out errMsg);
        }
        /// <summary>
        /// 返回指定商品类型的商品列表（分页查询）
        /// </summary>
        /// <param name="sellerId">商家编号</param>
        /// <param name="productClassId">商品分类编号，如果为0：返回所有商品列表</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView<ProductInfo> GetList(int sellerId, int productClassId, int page, int pageSize, out string errMsg)
        {
            return Dal.GetList(sellerId, productClassId, page, pageSize, GetConnectionString(sellerId), out errMsg);

        }

        public static string GetProductCode(int SellerId)
        {
            string sql = string.Format("select count(ProductId) from ProductInfo where SellerId=" + SellerId);
            object res = Dal.ExecuteScalar(sql, GetConnectionString(SellerId));
            if (res != null)
            {
                return (Convert.ToInt32(res) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return "0001";
            }
        }

        /// <summary>
        /// 分页显示获取商品list
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetProductList(int page, int pagesize, string sort, string content, int sType, int customerId, out string errMsg, int status = -1, int pType = -1)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " ProductInfo p left join ProductClassInfo c on p.ClassId=c.ClassId left join PictureInfo i1 on i1.PicId=p.Picture1 left join PictureInfo i2 on i2.PicId=p.Picture2 left join PictureInfo i3 on i3.PicId=p.Picture3 ";
            criteria.Condition = " 1=1 and p.Status!=100 ";
            criteria.PrimaryKey = "p.ProductId";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            if (!string.IsNullOrEmpty(content))
            {
                #region 按搜索类型

                switch (sType)
                {
                    case 0:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 1:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 2:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 3:
                        criteria.Condition += string.Format(@" and p.ProductCode like '%{0}%' ", content);
                        break;
                    case 4:
                        criteria.Condition += string.Format(@" and p.ProductShortName like '%{0}%' ", content);
                        break;
                    case 5:
                        criteria.Condition += string.Format(@" and p.ProductSpec like '%{0}%' ", content);
                        break;
                    case 6:
                        criteria.Condition += string.Format(@" and p.ProductModel like '%{0}%' ", content);
                        break;
                    case 7:
                        criteria.Condition += string.Format(@" and p.Place like '%{0}%' ", content);
                        break;
                    case 8:
                        criteria.Condition += string.Format(@" and p.BarCode like '%{0}%' ", content);
                        break;
                    case 9:
                        criteria.Condition += string.Format(@" and p.PinyinCode like '%{0}%' ", content);
                        break;
                }

                #endregion
            }
            if (status != -1)
            {
                criteria.Condition += " and p.ProductState=" + status;
            }
            if (pType != -1)
            {
                criteria.Condition += " and p.ClassId=" + pType;
            }
            criteria.Condition += " and p.SellerId=" + customerId + " ";
            criteria.Fields = string.Format(@" c.Name,i1.Resource as pic1,i2.Resource as pic2,i3.Resource as pic3,p.ProductId,p.ClassId,p.ProductCode,p.ProductFullName,p.ProductShortName,
            p.PinyinCode,p.Package,p.ProductSpec,i1.Format format1,i2.Format format2,i3.Format format3,p.ProductModel,p.ProductUnit,p.Weight,
            p.BarCode,p.Place,p.ProductState,p.Price1,p.Price2,p.Price3,p.Price4,p.Price5,p.Price6,p.Price7,p.notes,
            p.Picture1,p.Picture2,p.Picture3,p.Summary,c.ClassId as ParentId ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(customerId), out errMsg);
        }


        /// <summary>
        /// 多包含商品数量
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="content"></param>
        /// <param name="sType"></param>
        /// <param name="customerId"></param>
        /// <param name="errMsg"></param>
        /// <param name="status"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public static PageDataView GetProductsList(int page, int pagesize, string sort, string content, int sType, int customerId, out string errMsg, int status = 0, int pType = 10001)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " ProductInfo p,ProductClassInfo c,PictureInfo i1,PictureInfo i2,PictureInfo i3,SellerStockPile s ";
            criteria.Condition = " 1=1 ";
            criteria.PrimaryKey = "ProductId";
            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            if (!string.IsNullOrEmpty(content))
            {
                #region 按搜索类型

                switch (sType)
                {
                    case 0:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 1:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 2:
                        criteria.Condition += string.Format(@" and p.ProductFullName like '%{0}%' ", content);
                        break;
                    case 3:
                        criteria.Condition += string.Format(@" and p.ProductCode like '%{0}%' ", content);
                        break;
                    case 4:
                        criteria.Condition += string.Format(@" and p.ProductShortName like '%{0}%' ", content);
                        break;
                    case 5:
                        criteria.Condition += string.Format(@" and p.ProductSpec like '%{0}%' ", content);
                        break;
                    case 6:
                        criteria.Condition += string.Format(@" and p.ProductModel like '%{0}%' ", content);
                        break;
                    case 7:
                        criteria.Condition += string.Format(@" and p.Place like '%{0}%' ", content);
                        break;
                    case 8:
                        criteria.Condition += string.Format(@" and p.BarCode like '%{0}%' ", content);
                        break;
                    case 9:
                        criteria.Condition += string.Format(@" and p.PinyinCode like '%{0}%' ", content);
                        break;
                }

                #endregion
            }
            criteria.Condition += " and p.ProductState=" + status;
            criteria.Condition += " and p.ClassId=" + pType;
            criteria.Condition += " and p.ClassId=c.ClassId ";
            criteria.Condition += " and i1.PicId=p.Picture1 ";
            criteria.Condition += " and i2.PicId=p.Picture1 ";
            criteria.Condition += " and i3.PicId=p.Picture1 ";
            criteria.Condition += " and s.ProductId=p.ProductId ";
            criteria.Condition += " and p.SellerId=" + customerId + " ";
            criteria.Fields = string.Format(@" c.Name,i1.Resource as pic1,i2.Resource as pic2,i3.Resource as pic3,p.ProductId,p.ClassId,p.ProductCode,p.ProductFullName,p.ProductShortName,
            p.PinyinCode,p.Package,p.ProductSpec,i1.Format format1,i2.Format format2,i3.Format format3,p.ProductModel,p.ProductUnit,p.Weight,s.Quantity,
            p.BarCode,p.Place,p.ProductState,p.Price1,p.Price2,p.Price3,p.Price4,p.Price5,p.Price6,p.Price7,p.notes,
            p.Picture1,p.Picture2,p.Picture3,p.Summary,c.ParentId ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(customerId), out errMsg);
        }

        /// <summary>
        /// 商品上架商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="SellerId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetPOSProductList(int page, int pagesize, string sort, int SellerId, int Type, int pType, out string errMsg)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.PageSize = pagesize;
            criteria.CurrentPage = page;
            criteria.TableName = " ProductInfo p left join ProductClassInfo c on p.ClassId=c.ClassId left join SellerStockPile s on s.ProductId=p.ProductId left join PictureInfo i on i.PicId=p.Picture1  ";
            criteria.Condition = " 1=1 and p.Status!=100 ";
            criteria.PrimaryKey = "p.ProductId";
            if (Type == 0)//全部客户
            {
                criteria.TableName = criteria.TableName + " left join SellerProductOnline o on o.ProductId=p.ProductId  ";
            }
            else//单个客户
            {
                criteria.TableName = criteria.TableName + " left join SellerProductOnlineCustomerStrategy o on o.ProductId=p.ProductId and o.BuyerId=" + Type + " ";
            }

            if (!string.IsNullOrEmpty(sort))
            {
                criteria.Sort = sort;
            }
            if (pType != -1)
            {
                criteria.Condition += " and p.ClassId=" + pType + " ";
            }
            criteria.Condition += " and p.SellerId=" + SellerId + " ";

            criteria.Fields = string.Format(@" p.ProductId,p.ProductCode,p.ProductFullName,c.Name as ClassName,i.Resource as picName,i.PicId,s.Quantity,p.ProductSpec,i.Format,o.SaleState,o.SaleStoreQuantity,case when o.Price1 is null then p.Price1 else o.Price1 end as Price1,o.IsNew,o.IsPromotion ");

            return dal.GetPageDataDataTable(criteria, GetConnectionString(SellerId), out errMsg);
        }

        /// <summary>
        /// 检查是否上线
        /// </summary>
        /// <param name="sellerId"></param>
        /// <param name="productId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool DeleteCheck(int sellerId, int productId, out string errMsg)
        {
            errMsg = string.Empty;
            List<SellerProductOnlineCustomerStrategy> socmodels = SellerProductOnlineCustomerStrategyBll.GetList().Where(c => c.SellerId == sellerId && c.ProductId == productId).ToList();
            if (socmodels == null || socmodels.Count == 0)
            {
                List<SellerProductOnline> spomodels = SellerProductOnlineBll.GetList().Where(c => c.SellerId == sellerId && c.ProductId == productId).ToList();
                if (spomodels == null || spomodels.Count == 0)
                    return false;
            }
            return true;
        }
    }
}
