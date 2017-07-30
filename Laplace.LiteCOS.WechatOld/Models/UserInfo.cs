using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Laplace.LiteCOS.Wechat.Models
{
    public class UserInfo
    {
        [Description("用户名")]
        [Display(Name = "用户名:")]
        public string Name { get; set; }
    }
}