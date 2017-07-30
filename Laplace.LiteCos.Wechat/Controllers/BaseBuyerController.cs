using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laplace.LiteCos.Wechat.Models;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;

namespace Laplace.LiteCos.Wechat.Controllers
{
    [Authorize]
    public class BaseBuyerController : Controller
    {
        protected BuyerInfo GetUser()
        {
            BuyerInfo user = Session["User"] as BuyerInfo;
            return user;
        }

        public ImageActionResult GetPic(int productId, int sellerId)
        {



            string msg;
            //var pic = PictureInfoBll.GetModel(pictureId, sellerId, out msg);
            var pic = PictureInfoBll.GetPic(productId, sellerId, out msg);
            if (pic != null)
            {
                return new ImageActionResult()
                {
                    ByteStream = pic.Resource
                };
            }
            else
            {
                return new ImageActionResult()
                {
                    ByteStream = new byte[] { }
                };
            }

        }


    }
}