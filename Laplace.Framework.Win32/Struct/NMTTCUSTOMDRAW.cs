using System;
using System.Runtime.InteropServices;

namespace Laplace.Framework.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTTCUSTOMDRAW
    {
        internal NMCUSTOMDRAW nmcd;
        internal uint uDrawFlags;
    }
}
