using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Laplace.Framework.Extensions
{
    /// <summary>
    /// 枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] members = type.GetMember(enumeration.CastTo<string>());
            if (members.Length > 0)
            {
                //return members[0].ToDescription();
                return members[0].ToDescription(false);
            }
            return enumeration.CastTo<string>();
        }
    }
}
