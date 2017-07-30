using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCOS.Web.Controllers
{
    public class UserCenterController : Controller
    {
        // GET: UserCenter
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            string name = string.Empty;
            string pwd = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["name"]))
            {
                name = Request.Form["name"];
            }
            if (!string.IsNullOrEmpty(Request.Form["name"]))
            {
                pwd = Request.Form["pwd"];
            }
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
            {
                string error = string.Empty;
                SellerInfo model = new SellerInfo();
                SellerInfoBll.RequestLogin(name, pwd, out model, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    Response.Write("<script>alert('"+ error + "');</script>");
                    //Response.Redirect("/UserCenter/Login");
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("userId");
                    cookie.Value = model.SellerId.ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);
                    Session["UserId"] = model.SellerId;
                    Response.Redirect("/BackStage/Index");
                }
            }
            return View();
        }

        /// <summary>
        /// 注册页面第一步
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            string phone = string.Empty;
            string jyCode = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["phone"]))
            {
                phone = Request.Form["phone"];
            }
            if (!string.IsNullOrEmpty(Request.Form["jyCode"]))
            {
                jyCode = Request.Form["jyCode"];
            }
            if (!string.IsNullOrEmpty(jyCode) && !string.IsNullOrEmpty(jyCode))
            {
                //Response.Redirect("/UserCenter/Register_Step2?phone="+ phone);
                string error = string.Empty;
                if (SellerInfoBll.CheckRegisterVerificationCode(phone, jyCode, out error))
                {
                    Response.Redirect("/UserCenter/Register_Step2?phone="+phone+"&jyCode="+jyCode);
                }
                else
                {
                    Response.Write("<script>alert('" + error + "')</script>");
                }
            }
            return View();
        }

        /// <summary>
        /// 注册页面第二步
        /// </summary>
        /// <returns></returns>
        public ActionResult Register_Step2()
        {
            //第一步跳转过来传的参数
            if (!string.IsNullOrEmpty(Request.Params["phone"]))
            {
                ViewBag.phone = Request.Params["phone"];
            }
            if (!string.IsNullOrEmpty(Request.Params["jyCode"]))
            {
                ViewBag.jyCode = Request.Params["jyCode"];
            }
            List<IndustryInfo> models= IndustryInfoBll.GetList().ToList();
            ViewBag.modes = models;
            //第二步保存提交的参数
            string cname = string.Empty;
            string pwd = string.Empty;
            string contact = string.Empty;
            string telphone = string.Empty;
            string industry = string.Empty;
            string jyCode = string.Empty;//验证码
            if (!string.IsNullOrEmpty(Request.Form["cname"]))
            {
                cname = Request.Form["cname"];
            }
            if (!string.IsNullOrEmpty(Request.Form["pwd"]))
            {
                pwd = Request.Form["pwd"];
            }
            if (!string.IsNullOrEmpty(Request.Form["contact"]))
            {
                contact = Request.Form["contact"];
            }
            if (!string.IsNullOrEmpty(Request.Form["Tel_phone"]))
            {
                telphone = Request.Form["Tel_phone"];
            }
            if (!string.IsNullOrEmpty(Request.Form["industry"]))
            {
                industry = Request.Form["industry"];
            }
            if (!string.IsNullOrEmpty(Request.Form["rs2_jyCode"]))
            {
                jyCode = Request.Form["rs2_jyCode"];
            }
            if (!string.IsNullOrEmpty(cname) && !string.IsNullOrEmpty(pwd))
            {
                //Response.Redirect("/UserCenter/Register_Step3?name=");

                int sellerId = 0;
                string error = string.Empty;
                if (SellerInfoBll.RequestRegisterNewUser(cname, pwd, contact, telphone, int.Parse(industry), jyCode, out sellerId, out error))
                {
                    SellerInfo model = SellerInfoBll.GetModel(sellerId, out error);
                    if (string.IsNullOrEmpty(error))
                    {
                        Response.Redirect("/UserCenter/Register_Step3?name=" + model.LoginName);
                    }

                }
                else
                {
                    Response.Write("<script>alert('" + error + "');</script>");
                    Response.Redirect("/UserCenter/Register");
                }
            }
            return View();
        }

        /// <summary>
        /// 注册页面第三步
        /// </summary>
        /// <returns></returns>
        public ActionResult Register_Step3()
        {
            if (!string.IsNullOrEmpty(Request.Params["name"]))
            {
                ViewBag.Lname = Request.Params["name"];
            }
            return View();
        }
        
        /// <summary>
        /// 注册获取校验码
        /// </summary>
        /// <returns></returns>
        public string GetVerCode()
        {
            string phone = Request.Params["phone"];
            string error = string.Empty;
            if (SellerInfoBll.RequestRegisterVerificationCode(phone, out error))
            {
                return "";
            }
            else {
                return error;
            }
        }

        /// <summary>
        /// 找回密码获取验证码
        /// </summary>
        /// <returns></returns>
        //public string GetForgetPwdCode()
        //{
        //    string phone = Request.Params["phone"];
        //    string error = string.Empty;
        //    if (SellerInfoBll.RequestGetPasswordVerificationCode(phone, out error))
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return error;
        //    }
        //}

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public string GetPwd()
        {
            string telphone = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["telphone"]))
            {
                telphone = Request.Form["telphone"];
            }
            if (!string.IsNullOrEmpty(telphone))//!string.IsNullOrEmpty(cname) && !string.IsNullOrEmpty(telphone) && !string.IsNullOrEmpty(varcode)
            {
                string error = string.Empty;
                if (SellerInfoBll.RequestGetPassword(telphone, out error))
                {
                    return "1";
                }
                else
                {
                    return error;
                }
            }
            return "获取参数失败!";
        }

        /// <summary>
        /// 修改密码保存
        /// </summary>
        /// <returns></returns>
        public string ChangePwd()
        {
            string newValue = string.Empty;
            string oldValue = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["oldPwd"]))
            {
                oldValue = Request.Form["oldPwd"];
            }
            if (!string.IsNullOrEmpty(Request.Form["newPwd"]))
            {
                newValue = Request.Form["newPwd"];
            }
            if (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
            {
                string errMsg = string.Empty;
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value),out errMsg);
                if (model != null)
                {
                    if (model.Password == oldValue)
                    {
                        model.Password = newValue;
                        if (SellerInfoBll.Update(model))
                        {
                            return "修改成功";
                        }
                        else
                        {
                            return "修改失败";
                        }
                    }
                    else
                    {
                        return "原密码不正确";
                    }
                }
                else {
                    return "请重新登录后操作";
                }
                
            }
            else {
                return "参数信息出错,请重试";
            }
        }

        /// <summary>
        /// 修改用户信息保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateSellerUserInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                JObject Jjson = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                SellerInfo NewModel = new SellerInfo();
                NewModel.MobilePhone = Jjson["MobilePhone"].ToString();
                NewModel.CompanyName= Jjson["CompanyName"].ToString();
                NewModel.ContactName = Jjson["ContactName"].ToString();
                NewModel.EMail = Jjson["EMail"].ToString();
                NewModel.IndustryId = int.Parse(Jjson["IndustryId"].ToString());
                //判断手机是否重复
                if (!string.IsNullOrEmpty(NewModel.MobilePhone)&&model.MobilePhone != NewModel.MobilePhone)//如果手机不为空且与原来的不一样则判断是否重复
                {
                    if (SellerInfoBll.GetModelByMobilePhone(NewModel.MobilePhone, out errMsg) != null)
                    {
                        return "手机号已经存在";
                    }
                }
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(NewModel.MobilePhone))
                    {
                        model.MobilePhone = NewModel.MobilePhone;
                    }
                    model.IndustryId = NewModel.IndustryId;
                    model.CompanyName = NewModel.CompanyName;
                    model.ContactName = NewModel.ContactName;
                    model.EMail = NewModel.EMail;
                    if (SellerInfoBll.Update(model))
                    {
                        return "修改成功";
                    }
                    else {
                        return "修改失败";
                    }
                }
            }
            return "参数错误,请刷新重试";
        }

        /// <summary>
        /// 获取卖家用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSellerUserInfo()
        {
            string errMsg = string.Empty;
            SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value),out errMsg);
            if (model != null)
            {
                model.MobilePhone= model.MobilePhone.Substring(0, 3) + "****" + model.MobilePhone.Substring(model.MobilePhone.Length - 4, 4); 
            }
            return JsonConvert.SerializeObject(model);
        }



    }
}