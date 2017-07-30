using System;
using System.Runtime.InteropServices;

namespace Laplace.Framework.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
        internal NMHDR hdr;
        internal uint dwDrawStage;
        internal IntPtr hdc;
        internal RECT rc;
        internal IntPtr dwItemSpec;
        internal uint uItemState;
        internal IntPtr lItemlParam;
    }
}
