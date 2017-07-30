using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Laplace.Framework.Log;

namespace Laplace.LiteCOS.Wechat.Areas.Product.Models
{
    public class ImageActionResult : ActionResult
    {
        //public ImageActionResult() { }



        public byte[] ByteStream;



        //重写ExecuteResult

        public override void ExecuteResult(ControllerContext context)

        {

            try
            {
                // 设置响应设置

                context.HttpContext.Response.ContentType = "image/jpeg";

                context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);

                context.HttpContext.Response.BufferOutput = false;

                MemoryStream stream = new MemoryStream(ByteStream);

                // 将图像流写入响应流中

                const int buffersize = 1024 * 32;

                byte[] buffer = new byte[buffersize];

                int count = stream.Read(buffer, 0, buffersize);

                while (count > 0)
                {

                    context.HttpContext.Response.OutputStream.Write(buffer, 0, count);

                    count = stream.Read(buffer, 0, buffersize);

                }

                context.HttpContext.Response.BinaryWrite(stream.ToArray());
            }
            catch (Exception ex)
            {
               Logger.LogError("获取图片失败:"+ex.Message);
            }

        }
    }
}