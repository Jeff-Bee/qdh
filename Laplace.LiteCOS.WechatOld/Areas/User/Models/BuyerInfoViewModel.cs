using System.ComponentModel.DataAnnotations;

namespace Laplace.LiteCOS.Wechat.Areas.User.Models
{
    public class BuyerInfoViewModel
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        [Required(ErrorMessage = "登录名不能为空")]
        [Display(Name = "登录名:")]
        public string LoginName { get; set; }



        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码:")]
        public string Password { get; set; }

    }



    public class BuyerInfoRegisterViewModel
    {

        /// <summary>
        /// 登录名称
        /// </summary>   
        [Display(Name = "登录名:")]
        public string LoginName { get; set; }



        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码:")]
        public string Password { get; set; }


        public string PasswordAgain { get; set; }


        /// <summary>
        /// 注册码
        /// </summary>
        public string RegisterCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }

}