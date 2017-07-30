using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Laplace.LiteCOS.Model
{
    /// <summary>
    /// 行业信息
    /// </summary>
    public class IndustryInfo : BaseModel
    {
        /// <summary>
        /// 行业编号
        /// </summary>
        public int IndustryId { get; set; } = 0;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 所属上级行业编号
        /// </summary>
        public int ParentId { get; set; } = 0;

    }
    public class IndustryInfoMapper : ClassMapper<IndustryInfo>
    {
        public IndustryInfoMapper()
        {
            Table("IndustryInfo");
            Map(x => x.IndustryId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
