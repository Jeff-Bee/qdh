using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家内部员工信息
    /// </summary>
    public class SellerEmployeeInfo : BaseModel
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public int EmployeeId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 员工代码
        /// </summary>
        public string EmployeeCode { get; set; } = string.Empty;
        /// <summary>
        /// 员工全名
        /// </summary>
        public string EmployeeFullName { get; set; } = string.Empty;
        /// <summary>
        /// 员工简名
        /// </summary>
        public string EmployeeShortName { get; set; } = string.Empty;
       
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;

        /// <summary>
        /// 所属部门编号
        /// </summary>
        public int DeptId { get; set; } = 0;

        /// <summary>
        /// 绑定手机
        /// </summary>
        public string MobilePhone { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; } = true;
        
    }
    public class SellerEmployeeInfoMapper : ClassMapper<SellerEmployeeInfo>
    {
        public SellerEmployeeInfoMapper()
        {
            Table("SellerEmployeeInfo");
            Map(x => x.EmployeeId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
