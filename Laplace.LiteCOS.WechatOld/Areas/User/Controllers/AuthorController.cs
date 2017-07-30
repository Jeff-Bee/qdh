using System;
using System.Web.Mvc;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Wechat.Areas.User.Models;
using Laplace.LiteCOS.Wechat.Infrastructure;
using Laplace.LiteCOS.Wechat.Models;

namespace Laplace.LiteCOS.Wechat.Areas.User.Controllers
{
    public class AuthorController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
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
                        authProvider.AuthSuccess();
                        return Redirect(returnUrl ?? Url.Action("MainView", "Product", new { area = "Product" }));
                    }
                    else
                    {
                        ModelState.AddModelError("", "密码不正确请重新输入");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "登录失败:" + msg);
                }
            }
            else
            {
                ModelState.AddModelError("", "输入信息有误,请重新输入");
            }

            authProvider.AuthFailed();
            return View();
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
        public ActionResult GetRegisterCode(string phoneNumber)
        {
            LplsResponse response = new LplsResponse();

            string code = "";
            response.Success = BuyerInfoBll.RequestRegisterVerificationCode(phoneNumber, out code);
            code = new Random().Next(100000, 1000000) + "";
            response.Data = code;
            //CurCode = code;
            return Json(response);
        }



        public ActionResult FindPassword()
        {
            return View();
        }



        [HttpPost]
        public ActionResult FindPassword(BuyerInfoRegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

            }
            else
            {
                ModelState.AddModelError("", "输入数据无效,请重新输入");
                return View();
            }

            int buyerId = 0;
            string msg = "";
            bool ret = BuyerInfoBll.RequestGetPasswordVerificationCode(model.PhoneNumber, out msg);

#if DEBUG

            ret = true;
#endif

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



        public ActionResult LoginInfo()
        {
            return View();
        }
    }
}