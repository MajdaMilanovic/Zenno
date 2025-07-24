namespace Zenno.Model.SearchObjects
{
    public class UserSearchObject
    {
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public bool? HasActiveReservations { get; set; }
    }
} 