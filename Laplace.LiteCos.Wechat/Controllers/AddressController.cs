using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;

namespace Laplace.LiteCos.Wechat.Controllers
{
    public class AddressController : BaseBuyerController
    {
        // GET: Address
        public ActionResult Index()
        {
            return View(GetUser());
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View(GetUser());
        }

        [HttpPost]
        public ActionResult Edit(string account, string address,string name,string province_id,string city_id,string area_id)
        {
            //tid=0&id=10800&account=18201069969&address=工人路100号&name=张三&province_id=41&city_id=4101&area_id=410102
            return View("Index");
        }


        public JsonResult DelAddress()
        {

            var data =  new JsonResponseData()
            {
                IsSuccess = true,
                Msg = "删除成功"
            };
           
            return new JsonResult()
            {
                Data = data
            };
        }
    }
}