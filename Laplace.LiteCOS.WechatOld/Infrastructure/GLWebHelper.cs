
using System.Web.Mvc;

namespace Laplace.LiteCOS.Wechat.Infrastructure
{
    public class GLWebHelper
    {
        /// <summary>
        /// 加载JS/CSS
        /// </summary>
        /// <param name="AType"></param>
        /// <returns></returns>
        public static MvcHtmlString LoadJSCSS(string ATypes)
        {
            string m_CSS = string.Empty;
            string m_JS = string.Empty;
            m_JS += "<script type=\"text/javascript\" src=\"/Scripts/jquery-1.8.2.min.js\" ></script> \n\r";
            m_JS += "<script type=\"text/javascript\" src=\"/resources/js/cookie.js\" charset=\"gb2312\" ></script> \n\r";
            m_JS += "<script type=\"text/javascript\" src=\"/resources/js/common.js\" charset=\"gb2312\"  ></script> \n\r";

            string[] m_Types = ATypes.Split(',');
            int m_Count = m_Types.Length;
            foreach (string m_item in m_Types)
            {
                switch (m_item)
                {
                    case "1":
                        break;

                }
            }

            return MvcHtmlString.Create(m_CSS + m_JS);
        }
    }
}