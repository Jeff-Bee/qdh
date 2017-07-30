using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家部门信息
    /// </summary>
    public class SellerDeptInfo : BaseModel
    {
        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int DeptId { get; set; } = 0;
        
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 部门代码
        /// </summary>
        public string DeptCode { get; set; } = string.Empty;
        /// <summary>
        /// 部门全名
        /// </summary>
        public string DeptFullName { get; set; } = string.Empty;
        /// <summary>
        /// 部门简名
        /// </summary>
        public string DeptShortName { get; set; } = string.Empty;
       
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; } = string.Empty;
  
        
    }
    public class SellerDeptInfoMapper : ClassMapper<SellerDeptInfo>
    {
        public SellerDeptInfoMapper()
        {
            Table("SellerDeptInfo");
            Map(x => x.DeptId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
