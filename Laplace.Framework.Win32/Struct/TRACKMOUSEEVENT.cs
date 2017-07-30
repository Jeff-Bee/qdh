using System;
using System.Runtime.InteropServices;
using Laplace.Framework.Win32.Enum;

namespace Laplace.Framework.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENT
    {
        internal uint cbSize;
        internal TRACKMOUSEEVENT_FLAGS dwFlags;
        internal IntPtr hwndTrack;
        internal uint dwHoverTime;
    }
}
