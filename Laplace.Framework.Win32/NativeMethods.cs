using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Laplace.Framework.Win32.Struct;

namespace Laplace.Framework.Win32
{
    public class NativeMethods
    {
        private NativeMethods()
        {
        }

        #region USER32.DLL

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll", SetLastError = true,
            CharSet = CharSet.Unicode, BestFitMapping = false)]
        public static extern IntPtr CreateWindowEx(
            int exstyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hwndParent,
            IntPtr Menu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(
            IntPtr hInstance, int lpIconName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint flags);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(
            IntPtr hWnd, ref RECT r);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(
            IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(
            IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool TrackMouseEvent(
            ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PtInRect(ref RECT lprc, Point pt);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(
            IntPtr hWnd,
            int nIDEvent,
            uint uElapse,
            IntPtr lpTimerFunc);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(
            IntPtr hWnd, uint uIDEvent);

        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref TOOLINFO lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref RECT lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            [MarshalAs(UnmanagedType.LPTStr)]string lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, ref NMHDR lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, int lParam);


        [DllImport("kernel32.dll", EntryPoint = "Beep")] // 第一个参数是指频率的高低，越大越高，第二个参数是指响的时间多长
        public extern static int Beep(int dwFreq, int dwDuration);
        #endregion

        #region GDI32.DLL

        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            BLENDFUNCTION blendFunction);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(
            IntPtr hDest,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int sX,
            int sY,
            int nWidthSrc,
            int nHeightSrc,
            int dwRop);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            IntPtr hdc,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA(
            [MarshalAs(UnmanagedType.LPStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)]string lpszDevice,
            [MarshalAs(UnmanagedType.LPStr)]string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCW(
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
            string lpszDriver,
            string lpszDevice,
            string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);


        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);


        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);
        #endregion

        #region comctl32.dll

        [DllImport("comctl32.dll",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool InitCommonControlsEx(
            ref INITCOMMONCONTROLSEX iccex);

        #endregion

        #region kernel32.dll
        // <summary>
        /// Allocates a new console for the calling process（启动控制台）
        /// </summary>
        /// <returns>If the function succeeds, return true</returns>
        [DllImport("kernel32")]
        public static extern bool AllocConsole();
        /// <summary>
        /// 释放控制台
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMHDR destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTDISPINFO destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            IntPtr destination, ref NMTTDISPINFO Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref POINT destination, ref RECT Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTCUSTOMDRAW destination, IntPtr Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMCUSTOMDRAW destination, IntPtr Source, int length);


        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern Boolean SetSystemTime([In, Out] SYSTEMTIME st);
        #endregion

        #region--winInet.dll--
        //InternetGetConnectedState返回的状态标识位的含义：
        public const int INTERNET_CONNECTION_MODEM = 1;
        public const int INTERNET_CONNECTION_LAN = 2;
        public const int INTERNET_CONNECTION_PROXY = 4;
        public const int INTERNET_CONNECTION_MODEM_BUSY = 8;
        /// <summary>
        /// 参数lpdwFlags返回当前网络状态,参数dwReserved依然是保留参数，设置为0即可。   
      //  INTERNET_CONNECTION_MODEM 通过调治解调器连接网络
      //  INTERNET_CONNECTION_LAN 通过局域网连接网络
      //  这个函数的功能是很强的。它可以：   
　　    //1.   判断网络连接是通过网卡还是通过调治解调器   
　　    //2.   是否通过代理上网   
　　    //3.   判断连接是On Line还是Off   Line   
　　    //4.   判断是否安装“拨号网络服务”   
　　    //5.   判断调治解调器是否正在使用
        /// </summary>
        /// <param name="dwFlag"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("winInet.dll ")]
        public static extern bool InternetGetConnectedState(ref int dwFlag,int dwReserved);
        #endregion-winInet.dll-
    }
}
