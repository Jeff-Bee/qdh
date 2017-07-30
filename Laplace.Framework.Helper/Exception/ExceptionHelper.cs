using Laplace.Framework.Log;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Laplace.Framework.Helper
{
    public class ExceptionHelper
    {
        private static bool _stopShowMessageDlg = false;
        public static void BindExceptionHandler()
        {
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            Logger.LogError4Exception(ex.Exception);
            if (!_stopShowMessageDlg)
            {
                //var exceptionForm = new ExceptionForm(ex.Exception.Message);
                //exceptionForm.ShowDialog();
                //_stopShowMessageDlg = exceptionForm.StopShowMessage;
            }


        }
        /// <summary>
        /// 处理未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            var ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                Logger.LogError4Exception(ex);
                if (!_stopShowMessageDlg)
                {
                    //var exceptionForm = new ExceptionForm(ex.Message);
                    //exceptionForm.ShowDialog();
                    //_stopShowMessageDlg = exceptionForm.StopShowMessage;
                }

            }
        }
    }
}
