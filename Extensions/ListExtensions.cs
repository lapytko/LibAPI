using System;
using System.Collections.Generic;

namespace LibAPI.Extensions
{
    public static class ListExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null)
                return;
            var i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }
    }
}