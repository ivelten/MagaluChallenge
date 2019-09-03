using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static float AverageOrDefault<T>(this IEnumerable<T> enumerable, Func<T, float> selector)
        {
            if (enumerable.Any())
                return enumerable.Average(selector);

            return 0f;
        }
    }
}
