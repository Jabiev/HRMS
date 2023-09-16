namespace CareerCompassAPI.Domain.Concretes
{
    public class PaginatedResponse<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }

        public PaginatedResponse(List<T> items, int totalItems)
        {
            Items = items;
            TotalItems = totalItems;
        }
    }

}
