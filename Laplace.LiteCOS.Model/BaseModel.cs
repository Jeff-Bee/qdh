using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Model
{
    public class BaseModel
    {
        #region--通用属性--

        /// <summary>
        /// 配置信息
        /// </summary>
        public string Config { get; set; } = string.Empty;

        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// 创建人
        /// </summary>
        public int RMan { get; set; } = 0;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime RDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改人
        /// </summary>
        public int LMan { get; set; } = 0;
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime LDate { get; set; } = DateTime.Now;
        #endregion-通用属性-
    }
}
