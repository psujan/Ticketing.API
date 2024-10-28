namespace Ticketing.API.Model
{
    public class PaginatedModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }

        public int ResultCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public PaginatedModel(IEnumerable<T> data, int totalCount, int resultCount , int pageNumber = 1 , int pageSize = 10 )
        {
            Data = data;
            TotalCount = totalCount;
            ResultCount = resultCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
