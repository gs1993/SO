using CSharpFunctionalExtensions;
//using Nest;

namespace ElasticSoDatabase.Utils
{
    public class SearchResponseWrapper<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TotalCount { get; }

        public SearchResponseWrapper(IEnumerable<T> items, int totalCount)
        {
            Items = items?.ToList()?.AsReadOnly() ?? new List<T>().AsReadOnly();
            TotalCount = totalCount;
        }
    }
}
