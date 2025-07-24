namespace Zenno.Model.SearchObjects
{
    public class ServiceSearchObject
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }
} 