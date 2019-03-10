﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static async Task<IEnumerable<T>> ForEach<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var item in source)
            {
                await action(item);
            }

            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }

            return source;
        }

        /// <summary>
        /// returns true if collection contains any element
        /// and false if collection is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ContainsElement<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}
