using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家进货单
    /// </summary>
    public class SellerBuyInfo : BaseModel
    {

        /// <summary>
        /// 进货单编号：自增，10000--2147483 647	0--1000000为系统保留*/
        /// </summary>
        public int BuyId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 进货单代码（确保同卖家唯一，格式：JH-20161101-002）
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 录单日期
        /// </summary>
        public DateTime ComeDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 经手人编号（SellerEmployeeInfo:EmployeeId）
        /// </summary>
        public int EmployeeId { get; set; } = 0;

        /// <summary>
        /// 部门编号(SellerDeptInfo:DeptId)
        /// </summary>
        public int DeptId { get; set; } = 0;
        /// <summary>
        /// 供货单位编号(SellerSupplierInfo:SupplierId)
        /// </summary>
        public int SupplierId { get; set; } = 0;
        /// <summary>
        /// 接收仓库编号(SellerStoreHouseInfo:StoreHouseId)
        /// </summary>
        public int StoreHouseId { get; set; } = 0;
        /// <summary>
        /// 摘要
        /// </summary>
        public string Remark { get; set; } = string.Empty;
        /// <summary>
        /// 付款账户(??)
        /// </summary>
        public int PaymentAccount { get; set; } = 0;

        /// <summary>
        /// 合计金额
        /// </summary>
        public float TotalAmount { get; set; } = 0;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public float DiscountAmount { get; set; } = 0;

        /// <summary>
        /// 优惠后金额
        /// </summary>
        public float ActualAmount { get; set; } = 0;
        /// <summary>
        /// 已付款金额
        /// </summary>
        public float PaidAmount { get; set; } = 0;
        /// <summary>
        /// 代付款金额
        /// </summary>
        public float ChargeAmount { get; set; } = 0;

    }
    public class SellerBuyInfoMapper : ClassMapper<SellerBuyInfo>
    {
        public SellerBuyInfoMapper()
        {
            Table("SellerBuyInfo");
            Map(x => x.BuyId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
