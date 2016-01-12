using System;
using System.Collections.Generic;
using System.Linq;

namespace Jane.Core.Extensions
{
    public static class IListExtensions
    {
        public static T InsertIfNotExists<T>(this IList<T> list,
            Func<T, bool> predicate,
            T obj)
            where T : class
        {
            var match = list.FirstOrDefault(predicate);
            if (match == null)
            {
                match = obj;
                list.Add(obj);
            }
            return match;
        }

        public static T GetOrAdd<T>(this IList<T> list, Func<T, bool> predicate, T obj)
        {
            var match = list.FirstOrDefault(predicate);
            if (match == null)
            {
                match = obj;
                list.Add(match);
            }
            return match;
        }

    }

}

