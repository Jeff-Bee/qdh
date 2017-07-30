using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
     
        /// <summary>
        /// 更新指定键的值，如果没有则添加
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Update<TKey,TValue>(this ConcurrentDictionary<TKey, TValue> dic,TKey key,TValue value) 
        {
            if (dic.ContainsKey(key))
            {
                TValue temp;
                dic.TryRemove(key, out temp);
            }
            return dic.TryAdd(key, value);
        }
        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key))
            {
                TValue temp;
                dic.TryRemove(key, out temp);
            }
        }
    }
}
