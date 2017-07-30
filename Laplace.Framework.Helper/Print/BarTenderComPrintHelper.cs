using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using BarTender;
using Microsoft.Win32;

namespace Laplace.Framework.Helper
{
    /// <summary>
    /// BarTender(9.40.0.0)COM引用打印工具
    /// </summary>
    public static class BarTenderComPrintHelper
    {
        // Declare a BarTender application variable 
        private static BarTender.Application _btApp;
        // Declare a BarTender document variable 
        private static BarTender.Format _btFormat;
        private static string _formatFile = string.Empty;
        private static List<string> _listNamedSubString = null;
        /// <summary>
        /// 初始化应用
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static bool InitApplication(out string errMsg)
        {
            errMsg = String.Empty;


            if (_btApp != null)
            {
                return true;
            }
            var ret = false;
            try
            {
                //Instantiate a BarTender application variable 
                _btApp = new BarTender.ApplicationClass() as BarTender.Application;
                _btApp.Visible = false;
                ret = true;

                //Store this instance as an object variable in C# 

                //object btObject = System.Runtime.InteropServices.Marshal.GetActiveObject("BarTender.Application");

                ////Convert the object variable to a BarTender application variable 

                //_btApp = btObject as BarTender.Application;




            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                // If the engine is unable to start, a PrintEngineException will be thrown.
                //MessageBox.Show(this, exception.Message, appName);
                //this.Close(); // Close this app. We cannot run without connection to an engine.
                //return;
            }
            return ret;
        }
        /// <summary>
        /// 返回默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            return new PrintDocument().PrinterSettings.PrinterName;//默认打印机名
        }
        /// <summary>
        /// 返回当前已经安装的打印机名称列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInstalledPrinters()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        }


        private static bool InitFormat(string file, out string errMsg)
        {
            errMsg = String.Empty;
            if (_btFormat != null && _formatFile == file)
            {
                return true;
            }
            var ret = false;
            try
            {
                _btFormat = _btApp.Formats.Open(file, false, "");
                _formatFile = file;
                _listNamedSubString = new List<string>();
                for (int i = 1; i <= _btFormat.NamedSubStrings.Count; i++)
                {
                    var btSubString = _btFormat.NamedSubStrings.GetSubString(i);
                    _listNamedSubString.Add(btSubString.Name);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                System.Console.WriteLine(ex);
            }
            return ret;
        }
        /// <summary>
        /// 关闭打开的文件
        /// </summary>
        public static void Close()
        {
            try
            {
                if (_btFormat != null)
                {
                    _btFormat.Close(BtSaveOptions.btDoNotSaveChanges);
                    _btFormat = null;
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            try
            {

                if (_btApp != null)
                {
                    _btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                    var process = Process.GetProcessById(_btApp.ProcessId);
                    if (process != null)
                    {
                        //process.Close();
                        process.Kill();
                    }
                    _btApp = null;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }




        }
        public static void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 判断BarTender9.4是否安装注册
        /// </summary>
        /// <returns></returns>
        public static bool IsBarTenderRegistered()
        {
            return IsRegistered("B9425246-4131-11D2-BE48-004005A04EDF");
            return IsRegistered("D58562C1-E51B-11CF-8941-00A024A9083F");
        }
        ///<summary>
        /// 检查指定的 COM 组件是否已注册到系统中
        /// </summary>
        /// <param name="clsid">指定 COM 组件的Class Id</param>
        /// <returns>true: 表示已注册；false: 表示未注册</returns>
        public static bool IsRegistered(String clsid)
        {

            //参数检查
            //System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(clsid), "clsid 不应该为空");

            //设置返回值
            Boolean result = false;

            //检查方法，查找注册表是否存在指定的clsid
            String key = String.Format(@"CLSID\{{{0}}}", clsid);
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(key);
            if (regKey != null)
            {
                result = true;
            }

            return result;
        }//end method
        public static bool Print(string file, Tuple<string, string>[] valueStrings, out string errMsg, string printName = "")
        {
            if (!InitApplication(out errMsg))
            {
                return false;
            }
            // Open a BarTender document 
            if (!InitFormat(file, out errMsg))
            {
                return false;
            }
            var ret = false;
            try
            {

                foreach (var value in valueStrings)
                {
                    if (_listNamedSubString.Contains(value.Item1))
                    {
                        // Set the value of the data source called Name 
                        _btFormat.SetNamedSubStringValue(value.Item1, value.Item2);
                    }
                }
                if (string.IsNullOrEmpty(printName))
                {
                    printName = GetDefaultPrinterName();
                }
                // Set the name of the printer 
                _btFormat.PrintSetup.Printer = printName;
                // Print the document 
                _btFormat.PrintOut(false, false);
                ret = true;

            }
            catch (System.Runtime.InteropServices.COMException comException)
            {
                errMsg = comException.Message;
                //format = null;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                //format = null;
            }
            return ret;
        }


    }
}
