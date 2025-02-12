﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.Core.Extensions
{
    public static class IEnumerableExtension
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> ls)
        {
            if (ls is null)
            {
                return default;
            }
            HashSet<T> sets = new HashSet<T>();
            foreach (var p in ls)
            {
                sets.Add(p);
            }
            return sets;
        }

        public static string ToSeqString<T>(this IEnumerable<T> ls, string separator)
        {
            if (ReferenceEquals(ls, null))
            {
                return null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var p in ls)
                {
                    stringBuilder.Append(p);
                    stringBuilder.Append(separator);
                }
                stringBuilder.Remove(stringBuilder.Length - separator.Length, separator.Length);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        ///     集合转为ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null) return null;
            return new ObservableCollection<T>(source);
        }
    }
}
