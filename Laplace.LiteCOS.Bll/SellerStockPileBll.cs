using Laplace.Framework.Orm;
using Laplace.LiteCOS.Dal;
using Laplace.LiteCOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Bll
{
    public class SellerStockPileBll : BaseBll<SellerStockPile>
    {
        protected static readonly SellerStockPileDal<SellerStockPile> Dal = new SellerStockPileDal<SellerStockPile>();

        /// <summary>
        /// 更新库存(加库存)
        /// </summary>
        /// <param name="SellerId"></param>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool UpdateModel( SellerStockPile model, out string errMsg)
        {
            var Smodel = Dal.GetModel(model.SellerId, model.ProductId, model.StoreHouseId, GetConnectionString(model.SellerId), out errMsg);
            if (Smodel != null && string.IsNullOrEmpty(errMsg))//存在更新
            {
                Smodel.Quantity = Smodel.Quantity + model.Quantity;
                Smodel.LMan = model.SellerId;
                Smodel.LDate = DateTime.Now;
                return Dal.Update(Smodel, GetConnectionString(model.SellerId), out errMsg);
            }
            else {
                return Dal.Insert(model,GetConnectionString(model.SellerId),out errMsg);
            }
            
        }

        /// <summary>
        /// 更新库存(减库存)
        /// </summary>
        /// <param name="SellerId"></param>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool UpdateMinusModel(SellerStockPile model, out string errMsg)
        {
            var Smodel = Dal.GetModel(model.SellerId, model.ProductId, model.StoreHouseId, GetConnectionString(model.SellerId), out errMsg);
            if (Smodel != null && string.IsNullOrEmpty(errMsg))//存在更新
            {
                if (Smodel.Quantity <= 0)
                {
                    Smodel.Quantity = 0;
                }
                else if (Smodel.Quantity - model.Quantity <= 0)
                {
                    Smodel.Quantity = 0;
                }
                else
                {
                    Smodel.Quantity = Smodel.Quantity - model.Quantity;
                }
                Smodel.LMan = model.SellerId;
                Smodel.LDate = DateTime.Now;
                return Dal.Update(Smodel, GetConnectionString(model.SellerId), out errMsg);
            }
            else
            {
                return true;
                //return Dal.Insert(model, GetConnectionString(model.SellerId), out errMsg);
            }

        }
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SellerStockPile GetModel(SellerStockPile model)
        {
            string errMsg = string.Empty;
            var Smodel = Dal.GetModel(model.SellerId, model.ProductId, model.StoreHouseId, GetConnectionString(model.SellerId), out errMsg);
            return Smodel;
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
            criteria.TableName = " ProductInfo p left join SellerStockPile s on p.ProductId = s.ProductId and p.SellerId = s.SellerId left join ProductClassInfo c on c.ClassId = p.ClassId and c.SellerId = p.SellerId ";
            criteria.Condition = " 1=1 ";
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
            criteria.Fields = string.Format(@" p.ProductModel,p.ProductId,p.ProductCode,p.ProductFullName,p.ProductShortName,p.ProductSpec,p.ClassId,c.Name,case  when s.Quantity is null then 0 else s.Quantity end as Quantity,case when s.Cost is null then 0 else s.Cost end as Cost,case when s.Price is null then 0 else s.Price end as Price,p.Place,p.BarCode,p.PinyinCode,p.ProductState,p.SellerId ");

            return Dal.GetPageDataDataTable(criteria, GetConnectionString(customerId), out errMsg);
        }
    }
}
