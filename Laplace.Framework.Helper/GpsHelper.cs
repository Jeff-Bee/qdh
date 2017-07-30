using FSLib.Network.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Laplace.Framework.Log;

namespace Laplace.Framework.Helper
{
    /// <summary>
    /// GPS转换工具
    /// </summary>
    public static class GpsHelper
    {
        #region--Wgs2Mgs--
        const double pi = 3.14159265358979324;
        //
        // Krasovsky 1940
        //
        // a = 6378245.0, 1/f = 298.3
        // b = a * (1 - f)
        // ee = (a^2 - b^2) / a^2;
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;

        /// <summary>
        /// World Geodetic System ==> Mars Geodetic System
        /// </summary>
        /// <param name="wgLon"></param>
        /// <param name="wgLat"></param>
        /// <param name="mgLon"></param>
        /// <param name="mgLat"></param>
        public static void Wgs2Mgs(double wgLon, double wgLat, out double mgLon, out double mgLat)
        {
            if (OutOfChina(wgLat, wgLon))
            {
                mgLat = wgLat;
                mgLon = wgLon;
                return;
            }
            double dLat = TransformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = TransformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            mgLat = wgLat + dLat;
            mgLon = wgLon + dLon;
        }

        public static bool OutOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }
        private static double TransformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        private static double TransformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }
        #endregion-Wgs2Mgs-

        #region--Mgs2Wgs--
        private static bool _initTable = false;
        private static readonly double[] TableX = new double[660 * 450];
        private static readonly double[] TableY = new double[660 * 450];

        public static void Mgs2Wgs(double mgLon, double mgLat, out double wgLon, out double wgLat)
        {
            wgLon = mgLon;
            wgLat = mgLat;
            int i, j, k;
            double x1, y1, x2, y2, x3, y3, x4, y4, xtry, ytry, dx, dy;
            double t, u;
            LoadText();
            if (!_initTable)
                return;

            xtry = mgLon;
            ytry = mgLat;

            for (k = 0; k < 10; ++k)
            {
                // 只对中国国境内数据转换
                if (xtry < 72 || xtry > 137.9 || ytry < 10 || ytry > 54.9)
                {
                    return;
                }

                i = (int)((xtry - 72.0) * 10.0);
                j = (int)((ytry - 10.0) * 10.0);

                x1 = TableX[GetId(i, j)];
                y1 = TableY[GetId(i, j)];
                x2 = TableX[GetId(i + 1, j)];
                y2 = TableY[GetId(i + 1, j)];
                x3 = TableX[GetId(i + 1, j + 1)];
                y3 = TableY[GetId(i + 1, j + 1)];
                x4 = TableX[GetId(i, j + 1)];
                y4 = TableY[GetId(i, j + 1)];

                t = (xtry - 72.0 - 0.1 * i) * 10.0;
                u = (ytry - 10.0 - 0.1 * j) * 10.0;

                dx = (1.0 - t) * (1.0 - u) * x1 + t * (1.0 - u) * x2 + t * u * x3 + (1.0 - t) * u * x4 - xtry;
                dy = (1.0 - t) * (1.0 - u) * y1 + t * (1.0 - u) * y2 + t * u * y3 + (1.0 - t) * u * y4 - ytry;

                xtry = (xtry + mgLon - dx) / 2.0;
                ytry = (ytry + mgLat - dy) / 2.0;
            }

            wgLon = xtry;
            wgLat = ytry;
        }
        private static void LoadText()
        {
            if (_initTable)
            {
                return;
            }
            const string fileName = "Mars2Wgs.txt";
            if (!File.Exists(fileName)) return;
            using (var sr = new StreamReader(fileName))
            {
                string s = sr.ReadToEnd();
                Match mp = Regex.Match(s, "(\\d+)");

                int i = 0;
                while (mp.Success)
                {
                    if (i % 2 == 0)
                    {
                        TableX[i / 2] = Convert.ToDouble(mp.Value) / 100000.0;
                    }
                    else
                    {
                        TableY[(i - 1) / 2] = Convert.ToDouble(mp.Value) / 100000.0;
                    }
                    i++;
                    mp = mp.NextMatch();
                }
                _initTable = true;
            }
        }
        private static int GetId(int i, int j)
        {
            return i + 660 * j;
        }
        #endregion-Mgs2Wgs-
        /// <summary>
        /// 纬度描述字符串转换纬度浮点数
        /// </summary>
        /// <param name="latitude">纬度描述字符串:格式为ddmm.mmmmN,纬度半球，N或S(北纬或南纬)</param>
        /// <returns></returns>
        public static float LatitudeConvert(string latitude)
        {
            if (string.IsNullOrEmpty(latitude)) return 0f;
            float fLatitude = 0.0f;
            var lastChar = latitude[latitude.Length - 1];
            //如果没有维度或经度符号则返回错误
            if (lastChar != 'N' || lastChar != 'S')
            {
                //提取纬度
                if (float.TryParse(latitude.Substring(0, latitude.Length - 1), out fLatitude))
                {
                    //计算纬度的度分秒
                    var nDeg = (int)(fLatitude / 100);
                    var nTemp = fLatitude - nDeg * 100;
                    var nMin = (int)nTemp;
                    var nSec = (int)((nTemp - nMin) * 60);
                    fLatitude = (float)nDeg + ((float)nMin) / 60 + ((float)nSec) / 3600;
                }
            }
            return fLatitude;
        }
        /// <summary>
        /// 经度描述字符串转换经度浮点数
        /// </summary>
        /// <param name="longitude">经度，格式为dddmm.mmmmE,经度半球，E或W(东经或西经)</param>
        /// <returns></returns>
        public static float LongitudeConvert(string longitude)
        {
            if (string.IsNullOrEmpty(longitude)) return 0f;
            float fLongitude = 0.0f;
            var lastChar = longitude[longitude.Length - 1];
            //如果没有维度或经度符号则返回错误
            if (lastChar != 'E' || lastChar != 'W')
            {
                //提取纬度
                if (float.TryParse(longitude.Substring(0, longitude.Length - 1), out fLongitude))
                {
                    //计算经度的度分秒
                    var nDeg = (int)(fLongitude / 100);
                    var nTemp = fLongitude - nDeg * 100;
                    var nMin = (int)nTemp;
                    var nSec = (int)((nTemp - nMin) * 60);
                    fLongitude = (float)nDeg + ((float)nMin) / 60 + ((float)nSec) / 3600;
                }
            }
            return fLongitude;
        }


        /// <summary>
        /// 根据经纬度坐标获取位置信息
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <returns></returns>
        public static string GetAddress(double lat, double lng, out string errMsg)
        {
            return GetAddressByBaiduMap(lat, lng, out errMsg);
        }
        private static readonly string ReverseGeocoderUrlFormat = "http://api.map.baidu.com/geocoder/v2/?ak=9ff782d16c5604e4a4a79a5269cedc86&location={0},{1}&output=json";
        public static string GetAddressByBaiduMap(double lat, double lng,out string errMsg)
        {
            errMsg = string.Empty;
            //通过百度Api获取地名url格式
            var url= string.Format(ReverseGeocoderUrlFormat, lat, lng);

            var client = new HttpClient();
            var context = client.Create<string>(HttpMethod.Get, url);
            context.Send();

            if (context.IsValid())
            {
                System.Console.WriteLine(context.Result);
                var result = JsonConvert.DeserializeObject(context.Result) as JObject;
                return result["result"]["formatted_address"].ToString();
                //AppendText("成功...");
                //AppendText("JSONP参数\t==>\t" + (context.AcquireResponseToObject().JsonpCallbackName ?? ""));
                //DumpObject(context.Result);
            }
            else
            {
                errMsg = string.Format("GetAddressByBaiduMap错误！状态码：{0},服务器响应：{1}"
                    , context.Response.Status,
                    context.ResponseContent);
                System.Console.WriteLine("错误！状态码：" + context.Response.Status);
                System.Console.WriteLine("服务器响应：" + context.ResponseContent);
                return string.Empty;
            }
            
            //return null;
        }

        /// <summary>
        /// 百度地图坐标转换（火星坐标转换到百度坐标）
        /// </summary>
        /// <param name="lng">火星坐标经度</param>
        /// <param name="lat">火星坐标纬度</param>
        /// <param name="baiduLng">百度坐标经度</param>
        /// <param name="baiduLat">百度坐标纬度</param>
        /// <returns></returns>
        public static bool BaiduGeoconv(float lng, float lat,out float baiduLng,out float baiduLat)
        {
            baiduLng = lng;
            baiduLat = lat;
            var ret = false;
            try
            {
                var url = "http://api.map.baidu.com/geoconv/v1/?coords=" + lng + "," + lat + "&from=1&to=5&ak=n2rlZgWuU9dN5cCpgTiNYZZjzZ88PbMb";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var myResponseStream = response.GetResponseStream();
                if (myResponseStream != null)
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    var json = JsonConvert.DeserializeObject(myStreamReader.ReadToEnd()) as JObject;
                    myStreamReader.Close();
                    myResponseStream.Close();
                    if (json != null && (int)json["status"] == 0)
                    {
                        baiduLng = (float)json["result"][0]["x"];
                        baiduLat = (float)json["result"][0]["y"];
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
            }


            return ret;
        }

    }
}
