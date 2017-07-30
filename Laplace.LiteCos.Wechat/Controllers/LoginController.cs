using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Laplace.LiteCOS.Wechat.Infrastructure;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(bool isAutoLogin = true)
        {
            ViewBag.AppName = "趣订货电子商务有限公司";


            if (isAutoLogin && Request.Cookies["name"] != null && Request.Cookies["pwd"] != null)
            {
                string msg;
                string name = Request.Cookies["name"].Value;
                string pwd = Request.Cookies["pwd"].Value;

                var user = BuyerInfoBll.GetModelByLoginName(name, out msg);
                if (user != null)
                {
                    if (user.Password.Equals(pwd))
                    {
                        Session["User"] = user;
                        Session["IsGuest"] = false;
                        IAuthProvider authProvider = new FormsAuthProvider();
                        authProvider.AuthSuccess();

                        var sellerList = BuyerInfoBll.GetSellerList(user.BuyerId, out msg);
                        if (sellerList.Count > 1)
                        {
                            return RedirectToAction("ListView", "Seller");
                        }
                        else if (sellerList.Count == 1)
                        {
                            Session["SellerId"] = sellerList[0].SellerId;
                            return RedirectToAction("List", "Goods", new { sellerId = sellerList[0].SellerId });
                        }

                        return RedirectToAction("NoSeller", "Seller");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(BuyerInfoViewModel buyer, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            IAuthProvider authProvider = new FormsAuthProvider();

            if (ModelState.IsValid)
            {
                string msg;
                var user = BuyerInfoBll.GetModelByLoginName(buyer.LoginName, out msg);
                if (user != null)
                {
                    if (user.Password.Equals(buyer.Password))
                    {
                        Session["User"] = user;
                        Session["IsGuest"] = false;
                        authProvider.AuthSuccess();


                        var sellerList = BuyerInfoBll.GetSellerList(user.BuyerId, out msg);

                        if (sellerList.Count > 1)
                        {
                            return RedirectToAction("ListView", "Seller");
                        }
                        else if (sellerList.Count == 1)
                        {
                            Session["SellerId"] = sellerList[0].SellerId;
                            return RedirectToAction("List", "Goods", new { sellerId = sellerList[0].SellerId });
                        }

                        return RedirectToAction("NoSeller", "Seller");

                    }
                    else
                    {
                        ModelState.AddModelError("", "密码不正确请重新输入");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "登录失败:用户不存在" + msg);
                }
            }
            else
            {
                ModelState.AddModelError("", "输入信息有误,请重新输入");
            }

            authProvider.AuthFailed();
            return View();
        }


        public ActionResult LostPwd()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetRegisterCode(string phoneNumber)
        {
            JsonResponseData response = new JsonResponseData();

            string code = "";
            response.IsSuccess = BuyerInfoBll.RequestRegisterVerificationCode(phoneNumber, out code);
            //code = new Random().Next(100000, 1000000) + "";
            response.Msg = code;
            //CurCode = code;
            return Json(response);
        }



        [HttpPost]
        public ActionResult GetPasswordVerificationCode(string phoneNumber)
        {
            JsonResponseData response = new JsonResponseData();
            string code = "";
            response.IsSuccess = BuyerInfoBll.RequestGetPasswordVerificationCode(phoneNumber, out code);
            response.Msg = code;
            return Json(response);
        }
        /// <summary>
        /// 修改密码：返回验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetChangePasswordVerificationCode(string phoneNumber)
        {
            JsonResponseData response = new JsonResponseData();
            string code = "";
            response.IsSuccess = BuyerInfoBll.RequestChangePasswordVerificationCode(phoneNumber, out code);
            response.Msg = code;
            return Json(response);
        }


        [HttpPost]
        public ActionResult LostPwd(BuyerInfoRegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

            }
            else
            {
                ModelState.AddModelError("", "输入数据无效,请重新输入");
                return View();
            }


            if (!model.Password.Equals(model.PasswordAgain))
            {
                ModelState.AddModelError("", "两次输入密码不一致");
                return View();
            }

            string msg = "";
            bool ret = BuyerInfoBll.RequestChangePassword(model.PhoneNumber,model.Password,model.RegisterCode, out msg);

            if (ret)
            {
                return RedirectToAction("Login", new { LoginName = model.PhoneNumber });
            }
            else
            {
                ModelState.AddModelError("", "找回密码失败:" + msg);
                return View();
            }
        }




        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(BuyerInfoRegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
            }
            else
            {
                ModelState.AddModelError("", "输入数据无效,请重新输入");
                return View();
            }

            if (!model.Password.Equals(model.PasswordAgain))
            {
                ModelState.AddModelError("", "两次输入密码不一致");
                return View();
            }


            int buyerId = 0;
            string msg = "";
            bool ret = BuyerInfoBll.RequestRegisterNewUser(model.PhoneNumber, model.Password, model.RegisterCode, out buyerId, out msg);
            if (ret)
            {
                return RedirectToAction("Login", new { LoginName = model.PhoneNumber });
            }
            else
            {
                ModelState.AddModelError("", "注册失败:" + msg);
                return View();
            }

        }


        [HttpPost]
        public ActionResult LoginAsGuest()
        {

            JsonResponseData response = new JsonResponseData() { IsSuccess = true, Msg = "登录成功" };

            IAuthProvider authProvider = new FormsAuthProvider();
            authProvider.AuthSuccess();

            string msg;
            var guest = BuyerInfoBll.GetModel(1000031, out msg);
            if (guest == null)
            {
                response.IsSuccess = false;
                response.Msg = "游客身份获取失败";
                return Json(response);
            }



            Session["User"] = guest;
            Session["IsGuest"] = true;


            var sellerList = BuyerInfoBll.GetSellerList(guest.BuyerId, out msg);
            if (sellerList.Count >= 1)
            {
                Session["SellerId"] = sellerList[0].SellerId;
            }

            else
            {
                Session["SellerId"] = 0;
                response.IsSuccess = false;
                response.Msg = "没有为体验用户指定卖家~";
                return Json(response);
            }


            return Json(response);



        }


        public ActionResult ExitLogin()
        {

            //JsonResponseData response = new JsonResponseData() { IsSuccess = true, Msg = "退出登录" };

            IAuthProvider authProvider = new FormsAuthProvider();
            authProvider.AuthFailed();


            Session["User"] = null;
            Session["SellerId"] = 0;
            Session["IsGuest"] = false;

            return RedirectToAction("Login", new
            {
                isAutoLogin = false
            }
        );



        }



    }
}