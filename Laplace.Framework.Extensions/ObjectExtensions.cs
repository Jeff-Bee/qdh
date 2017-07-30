using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T CastTo<T>(this object value)
        {
            object result;
            Type type = typeof(T);
            try
            {
                if (value == null || Convert.IsDBNull(value))
                {
                    return default(T);
                }

                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                }
                //.Net 4.0才支持
                //else if (type == typeof(Guid))
                //{
                //    result = Guid.Parse(value.ToString());
                //}
                else if (type == typeof(int) && value is string)
                {
                    switch (value.ToString().ToUpper())
                    {
                        case "OK":
                        case "YES":
                        case "TRUE":
                            result = 1;
                            break;
                        case "No":
                        case "FALSE":
                            result = 0;
                            break;
                        default:
                            result = int.Parse(value.ToString());
                            break;
                    }
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch
            {
                result = default(T);
            }

            return (T)result;
        }

        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            object result;
            Type type = typeof(T);
            try
            {
                result = type.IsEnum ? Enum.Parse(type, value.ToString()) : Convert.ChangeType(value, type);
            }
            catch
            {
                result = defaultValue;
            }
            return (T)result;
        }

        /// <summary>
        /// 获取未知类的某个属性值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="value">类</param>
        /// <param name="propertyName">属性名称.</param>
        /// <returns>属性值</returns>
        public static T GetProperty<T>(this object value, string propertyName)
        {
            var property = value.GetType().GetProperty(propertyName);

            return property == null ? default(T) : property.GetValue(value, null).CastTo<T>();
        }

        public static void SetProperty(this object value, string propertyName, object propertyValue)
        {
            var property = value.GetType().GetProperty(propertyName);
            property.SetValue(value, propertyValue, null);
        }

        public static T InvokeMethod<T>(this object value, string methodName, params object[] parameters)
        {
            var method = value.GetType().GetMethod(methodName);
            return (T)method.Invoke(value, parameters);
        }
    }
}
