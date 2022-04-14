namespace BookLibWebApi.Models
{
    public class PaginationModel<T>
    {
        public T Value { get; set; }
        public int Total { get; set; }
    }
}