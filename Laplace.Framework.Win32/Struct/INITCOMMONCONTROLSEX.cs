using System;
using System.Runtime.InteropServices;

namespace Laplace.Framework.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INITCOMMONCONTROLSEX
    {
        internal INITCOMMONCONTROLSEX(int flags)
        {
            this.dwSize = Marshal.SizeOf(typeof(INITCOMMONCONTROLSEX));
            this.dwICC = flags;
        }

        internal int dwSize;
        internal int dwICC;
    }
}
