using Laplace.Framework.Orm;
using Laplace.LiteCOS.Bll;
using Laplace.LiteCOS.Model;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laplace.LiteCOS.Web.Controllers
{
    //基本资料
    public class BasicInfoController : Controller
    {
        #region 商品信息

        // GET: BasicInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取商品类别
        /// </summary>
        /// <returns></returns>
        public string GetProductTypes()
        {
            string errMsg = string.Empty;
            int pid = 0;
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                pid = int.Parse(Request.Params["id"]);
                List<ProductClassInfo> models = ProductClassInfoBll.GetListByParentId(int.Parse(Request.Cookies["userId"].Value), pid, out errMsg);
                json += "[";
                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        json += "{";
                        json += "\"id\":\"" + model.ClassId + "\",\"text\":\"" + model.Name + "\",\"state\":\"closed\"";
                        json += "},";
                    }
                }
                if (json.Substring(json.Length - 1, 1) == ",")
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]";
            }
            else
            {
                json += "[";
                json += "{\"id\":\"0\",\"text\":\"所有分类\",\"state\":\"open\",\"children\":[";
                List<ProductClassInfo> lists = ProductClassInfoBll.GetList(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                var rootList = lists.Where(c => c.ParentId == 0).ToList();
                foreach (var list in rootList)
                {
                    string str = GetProductClassTreeList(lists, list.ClassId);
                    if (!string.IsNullOrEmpty(str) && str.Substring(str.Length - 1, 1) == ",")
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    json += "{\"id\":\"" + list.ClassId + "\",\"text\":\"" + list.Name + "\",\"state\":\"closed\",\"children\":[" + str + "]},";
                }
                if (json.Substring(json.Length - 1, 1) == ",")
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]}]";
            }

            return json;
        }

        /// <summary>
        /// 获取商品类型树数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public string GetProductClassTreeList(List<ProductClassInfo> list, int classId)
        {
            var child = list.Where(c => c.ParentId == classId).ToList();
            string json = string.Empty;
            if (child.Count != 0)
            {
                foreach (var chil in child)
                {
                    json += "{\"id\":\"" + chil.ClassId + "\",\"text\":\"" + chil.Name + "\",\"state\":\"closed\",\"chlidren\":[" + GetProductClassTreeList(list, chil.ClassId) + "]},";
                }

            }
            else
            {
                json = "";
            }
            return json;
        }

        /// <summary>
        /// 获取商品搜索类型下拉框
        /// </summary>
        /// <returns></returns>
        public string GetSearchTypeList()
        {

            string json = string.Empty;
            json += "[";
            json += "{";
            json += "  \"id\":\"0\",\"text\":\"快速查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"1\",\"text\":\"按全名查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"2\",\"text\":\"按品牌查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"3\",\"text\":\"按编号查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"4\",\"text\":\"按简名查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"5\",\"text\":\"按规格查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"6\",\"text\":\"按型号查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"7\",\"text\":\"按产地查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"8\",\"text\":\"按条码查询\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"9\",\"text\":\"按拼音码查询\"";
            json += "}";


            json += "]";
            return json;
        }
        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <returns></returns>
        public string GetGLlist()
        {
            string json = string.Empty;
            json += "[";
            json += "{";
            json += "  \"id\":\"0\",\"text\":\"仅显示正常商品\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"2\",\"text\":\"仅显示停用商品\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"-1\",\"text\":\"显示全部商品\"";
            json += "}";
            json += "]";
            return json;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <returns></returns>
        public string GetList()
        {
            int searchType = 0;
            string content = string.Empty;
            int type = -1;
            int status = -1;//表示全部状态
            int current = 1;
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request.Params["searchType"]))
            {
                searchType = int.Parse(Request.Params["searchType"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["searchContent"]))
            {
                content = Request.Params["searchContent"];
            }
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                current = int.Parse(Request.Params["page"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["rows"]))
            {
                pagesize = int.Parse(Request.Params["rows"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                type = int.Parse(Request.Params["type"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["status"]))
            {
                status = int.Parse(Request.Params["status"]);
            }
            string errMsg = string.Empty;
            string userId = Request.Cookies["userId"].Value;
            PageDataView data = ProductInfoBll.GetProductList(current, pagesize, "", content, searchType, int.Parse(userId), out errMsg, status, type);
            DataTable dt = new DataTable();
            string json = string.Empty;
            if (data != null && data.DataTable.Rows.Count > 0)
            {
                dt = data.DataTable;
                dt.Columns.Add("productImg", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["pic1"] != null && !string.IsNullOrEmpty(dr["pic1"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic1"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture1"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture1"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture1"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                    else if (dr["pic2"] != null && !string.IsNullOrEmpty(dr["pic2"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic2"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture2"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture2"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture2"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                    else if (dr["pic3"] != null && !string.IsNullOrEmpty(dr["pic3"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic3"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture3"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture3"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture3"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                }
                string str = JsonConvert.SerializeObject(dt);

                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + data.TotalNum + "\",\"rows\":" + str + "";
                json += "}";
            }
            else
            {
                json = string.Empty;
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }



        /// <summary>
        /// 商品分类新增
        /// </summary>
        /// <returns></returns>
        public string TypeCreate()
        {
            string pid = string.Empty;
            string text = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["parentId"]))
            {
                pid = Request.Params["parentId"];
            }
            if (!string.IsNullOrEmpty(Request.Params["text"]))
            {
                text = Request.Params["text"];
            }
            if (Request.Cookies["userId"] == null || string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                return "0";
            }
            else if (string.IsNullOrEmpty(pid))
            {
                return "0";
            }
            else
            {
                string errMsg = string.Empty;
                ProductClassInfo model = new ProductClassInfo();
                model.Name = text;
                model.PinyinCode = "";
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                model.ParentId = int.Parse(pid);
                int id = ProductClassInfoBll.InsertWithReturnId(model, out errMsg);
                if (id > 0)
                {
                    return id.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 商品分类编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string TypeEdit()
        {
            string id = Request.Params["id"];
            string text = Request.Params["text"];
            if (Request.Cookies["userId"] == null || string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                return "1";
            }
            else if (string.IsNullOrEmpty(id))
            {
                return "1";
            }
            else
            {
                ProductClassInfo model = new ProductClassInfo();
                model.Name = text;
                model.PinyinCode = "";
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                model.ClassId = int.Parse(id);
                if (ProductClassInfoBll.Update(model))
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
        }

        /// <summary>
        /// 商品分类删除
        /// </summary>
        /// <returns></returns>
        public string TypeDel()
        {
            string id = Request.Params["id"];

            if (Request.Cookies["userId"] == null || string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                return "1";
            }
            else if (string.IsNullOrEmpty(id))
            {
                return "1";
            }
            else
            {
                ProductClassInfo model = new ProductClassInfo();
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                model.ClassId = int.Parse(id);
                string reeMsg = string.Empty;
                if (ProductClassInfoBll.Delete(model.SellerId, model.ClassId, out reeMsg))
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
        }

        /// <summary>
        /// 商品分类拖拽保存
        /// </summary>
        /// <returns></returns>
        public string TypeDnd()
        {
            string dndId = Request.Params["id"];
            string targetId = Request.Params["targetId"];
            if (Request.Cookies["userId"] == null || string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                return "1";
            }
            else if (string.IsNullOrEmpty(dndId) || string.IsNullOrEmpty(targetId))
            {
                return "1";
            }
            else
            {
                string errMsg = string.Empty;
                ProductClassInfo model = new ProductClassInfo();
                model = ProductClassInfoBll.GetModel(int.Parse(dndId), int.Parse(Request.Cookies["userId"].Value), out errMsg);
                if (model == null)
                {
                    return "1";
                }
                model.ParentId = int.Parse(targetId);
                if (ProductClassInfoBll.Update(model))
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
        }


        public ActionResult Demo()
        {
            return View();
        }

        /// <summary>
        /// 获取到定位的那么对应的id
        /// </summary>
        /// <returns></returns>
        public string Position()
        {
            string id = string.Empty;
            string name = Request.Params["name"];
            string sql = string.Format(@"select ClassId,Name,ParentId from ProductClassInfo where SellerId={1} and (PinyinCode = '{0}' or Name = '{0}') ", name, Request.Cookies["userId"].Value);
            DataTable dt = ProductClassInfoBll.ExecuteDt(sql, int.Parse(Request.Cookies["userId"].Value));
            if (dt == null || dt.Rows.Count <= 0)
            {
                id = "";
            }
            else
            {
                id = dt.Rows[0]["ClassId"].ToString();
            }
            return id;
        }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void fileUpload()
        {
            string json = string.Empty;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string filePath = file.FileName;
                string type = filePath.Split('.')[1];
                string userId = Request.Cookies["userId"].Value;
                if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                {
                    Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                }
                string mapPath = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                string name = DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + type;
                string savePath = mapPath + "\\" + name;
                string returnUrl = "/UploadImgs/" + userId + "/" + name;
                file.SaveAs(savePath);
                json = "{\"success\":\"1\",\"src\":\"" + returnUrl + "\"}";
            }
            else
            {
                json = "{\"success\":\"0\",\"src\":\"\"}";
            }
            Response.Write(json);
            Response.End();
        }

        /// <summary>
        /// 图片删除
        /// </summary>
        public void DelPic()
        {
            string name = Request.Params["name"];
            string userId = Request.Cookies["userId"].Value;
            string path = Server.MapPath("\\UploadImgs\\" + userId + "");

            name = name.Split('/')[3];
            path = path + "\\" + name;
            if (Directory.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            Response.Write(1);
            Response.End();
        }

        /// <summary>
        /// 商品新增保存
        /// </summary>
        [HttpPost]
        public void ProductAdd()
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["json"]))
            {
                str = Request.Params["json"];
                JObject Jjson = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                string list_Product = Jjson["productInfo"].ToString();
                string list_Pic = Jjson["productPic"].ToString();
                ProductInfo pmodel = new ProductInfo();
                pmodel = (ProductInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(list_Product, typeof(ProductInfo));
                List<PictureInfo> pmodels = (List<PictureInfo>)Newtonsoft.Json.JsonConvert.DeserializeObject(list_Pic, typeof(List<PictureInfo>));
                string errMsg = string.Empty;
                pmodel.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (pmodel.ProductId > 0)//编辑
                {
                    #region 编辑保存

                    var nmodel = ProductInfoBll.GetModel(pmodel.ProductFullName, int.Parse(Request.Cookies["userId"].Value), out errMsg);
                    if (nmodel != null && nmodel.ProductId != pmodel.ProductId)
                    {
                        Response.Write("4");//商品名称已存在
                        Response.End();
                        return;
                    }
                    var cmodel = ProductInfoBll.GetModelByCode(pmodel.ProductCode, int.Parse(Request.Cookies["userId"].Value), out errMsg);
                    //判断商品code是否存在
                    if (cmodel != null && cmodel.ProductId != pmodel.ProductId)
                    {
                        Response.Write("5");//商品编号已存在
                        Response.End();
                        return;
                    }
                    bool isAdd = true;
                    List<int> picIds = new List<int>();
                    //先获取图片数据流保存图片
                    foreach (var model in pmodels)
                    {
                        PictureInfo infoModel = new PictureInfo();
                        infoModel.Name = model.Name;
                        infoModel.PicId = model.PicId;
                        if (!string.IsNullOrEmpty(model.Format))
                        {
                            string mapPath = HttpContext.Server.MapPath(model.Format);
                            FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read);
                            Byte[] bytPic = new Byte[fs.Length];
                            infoModel.Size = Convert.ToInt32(fs.Length.ToString());
                            fs.Read(bytPic, 0, bytPic.Length);
                            fs.Close();
                            infoModel.Resource = bytPic;
                            infoModel.SellerId = int.Parse(Request.Cookies["userId"].Value);
                            Image img = Image.FromFile(mapPath);
                            infoModel.Width = short.Parse(img.Width.ToString());
                            infoModel.Height = short.Parse(img.Height.ToString());
                            img.Dispose();
                            infoModel.Format = model.Format.Split('.')[1];
                            System.IO.File.Delete(mapPath);
                        }
                        if (infoModel.PicId > 0)//已存在保存
                        {
                            if (PictureInfoBll.Update(infoModel, out errMsg))
                            {
                                picIds.Add(infoModel.PicId);
                            }
                            else
                            {
                                isAdd = false;
                                break;
                            }
                        }
                        else
                        {
                            int id = 0;
                            if (PictureInfoBll.Insert(infoModel, out id, out errMsg))
                            {
                                picIds.Add(id);
                            }
                            else
                            {
                                isAdd = false;
                                break;
                            }
                        }


                    }
                    if (isAdd)
                    {
                        if (picIds.Count == 3)
                        {
                            pmodel.Picture1 = picIds[0];
                            pmodel.Picture2 = picIds[1];
                            pmodel.Picture3 = picIds[2];
                        }
                        else if (picIds.Count == 2)
                        {
                            pmodel.Picture1 = picIds[0];
                            pmodel.Picture2 = picIds[1];
                        }
                        else if (picIds.Count == 1)
                        {
                            pmodel.Picture1 = picIds[0];
                        }
                        if (ProductInfoBll.Update(pmodel, out errMsg))
                        {
                            Response.Write("1");//成功
                            Response.End();
                        }
                        else
                        {
                            Response.Write("3");//商品保存失败
                            Response.End();
                        }

                    }
                    else
                    {
                        Response.Write("2");//图片保存失败
                        Response.End();
                    }

                    #endregion
                }
                else
                {//新增
                    #region 新增保存

                    //判断商品名称是否存在
                    if (ProductInfoBll.GetModel(pmodel.ProductFullName, int.Parse(Request.Cookies["userId"].Value), out errMsg) != null)
                    {
                        Response.Write("4");//商品名称已存在
                        Response.End();
                        return;
                    }
                    //判断商品code是否存在
                    if (ProductInfoBll.GetModelByCode(pmodel.ProductCode, int.Parse(Request.Cookies["userId"].Value), out errMsg) != null)
                    {
                        Response.Write("5");//商品编号已存在
                        Response.End();
                        return;
                    }

                    bool isAdd = true;
                    List<int> picIds = new List<int>();
                    //先获取图片数据流保存图片
                    foreach (var model in pmodels)
                    {
                        PictureInfo infoModel = new PictureInfo();
                        infoModel.Name = model.Name;
                        if (!string.IsNullOrEmpty(model.Format))
                        {
                            string mapPath = HttpContext.Server.MapPath(model.Format);
                            FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read);
                            Byte[] bytPic = new Byte[fs.Length];
                            infoModel.Size = Convert.ToInt32(fs.Length.ToString());
                            fs.Read(bytPic, 0, bytPic.Length);
                            fs.Close();
                            infoModel.Resource = bytPic;
                            infoModel.SellerId = int.Parse(Request.Cookies["userId"].Value);
                            Image img = Image.FromFile(mapPath);
                            infoModel.Width = short.Parse(img.Width.ToString());
                            infoModel.Height = short.Parse(img.Height.ToString());
                            img.Dispose();
                            infoModel.Format = model.Format.Split('.')[1];
                            System.IO.File.Delete(mapPath);
                        }
                        int id = 0;
                        if (PictureInfoBll.Insert(infoModel, out id, out errMsg))
                        {
                            picIds.Add(id);
                        }
                        else
                        {
                            isAdd = false;
                            break;
                        }
                    }
                    if (isAdd)
                    {
                        if (picIds.Count == 3)
                        {
                            pmodel.Picture1 = picIds[0];
                            pmodel.Picture2 = picIds[1];
                            pmodel.Picture3 = picIds[2];
                        }
                        else if (picIds.Count == 2)
                        {
                            pmodel.Picture1 = picIds[0];
                            pmodel.Picture2 = picIds[1];
                        }
                        else if (picIds.Count == 1)
                        {
                            pmodel.Picture1 = picIds[0];
                        }
                        int pid = 0;
                        if (ProductInfoBll.Insert(pmodel, out pid, out errMsg))
                        {
                            Response.Write("1");//成功
                            Response.End();
                        }
                        else
                        {
                            Response.Write("3");//商品保存失败
                            Response.End();
                        }

                    }
                    else
                    {
                        Response.Write("2");//图片保存失败
                        Response.End();
                    }

                    #endregion
                }
            }
            else
            {
                Response.Write("0");//获取参数出错
                Response.End();
            }

        }


        /// <summary>
        /// 刪除商品信息
        /// </summary>
        /// <returns></returns>
        public string DelProduct()
        {
            if (!string.IsNullOrEmpty(Request.Params["ids"]))
            {
                string errMsg = string.Empty;
                bool res = true;
                string str = Request.Params["ids"];
                string[] strs = str.Split(',');
                foreach (string item in strs)
                {
                    res = res && ProductInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), int.Parse(item), out errMsg);
                }
                if (res)//刪除成功
                {
                    return "1";
                }
                else
                {//刪除失敗
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 检查商品是否上线
        /// </summary>
        /// <returns></returns>
        public string DelCheck()
        {
            if (!string.IsNullOrEmpty(Request.Params["ids"]))
            {
                string errMsg = string.Empty;
                bool res = true;
                string pName = string.Empty;
                string str = Request.Params["ids"];
                string[] strs = str.Split(',');
                foreach (string item in strs)
                {
                    if (ProductInfoBll.DeleteCheck(int.Parse(Request.Cookies["userId"].Value), int.Parse(item), out errMsg))
                    {
                        pName = item;
                        res = false;
                        break;
                    }

                }
                if (res)//无上线商品
                {
                    return "";
                }
                else
                {//有上线商品
                    ProductInfo model = ProductInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), int.Parse(pName), out errMsg);
                    if (model != null)
                    {
                        pName = model.ProductFullName;
                    }
                    return pName;
                }
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 编辑获取商品资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProductInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string id = Request.Params["id"];
                string errMsg = string.Empty;
                ProductInfo model = ProductInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), int.Parse(id), out errMsg);
                string json = JsonConvert.SerializeObject(model);
                string imgstr = string.Empty;
                if (model != null)
                {
                    imgstr = "],\"pics\":[";
                    if (model.Picture1 != 0)
                    {
                        PictureInfo picmodel = PictureInfoBll.GetModel(model.Picture1, int.Parse(Request.Cookies["UserId"].Value), out errMsg);

                        string userId = Request.Cookies["userId"].Value;
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + model.Picture1 + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }

                            byte[] img = picmodel.Resource;
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = model.Picture1 + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";

                        }
                        else
                        {
                            string imgsSrc = model.Picture1 + ".jpg";
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";
                        }

                    }
                    if (model.Picture2 != 0)
                    {
                        PictureInfo picmodel = PictureInfoBll.GetModel(model.Picture2, int.Parse(Request.Cookies["UserId"].Value), out errMsg);
                        string userId = Request.Cookies["userId"].Value;
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + model.Picture1 + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }

                            byte[] img = picmodel.Resource;
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = model.Picture2 + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";

                        }
                        else
                        {
                            string imgsSrc = model.Picture1 + ".jpg";
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";
                        }
                    }
                    if (model.Picture3 != 0)
                    {
                        PictureInfo picmodel = PictureInfoBll.GetModel(model.Picture3, int.Parse(Request.Cookies["UserId"].Value), out errMsg);
                        string userId = Request.Cookies["userId"].Value;
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + model.Picture1 + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }

                            byte[] img = picmodel.Resource;
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = model.Picture3 + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";

                        }
                        else
                        {
                            string imgsSrc = model.Picture1 + ".jpg";
                            imgstr += "{\"picUrl\":\"" + imgsSrc + "\",\"picName\":\"" + picmodel.Name + "\",\"picId\":\"" + picmodel.PicId + "\"},";
                        }
                    }
                    if (imgstr.Substring(imgstr.Length - 1, 1) == ",")
                    {
                        imgstr = imgstr.Substring(0, imgstr.Length - 1);
                    }
                    imgstr += "]";
                }
                json = "{\"productInfo\":[" + json + imgstr + "}";
                return json;
            }
            return "";
        }

        /// <summary>
        /// 获取下一个商品code
        /// </summary>
        /// <returns></returns>
        public string GetProductCode()
        {
            string code = ProductInfoBll.GetProductCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }

        #endregion

        #region 内部职员

        /// <summary>
        /// 内部职员信息
        /// </summary>
        /// <returns></returns>
        public ActionResult InternalStaffInfo()
        {
            return View();
        }

        /// <summary>
        /// 内部职员新增保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ISSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string str = Request.Params["str"];
                SellerEmployeeInfo smolde = (SellerEmployeeInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(str, typeof(SellerEmployeeInfo));
                smolde.SellerId = int.Parse(Request.Cookies["userId"].Value);
                //smolde.DeptId = 10001;//先写死
                string errMsg = string.Empty;
                if (smolde.EmployeeId <= 0)//新增
                {
                    //检查员工编号是否重复
                    if (!SellerEmployeeInfoBll.CheckCode(smolde.SellerId, smolde.EmployeeCode, out errMsg))
                    {
                        if (SellerEmployeeInfoBll.Insert(smolde))
                        {
                            return "1";//新增成功
                        }
                        else
                        {
                            return "3";//新增出错
                        }
                    }
                    else
                    {
                        return "0";//code重复
                    }

                }
                else
                {//编辑
                    if (SellerEmployeeInfoBll.Update(smolde))
                    {
                        return "1";//编辑成功
                    }
                    else
                    {
                        return "3";//编辑出错
                    }
                }
            }
            else
            {
                return "2";//接受参数出错
            }

        }


        /// <summary>
        /// 内部员工删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DelInternalStaff()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                if (SellerEmployeeInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), id, out errMsg))
                {
                    return "1";//成功
                }
                else
                {
                    return "0";//失败
                }
            }
            return "";
        }

        /// <summary>
        /// 获取内部员工列表
        /// </summary>
        /// <returns></returns>
        public string GetInternalUserList()
        {
            List<SellerEmployeeInfo> list = SellerEmployeeInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && c.IsUsed == true).ToList();
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 获取部门下拉框信息
        /// </summary>
        /// <returns></returns>
        public string GetDepList()
        {
            List<SellerDeptInfo> list = SellerDeptInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            string json = string.Empty;
            //json += "[";
            //foreach (var item in list)
            //{
            //    json += "{\"DeptId\":\""+item.DeptId+ "\",\"DeptFullName\":\""+item.DeptFullName+"\"},";
            //}
            //if (json.Substring(json.Length - 1, 1) == ",")
            //{
            //    json = json.Substring(0, json.Length - 1);
            //}
            //json += "]";
            json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 获取code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetEmployeeCode()
        {
            string code = SellerEmployeeInfoBll.GetEmployeeCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }
        #endregion

        #region 部门信息

        /// <summary>
        /// 部门信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DeparentmentList()
        {
            return View();
        }

        /// <summary>
        /// 部门新增
        /// </summary>
        /// <returns></returns>
        public string DepAddSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                SellerDeptInfo model = (SellerDeptInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(str, typeof(SellerDeptInfo));
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (model.DeptId <= 0)//编辑
                {
                    if (SellerDeptInfoBll.Update(model))
                    {
                        return "1";//成功
                    }
                    else
                    {
                        return "0";//失败
                    }

                }
                else
                {//新增

                    if (!SellerDeptInfoBll.CheckCode(model.SellerId, model.DeptCode, out errMsg))
                    {
                        if (SellerDeptInfoBll.Insert(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 新增/编辑保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddDeptSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                SellerDeptInfo model = (SellerDeptInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(str, typeof(SellerDeptInfo));
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (model.DeptId <= 0)//新增
                {
                    if (!SellerDeptInfoBll.CheckCode(model.SellerId, model.DeptCode, out errMsg))
                    {
                        if (SellerDeptInfoBll.Insert(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//保存失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
                else
                {//编辑
                    if (!SellerDeptInfoBll.CheckCode(model.SellerId, model.DeptCode, out errMsg, model.DeptId))
                    {
                        if (SellerDeptInfoBll.Update(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//保存失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";//获取参数出错
        }

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string delDeptInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                if (SellerDeptInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), int.Parse(Request.Params["id"]), out errMsg))
                {
                    return "1";//成功
                }
                else
                {
                    return "0";//删除失败
                }
            }
            return "";
        }

        /// <summary>
        /// 获取部门code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDepCode()
        {
            string code = SellerDeptInfoBll.GetDepartmentCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }
        #endregion

        #region 地区
        public ActionResult AreaIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取地区list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAreaIndexList()
        {
            List<SellerAreaInfo> list = SellerAreaInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 新增/编辑保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SaveAreaEdit()
        {
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string str = Request.Params["str"];
                SellerAreaInfo model = (SellerAreaInfo)JsonConvert.DeserializeObject(str, typeof(SellerAreaInfo));
                model.SellerId = int.Parse(Request.Params["userId"]);
                if (model.AreaId != 0)//修改
                {
                    if (!SellerAreaInfoBll.CheckCode(model.SellerId, model.Code, out errMsg, model.AreaId))
                    {
                        if (SellerAreaInfoBll.Update(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
                else
                {//新增
                    if (!SellerAreaInfoBll.CheckCode(model.SellerId, model.Code, out errMsg, model.AreaId))
                    {
                        if (SellerAreaInfoBll.Insert(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取地区code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAreaCode()
        {
            string code = SellerAreaInfoBll.GetAreaCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }


        /// <summary>
        /// 删除地区
        /// </summary>
        /// <returns></returns>
        public string DelArea()
        {
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string id = Request.Params["id"];
                string[] ids = id.Split(',');
                List<int> areaIds = new List<int>();
                foreach (string item in ids)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        areaIds.Add(int.Parse(item));
                    }
                }
                return SellerAreaInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), areaIds, out errMsg) ? "1" : "0";
            }
            return "";
        }
        #endregion

        #region 列配置

        /// <summary>
        /// 获取列配置
        /// </summary>
        /// <returns></returns>
        public string GetColumnConfig()
        {
            string json = string.Empty;
            string errMsg = string.Empty;
            List<SellerColumnConfig> lists = SellerColumnConfigBll.GetList(int.Parse(Request.Cookies["userId"].Value), "ProductInfo", out errMsg).OrderBy(c => c.Index).ToList();
            if (lists != null && lists.Count > 0)
            {
                json = JsonConvert.SerializeObject(lists);
            }
            return json;
        }

        /// <summary>
        /// 更改列配置的显示字段
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateShowState()
        {
            int index = -1;
            if (!string.IsNullOrEmpty(Request.Params["index"]))
            {
                index = int.Parse(Request.Params["index"]);
                string errMsg = string.Empty;
                SellerColumnConfig model = SellerColumnConfigBll.GetModel(int.Parse(Request.Cookies["userId"].Value), "ProductInfo", index, out errMsg);
                if (model == null)
                {
                    return "";
                }
                if (model.Visible)
                {
                    model.Visible = false;
                }
                else
                {
                    model.Visible = true;
                }
                if (SellerColumnConfigBll.Update(model))
                {
                    return "1";
                }
                else
                {
                    return "";
                }

            }
            return "";
        }

        /// <summary>
        /// 更改列配置显示名字
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateColumnName()
        {
            if (!string.IsNullOrEmpty(Request.Params["index"]) && !string.IsNullOrEmpty(Request.Params["newName"]))
            {
                int index = int.Parse(Request.Params["index"]);
                string nName = Request.Params["newName"];
                string errMsg = string.Empty;
                SellerColumnConfig model = SellerColumnConfigBll.GetModel(int.Parse(Request.Cookies["userId"].Value), "ProductInfo", index, out errMsg);
                if (model != null)
                {
                    model.DisplayName = nName;
                    if (SellerColumnConfigBll.Update(model))
                    {
                        return "1";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        /// <summary>
        /// 更改列配置列的显示位置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateColumnIndex()
        {
            if (!string.IsNullOrEmpty(Request.Params["index"]) && !string.IsNullOrEmpty(Request.Params["stype"]))
            {
                int index = int.Parse(Request.Params["index"]);
                int type = int.Parse(Request.Params["stype"]);
                string errMsg = string.Empty;
                if (SellerColumnConfigBll.UpdateIndex(int.Parse(Request.Cookies["userId"].Value), "ProductInfo", index, type, out errMsg))
                {
                    return "1";
                }
                return "";
            }
            return "";
        }

        /// <summary>
        /// 重置列配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ResetColumnConfig()
        {
            string errMsg = string.Empty;
            if (SellerColumnConfigBll.Delete(int.Parse(Request.Cookies["userId"].Value), "ProductInfo", out errMsg))
            {
                return "1";
            }
            return "";
        }
        #endregion


        #region 进货单

        /// <summary>
        /// 进货单主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseOrderList()
        {
            if (Request.Cookies["userId"] != null && !string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                string errMsg = string.Empty;
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                if (model != null)
                {
                    ViewBag.Name = model.ContactName == "" ? model.LoginName : model.ContactName;
                }
            }
            else
            {
                Response.Redirect("/UserCenter/Login");
            }
            ViewBag.OrderListCode = SellerBuyInfoBll.GetBuyerInfoCode(int.Parse(Request.Cookies["userId"].Value));
            return View();
        }

        /// <summary>
        /// 获取添加商品页面
        /// </summary>
        /// <returns></returns>
        public string GetPOLProduct()
        {
            int searchType = 0;
            string content = string.Empty;
            int type = 0;
            int status = 0;
            int current = 1;
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request.Params["searchType"]))
            {
                searchType = int.Parse(Request.Params["searchType"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["searchContent"]))
            {
                content = Request.Params["searchContent"];
            }
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                current = int.Parse(Request.Params["page"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["rows"]))
            {
                pagesize = int.Parse(Request.Params["rows"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                type = int.Parse(Request.Params["type"]);
            }
            string errMsg = string.Empty;
            string userId = Request.Cookies["userId"].Value;
            PageDataView data = ProductInfoBll.GetProductList(current, pagesize, "", content, searchType, int.Parse(userId), out errMsg, status, type);
            DataTable dt = new DataTable();
            string json = string.Empty;
            if (data != null && data.DataTable.Rows.Count > 0)
            {
                dt = data.DataTable;
                dt.Columns.Add("productImg", typeof(string));
                dt.Columns.Add("downId", typeof(string));
                #region 查询改分类下边的子分类
                List<ProductClassInfo> models = ProductClassInfoBll.GetListByParentId(int.Parse(userId), type, out errMsg);
                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        //判断该分类下是否有商品
                        List<ProductInfo> plist = ProductInfoBll.GetList(int.Parse(userId), model.ClassId, out errMsg).Where(c => c.Status != 100).ToList();
                        List<ProductClassInfo> lists = ProductClassInfoBll.GetListByParentId(int.Parse(userId), model.ClassId, out errMsg);
                        DataRow dr = dt.NewRow();
                        dr["ProductFullName"] = model.Name;
                        dr["ClassId"] = model.ClassId;
                        dr["ParentId"] = model.ParentId;
                        dr["Name"] = "";
                        dr["pic1"] = DBNull.Value;
                        dr["pic2"] = DBNull.Value;
                        dr["pic3"] = DBNull.Value;
                        if ((plist != null && plist.Count > 0) || (lists != null && lists.Count > 0))
                        {
                            dr["downId"] = "1";
                        }
                        else
                        {
                            dr["downId"] = "0";
                        }
                        dt.Rows.Add(dr);
                    }
                }
                #endregion


                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["pic1"] != null && !string.IsNullOrEmpty(dr["pic1"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic1"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture1"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture1"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture1"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                    else if (dr["pic2"] != null && !string.IsNullOrEmpty(dr["pic2"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic2"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture2"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture2"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture2"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                    else if (dr["pic3"] != null && !string.IsNullOrEmpty(dr["pic3"].ToString()))
                    {
                        byte[] img = (byte[])dr["pic3"];
                        if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + dr["Picture3"] + ".jpg")))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["Picture3"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(path, img);
                            dr["productImg"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["Picture3"].ToString() + ".jpg";
                            dr["productImg"] = imgsSrc;
                        }
                    }
                }
                string str = JsonConvert.SerializeObject(dt);

                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + (data.TotalNum + models.Count) + "\",\"rows\":" + str + "";
                json += "}";
            }
            else
            {
                dt.Columns.Add("ProductFullName", typeof(string));
                dt.Columns.Add("ClassId", typeof(string));
                dt.Columns.Add("ProductId", typeof(string));
                dt.Columns.Add("ParentId", typeof(string));

                dt.Columns.Add("ProductShortName", typeof(string));
                dt.Columns.Add("ProductUnit", typeof(string));
                dt.Columns.Add("Place", typeof(string));
                dt.Columns.Add("BarCode", typeof(string));
                dt.Columns.Add("productImg", typeof(string));
                dt.Columns.Add("downId", typeof(string));
                #region 查询改分类下边的子分类
                List<ProductClassInfo> models = ProductClassInfoBll.GetListByParentId(int.Parse(userId), type, out errMsg);
                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        //判断该分类下是否有商品
                        List<ProductInfo> plist = ProductInfoBll.GetList(int.Parse(userId), model.ClassId, out errMsg).Where(c => c.Status != 100).ToList();
                        List<ProductClassInfo> lists = ProductClassInfoBll.GetListByParentId(int.Parse(userId), model.ClassId, out errMsg);
                        DataRow dr = dt.NewRow();
                        dr["ProductFullName"] = model.Name;
                        dr["ClassId"] = model.ClassId;
                        dr["ParentId"] = model.ParentId;
                        dr["ProductId"] = "";
                        if ((plist != null && plist.Count > 0) || (lists != null && lists.Count > 0))
                        {
                            dr["downId"] = "1";
                        }
                        else
                        {
                            dr["downId"] = "0";
                        }
                        dt.Rows.Add(dr);
                    }
                }
                #endregion
                string str = JsonConvert.SerializeObject(dt);
                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + models.Count + "\",\"rows\":" + str + "";
                json += "}";
            }
            return json;
        }

        /// <summary>
        /// 获取返回上级的父分类id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetPParentId()
        {
            int id = 0;
            if (!string.IsNullOrEmpty(Request.Params["id"]) && Request.Cookies["userId"] != null && !string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                id = ProductClassInfoBll.GetParentClassId(int.Parse(Request.Params["id"]), int.Parse(Request.Cookies["userId"].Value));
                return id.ToString();
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 进货单新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string POL_Save()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string str = Request.Params["str"];
                JObject Jjson = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                string list_SellerBuyInfo = Jjson["SellerBuyInfo"].ToString();
                string list_SellerBuyDetail = Jjson["SellerBuyDetail"].ToString();
                SellerBuyInfo smolde = (SellerBuyInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(list_SellerBuyInfo, typeof(SellerBuyInfo));
                List<SellerBuyDetail> smodels = (List<SellerBuyDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject(list_SellerBuyDetail, typeof(List<SellerBuyDetail>));
                string errMsg = string.Empty;
                smolde.SellerId = int.Parse(Request.Cookies["userId"].Value);
                smolde.DiscountAmount = 0;
                smolde.ActualAmount = smolde.TotalAmount;
                smolde.ChargeAmount = smolde.TotalAmount - smolde.PaidAmount;
                if (SellerBuyInfoBll.CheckCode(smolde.SellerId, smolde.Code, out errMsg))
                {
                    return "2";//code重复
                }
                int id = SellerBuyInfoBll.InsertWithReturnId(smolde, out errMsg);
                if (id != 0)
                {
                    foreach (var model in smodels)
                    {
                        model.SellerId = smolde.SellerId;
                        model.BuyId = id;
                    }
                    if (SellerBuyDetailBll.Insert(smodels, int.Parse(Request.Cookies["userId"].Value)))
                    {
                        bool isTrue = true;
                        foreach (var model in smodels)
                        {
                            SellerStockPile sspModel = new SellerStockPile();
                            sspModel.SellerId = model.SellerId;
                            sspModel.Quantity = model.Quantity;
                            sspModel.ProductId = model.ProductId;
                            sspModel.StoreHouseId = 0;
                            sspModel.UpperLimit = 9999;
                            sspModel.LowerLitmit = 0;
                            sspModel.RMan = model.SellerId;
                            sspModel.LMan = model.SellerId;
                            isTrue = isTrue && SellerStockPileBll.UpdateModel(sspModel, out errMsg);
                        }
                        if (isTrue)
                        {
                            return "1";//新增成功
                        }
                        else
                        {
                            SellerBuyDetailBll.Delete(id, smolde.SellerId);//删除明细
                            SellerBuyInfoBll.Delete(id, smolde.SellerId);//删除主表
                            return "3";//更新库存失败
                        }
                    }
                    else
                    {
                        return "0";//新增明细失败
                    }
                }
                else
                {
                    return "0";//新增主表失败
                }

            }
            else
            {
                return "";//获取参数失败
            }
        }
        #endregion

        #region 进货单报表
        public ActionResult PurchaseOrderReport()
        {
            return View();
        }

        public string GetPorRun()
        {
            string errMsg = string.Empty;
            int searchType = -1;
            string content = string.Empty;
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            int current = 1;
            int pagesize = 10;
            string json = string.Empty;
            if (string.IsNullOrEmpty(Request.Params["SupplierId"]) || string.IsNullOrEmpty(Request.Params["StartTime"]) || string.IsNullOrEmpty(Request.Params["EndTime"]))
            {
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
                return json;
            }

            if (!string.IsNullOrEmpty(Request.Params["SupplierId"]))
            {
                searchType = int.Parse(Request.Params["SupplierId"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["StartTime"]))
            {
                startTime = DateTime.Parse(Request.Params["StartTime"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["EndTime"]))
            {
                endTime = DateTime.Parse(Request.Params["EndTime"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                current = int.Parse(Request.Params["page"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["rows"]))
            {
                pagesize = int.Parse(Request.Params["rows"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["content"]))
            {
                content = Request.Params["content"];
            }
            PageDataView PD= SellerBuyInfoBll.GetSellerBuyInfoList(current, pagesize,"", searchType,startTime,endTime,content,int.Parse(Request.Cookies["UserId"].Value),out errMsg);
            if (PD != null && PD.DataTable != null && PD.DataTable.Rows.Count > 0)
            {
                json += "{";
                json += "\"total\":\""+ PD.TotalNum + "\",\"rows\":"+JsonConvert.SerializeObject(PD.DataTable) +"";
                json += "}";
            }
            else {
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }

        /// <summary>
        /// 根据订单编号 获取商品信息
        /// </summary>
        /// <returns></returns>
        public string GetPorDetail() {
            string json = string.Empty;
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["BuyId"]))
            {
                int Buyid = int.Parse(Request.Params["BuyId"]);
                PageDataView pd = SellerBuyDetailBll.GetPORProductList(1,999,"",int.Parse(Request.Cookies["UserId"].Value), Buyid,out errMsg);
                if (pd != null && pd.DataTable != null && pd.DataTable.Rows.Count > 0)
                {
                    json += "{";
                    json += "\"total\":\""+pd.TotalNum+"\",\"rows\":"+JsonConvert.SerializeObject(pd.DataTable) +"";
                    json += "}";
                }
                else {
                    json += "{";
                    json += "\"total\":\"0\",\"rows\":[]";
                    json += "}";
                }
            }
            else {
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }

        #endregion

        #region 往来客户

        #region 供应商信息

        /// <summary>
        /// 供应商页面
        /// </summary>
        /// <returns></returns>
        public ActionResult suppliersList()
        {
            return View();
        }

        /// <summary>
        /// 供应商保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddSuppSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                SellerSupplierInfo model = (SellerSupplierInfo)JsonConvert.DeserializeObject(str, typeof(SellerSupplierInfo));
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (model.SupplierId > 0)//编辑
                {
                    if (!SellerSupplierInfoBll.CheckCode(model.SellerId, model.Code, out errMsg, model.SupplierId))
                    {
                        if (SellerSupplierInfoBll.Update(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
                else
                {//新增
                    if (!SellerSupplierInfoBll.CheckCode(model.SellerId, model.Code, out errMsg))
                    {
                        if (SellerSupplierInfoBll.Insert(model))
                        {
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";//获取参数失败
        }


        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <returns></returns>
        public string GetSuppList()
        {
            List<SellerSupplierInfo> list = SellerSupplierInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            return JsonConvert.SerializeObject(list);
        }


        /// <summary>
        /// 供货单位下拉框
        /// </summary>
        /// <returns></returns>
        public string GetSuppCbxList()
        {
            List<SellerSupplierInfo> list = SellerSupplierInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            string json = "[{\"FullName\":\"所有供应商\",\"SupplierId\":\"-1\"},";
            json += JsonConvert.SerializeObject(list).Replace("[","");
            return json;
        }

        /// <summary>
        /// 获取经手人下拉框
        /// </summary>
        /// <returns></returns>
        public string GetGuy()
        {
            List<SellerEmployeeInfo> list = SellerEmployeeInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && c.IsUsed == true).ToList();
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string delSuppliser()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                List<int> supIds = new List<int>();
                string id = Request.Params["id"];
                string[] ids = id.Split(',');
                foreach (var item in ids)
                {
                    supIds.Add(int.Parse(item));
                }
                return SellerSupplierInfoBll.Delete(supIds, int.Parse(Request.Cookies["userId"].Value), out errMsg) ? "1" : "0";
            }
            return "";
        }

        /// <summary>
        /// 获取最新的code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSupperCode()
        {
            string code = SellerSupplierInfoBll.GetSupperCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }

        /// <summary>
        /// 获取地区下拉框
        /// </summary>
        /// <returns></returns>
        public string GetSupAreaList()
        {

            List<SellerAreaInfo> list = SellerAreaInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            List<SellerAreaInfo> result = new List<SellerAreaInfo>();
            if (!string.IsNullOrEmpty(Request.Params["id"]) && !string.IsNullOrEmpty(Request.Params["text"]))
            {
                string id = Request.Params["id"];
                string text = Request.Params["text"];
                if (id == "0")
                {
                    result = list.Where(c => c.Code.IndexOf(text) >= 0 || c.FullName.IndexOf(text) >= 0).ToList();
                }
                else if (id == "1")
                {
                    result = list.Where(c => c.Code.IndexOf(text) >= 0).ToList();
                }
                else if (id == "2")
                {
                    result = list.Where(c => c.FullName.IndexOf(text) >= 0).ToList();
                }
            }
            else
            {
                result.AddRange(list);
            }
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 获取地区搜索条件下拉框
        /// </summary>
        /// <returns></returns>
        public string SearchAreaCondition()
        {
            string json = "[";
            json += "{\"id\":\"0\",\"text\":\"快速查询\"},";
            json += "{\"id\":\"1\",\"text\":\"Code查询\"},";
            json += "{\"id\":\"2\",\"text\":\"名称查询\"}";
            json += "]";
            return json;
        }
        #endregion

        #region 客户信息

        /// <summary>
        /// 客户信息界面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfoList()
        {
            return View();
        }


        /// <summary>
        /// 客户信息
        /// </summary>
        /// <returns></returns>
        public string GetCustomerList()
        {
            string json = string.Empty;
            List<SellerCustomerInfo> list = SellerCustomerInfoBll.GetList().ToList();
            if (list != null && list.Count > 0)
            {
                var lists = list.Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
                json = JsonConvert.SerializeObject(lists);
                json = "{\"total\":\"" + lists.Count + "\",\"rows\":" + json + "}";
            }
            else
            {
                json = "{\"total\":\"0\",\"rows\":[]}";
            }

            return json;
        }

        /// <summary>
        /// 客户信息新增保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string CorrespondentsAdd()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                SellerCustomerInfo model = (SellerCustomerInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(str, typeof(SellerCustomerInfo));
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (model.BuyerId > 0)//编辑
                {
                    if (!SellerCustomerInfoBll.CheckCode(model.SellerId, model.Code, out errMsg, model.BuyerId))
                    {
                        if (SellerCustomerInfoBll.Update(model))
                        {
                            return "1";//保存成功
                        }
                        else
                        {
                            return "0";//保存失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
                else
                {//新增


                    if (!SellerCustomerInfoBll.CheckCode(model.SellerId, model.Code, out errMsg))
                    {
                        #region 先新增buyerinfo表
                        BuyerInfo bmodel = new BuyerInfo();
                        bmodel.Address = model.Address;
                        bmodel.ContactName = model.ConstactPerson;
                        bmodel.Password = "888888";
                        bmodel.MobilePhone = model.MobilePhone;
                        bmodel.LoginName = model.MobilePhone;
                        int buyerId = BuyerInfoBll.InsertWithReturnId(bmodel, out errMsg);
                        if (buyerId <= 0)
                        {
                            return "0";//保存失败
                        }
                        model.BuyerId = buyerId;
                        #endregion
                        if (SellerCustomerInfoBll.Insert(model))
                        {
                            return "1";//保存成功
                        }
                        else
                        {
                            return "0";//保存失败
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";//获取参数失败
        }

        /// <summary>
        /// 获取卖家地区信息
        /// </summary>
        /// <returns></returns>
        public string GetAreaList()
        {
            List<SellerAreaInfo> list = SellerAreaInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            string json = "[";
            foreach (var model in list)
            {
                json += "{\"id\":\"" + model.AreaId + "\",\"text\":\"" + model.FullName + "\"},";
            }
            if (json.Substring(json.Length - 1, 1) == ",")
            {
                json = json.Substring(0, json.Length - 1);
            }
            json += "]";
            return json;
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string delCustomerInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                int id = int.Parse(Request.Params["id"]);
                if (SellerCustomerInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), id))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return "";
        }


        /// <summary>
        /// 客户信息编号
        /// </summary>
        /// <returns></returns>
        public string GetCustomerCode()
        {
            string code = SellerCustomerInfoBll.GetCustomerCode(int.Parse(Request.Cookies["userId"].Value));
            return code;
        }
        #endregion

        #endregion

        #region 设备信息
        public ActionResult EquipmentInformation()
        {
            return View();
        }
        #endregion

        #region 设备信息

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCarInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["Code"]))
            {
                string errMsg = string.Empty;
                string Code = Request.Params["Code"];
                SellerDeliveryCarInfo model = SellerDeliveryCarInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), Code, out errMsg);
                return JsonConvert.SerializeObject(model);
            }
            return "";
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentList()
        {
            List<SellerDeliveryCarInfo> list = SellerDeliveryCarInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <returns></returns>
        public string DelDeliveryCarInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                int id = int.Parse(Request.Params["id"]);
                if (SellerDeliveryCarInfoBll.Delete(int.Parse(Request.Cookies["userId"].Value), id))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return "";
        }

        /// <summary>
        /// 设备新增/编辑保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SaveEquitment()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]) && !string.IsNullOrEmpty(Request.Params["jym"]))
            {
                string errMsg = string.Empty;
                string str = Request.Params["str"];
                string jym = Request.Params["jym"];
                if (!SellerDeliveryCarInfoBll.CheckCodeChecked(jym))
                {
                    return "3";
                }
                SellerDeliveryCarInfo model = (SellerDeliveryCarInfo)JsonConvert.DeserializeObject(str, typeof(SellerDeliveryCarInfo));
                model.SellerId = int.Parse(Request.Cookies["userId"].Value);
                if (model.CarId != 0)//编辑
                {
                    if (!SellerDeliveryCarInfoBll.CheckCode(model.SellerId, model.Code, out errMsg, model.CarId))
                    {
                        if (SellerDeliveryCarInfoBll.Update(model))
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
                else
                {//新增
                    if (!SellerDeliveryCarInfoBll.CheckCode(model.SellerId, model.Code, out errMsg))
                    {
                        if (SellerDeliveryCarInfoBll.Insert(model))
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";//code重复
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取设备信息code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentCode()
        {
            return SellerDeliveryCarInfoBll.GetDeliveryCarCode(int.Parse(Request.Cookies["userId"].Value)).ToString();
        }
        #endregion

        #region 商品上架

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductOnSale()
        {
            return View();
        }

        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetPOSCustomer()
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["Stype"]))
            {
                string Stype = Request.Params["Stype"];
                List<SellerCustomerInfo> list = new List<SellerCustomerInfo>();
                if (Stype == "online")
                {
                    list = SellerCustomerInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && (c.BuyerType == 0 || c.BuyerType == 2)).ToList();
                }
                else
                {
                    list = SellerCustomerInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && (c.BuyerType == 1 || c.BuyerType == 2)).ToList();
                }

                json += "[";
                if (string.IsNullOrEmpty(Request.Params["IsAll"]))
                {
                    json += "{\"id\":\"0\",\"text\":\"所有客户\"},";
                }

                foreach (var item in list)
                {
                    json += "{\"id\":\"" + item.BuyerId + "\",\"text\":\"" + item.FullName + "\"},";
                }
                if (json.Substring(json.Length - 1, 1) == ",")
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json += "]";
            }
            else
            {
                json = "[{\"id\":\"0\",\"text\":\"所有客户\"}]";
            }
            return json;
        }

        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetPOSPType()
        {
            List<ProductClassInfo> list = ProductClassInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            string json = string.Empty;
            json = "[";
            json += "{\"ClassId\":\"-1\",\"Name\":\"所有分类\"},";
            foreach (var item in list)
            {
                json += "{\"ClassId\":\"" + item.ClassId + "\",\"Name\":\"" + item.Name + "\"},";
            }
            if (json.Substring(json.Length - 1, 1) == ",")
            {
                json = json.Substring(0, json.Length - 1);
            }
            json += "]";
            return json;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        public string GetPOSList()
        {
            string json = string.Empty;
            int type = 0;
            int pType = -1;
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                type = int.Parse(Request.Params["type"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["pType"]))
            {
                pType = int.Parse(Request.Params["pType"]);
            }
            string errMsg = string.Empty;
            int userId = int.Parse(Request.Cookies["userId"].Value);
            PageDataView pd = ProductInfoBll.GetPOSProductList(1, 100, "", userId, type, pType, out errMsg);
            DataTable dt = new DataTable();
            if (pd != null && pd.DataTable != null && pd.DataTable.Rows.Count > 0)
            {
                dt = pd.DataTable;
                dt.Columns.Add("picSrc", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Quantity"] == null || string.IsNullOrEmpty(dr["Quantity"].ToString()))
                    {
                        dr["Quantity"] = 0;
                    }
                    if (dr["SaleStoreQuantity"] == null || string.IsNullOrEmpty(dr["SaleStoreQuantity"].ToString()))
                    {
                        dr["SaleStoreQuantity"] = 1000;
                    }
                    if (dr["Price1"] == null || string.IsNullOrEmpty(dr["Price1"].ToString()))
                    {
                        dr["Price1"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dr["picName"].ToString()))
                    {
                        byte[] img = (byte[])dr["picName"];
                        string picUrl = Server.MapPath("\\UploadImgs\\") + userId + "\\" + dr["PicId"].ToString() + ".jpg";
                        if (!Directory.Exists(picUrl))
                        {
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                            {
                                Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                            }
                            string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                            string imgsSrc = dr["PicId"].ToString() + ".jpg";
                            path = path + imgsSrc;
                            imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                            System.IO.File.WriteAllBytes(picUrl, img);
                            dr["picSrc"] = imgsSrc;
                        }
                        else
                        {
                            string imgsSrc = "/UploadImgs/" + userId + "/" + dr["PicId"].ToString() + ".jpg";
                            dr["picSrc"] = imgsSrc;
                        }
                    }
                }
                dt.Columns.RemoveAt(4);
                json += "{\"total\":\"" + pd.TotalNum + "\",\"rows\":";
                json += JsonConvert.SerializeObject(dt);
                json += "}";
            }
            else
            {
                json = "{\"rows\":[],\"total\":\"0\"}";
            }
            return json;
        }

        /// <summary>
        /// 商品上架保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SavePOS()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]) && !string.IsNullOrEmpty(Request.Params["type"]))
            {
                string str = Request.Params["str"];
                string type = Request.Params["type"];
                List<SellerProductOnline> model = new List<SellerProductOnline>();
                List<SellerProductOnlineCustomerStrategy> smodel = new List<SellerProductOnlineCustomerStrategy>();
                if (type == "0")
                {
                    model = (List<SellerProductOnline>)JsonConvert.DeserializeObject(str, typeof(List<SellerProductOnline>));
                    foreach (var item in model)
                    {
                        item.SellerId = int.Parse(Request.Cookies["userId"].Value);
                    }
                    if (SellerProductOnlineBll.Save(model))
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    smodel = (List<SellerProductOnlineCustomerStrategy>)JsonConvert.DeserializeObject(str, typeof(List<SellerProductOnlineCustomerStrategy>));
                    foreach (var item in smodel)
                    {
                        item.SellerId = int.Parse(Request.Cookies["userId"].Value);
                    }
                    if (SellerProductOnlineCustomerStrategyBll.Save(smodel))
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetPOSProductInfo()
        {
            if (!string.IsNullOrEmpty(Request.Params["pid"]))
            {

            }
            return "";
        }
        #endregion

        #region 订单管理
        public ActionResult OrderControl()
        {
            string type = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                if (Request.Params["type"] == "msg")
                {
                    type = "-2";
                }

            }

            ViewBag.Type = type;
            return View();
        }

        public void Bind(ReportViewer ReportViewer1, string reportName, List<ReportParameter> rp,DataTable dt)
        {
            try
            {
                ReportViewer1.LocalReport.ReportPath = Common.GetRdlcPath(reportName);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dt));
                ReportViewer1.LocalReport.SetParameters(rp);

                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.ZoomMode = ZoomMode.PageWidth;
                ReportViewer1.LocalReport.Refresh();
                //Export(ReportViewer1, reportName);

            }
            catch (Exception ex)
            {

            }
        }
        private void Export(ReportViewer ReportViewer1, string reportName)
        {
            string FileName = string.Format("{0}{1}.pdf", reportName, "");
            string Format = "PDF";
            string FullPath = ReportExport(ReportViewer1, FileName, Format);
            string Url = FullPath;
        }

        public string ReportExport(ReportViewer ReportViewer1, string FileName, string Format = "Excel")
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                   Format, null, out mimeType, out encoding, out extension,
                   out streamids, out warnings);

                string FilePath = HttpContext.Server.MapPath("~/") + string.Format(@"\报表导出\");
                if (!System.IO.Directory.Exists(FilePath))
                    System.IO.Directory.CreateDirectory(FilePath);
                System.IO.FileStream fs = new System.IO.FileStream(FilePath + FileName, System.IO.FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                FileInfo info = new FileInfo(FilePath + FileName);

                //System.Diagnostics.Process.Start(FilePath + FileName);

                return (FilePath + FileName);
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EPointName", typeof(string));
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["EPointName"] = i;
                dt.Rows.Add(dr);
            }

            return dt;

        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        public string GetOrderRun()
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["customerId"]) && !string.IsNullOrEmpty(Request.Params["startTime"]) && !string.IsNullOrEmpty(Request.Params["endTime"]))
            {
                string type = string.Empty;
                string errMsg = string.Empty;
                string content = string.Empty;
                int page = 1;
                int pageSize = 10;
                if (!string.IsNullOrEmpty(Request.Params["page"]))
                {
                    page = int.Parse(Request.Params["page"]);
                }
                if (!string.IsNullOrEmpty(Request.Params["pagesize"]))
                {
                    pageSize = int.Parse(Request.Params["pagesize"]);
                }
                if (!string.IsNullOrEmpty(Request.Params["type"]))
                {
                    type = Request.Params["type"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.Params["orderStatus"]))
                    {
                        type = Request.Params["orderStatus"];
                    }
                }
                if (!string.IsNullOrEmpty(Request.Params["scontent"]))
                {
                    content = Request.Params["scontent"];
                }
                int customerId = int.Parse(Request.Params["customerId"]);
                DateTime start = DateTime.Parse(DateTime.Parse(Request.Params["startTime"]).ToString("yyyy-MM-dd 00:00:00"));
                DateTime end = DateTime.Parse(DateTime.Parse(Request.Params["endTime"]).ToString("yyyy-MM-dd 23:59:59"));
                PageDataView pdv = SellerOrderRunBll.GetOrderRunList(page, pageSize, "", int.Parse(Request.Cookies["userId"].Value), start, end, content, out errMsg, customerId, type);
                if (pdv != null && pdv.DataTable != null && pdv.DataTable.Rows.Count > 0)
                {
                    DataTable data = pdv.DataTable;
                    //foreach (DataRow dr in data.Rows)
                    //{
                    //    if (dr["OrderDate"] != null && !string.IsNullOrEmpty(dr["OrderDate"].ToString()))
                    //    {
                    //        dr["OrderDate"] = Convert.ToDateTime(dr["OrderDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    //    }
                    //}

                    string str = JsonConvert.SerializeObject(data);
                    json += "{";
                    json += "\"total\":\"" + pdv.TotalNum + "\",\"rows\":" + str + "";
                    json += "}";
                }
                else
                {
                    json = "{\"total\":\"0\",\"rows\":[]}";
                }
            }
            else
            {
                json = "{\"total\":\"0\",\"rows\":[]}";
            }
            return json;
        }

        /// <summary>
        /// 订单处理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderProcess()
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                ViewBag.OrderId = Request.Params["id"];

                int orderId = int.Parse(Request.Params["id"]);
                
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("ProductFullName", typeof(string));
                dt.Columns.Add("ProductSpec", typeof(string));
                dt.Columns.Add("Unit", typeof(string));
                dt.Columns.Add("num", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("TotalPrice", typeof(string));
                dt.Columns.Add("Notes", typeof(string));

                SellerOrderRun model = SellerOrderRunBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && c.OrderId == orderId).First();
                rp.Add(new ReportParameter("pOrderCode", model.Code));
                rp.Add(new ReportParameter("pTotalPriceInt", model.Amount.ToString("0.00")));
                rp.Add(new ReportParameter("pTotalNum", model.ProductQuantity.ToString()));
                rp.Add(new ReportParameter("pTotalPrice", Common.CmycurD(model.Amount.ToString("0.00"))));

                SellerCustomerInfo bmodel = SellerCustomerInfoBll.GetList(Convert.ToInt32(Request.Cookies["UserId"].Value)).FirstOrDefault(c => c.BuyerId == model.BuyerId);
                rp.Add(new ReportParameter("pBuyer", bmodel.FullName));
                string errMsg = string.Empty;
                SellerInfo iModel = SellerInfoBll.GetModel(model.SellerId,out errMsg);
                if (iModel != null)
                {
                    rp.Add(new ReportParameter("pCompanyName", iModel.CompanyName));
                }
                if (bmodel.AreaId != 0)
                {
                    SellerAreaInfo saModel = SellerAreaInfoBll.GetList().FirstOrDefault(c => c.AreaId == bmodel.AreaId && c.SellerId == model.SellerId);
                    if (saModel != null)
                    {
                        rp.Add(new ReportParameter("pAreaName", saModel.FullName));
                    }
                }
                #region 添加对应的商品数据
                List<SellerOrderDetail> lists = SellerOrderDetailBll.GetList().Where(c => c.OrderId == model.OrderId && c.BuyerId == model.BuyerId).ToList();
                foreach (var list in lists)
                {
                    DataRow dr = dt.NewRow();
                    dr["num"] = list.ProductQuantity.ToString();
                    dr["Price"] = list.ProductPrice.ToString("0.00");
                    dr["TotalPrice"] = list.ProductPrice.ToString("0.00");
                    dr["Notes"] = list.Notes;
                   
                    ProductInfo piModel = ProductInfoBll.GetModel(model.SellerId,list.ProductId,out errMsg);
                    if (piModel != null)
                    {
                        dr["Code"] = piModel.ProductCode;
                        dr["ProductFullName"] = piModel.ProductFullName;
                        dr["ProductSpec"] = piModel.ProductSpec;
                        dr["Unit"] = piModel.ProductUnit;
                    }
                    dt.Rows.Add(dr);
                }
                #endregion

            }
            if (Request.Cookies["userId"] != null && !string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                string errMsg = string.Empty;
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                if (model != null)
                {
                    ViewBag.UName = model.CompanyName;
                }
            }

            #region 加载reportiew
            string reportName = string.Empty;
            reportName = "订单详细报表";
            ReportViewer rv = new ReportViewer();


            Bind(rv, reportName, rp,dt);
            reportName = reportName + "-" + Request.Cookies["userId"].Value;
            string FileName = string.Format("{0}{1}.pdf", reportName, "");
            string Format = "PDF";
            string FullPath = ReportExport(rv, FileName, Format);
            #endregion

            ViewBag.IframeUrl = FullPath;

            return View();
        }

        /// <summary>
        /// 订单处理订单下边的商品
        /// </summary>
        /// <returns></returns>
        public string GetOPProductList()
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["OrderId"]))
            {
                string errMsg = string.Empty;
                int page = 1;
                int pageSize = 10;
                int OrderId = 0;
                if (!string.IsNullOrEmpty(Request.Params["page"]))
                {
                    page = int.Parse(Request.Params["page"]);
                }
                if (!string.IsNullOrEmpty(Request.Params["pagesize"]))
                {
                    pageSize = int.Parse(Request.Params["pagesize"]);
                }
                OrderId = int.Parse(Request.Params["OrderId"]);
                PageDataView pdv = SellerOrderDetailBll.GetOPProductList(page, pageSize, "", int.Parse(Request.Cookies["userId"].Value), OrderId, out errMsg);
                if (pdv != null && pdv.DataTable != null && pdv.DataTable.Rows.Count > 0)
                {
                    DataTable data = pdv.DataTable;
                    string str = JsonConvert.SerializeObject(data);
                    json += "{";
                    json += "\"total\":\"" + pdv.TotalNum + "\",\"rows\":" + str + "";
                    json += "}";
                }
                else
                {
                    json = "{\"total\":\"0\",\"rows\":[]}";
                }
            }
            else
            {
                json = "{\"total\":\"0\",\"rows\":[]}";
            }
            return json;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetOrderInfo()
        {
            string errMsg = string.Empty;
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["OrderId"]))
            {
                int orderId = int.Parse(Request.Params["OrderId"]);
                SellerOrderRun model = SellerOrderRunBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value) && c.OrderId == orderId).First();
                if (model != null)
                {
                    //BuyerInfo bmodel = BuyerInfoBll.GetModel(model.BuyerId, out errMsg);
                    SellerCustomerInfo bmodel = SellerCustomerInfoBll.GetList(Convert.ToInt32(Request.Cookies["UserId"].Value)).FirstOrDefault(c => c.BuyerId == model.BuyerId);
                    json = JsonConvert.SerializeObject(model);
                    json = json.Replace("}", ",\"CompanyName\":\"" + bmodel.FullName + "\"}");
                }
                else
                {
                    json = JsonConvert.SerializeObject(model);
                    json = json.Replace("}", ",\"CompanyName\":\"\"}");
                }
            }
            return json;
        }

        //取消订单
        [HttpPost]
        public string UnSubscribeOrder()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                if (model != null)
                {
                    model.OrderState = 11;
                    if (SellerOrderRunBll.Update(model))
                    {
                        return "1";//成功
                    }
                    else
                    {
                        return "0";//失败
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 卖家恢复订单
        /// </summary>
        /// <returns></returns>
        public string resumedOrder()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                if (model != null)
                {
                    model.OrderState = 0;
                    if (SellerOrderRunBll.Update(model))
                    {
                        return "1";//成功
                    }
                    else
                    {
                        return "0";//失败
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 卖家确认订单
        /// </summary>
        /// <returns></returns>
        public string FirmOrder()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                if (model != null)
                {
                    model.OrderState = 10;
                    if (SellerOrderRunBll.Update(model))
                    {
                        return "1";//成功
                    }
                    else
                    {
                        return "0";//失败
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 确认收款
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string OrderFinish()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                if (model != null)
                {
                    model.PayAmount = model.Amount;
                    if (model.OrderState == 2)
                    {
                        model.OrderState = 100;
                    }
                    else
                    {
                        model.PayState = 11;
                    }
                    if (SellerOrderRunBll.Update(model))
                    {
                        return "1";//成功
                    }
                    else
                    {
                        return "0";//失败
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <returns></returns>
        public string GetOPCars()
        {
            List<SellerDeliveryCarInfo> list = SellerDeliveryCarInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取车辆个数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCarCount()
        {
            List<SellerDeliveryCarInfo> list = SellerDeliveryCarInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["userId"].Value)).ToList();
            return list.Count.ToString();
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SaveOPDeliver()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                int id = int.Parse(Request.Params["id"]);
                if (!string.IsNullOrEmpty(Request.Params["carId"]))
                {
                    int carId = int.Parse(Request.Params["carId"]);
                    string errMsg = string.Empty;
                    //添加车辆配送信息
                    SellerDeliveryRun rmodel = new SellerDeliveryRun();
                    SellerDeliveryCarInfo cModel = SellerDeliveryCarInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), "", out errMsg, carId);
                    if (cModel == null)
                    {
                        return "";
                    }
                    rmodel.SellerId = cModel.SellerId;
                    rmodel.CarId = carId;
                    rmodel.Driver = cModel.Driver;
                    rmodel.MobilePhone = cModel.MobilePhone;
                    rmodel.OrderId = id;
                    int NewId = SellerDeliveryRunBll.InsertWithReturnId(rmodel, out errMsg);
                    if (NewId <= 0)
                    {
                        return "2";//添加失败
                    }
                    SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                    if (model != null)
                    {
                        model.OrderState = 12;
                        if (SellerOrderRunBll.Update(model))
                        {
                            List<SellerOrderDetail> lists = SellerOrderDetailBll.GetList(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                            bool isTrue = true;
                            foreach (var list in lists)
                            {
                                SellerStockPile OldModel = new SellerStockPile();
                                OldModel.SellerId = list.SellerId;
                                OldModel.ProductId = list.ProductId;
                                OldModel.StoreHouseId = 0;
                                SellerStockPile sspModel = SellerStockPileBll.GetModel(OldModel);
                                if (sspModel != null)
                                {
                                    sspModel.Quantity = sspModel.Quantity - list.ProductQuantity;
                                    isTrue = isTrue && SellerStockPileBll.Update(sspModel);
                                }
                                else
                                {//库存表没数据  添加数据

                                    //isTrue = false;
                                }

                            }
                            if (!isTrue)
                            {
                                SellerDeliveryRunBll.Delete(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                                model.OrderState = 10;
                                SellerOrderRunBll.Update(model);
                                return "3";//更新库存失败
                            }
                            else
                            {

                                return "1";//成功
                            }
                        }
                        else
                        {
                            SellerDeliveryRunBll.Delete(NewId, int.Parse(Request.Cookies["userId"].Value), out errMsg);
                            return "0";//失败
                        }
                    }
                }
                else
                {
                    string errMsg = string.Empty;

                    SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                    if (model != null)
                    {

                        model.OrderState = 12;
                        if (SellerOrderRunBll.Update(model))
                        {
                            List<SellerOrderDetail> lists = SellerOrderDetailBll.GetList(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                            bool isTrue = true;
                            foreach (var list in lists)
                            {
                                SellerStockPile OldModel = new SellerStockPile();
                                OldModel.SellerId = list.SellerId;
                                OldModel.ProductId = list.ProductId;
                                OldModel.StoreHouseId = 0;
                                SellerStockPile sspModel = SellerStockPileBll.GetModel(OldModel);
                                if (sspModel != null)
                                {
                                    sspModel.Quantity = sspModel.Quantity - list.ProductQuantity;
                                    isTrue = isTrue && SellerStockPileBll.Update(sspModel);
                                }
                                else
                                {
                                    //isTrue = false;
                                }

                            }
                            if (!isTrue)
                            {
                                SellerDeliveryRunBll.Delete(int.Parse(Request.Cookies["userId"].Value), id, out errMsg);
                                model.OrderState = 0;
                                SellerOrderRunBll.Update(model);
                                return "3";//更新库存失败
                            }
                            return "1";//成功
                        }
                        else
                        {
                            return "0";//失败
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 订单管理保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string OP_Order_Save()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))//!string.IsNullOrEmpty(Request.Params["payCount"]) && !string.IsNullOrEmpty(Request.Params["totalNum"]) && !string.IsNullOrEmpty(Request.Params["totalPrice"]) && !string.IsNullOrEmpty(Request.Params["pids"])
            {
                string remark = Request.Params["abstract"];
                int orderId = int.Parse(Request.Params["id"]);
                string errMsg = string.Empty;
                SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), orderId, out errMsg);
                if (model != null)
                {
                    model.Notes = remark;
                    if (SellerOrderRunBll.Update(model))//更新记录表
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }

                #region 废弃代码,保存全部

                //string payCount = Request.Params["payCount"];
                //int totalNum = int.Parse(Request.Params["totalNum"]);
                //string totalPrice = Request.Params["totalPrice"];
                //string pids = Request.Params["pids"];
                //string errMsg = string.Empty;
                //SellerOrderRun model = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["userId"].Value), orderId, out errMsg);
                //if (model != null)
                //{
                //    model.PayAmount = float.Parse(payCount);
                //    model.ProductQuantity = totalNum;
                //    model.Amount = float.Parse(totalPrice);

                //    if (!SellerOrderRunBll.Update(model))//更新记录表
                //    {
                //        return "2";
                //    }
                //    List<SellerOrderDetail> models = SellerOrderDetailBll.GetList(int.Parse(Request.Cookies["userId"].Value), orderId, out errMsg);
                //    List<int> RemoveIds = new List<int>();
                //    if (models != null && models.Count > 0)
                //    {
                //        foreach (var item in models)
                //        {
                //            if (pids.IndexOf(item.ProductId.ToString()) >= 0)
                //            {
                //                RemoveIds.Add(models.IndexOf(item));
                //            }
                //        }
                //        foreach (var i in RemoveIds)
                //        {
                //            models.RemoveAt(i);
                //        }
                //        if (models != null && models.Count > 0)
                //        {
                //            if (SellerOrderDetailBll.Delete(models, out errMsg))//更新明细表
                //            {
                //                return "1";
                //            }
                //            else
                //            {
                //                return "2";
                //            }
                //        }
                //    }
                //}

                #endregion
            }
            return "";
        }

        /// <summary>
        /// 获取订单是否符合强制确认收货后货条件  1:符合,0:不符合
        /// </summary>
        /// <returns></returns>
        public string OC_GetForcedReceipt()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                int orderId = int.Parse(Request.Params["id"]);
                bool res = SellerOrderRunBll.GetForcedReceipt(int.Parse(Request.Cookies["UserId"].Value), orderId);
                if (res)
                    return "1";
                else
                    return "0";
            }
            return "";
        }

        /// <summary>
        /// 强制确认收货
        /// </summary>
        /// <returns></returns>
        public string OC_ForcedReceiptSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int orderId = int.Parse(Request.Params["id"]);
                SellerOrderRun mode = SellerOrderRunBll.GetModel(int.Parse(Request.Cookies["UserId"].Value), orderId, out errMsg);
                mode.OrderState = 100;
                if (SellerOrderRunBll.Update(mode))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }


            }
            return "";
        }
        #endregion

        #region 库存状态
        public ActionResult InventoryLevelsView()
        {
            return View();
        }

        public string InventoryLevelsProductList()
        {
            int searchType = 0;
            string content = string.Empty;
            int type = -1;
            int status = -1;//表示全部状态
            int current = 1;
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request.Params["searchType"]))
            {
                searchType = int.Parse(Request.Params["searchType"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["searchContent"]))
            {
                content = Request.Params["searchContent"];
            }
            if (!string.IsNullOrEmpty(Request.Params["page"]))
            {
                current = int.Parse(Request.Params["page"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["rows"]))
            {
                pagesize = int.Parse(Request.Params["rows"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                type = int.Parse(Request.Params["type"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["status"]))
            {
                status = int.Parse(Request.Params["status"]);
            }
            string errMsg = string.Empty;
            string userId = Request.Cookies["userId"].Value;
            PageDataView data = SellerStockPileBll.GetProductList(current, pagesize, "", content, searchType, int.Parse(userId), out errMsg, status, type);
            DataTable dt = new DataTable();
            string json = string.Empty;
            if (data != null && data.DataTable != null && data.DataTable.Rows.Count > 0)
            {
                dt = data.DataTable;
                dt.Columns.Add("totalPrice", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["totalPrice"] = (Convert.ToInt32(dr["Quantity"].ToString()) * float.Parse(dr["Cost"].ToString())).ToString("0.00");
                }
            }
            string str = JsonConvert.SerializeObject(dt);

            json = string.Empty;
            json += "{";
            json += "\"total\":\"" + data.TotalNum + "\",\"rows\":" + str + "";
            json += "}";
            return json;
        }

        /// <summary>
        /// 获取商品对应的订单列表
        /// </summary>
        /// <returns></returns>
        public string InventoryLevelsOrderList()
        {
            string json = string.Empty;
            if (string.IsNullOrEmpty(Request.Params["pid"]))
            {
                json = string.Empty;
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
                return json;
            }
            string ProductId = Request.Params["pid"];
            string errMsg = string.Empty;
            string userId = Request.Cookies["userId"].Value;
            PageDataView data = SellerBuyInfoBll.GetOrderList(1, 9999, "", int.Parse(userId), ProductId, out errMsg);
            DataTable dt = new DataTable();
            int total = 0;
            if (data != null && data.DataTable != null && data.DataTable.Rows.Count > 0)
            {
                dt = data.DataTable;
                total = data.TotalNum;
            }
            string str = JsonConvert.SerializeObject(dt);

            json = string.Empty;
            json += "{";
            json += "\"total\":\"" + total + "\",\"rows\":" + str + "";
            json += "}";
            return json;
        }

        #endregion

        #region 促销信息
        public ActionResult PromotionInfo()
        {
            return View();
        }

        /// <summary>
        /// 关联设备类型
        /// </summary>
        /// <returns></returns>
        public string GetTerminal()
        {
            string json = string.Empty;
            json += "[";
            json += "{";
            json += "  \"id\":\"0\",\"text\":\"全部\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"1\",\"text\":\"微信\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"2\",\"text\":\"手机\"";
            json += "},";
            json += "{";
            json += "  \"id\":\"3\",\"text\":\"APP\"";
            json += "}";

            json += "]";
            return json;
        }

        /// <summary>
        /// 选择商品查询列表
        /// </summary>
        /// <returns></returns>
        public string GetPIProductList()
        {
            string json = string.Empty;
            string errMsg = string.Empty;
            string type = Request.Params["type"];
            string content = Request.Params["content"];
            List<ProductInfo> list = new List<Model.ProductInfo>();
            if (!string.IsNullOrEmpty(type))
            {
                if (!string.IsNullOrEmpty(content))
                {
                    if (type == "0")
                    {
                        list = ProductInfoBll.GetList().Where(c => c.Status != 100 && c.SellerId == 1000 && (c.ProductFullName.IndexOf(content) >= 0 || c.ProductShortName.IndexOf(content) >= 0 || c.ProductCode.IndexOf(content) >= 0)).ToList();
                    }
                    else if (type == "1")
                    {
                        list = ProductInfoBll.GetList().Where(c => c.Status != 100 && c.SellerId == 1000 && c.ProductCode.IndexOf(content) >= 0).ToList();
                    }
                    else if (type == "2")
                    {
                        list = ProductInfoBll.GetList().Where(c => c.Status != 100 && c.SellerId == 1000 && (c.ProductFullName.IndexOf(content) >= 0 || c.ProductShortName.IndexOf(content) >= 0)).ToList();
                    }
                }
                else
                {
                    list = ProductInfoBll.GetList().Where(c => c.Status != 100 && c.SellerId == 1000).ToList();
                }
                string str = JsonConvert.SerializeObject(list);
                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + list.Count + "\",\"rows\":" + str + "";
                json += "}";
            }
            else
            {
                json = string.Empty;
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }

        /// <summary>
        /// 商品下拉框
        /// </summary>
        /// <returns></returns>
        public string GetPIPList()
        {
            return JsonConvert.SerializeObject(ProductInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value)).ToList());
        }
        /// <summary>
        /// 促销信息保存
        /// </summary>
        /// <returns></returns>
        public string PI_AddSave()
        {
            string json = string.Empty;
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string imgSrc = Request.Params["picSrc"];
                string mapPath = string.Empty;
                json = Request.Params["str"];
                SaleAds model = (SaleAds)JsonConvert.DeserializeObject(json, typeof(SaleAds));
                PictureInfo infoModel = new PictureInfo();
                model.SellerId = int.Parse(Request.Cookies["UserId"].Value);
                if (model.Picture == 0)
                {
                    if (!string.IsNullOrEmpty(imgSrc))
                    {
                        mapPath = HttpContext.Server.MapPath(imgSrc);
                        FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read);
                        Byte[] bytPic = new Byte[fs.Length];
                        infoModel.Size = Convert.ToInt32(fs.Length.ToString());
                        fs.Read(bytPic, 0, bytPic.Length);
                        fs.Close();
                        infoModel.Resource = bytPic;
                        infoModel.SellerId = int.Parse(Request.Cookies["userId"].Value);
                        Image img = Image.FromFile(mapPath);
                        infoModel.Width = short.Parse(img.Width.ToString());
                        infoModel.Height = short.Parse(img.Height.ToString());
                        img.Dispose();
                        infoModel.Format = "jpg";

                        int pid = PictureInfoBll.InsertWithReturnId(infoModel, out errMsg);
                        if (pid > 0)
                        {
                            model.Picture = pid;
                        }
                        else
                        {
                            return "3";
                        }
                    }
                    else
                    {
                        model.Picture = null;
                    }
                }


                if (model.AdsId != 0)//编辑
                {
                    SaleAds newModel = SaleAdsBll.GetModel(model.AdsId, model.SellerId, out errMsg);
                    if (newModel.Title == model.Title || (newModel != null && newModel.Title != model.Title && !SaleAdsBll.CheckName(model.Title, model.SellerId, out errMsg)))
                    {
                        model.IsUsed = newModel.IsUsed;
                        if (SaleAdsBll.Update(model))
                        {
                            if (Directory.Exists(mapPath))
                            {
                                System.IO.File.Delete(mapPath);
                            }
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {//新增
                    model.IsUsed = false;
                    if (!SaleAdsBll.CheckName(model.Title, model.SellerId, out errMsg))
                    {
                        if (SaleAdsBll.Insert(model))
                        {
                            if (Directory.Exists(mapPath))
                            {
                                System.IO.File.Delete(mapPath);
                            }
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";
                    }
                }
            }
            return json;
        }

        /// <summary>
        /// 获取促销信息
        /// </summary>
        /// <returns></returns>
        public string GetPIModelList()
        {
            string json = string.Empty;
            List<SaleAds> list = SaleAdsBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value)).ToList();
            if (list != null && list.Count > 0)
            {
                string errMsg = string.Empty;
                string userId = Request.Cookies["UserId"].Value;
                //用config存储图片路径,用lman存储图片id
                foreach (var item in list)
                {
                    #region 关联商品
                    if (item.ProductId != 0)
                    {
                        ProductInfo pmodel = ProductInfoBll.GetModel(int.Parse(userId), item.ProductId, out errMsg);
                        if (pmodel != null)
                        {
                            item.Notes = pmodel.ProductFullName;
                        }
                    }
                    #endregion

                    if (item.Picture != null && item.Picture != 0)
                    {
                        PictureInfo pmodel = PictureInfoBll.GetModel((int)item.Picture, int.Parse(userId), out errMsg);
                        if (pmodel != null)
                        {
                            byte[] img = pmodel.Resource;
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + item.Picture + ".jpg")))
                            {
                                if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                                {
                                    Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                                }
                                string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                                string imgsSrc = item.Picture.ToString() + ".jpg";
                                path = path + imgsSrc;
                                imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                                System.IO.File.WriteAllBytes(path, img);
                                item.Config = imgsSrc;
                                item.LMan = (int)item.Picture;
                            }
                            else
                            {
                                string imgsSrc = "/UploadImgs/" + userId + "/" + item.Picture.ToString() + ".jpg";
                                item.Config = imgsSrc;
                                item.LMan = (int)item.Picture;
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        item.Config = "";
                        item.LMan = 0;
                    }
                }
                string str = JsonConvert.SerializeObject(list);
                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + list.Count + "\",\"rows\":" + str + "";
                json += "}";
            }
            else
            {
                json = string.Empty;
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }

        //删除
        public string PI_DelSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                if (SaleAdsBll.Delete(id, int.Parse(Request.Cookies["UserId"].Value), out errMsg))
                {
                    return "1";
                }
                return "0";
            }
            return "";
        }

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <returns></returns>
        public string PI_UpdateIsUsed()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int Adsid = int.Parse(Request.Params["id"]);
                SaleAds model = SaleAdsBll.GetModel(Adsid, int.Parse(Request.Cookies["UserId"].Value), out errMsg);
                SaleAds OldModel = SaleAdsBll.GetModel(true, int.Parse(Request.Cookies["UserId"].Value), out errMsg);//获取原来被启用的
                if (OldModel != null)
                {
                    OldModel.IsUsed = false;
                    SaleAdsBll.Update(OldModel);
                }
                if (model != null)
                {
                    model.IsUsed = true;
                    if (SaleAdsBll.Update(model))
                    {
                        return "1";
                    }
                    return "0";
                }
                return "0";
            }
            return "";
        }
        #endregion

        #region 销售报表
        public ActionResult SaleReport()
        {
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                string type = Request.Params["type"];
                if (type == "online")
                {
                    ViewBag.Type = 1;
                }
                else if (type == "outline")
                {
                    ViewBag.Type = 0;
                }
            }
            return View();
        }

        /// <summary>
        /// 销售报表汇出
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            if (!string.IsNullOrEmpty(Request.Params["customerId"]) && !string.IsNullOrEmpty(Request.Params["orderStatus"]) && !string.IsNullOrEmpty(Request.Params["startTime"]) && !string.IsNullOrEmpty(Request.Params["endTime"]))
            {
                string type = string.Empty;
                string errMsg = string.Empty;
                string orderStatus = "-1";
                orderStatus = Request.Params["orderStatus"];
                int page = 1;
                int pageSize = 10000;
                int customerId = int.Parse(Request.Params["customerId"]);
                DateTime start = DateTime.Parse(DateTime.Parse(Request.Params["startTime"]).ToString("yyyy-MM-dd 00:00:00"));
                DateTime end = DateTime.Parse(DateTime.Parse(Request.Params["endTime"]).ToString("yyyy-MM-dd 23:59:59"));
                PageDataView pdv = SellerOrderRunBll.GetOrderRunList(page, pageSize, "", int.Parse(Request.Cookies["userId"].Value), start, end, "", out errMsg, customerId, orderStatus);
                DataTable dt = new DataTable();
                dt.Columns.Add("订单编号", typeof(string));
                dt.Columns.Add("下单时间", typeof(string));
                dt.Columns.Add("客户名称", typeof(string));
                dt.Columns.Add("订单金额", typeof(string));
                dt.Columns.Add("订单状态", typeof(string));
                dt.Columns.Add("支付状态", typeof(string));
                dt.Columns.Add("摘要", typeof(string));
                if (pdv != null && pdv.DataTable != null && pdv.DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in pdv.DataTable.Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["订单编号"] = dr["Code"];
                        newRow["下单时间"] = dr["OrderDate"];
                        newRow["客户名称"] = dr["FullName"];
                        newRow["订单金额"] = dr["Amount"];
                        string OrderState = string.Empty;
                        switch (dr["OrderState"].ToString())
                        {
                            case "1":
                                OrderState = "买家已取消";
                                break;
                            case "2":
                                OrderState = "确认收货";
                                break;
                            case "10":
                                OrderState = "已确认";
                                break;
                            case "11":
                                OrderState = "已取消";
                                break;
                            case "12":
                                OrderState = "已发货";
                                break;
                            case "100":
                                OrderState = "完成";
                                break;
                            case "0":
                                OrderState = "待确认";
                                break;
                        }
                        newRow["订单状态"] = OrderState;
                        string PayState = string.Empty;
                        switch (dr["PayState"].ToString())
                        {
                            case "0":
                                PayState = "未支付";
                                break;
                            case "11":
                                PayState = "货到付款(现金支付)";
                                break;
                        }
                        newRow["支付状态"] = PayState;
                        newRow["摘要"] = dr["Notes"];
                        dt.Rows.Add(newRow);
                    }

                }
                string name = "销售报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string FullName = Server.MapPath("~/") + "ExportFile/" + name;

                Common.outputExcel(dt, name, FullName);
                string url = Request.Url.Authority;
                url = "/ExportFile/" + name;
                return url;
            }
            return "";
        }


        #endregion

        #region 支付方式设置
        public ActionResult PayMentMode()
        {
            return View();
        }

        /// <summary>
        /// 微信/支付宝设置保存
        /// </summary>
        /// <returns></returns>
        public string ZFBSetSave()
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["str"]) && !string.IsNullOrEmpty(Request.Params["type"]))
            {
                json = Request.Params["str"];
                string type = Request.Params["type"];
                SellerOnlinePaymentInfo model = (SellerOnlinePaymentInfo)JsonConvert.DeserializeObject(json, typeof(SellerOnlinePaymentInfo));
                model.SellerId = int.Parse(Request.Cookies["UserId"].Value);
                if (type == "edit")
                {
                    if (SellerOnlinePaymentInfoBll.Update(model))
                    {
                        return "1";
                    }
                    else
                    {
                        return "2";
                    }
                }
                else if (type == "add")
                {
                    if (SellerOnlinePaymentInfoBll.Insert(model))
                    {
                        return "1";
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        public string GetPMStatus()
        {
            if (Request.Cookies["UserId"] != null && !string.IsNullOrEmpty(Request.Cookies["UserId"].Value))
            {
                string str = string.Empty;
                List<SellerOnlinePaymentInfo> list = SellerOnlinePaymentInfoBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value)).ToList();
                if (list != null && list.Count > 0)
                {
                    str = "{";
                    foreach (var item in list)
                    {
                        if (item.PayType == 1)//微信
                        {
                            str += "\"PayType1\":\"" + item.PayType + "\",\"MchId1\":\"" + item.MchId + "\",\"IsUsed1\":\"" + (item.IsUsed ? "1" : "0") + "\",";
                        }
                        if (item.PayType == 2)//支付宝
                        {
                            str += "\"PayType2\":\"" + item.PayType + "\",\"MchId2\":\"" + item.MchId + "\",\"IsUsed2\":\"" + (item.IsUsed ? "1" : "0") + "\",";
                        }
                    }
                    if (str.Substring(str.Length - 1, 1) == ",")
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    str += "}";
                    return str;
                }
                return "";
            }
            else
            {
                Response.Redirect("/UserCenter/Login");
                return "";
            }
        }


        /// <summary>
        /// 获取绑定的微信/支付宝信息
        /// </summary>
        /// <returns></returns>
        public string GetPMModel()
        {
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                string type = Request.Params["type"];
                SellerOnlinePaymentInfo model = SellerOnlinePaymentInfoBll.GetList().FirstOrDefault(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value) && c.PayType.ToString() == type);
                return JsonConvert.SerializeObject(model);
            }
            return "";
        }


        /// <summary>
        /// 更改微信/支付宝启用状态
        /// </summary>
        /// <returns></returns>
        public string PM_Change_IsUsed()
        {
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                string type = Request.Params["type"];
                SellerOnlinePaymentInfo model = SellerOnlinePaymentInfoBll.GetList().FirstOrDefault(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value) && c.PayType.ToString() == type);
                if (model != null)
                {
                    if (model.IsUsed)
                    {
                        model.IsUsed = false;
                        if (SellerOnlinePaymentInfoBll.Update(model))
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        model.IsUsed = true;
                        if (SellerOnlinePaymentInfoBll.Update(model))
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        #endregion

        #region 新品上线
        public ActionResult NewLine()
        {
            return View();
        }

        /// <summary>
        /// 获取新品上线列表
        /// </summary>
        /// <returns></returns>
        public string GetNLModelList()
        {
            string json = string.Empty;
            List<NewProductLaunch> list = NewProductLaunchBll.GetList().Where(c => c.SellerId == int.Parse(Request.Cookies["UserId"].Value)).ToList();
            if (list != null && list.Count > 0)
            {
                string errMsg = string.Empty;
                string userId = Request.Cookies["UserId"].Value;
                //用config存储图片路径,用lman存储图片id
                foreach (var item in list)
                {
                    #region 关联商品
                    if (item.ProductId != 0)
                    {
                        ProductInfo pmodel = ProductInfoBll.GetModel(int.Parse(userId), item.ProductId, out errMsg);
                        if (pmodel != null)
                        {
                            item.Notes = pmodel.ProductFullName;
                        }
                    }
                    #endregion
                    if (item.Picture != null && item.Picture != 0)
                    {
                        PictureInfo pmodel = PictureInfoBll.GetModel((int)item.Picture, int.Parse(userId), out errMsg);
                        if (pmodel != null)
                        {
                            byte[] img = pmodel.Resource;
                            if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\" + item.Picture + ".jpg")))
                            {
                                if (!Directory.Exists(Server.MapPath("\\UploadImgs\\" + userId + "\\")))
                                {
                                    Directory.CreateDirectory(Server.MapPath("\\UploadImgs\\" + userId + "\\"));
                                }
                                string path = Server.MapPath("\\UploadImgs\\" + userId + "\\");
                                string imgsSrc = item.Picture.ToString() + ".jpg";
                                path = path + imgsSrc;
                                imgsSrc = "/UploadImgs/" + userId + "/" + imgsSrc;
                                System.IO.File.WriteAllBytes(path, img);
                                item.Config = imgsSrc;
                                item.LMan = (int)item.Picture;
                            }
                            else
                            {
                                string imgsSrc = "/UploadImgs/" + userId + "/" + item.Picture.ToString() + ".jpg";
                                item.Config = imgsSrc;
                                item.LMan = (int)item.Picture;
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        item.Config = "";
                        item.LMan = 0;
                    }
                }
                string str = JsonConvert.SerializeObject(list);
                json = string.Empty;
                json += "{";
                json += "\"total\":\"" + list.Count + "\",\"rows\":" + str + "";
                json += "}";
            }
            else
            {
                json = string.Empty;
                json += "{";
                json += "\"total\":\"0\",\"rows\":[]";
                json += "}";
            }
            return json;
        }

        /// <summary>
        /// 新品上线保存
        /// </summary>
        /// <returns></returns>
        public string NL_AddSave()
        {
            string json = string.Empty;
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string imgSrc = Request.Params["picSrc"];
                string mapPath = string.Empty;
                json = Request.Params["str"];
                NewProductLaunch model = (NewProductLaunch)JsonConvert.DeserializeObject(json, typeof(NewProductLaunch));
                PictureInfo infoModel = new PictureInfo();
                model.SellerId = int.Parse(Request.Cookies["UserId"].Value);
                if (model.Picture == 0)
                {
                    if (!string.IsNullOrEmpty(imgSrc))
                    {
                        mapPath = HttpContext.Server.MapPath(imgSrc);
                        FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read);
                        Byte[] bytPic = new Byte[fs.Length];
                        infoModel.Size = Convert.ToInt32(fs.Length.ToString());
                        fs.Read(bytPic, 0, bytPic.Length);
                        fs.Close();
                        infoModel.Resource = bytPic;
                        infoModel.SellerId = int.Parse(Request.Cookies["userId"].Value);
                        Image img = Image.FromFile(mapPath);
                        infoModel.Width = short.Parse(img.Width.ToString());
                        infoModel.Height = short.Parse(img.Height.ToString());
                        img.Dispose();
                        infoModel.Format = "jpg";

                        int pid = PictureInfoBll.InsertWithReturnId(infoModel, out errMsg);
                        if (pid > 0)
                        {
                            model.Picture = pid;
                        }
                        else
                        {
                            return "3";
                        }
                    }
                    else
                    {
                        model.Picture = null;
                    }
                }


                if (model.Id != 0)//编辑
                {
                    NewProductLaunch newModel = NewProductLaunchBll.GetModel(model.Id, model.SellerId, out errMsg);
                    if (newModel.Title == model.Title || (newModel != null && newModel.Title != model.Title && !NewProductLaunchBll.CheckName(model.Title, model.SellerId, out errMsg)))
                    {
                        if (NewProductLaunchBll.Update(model))
                        {
                            if (Directory.Exists(mapPath))
                            {
                                System.IO.File.Delete(mapPath);
                            }
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {//新增
                    if (!NewProductLaunchBll.CheckName(model.Title, model.SellerId, out errMsg))
                    {
                        if (NewProductLaunchBll.Insert(model))
                        {
                            if (Directory.Exists(mapPath))
                            {
                                System.IO.File.Delete(mapPath);
                            }
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                    else
                    {
                        return "2";
                    }
                }
            }
            return json;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public string NL_DelSave()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                if (NewProductLaunchBll.Delete(id, int.Parse(Request.Cookies["UserId"].Value), out errMsg))
                {
                    return "1";
                }
                return "0";
            }
            return "";
        }

        /// <summary>
        /// 更改启用状态
        /// </summary>
        /// <returns></returns>
        public string NL_UpdateIsUsed()
        {
            if (!string.IsNullOrEmpty(Request.Params["id"]))
            {
                string errMsg = string.Empty;
                int id = int.Parse(Request.Params["id"]);
                NewProductLaunch model = NewProductLaunchBll.GetModel(id, int.Parse(Request.Cookies["UserId"].Value), out errMsg);
                if (model != null)
                {
                    if (model.IsUsed)
                    {
                        model.IsUsed = false;
                        NewProductLaunchBll.Update(model);
                    }
                    else
                    {
                        model.IsUsed = true;
                        NewProductLaunchBll.Update(model);
                    }
                    return "1";

                }
                return "0";
            }
            return "";
        }
        #endregion

        #region 销售出库
        public ActionResult SalesOutOfTheLibrary()
        {
            if (Request.Cookies["userId"] != null && !string.IsNullOrEmpty(Request.Cookies["userId"].Value))
            {
                string errMsg = string.Empty;
                SellerInfo model = SellerInfoBll.GetModel(int.Parse(Request.Cookies["userId"].Value), out errMsg);
                if (model != null)
                {
                    ViewBag.Name = model.ContactName == "" ? model.LoginName : model.ContactName;
                }
            }
            else
            {
                Response.Redirect("/UserCenter/Login");
            }
            ViewBag.OrderListCode = SalersOrderRunBll.GetSalersOrderRunCode(int.Parse(Request.Cookies["userId"].Value));
            return View();
        }

        public string STL_Save()
        {
            if (!string.IsNullOrEmpty(Request.Params["str"]))
            {
                string str = Request.Params["str"];
                JObject Jjson = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(str);
                string list_SalersOrderRun = Jjson["SalersOrderRun"].ToString();
                string list_SalersOrderDetail = Jjson["SalersOrderDetail"].ToString();
                SalersOrderRun smolde = (SalersOrderRun)Newtonsoft.Json.JsonConvert.DeserializeObject(list_SalersOrderRun, typeof(SalersOrderRun));
                List<SalersOrderDetail> smodels = (List<SalersOrderDetail>)Newtonsoft.Json.JsonConvert.DeserializeObject(list_SalersOrderDetail, typeof(List<SalersOrderDetail>));
                string errMsg = string.Empty;
                smolde.SellerId = int.Parse(Request.Cookies["userId"].Value);

                if (SalersOrderRunBll.CheckCode(smolde.SellerId, smolde.Code, out errMsg))
                {
                    return "2";//code重复
                }
                int id = SalersOrderRunBll.InsertWithReturnId(smolde, out errMsg);
                if (id != 0)
                {
                    foreach (var model in smodels)
                    {
                        model.SellerId = smolde.SellerId;
                        model.BuyerId = smolde.BuyerId;
                        model.OrderId = id;
                        model.TotalPrice = float.Parse((model.ProductPrice * model.ProductQuantity).ToString("0.00"));
                    }
                    if (SalersOrderDetailBll.Insert(smodels, int.Parse(Request.Cookies["userId"].Value)))
                    {
                        bool isTrue = true;
                        foreach (var model in smodels)
                        {
                            SellerStockPile sspModel = new SellerStockPile();
                            sspModel.SellerId = model.SellerId;
                            sspModel.Quantity = model.ProductQuantity;
                            sspModel.ProductId = model.ProductId;
                            sspModel.StoreHouseId = 0;
                            sspModel.UpperLimit = 9999;
                            sspModel.LowerLitmit = 0;
                            sspModel.RMan = model.SellerId;
                            sspModel.LMan = model.SellerId;
                            isTrue = isTrue && SellerStockPileBll.UpdateMinusModel(sspModel, out errMsg);
                        }
                        if (isTrue)
                        {
                            return "1";//新增成功
                        }
                        else
                        {
                            SalersOrderDetailBll.Delete(id, smolde.SellerId, out errMsg);//删除明细
                            SalersOrderRunBll.Delete(id, smolde.SellerId, out errMsg);//删除主表
                            return "3";//更新库存失败
                        }
                    }
                    else
                    {
                        SalersOrderDetailBll.Delete(id, int.Parse(Request.Cookies["userId"].Value), out errMsg);//删除明细
                        SalersOrderRunBll.Delete(id, int.Parse(Request.Cookies["userId"].Value), out errMsg);//删除主表
                        return "0";//新增明细失败
                    }
                }
                else
                {
                    return "0";//新增主表失败
                }

            }
            else
            {
                return "";//获取参数失败
            }
        }
        #endregion

        #region 销售报表(线下)
        public ActionResult OffLineSaleReport()
        {
            return View();
        }

        /// <summary>
        /// 销售报表(线下)列表
        /// </summary>
        /// <returns></returns>
        public string GetSalersOrderRun()
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(Request.Params["customerId"]) && !string.IsNullOrEmpty(Request.Params["startTime"]) && !string.IsNullOrEmpty(Request.Params["endTime"]))
            {
                string type = string.Empty;
                string errMsg = string.Empty;
                int page = 1;
                int pageSize = 10;
                if (!string.IsNullOrEmpty(Request.Params["page"]))
                {
                    page = int.Parse(Request.Params["page"]);
                }
                if (!string.IsNullOrEmpty(Request.Params["pagesize"]))
                {
                    pageSize = int.Parse(Request.Params["pagesize"]);
                }
                if (!string.IsNullOrEmpty(Request.Params["type"]))
                {
                    type = Request.Params["type"];
                }
                int customerId = int.Parse(Request.Params["customerId"]);
                DateTime start = DateTime.Parse(DateTime.Parse(Request.Params["startTime"]).ToString("yyyy-MM-dd 00:00:00"));
                DateTime end = DateTime.Parse(DateTime.Parse(Request.Params["endTime"]).ToString("yyyy-MM-dd 23:59:59"));
                PageDataView pdv = SalersOrderRunBll.GetOrderRunList(page, pageSize, "", int.Parse(Request.Cookies["userId"].Value), start, end, out errMsg, customerId, type);
                if (pdv != null && pdv.DataTable != null && pdv.DataTable.Rows.Count > 0)
                {
                    DataTable data = pdv.DataTable;
                    string str = JsonConvert.SerializeObject(data);
                    json += "{";
                    json += "\"total\":\"" + pdv.TotalNum + "\",\"rows\":" + str + "";
                    json += "}";
                }
                else
                {
                    json = "{\"total\":\"0\",\"rows\":[]}";
                }
            }
            else
            {
                json = "{\"total\":\"0\",\"rows\":[]}";
            }
            return json;
        }

        public string ExportOSR()
        {
            if (!string.IsNullOrEmpty(Request.Params["customerId"]) && !string.IsNullOrEmpty(Request.Params["startTime"]) && !string.IsNullOrEmpty(Request.Params["endTime"]))
            {
                string type = string.Empty;
                string errMsg = string.Empty;
                int page = 1;
                int pageSize = 10000;
                int customerId = int.Parse(Request.Params["customerId"]);
                DateTime start = DateTime.Parse(DateTime.Parse(Request.Params["startTime"]).ToString("yyyy-MM-dd 00:00:00"));
                DateTime end = DateTime.Parse(DateTime.Parse(Request.Params["endTime"]).ToString("yyyy-MM-dd 23:59:59"));
                PageDataView pdv = SalersOrderRunBll.GetOrderRunList(page, pageSize, "", int.Parse(Request.Cookies["userId"].Value), start, end, out errMsg, customerId);
                DataTable dt = new DataTable();
                dt.Columns.Add("订单编号", typeof(string));
                dt.Columns.Add("下单时间", typeof(string));
                dt.Columns.Add("客户名称", typeof(string));
                dt.Columns.Add("订单金额", typeof(string));
                dt.Columns.Add("商品总数", typeof(string));
                dt.Columns.Add("摘要", typeof(string));
                if (pdv != null && pdv.DataTable != null && pdv.DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in pdv.DataTable.Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["订单编号"] = dr["Code"];
                        newRow["下单时间"] = dr["OrderDate"];
                        newRow["客户名称"] = dr["FullName"];
                        newRow["订单金额"] = dr["Amount"];
                        newRow["商品总数"] = dr["ProductQuantity"];
                        newRow["摘要"] = dr["Notes"];
                        dt.Rows.Add(newRow);
                    }

                }
                string name = "销售报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string FullName = Server.MapPath("~/") + "ExportFile/" + name;

                Common.outputExcel(dt, name, FullName);
                string url = Request.Url.Authority;
                url = "/ExportFile/" + name;
                return url;
            }
            return "";
        }
        #endregion
    }
}