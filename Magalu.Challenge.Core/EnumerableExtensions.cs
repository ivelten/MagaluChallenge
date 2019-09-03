using System.Collections.Generic;
using System.Linq;

namespace Magalu.Challenge.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectPage<T>(this IEnumerable<T> enumerable, int pageNumber, int pageSize)
        {
            return enumerable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
    }
}
