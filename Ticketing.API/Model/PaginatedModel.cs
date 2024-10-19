namespace Ticketing.API.Model
{
    public class PaginatedModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }

        public int ResultCount { get; set; }

        public PaginatedModel(IEnumerable<T> data, int totalCount, int resultCount)
        {
            Data = data;
            TotalCount = totalCount;
            ResultCount = resultCount;
        }
    }
}
