namespace Zenno.Model.SearchObjects
{
    public class ReservationSearchObject
    {
        public int? UserId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ReservationStatus? Status { get; set; }
    }
} 