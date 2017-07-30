using System;
using System.Collections.Generic;
using System.Text;

namespace Laplace.Framework.Win32.Const
{
    public static class WinUser
    {
        public const int WM_SYSCOMMAND = 0x0112;//点击窗口左上角那个图标时的系统信息 
        public const int SC_MOVE = 0xF010;//移动信息 
        public const int HTCAPTION = 0x0002;//表示鼠标在窗口标题栏时的系统信息 
        public const int WM_NCHITTEST = 0x84;//鼠标在窗体客户区（除了标题栏和边框以外的部分）时发送的消息 
        public const int HTCLIENT = 0x1;//表示鼠标在窗口客户区的系统消息 
        public const int SC_MAXIMIZE = 0xF030;//最大化信息 
        public const int SC_MINIMIZE = 0xF020;//最小化信息 
    }
}
