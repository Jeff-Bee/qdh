using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCOS.Web.Controllers
{
    public class BackStageController : Controller
    {
        // GET: BackStage
        public ActionResult Index()
        {
            ViewBag.menu = GetMenuList();
            //ViewBag.UserName = "laolace";
            if (Request.Cookies["userId"] != null && !string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                string errMsg = string.Empty;
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                ViewBag.UserName = model.CompanyName;
                //ViewBag.phone = model.MobilePhone.Substring(0,3)+"****"+model.MobilePhone.Substring(model.MobilePhone.Length-4,4);
            }
            else {
                Response.Redirect("/UserCenter/Login");
            }
            return View();
        }

        /// <summary>
        /// 获取登录用户对应的菜单项
        /// </summary>
        /// <returns></returns>
        public string GetMenuList()
        {
            string html = string.Empty;
          
            html += " <li class=\"easyui-menubutton\" data-options=\"menu:'#ste2',\">在线商城</li>";
            html += "   <div id=\"ste2\" style=\"width: 50px;display:none; \">";
            html += "       <div onclick=\"AddTab('买家信息','/BasicInfo/GetUserInfoList')\">买家信息</div>";
            html += "       <div onclick=\"AddTab('商品上架','/BasicInfo/ProductOnSale')\">商品上架</div>";
            html += "       <div onclick=\"AddTab('订单管理','/BasicInfo/OrderControl')\">订单管理</div>";
            html += "       <div onclick=\"AddTab('图片轮播','/BasicInfo/NewLine')\">图片轮播</div>";
            html += "       <div onclick=\"AddTab('销售报表','/BasicInfo/SaleReport?type=online')\">销售报表</div>";
            html += "       <div onclick=\"AddTab('消息推送','/BasicInfo/PromotionInfo')\">消息推送</div>";
            html += "   </div>";

            html += " <li class=\"easyui-menubutton\" data-options=\"menu:'#ste1',\" id=\"OfflineSales\">线下销售</li>";
            html += "   <div id=\"ste1\" style=\"width: 50px;display:none; \">";
            html += "       <div id=\"index_SalesOut\" onclick=\"AddTab('销售出库','/BasicInfo/SalesOutOfTheLibrary')\">销售出库</div>"; 
             html += "       <div id=\"index_SalesReport\" onclick=\"AddTab('销售报表','/BasicInfo/SaleReport?type=outline')\">销售报表</div>";
            html += "   </div>";
            


            html += " <li class=\"easyui-menubutton\" data-options=\"menu:'#ste3',\" id=\"InventoryManagement\">进货管理</li>";
            html += "   <div id=\"ste3\" style=\"width: 50px;display:none; \">";
            html += "       <div onclick=\"AddTab('供应商','/BasicInfo/suppliersList')\">供应商</div>";
            html += "       <div onclick=\"AddTab('商品进货','/BasicInfo/PurchaseOrderList')\">商品进货</div>";
            html += "       <div onclick=\"AddTab('库存状态','/BasicInfo/InventoryLevelsView')\">库存状态</div>";
            html += "       <div onclick=\"AddTab('进货报表','/BasicInfo/PurchaseOrderReport')\">进货报表</div>";
            //html += "       <div id=\"LossReport\">报损单</div>";
            html += "   </div>";
            //html += " <li class=\"easyui-menubutton\" data-options=\"menu:'#ste4',\">统计报表</li>";
            //html += "   <div id=\"ste4\" style=\"width: 50px; \">";
            ////html += "       <div>业务草稿</div>";
            ////html += "       <div>单据中心</div>";
            ////html += "       <div>库存状况</div>";
            //html += "   </div>";
            html += " <li class=\"easyui-menubutton\" data-options=\"menu:'#ste5',\">基本资料</li>";
            html += "   <div id=\"ste5\" style=\"width: 50px;display:none; \">";
            html += "       <div onclick=\"AddTab('商品信息','/BasicInfo/ProductInfo')\">商品</div>";
            html += "       <div onclick=\"AddTab('买家信息','/BasicInfo/GetUserInfoList')\">买家信息</div>";
            html += "       <div onclick=\"AddTab('供应商','/BasicInfo/suppliersList')\">供应商</div>";
            html += "       <div onclick=\"AddTab('职员信息','/BasicInfo/InternalStaffInfo')\">职员信息</div>";
            html += "       <div onclick=\"AddTab('在线支付','/BasicInfo/PayMentMode')\">在线支付</div>";
            html += "       <div onclick=\"AddTab('库存状态','/BasicInfo/InventoryLevelsView')\">库存状态</div>";
            html += "       <div onclick=\"AddTab('地区信息','/BasicInfo/AreaIndex')\">地区信息</div>";

            //html += "       <div onclick=\"AddTab('部门信息','/BasicInfo/DeparentmentList')\">部门信息</div>";
            //html += "       <div onclick=\"AddTab('设备信息','/BasicInfo/EquipmentInformation')\">设备信息</div>"; 

            html += "   </div>"; 
            return html;

        }

        /// <summary>
        /// 行业列表
        /// </summary>
        /// <returns></returns>
        public string GetInsudtryList()
        {
            List<IndustryInfo> list = IndustryInfoBll.GetList().ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取新订单信息个数
        /// </summary>
        /// <returns></returns>
        public string GetOrderMsgCount()
        {
            if (Request.Cookies["UserId"] != null && !string.IsNullOrEmpty(Request.Cookies["UserId"].Value))
            {
                return SellerOrderRunBll.GetNewOrderCount(int.Parse(Request.Cookies["UserId"].Value));
            }
            return "";
        }
    }
}