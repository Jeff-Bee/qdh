using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Laplace.Framework.Win32.Struct
{
    /// <summary>
    /// 系统时间类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
        public override string ToString()
        {
            return ("[SYSTEMTIME: " + this.wDay.ToString(CultureInfo.InvariantCulture) + "/" + this.wMonth.ToString(CultureInfo.InvariantCulture) + "/" + this.wYear.ToString(CultureInfo.InvariantCulture) + " " + this.wHour.ToString(CultureInfo.InvariantCulture) + ":" + this.wMinute.ToString(CultureInfo.InvariantCulture) + ":" + this.wSecond.ToString(CultureInfo.InvariantCulture) + "]");
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class SYSTEMTIMEARRAY
    {
        public short wYear1;
        public short wMonth1;
        public short wDayOfWeek1;
        public short wDay1;
        public short wHour1;
        public short wMinute1;
        public short wSecond1;
        public short wMilliseconds1;
        public short wYear2;
        public short wMonth2;
        public short wDayOfWeek2;
        public short wDay2;
        public short wHour2;
        public short wMinute2;
        public short wSecond2;
        public short wMilliseconds2;
    }
}
