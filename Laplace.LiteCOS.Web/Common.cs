using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Laplace.LiteCOS.Web
{
    public static class Common
    {

        /// <summary>
        /// 获取传入字段的对应的拼音码
        /// </summary>
        /// <param name="str"></param>
        public static string GetPYM(string str)
        {
            string pym = string.Empty;
            for (var i = 0; i < str.Length; i++)
            {
                if (IsChinese(str[i]))
                {
                    pym += "";
                }
                else {
                    pym += str[i];
                }
            }
            return pym;
        }

        /// <summary>
        /// 判断字符是否是汉字(用ASCII码判断)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsChinese(char a)
        {
            if ((int)a > 127)
                return true;
            else
                return false;
        }


        #region 汇出excel
        public static void ReportExcel(string tableName,string[] columns,DataTable dt)
        {
            MemoryStream ms = new MemoryStream();
            XSSFWorkbook workbook = new XSSFWorkbook();//创建Workbook对象
            if (columns.Length == dt.Columns.Count)
            {
                ISheet sheet = workbook.CreateSheet("sheet1");//创建工作表
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.WrapText = true;
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 16;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font.FontName = "標楷體";
                style.SetFont(font);//HEAD 样式
                IRow row = sheet.CreateRow(0);



                row.Height = 200 * 5;
                row.CreateCell(0).SetCellValue("");

                //设置Head的样式
                row.GetCell(0).CellStyle = style;

            }
            else {
                throw new Exception("");
            }
        }


        public static void outputExcel(DataTable table, string name, string FullName)
        {

            IWorkbook wb = new HSSFWorkbook();
            ISheet tb = wb.CreateSheet("Sheet1");



            IRow rowPerfix = tb.CreateRow(0);
            tb.CreateRow(0).HeightInPoints = 23;
            for (int k = 0; k < table.Columns.Count; k++)
            {
                ICell cell = rowPerfix.CreateCell(k);
                cell.SetCellValue(table.Columns[k].Caption);
            }



            for (int i = 0; i < table.Rows.Count; i++)
            {
                IRow row = tb.CreateRow(i + 1);
                tb.CreateRow(i + 1).HeightInPoints = 20;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    tb.SetColumnWidth(j, 15 * 256);
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(table.Rows[i][j].ToString());
                }
            }
            using (FileStream stm = File.OpenWrite(FullName))
            {
                if (wb == null)
                {

                }
                else
                {
                    wb.Write(stm);
                }
            }
           

        }
        #endregion


        public static string AppCode = "";//"ePipeMonitor";
        public static string ConfigKey_rdlc = "rdlc";
        public static string ConfigKey_Title = "Title";
        public static string ConfigKey_Footer = "Footer";
        public static string ConfigKey_MineName = "MineName";
        public static string ConfigKey_IsShowIndustrial = "IsShowIndustrial";
        public static string ConfigKey_IsShowStandard = "IsShowStandard";
        public static string ConfigKey_IsShowAvg = "IsShowAvg";
        public static string ConfigKey_IsShowCO = "IsShowCO";
        public static string ConfigKey_IsShowSum = "IsShowSum";
        public static string ConfigKey_IsShowSpanTime = "IsShowSpanTime";


        public static string FormCode_MPReportSetting = "MPReportSetting";
        public static string MPReportSetting = HttpContext.Current.Server.MapPath("~/") + @"\Config\MPReportSetting.xml";


        public static string GetRdlcPath(string ReportName)
        {
            return HttpContext.Current.Server.MapPath("~/") + string.Format(@"\报表文件\{0}.rdlc", ReportName);
        }

        #region 人民币金额大小写转换
        /// <summary>
        /// 转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = "";    //从原num值中取出的值
            string str4 = "";    //数字的字符串形式
            string str5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式
            j = str4.Length;      //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(str3);      //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /**/
        /// <summary>
        /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num)
        /// </summary>
        /// <param name="num">用户输入的金额，字符串形式未转成decimal</param>
        /// <returns></returns>
        public static string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "非数字形式！";
            }
        }
        #endregion



    }
}