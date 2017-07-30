using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 卖家表格列展示配置信息
    /// </summary>
    public class SellerColumnConfig : BaseModel
    {
        /// <summary>
        /// 卖家用户编号
        /// </summary>
        public int SellerId { get; set; } = 0;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; } = string.Empty;
        /// <summary>
        /// 列（默认）名称
        /// </summary>
        public string DefaultName { get; set; } = string.Empty;
        /// <summary>
        /// （用户自定义）显示名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// 列序号
        /// </summary>
        public int Index { get; set; } = 0;
        

  
        
    }
    public class SellerColumnConfigMapper : ClassMapper<SellerColumnConfig>
    {
        public SellerColumnConfigMapper()
        {
            Table("SellerColumnConfig");
            Map(x => x.SellerId).Key(KeyType.Assigned);
            Map(x => x.TableName).Key(KeyType.Assigned);
            Map(x => x.FieldName).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
