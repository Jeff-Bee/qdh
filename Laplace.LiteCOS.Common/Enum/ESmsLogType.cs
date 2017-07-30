using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laplace.LiteCOS.Common.Enum
{
    /// <summary>
    /// 短信记录类型
    /// </summary>
    public enum ESmsLogType:short
    {
        SellerRegister = 1,             //商家用户注册
        SellerGetPassword = 2,          //商家用户找回密码
        SellerChangeMobilePhone = 3,    //商家用户修改注册手机号

        BuyerRegister = 1,             //买家用户注册
        BuyerGetPassword = 2,          //买家用户找回密码
        BuyerChangeMobilePhone = 3,    //买家用户修改注册手机号
        BuyerChangePassword,
    }
}
