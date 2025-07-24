using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenno.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!; // Navigation property to Room

        public int UserId { get; set; }
        public User User { get; set; } = null!; // Navigation property to User

        public ICollection<Service>? Services { get; set; } // Navigation property to Services
    }
}
