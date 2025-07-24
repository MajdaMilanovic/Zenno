namespace Zenno.Model.SearchObjects
{
    public class ReviewSearchObject
    {
        public string? SearchTerm { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? UserId { get; set; }
        public int? RoomId { get; set; }
    }
} 