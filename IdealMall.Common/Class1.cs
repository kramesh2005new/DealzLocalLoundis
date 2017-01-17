using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdealMall.Common
{
    public static class Common
    {
        // GET: /Trade/Create
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static string GeneralUserName = "POST@CODE";
        public static string GeneralUserPostCode = string.Empty;
        public static string PublicUserGeneralRadius = "0.5";
    }
}
