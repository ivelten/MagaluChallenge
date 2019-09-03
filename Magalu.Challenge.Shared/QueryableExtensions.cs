namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> SelectPage<T>(this IQueryable<T> enumerable, int pageNumber, int pageSize)
        {
            return enumerable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
    }
}
