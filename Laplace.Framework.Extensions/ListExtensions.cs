using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections;


namespace Laplace.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dictionaries.
    /// </summary>
    public static class ListExtensions
    {        
        /// <summary>
        /// AddRange of items of same type to IList 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="itemsToAdd"></param>
        public static IList<T> AddRange<T>(this IList<T> items, IList<T> itemsToAdd)
        {
            if (items == null || itemsToAdd == null)
                return items;

            foreach (T item in itemsToAdd)
                items.Add(item);

            return items;
        }


        /// <summary>
        /// Is empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        [Obsolete("Method moved to Utilities.EnumerableExtensions.IsNullOrEmpty()")]
        public static bool IsNullOrEmpty<T>(IList<T> items)
        {
            return items.IsNullOrEmpty();
        }


        public static DataTable ConvertToDataTable<T>(this IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }


            DataTable dt = new DataTable(typeof (T).Name);

            DataColumn column;

            DataRow row;

            var propertes = typeof (T).GetProperties(
                BindingFlags.Public
                | BindingFlags.Instance);

            foreach (T t in list)
            {
                if (t == null)
                {
                    continue;
                }
                row = dt.NewRow();


                for (int i = 0; i < propertes.Length; i++)
                {
                    var pi = propertes[i];
                    var name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        column = new DataColumn(name,pi.PropertyType);

                        dt.Columns.Add(column);
                    }

                    row[name] = pi.GetValue(t, null);
                }

                dt.Rows.Add(row);
            }


            return dt;
        }
    }
}
