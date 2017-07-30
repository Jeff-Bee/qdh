using System;
using System.Runtime.InteropServices;

namespace Laplace.Framework.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
        internal byte BlendOp;
        internal byte BlendFlags;
        internal byte SourceConstantAlpha;
        internal byte AlphaFormat;

        public BLENDFUNCTION(
            byte op, byte flags, byte alpha, byte format)
        {
            BlendOp = op;
            BlendFlags = flags;
            SourceConstantAlpha = alpha;
            AlphaFormat = format;
        }
    }
}
