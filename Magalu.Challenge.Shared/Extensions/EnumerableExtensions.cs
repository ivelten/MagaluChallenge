using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectPage<T>(this IEnumerable<T> enumerable, int pageNumber, int pageSize)
        {
            return enumerable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            var index = -1;

            foreach (var item in enumerable)
            {
                index++;
                action(index, item);
            }
        }

        public static float? AverageOrDefault<T>(this IEnumerable<T> enumerable, Func<T, float> selector)
        {
            if (enumerable.Any())
                return enumerable.Average(selector);

            return null;
        }
    }
}
