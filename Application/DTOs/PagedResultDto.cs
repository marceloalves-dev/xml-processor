namespace Application.DTOs
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
    }
}
