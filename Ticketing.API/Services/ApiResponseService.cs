namespace Ticketing.API.Services
{
    public class ApiResponseService<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
